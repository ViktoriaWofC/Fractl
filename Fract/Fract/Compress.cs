using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fract
{
    public class Compress : Compression
    {
        private List<Rang> rangList = new List<Rang>();
        private int r;//размер рангового блока
        private int[,] pix;
        private int width;//ширина картинки
        private int height;//высота картинки
        private int n;//количество блоков по высоте
        private int m;//количество блоков по ширине
        private double epsilon;//коэффициент компрессии?

        // 

        //
        private String sss;
        private int jhgjhg = 0;

        public Compress(int[,] pix, int r, double epsilon)
        {
            this.pix = pix;
            this.r = r;
            this.width = pix.GetLength(1);//pix[0].Length;
            this.height = pix.GetLength(0); //pix.Length;
            this.m = width / r;
            this.n = height / r;
            this.epsilon = epsilon;

            this.sss = "";
        }

        public List<Rang> getRangList()
        {
            return rangList;
        }

        public void compressImage()
        {
            bool b = false;
            int[,] rang = new int[r,r];// ранговый блок

            //перебор ранговых блоков
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    //выделяем ранговый блок
                    for (int ii = 0; ii < r; ii++)
                        for (int jj = 0; jj < r; jj++)
                            rang[ii,jj] = pix[r * i + ii, r * j + jj];

                    //пербор доменных блоков
                    getDomenBloc(rang, 1, j * r, i * r);
                    //rangsList.add(ran);
                }

            //return rangsList;
        }

        public void getDomenBloc(int[,] rang, int k, int x, int y)
        {
            //x - начальная координата блока
            //y - начальная координата блока
            //пербор доменных блоков
            Rang ran = null;
            int z = rang.GetLength(0);//размер доменного блока = размер рангового
            //int f = r * 2 / z;//кол-во усредняемых пикселей
            int[,] domen = new int[z,z];//уменьшенный доменный блок
            int[,] domenAfin = new int[r * 2, r * 2];//доменный блок подвергнутый афинному преобразованию
            int[,] domenBig = new int[r * 2,r * 2];//доменный блок

            bool b = false;
            int id = 0;
            int jd = 0;

            while ((id < n - 1) && (b == false))
            {
                jd = 0;
                while ((jd < m - 1) && (b == false))
                {
                    int sum = 0;
                    //выделяем доменный блок
                    for (int i = 0; i < r * 2; i++)
                        for (int j = 0; j < r * 2; j++)
                            domenBig[i,j] = pix[r * id + i, r * jd + j];

                    for (int afi = 0; afi < domenBig.GetLength(0); afi++)
                        for (int afj = 0; afj < domenBig.GetLength(0); afj++)
                            domenAfin[afi, afj] = domenBig[afi, afj];

                    
                    int h = 0;
                    while ((h < 6) && (b == false))
                    {
                        //применяем афинное преобразование                           
                        domenAfin = setAfinnInt(domenBig, h);
                        //уменьшаем его усреднением
                        domen = reduceBlock(domenAfin, z, k);

                        //сравниваем ранговый и доменный блок 
                        if (compareBlocs(rang, domen))//if (compareBlocs(rang, domenAfin))
                        {
                            b = true;
                            ran = new Rang(jd * r, id * r, h, k, x, y, 1);
                        }
                        else {
                            //проверяем яркости
                            double bright = 4;
                            while ((bright >= 0.25) && (b == false))
                            {
                                if (compareBlocs(rang, changeBright(domen, bright)))//changeBright(domenAfin, bright))
                                {
                                    b = true;
                                    ran = new Rang(jd * r, id * r, h, k, x, y, bright);
                                }
                                else {
                                    if (bright / 2 == 1)
                                        bright = bright / 4;
                                    else bright = bright / 2;
                                }
                            }
                            h++;
                            //domenAfin = setAfinnInt(domen, h);
                        }
                    }
                    jd++;
                }
                id++;
            }
            //если для рангового блока не нашли доменного
            if (ran == null)
            {
                k = k * 2;
                //уменьшаем r/2 и снова ищем доменный пресуя его в 4 раза и т.д пока r>2
                if (r / k >= 2)//(r / k >= 4)
                               //while (ran == null)
                {
                    int[,] rangDop = new int[z / 2, z / 2];
                    for (int ir = 0; ir < 2; ir++)
                        for (int jr = 0; jr < 2; jr++)
                        {
                            //выделяем ранговый блок
                            for (int i = 0; i < z / 2; i++)
                                for (int j = 0; j < z / 2; j++)
                                    rangDop[i, j] = pix[x + ir * z / 2 + i, y + jr * z / 2 + j];

                            getDomenBloc(rangDop, k, x + ir * z / 2, y + jr * z / 2);//x*r,y*r
                        }

                }
                //ran = new Rang(0, 0, 0,k,x,y);
                //rangList.add(ran);

            }
            else
            {
                rangList.Add(ran);
                printBlock(ran, rangList.Count-1);
            }

            //return ran;
        }

        public bool compareBlocs(int[,] rang, int[,] domen)
        {
            bool b = false;
            double sum = 0;
            int k = rang.GetLength(0);
            double h = 0;

            Color colorDomen, colorRang;

            for (int i = 0; i < k; i++)
                for (int j = 0; j < k; j++)
                {

                    colorDomen = Color.FromArgb(domen[i, j]);
                    colorRang = Color.FromArgb(rang[i, j]);
                    //sum += color.R;

                    h = (colorDomen.R - colorRang.R);
                    //h = (domen[i, j] - rang[i, j])/100000;// / 10000;

                    sum += h * h;
                }

            sum = sum/1000;
            // if((sum!=0)&&(sum!=1531)&&(sum!=441032))
            //   System.out.println("");

            if (jhgjhg < 150)
            {
                jhgjhg++;
                sss += " " + sum+"\r\n";
            }

            if (sum < epsilon)
                b = true;
            else b = false;

            return b;
        }

        public void SaveSumCompare()
        {
            File.WriteAllText(@"sum_compare.txt", sss);
        }

        public int[,] changeBright(int[,] pix, double k)
        {
            int n = pix.GetLength(0);
            int x;
            int[,] p = new int[n,n];
            Color color;

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    color = Color.FromArgb(pix[i, j]);//new Color(pix[i,j]);
                    x = color.R;//color.getRed();
                    x = (int)(x * k);
                    if (x > 255)
                        x = 255;
                    color = Color.FromArgb(x, x, x);
                    p[i, j] = color.ToArgb();
                }
            return p;
        }

        public int[,] setAfinnInt(int[,] pix, int k)
        {
            //int argb;
            int n = pix.GetLength(0);
            int x, y;
            int[,] p = new int[n,n];

            if (k < 4)
            {

                if (k == 1)
                {//поворот на 90

                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                        {
                            x = n - 1 - i;
                            y = j;
                            p[y, x] = pix[i,j];
                        }

                }
                else if (k == 2)
                {//поворот на 180
                    int h;
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                        {
                            x = n - 1 - i;
                            y = j;
                            h = x;
                            x = n - 1 - y;
                            y = h;
                            p[y,x] = pix[i,j];
                        }
                }
                else if (k == 3)
                {//поворот на 270
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                        {
                            x = i;
                            y = n - 1 - j;
                            p[y,x] = pix[i,j];
                        }
                }

            }
            else {
                if (k == 4)
                {//отражение по вертикали
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                        {
                            x = n - 1 - j;
                            y = i;
                            p[y,x] = pix[i,j];
                        }
                }
                else if (k == 5)
                {//отражение по горизонтали
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                        {
                            x = j;
                            y = n - 1 - i;
                            p[y,x] = pix[i,j];
                        }
                }
            }

            if (k == 0)
                return pix;
            else return p;
        }

        public int[,] reduceBlock(int[,] blockBig, int z, int k)
        {
            int n = blockBig.GetLength(0);
            int[,] block = new int[n / 2, n / 2];
            Color color;// = new Color(domen[i][j]);
            int d = 0, sum;
            for (int i = 0; i < z * 2; i = i + 2 * k)//i++)z
                for (int j = 0; j < z * 2; j = j + 2 * k)//j++)z
                {
                    sum = 0;
                    for (int ii = 0; ii < 2 * k; ii++)
                        for (int jj = 0; jj < 2 * k; jj++)
                        {
                            color = Color.FromArgb(blockBig[i + ii, j + jj]);
                            sum += color.R;
                        }
                    d = (int)(sum / Math.Pow(4, k));


                    color = Color.FromArgb(d, d, d);
                    block[i / (2 * k), j / (2 * k)] = color.ToArgb();
                }
            return block;
        }

        public void printBlock(Rang rang, int k)
        {
            Bitmap bitmap = new Bitmap(pix.GetLength(1), pix.GetLength(0));
            Color color;// = Color.White;
            //bi.SetPixel(rang.getX0() + j, rang.getY0() + i, color);

            for(int i = 0; i < bitmap.Width; i++)
                for (int j = 0; j < bitmap.Height; j++)
                {
                    if (i % r != 0 && j % r != 0)
                        bitmap.SetPixel(i, j, Color.White);
                    else bitmap.SetPixel(i, j, Color.Black);
                }

            for (int i = 0; i < r; i++)
                for (int j = 0; j < r; j++)
                {
                    bitmap.SetPixel(rang.getX0()+i, rang.getY0()+j, Color.FromArgb(pix[rang.getX0() + i, rang.getY0() + j]));
                }

            for (int i = 0; i < r*2; i++)
                for (int j = 0; j < r*2; j++)
                {
                    bitmap.SetPixel(rang.getX() + i, rang.getY() + j, Color.FromArgb(pix[rang.getX() + i, rang.getY() + j]));
                }

            try
            {

                bitmap.Save("D:\\университет\\диплом\\bloks\\"+k+".jpg");
                //Button5.Text = "Saved file.";
            }
            catch (Exception)
            {
                //MessageBox.Show("There was a problem saving the file." +
                    //"Check the file permissions.");
            }
        }
    }
}
