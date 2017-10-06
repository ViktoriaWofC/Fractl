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

        //private double minSKO=10000000;
        //private Rang minRang;

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
            int[,] rang = new int[r, r];// ранговый блок

            //перебор ранговых блоков
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    //выделяем ранговый блок
                    for (int ii = 0; ii < r; ii++)
                        for (int jj = 0; jj < r; jj++)
                            rang[ii, jj] = pix[r * i + ii, r * j + jj];

                    //пербор доменных блоков
                    getDomenBloc(rang, 1, j * r, i * r);
                    //rangsList.add(ran);

                }

            //
            //SaveSumCompare();
            //return rangsList;
        }

        public void getDomenBloc(int[,] rang, int k, int x0, int y0)
        {
            //x - начальная координата блока
            //y - начальная координата блока
            //пербор доменных блоков
            Rang ran = null;
            int R = r / k;//размер рангового блока
            int D = R * 2;//размер доменного блока
            //int domenSize = rang.GetLength(0);//размер доменного блока = размер рангового
            //int f = r * 2 / z;//кол-во усредняемых пикселей
            int[,] domen = new int[R,R];//уменьшенный доменный блок
            int[,] domenAfin = new int[R, R];//доменный блок подвергнутый афинному преобразованию
            int[,] domenBig = new int[D, D];//доменный блок

            double minSKO = 10000000;
            Rang minRang = new Rang(0,0,0,0,0,0,0,0, epsilon);

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
                    for (int i = 0; i < D; i++)
                        for (int j = 0; j < D; j++)
                            domenBig[i,j] = pix[R * id + i, R * jd + j];

                    //уменьшаем его усреднением
                    domen = reduceBlock(domenBig);

                    //for (int afi = 0; afi < domenBig.GetLength(0); afi++)
                    //    for (int afj = 0; afj < domenBig.GetLength(0); afj++)
                    //        domenAfin[afi, afj] = domenBig[afi, afj];

                    double[] so;
                    double s, o, sko;
                    List<double> skoMass = new List<double>();

                    Color colorDomen, colorRang;

                    //вычисляемм все СКО
                    for (int h = 0; h < 8; h++)
                    {
                        sko = 0;
                        //skoMass.Clear();// = new double[8];

                        domenAfin = setAfinnInt(domen, h);
                        so = getSO(rang, domenAfin);
                        s = so[0];
                        o = so[1];

                        ///
                        if (s == 0.0432511133949983 && o == 29.784600890716)
                            s = so[0];
                        ///

                        for (int i = 0; i < R; i++)
                            for (int j = 0; j < R; j++)
                            {
                                colorDomen = Color.FromArgb(domen[i, j]);
                                colorRang = Color.FromArgb(rang[i, j]);
                                double per = (s * colorDomen.R + o) - colorRang.R;
                                sko = sko + (per) * (per);
                                //sko = sko + ((s*domen[i, j] + o)-rang[i,j]) * ((s * domen[i, j] + o) - rang[i, j]);
                            }

                        skoMass.Add(sko);//skoMass[h] = sko;

                    }
                    //ищем минимальное СКО
                    double min = skoMass.Min();
                    
                    if (jhgjhg < 150)
                    {
                        jhgjhg++;
                        sss += " " + min + "\r\n";
                    }
                    else if(jhgjhg == 150)
                    {
                        SaveSumCompare();
                        jhgjhg++;
                    }

                    //сравниваем с коэффициентом компрессии
                    if (min < epsilon)
                    {
                        b = true;

                        int afin = skoMass.IndexOf(min);
                        domenAfin = setAfinnInt(domen, afin);
                        so = getSO(rang, domenAfin);
                        s = so[0];
                        o = so[1];

                        ran = new Rang(jd * R, id * R, afin, k, x0, y0, s, o, min);
                    }
                    else
                    {
                        if(min<minSKO)
                        {
                            int afin = skoMass.IndexOf(min);
                            domenAfin = setAfinnInt(domen, afin);
                            so = getSO(rang, domenAfin);
                            s = so[0];
                            o = so[1];

                            minSKO = min;
                            minRang = new Rang(jd * R, id * R, afin, k, x0, y0, s, o, minSKO);
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
                if (r / k >= 4)//(r / k >= 4)  //while (ran == null)
                {
                    int newR = r / k;
                    int[,] rangDop = new int[newR, newR];
                    for (int ir = 0; ir < 2; ir++)
                        for (int jr = 0; jr < 2; jr++)
                        {
                            //выделяем ранговый блок
                            for (int i = 0; i < newR; i++)
                                for (int j = 0; j < newR; j++)
                                {
                                    rangDop[i, j] = rang[ir * newR + i, jr * newR + j];
                                }
                            /*for (int i = 0; i < domenSize / 2; i++)
                                for (int j = 0; j < domenSize / 2; j++)
                                    rangDop[i, j] = pix[x0 + ir * domenSize / 2 + i, y0 + jr * domenSize / 2 + j];
                                    */

                            getDomenBloc(rangDop, k, x0 + ir * newR, y0 + jr * newR);//x*r,y*r
                        }

                }
                else
                {
                    rangList.Add(minRang);
                    printBlock(minRang, rangList.Count - 1);
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

        public void printRang(int z)
        {
            Rang rang = rangList[z];
            String text = ""
                + "Afin = "+ rang.getAfinn()+ "\r\n"
                + "k = " + rang.getK() + "\r\n"
                + "s = " + rang.getS() + "\r\n"
                + "o = " + rang.getO() + "\r\n"
                + "epsilon = " + rang.getEpsilon();
            File.WriteAllText(@"Rang_"+z+".txt", text);
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

        public int[,] changeBright(int[,] pix, double s, double o)
        {
            int n = pix.GetLength(0);
            double x;
            int[,] p = new int[n, n];
            Color color;

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    color = Color.FromArgb(pix[i, j]);//new Color(pix[i,j]);
                    //x = (color.R - o) / s;
                    x = s*color.R +o;
                    /*x = color.R;//color.getRed();
                    x = (int)(x * k);*/
                    if (x > 255)
                        x = 255;
                    if (x < 0)
                        x = 0;
                    color = Color.FromArgb((int)x, (int)x, (int)x);
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
                else if(k == 6)//k=1 + k=4
                {
                    int[,] p2 = new int[n, n];

                    //поворот на 90(k=1)
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                        {
                            x = n - 1 - i;
                            y = j;
                            p2[y, x] = pix[i, j];
                        }

                    
                    //отражение по вертикали (k=4)
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                        {
                            x = n - 1 - j;
                            y = i;
                            p[y, x] = p2[i, j];
                        }

                }
                else if (k == 7)//k=3 + k=4
                {
                    int[,] p2 = new int[n, n];
                    //поворот на 270 (k=3)
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                        {
                            x = i;
                            y = n - 1 - j;
                            p2[y, x] = pix[i, j];
                        }

                    //отражение по вертикали (k=4)
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                        {
                            x = n - 1 - j;
                            y = i;
                            p[y, x] = p2[i, j];
                        }
                }
            }

            if (k == 0)
                return pix;
            else return p;
        }

        public int[,] reduceBlock(int[,] blockBig)
        {
            int n = blockBig.GetLength(0);
            int[,] block = new int[n / 2, n / 2];
            Color color;// = new Color(domen[i][j]);
            int d = 0, sum;
            for (int i = 0; i < n ; i = i + 2)
                for (int j = 0; j < n; j = j + 2)
                {
                    sum = 0;

                    //color = Color.FromArgb(blockBig[i + ii, j + jj]);
                    sum += Color.FromArgb(blockBig[i, j]).R;
                    sum += Color.FromArgb(blockBig[i + 1 , j]).R;
                    sum += Color.FromArgb(blockBig[i, j + 1]).R;
                    sum += Color.FromArgb(blockBig[i + 1, j + 1]).R;

                    //d = (int)(sum / Math.Pow(4, k));
                    d = (int)(sum /4);


                    color = Color.FromArgb(d, d, d);
                    block[i/2, j/2] = color.ToArgb();
                    //block[i / (2 * k), j / (2 * k)] = color.ToArgb();
                }
            return block;
        }

        public void printBlock(Rang rang, int k)
        {
            int otst = r * 2 + 10;
            int width = pix.GetLength(1);
            Bitmap bitmap = new Bitmap(width+otst, pix.GetLength(0));
            Color color;// = Color.White;
            int R = r / rang.getK();//размер рангового блока
            int D = R*2;//размер доменного блока
            int[,] rangMatr = new int[R, R];
            //bi.SetPixel(rang.getX0() + j, rang.getY0() + i, color);

            for (int i = 0; i < bitmap.Width-otst; i++)
                for (int j = 0; j < bitmap.Height; j++)
                {
                    if (i % r != 0 && j % r != 0)
                        bitmap.SetPixel(i, j, Color.White);
                    else bitmap.SetPixel(i, j, Color.Black);
                }

            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(rang.getX0()+i, rang.getY0()+j, Color.FromArgb(pix[rang.getX0() + i, rang.getY0() + j]));
                    rangMatr[i,j] = pix[rang.getX0() + i, rang.getY0() + j];
                }

            for (int i = 0; i < D; i++)
                for (int j = 0; j < D; j++)
                {
                    bitmap.SetPixel(rang.getX() + i, rang.getY() + j, Color.FromArgb(pix[rang.getX() + i, rang.getY() + j]));
                }

            //////////////////////////////////////////
            int[,] domen = new int[D, D], domenAfin, domenBright;
            int[,] domenMin = new int[R, R];
            for (int i = 0; i < D; i++)
                for (int j = 0; j < D; j++)
                {
                    bitmap.SetPixel(width + 5 + i, 5 + j, Color.FromArgb(pix[rang.getX() + i, rang.getY() + j]));
                    domen[i,j] = pix[rang.getX() + i, rang.getY() + j];
                }

            domenMin = reduceBlock(domen);            
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(width + 5 + i, (5+R*2+10) + j, Color.FromArgb(domenMin[i,j]));
                }

            domenAfin = setAfinnInt(domenMin, rang.getAfinn());            
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(width + 5 + i, (5 + R * 3 + 20) + j, Color.FromArgb(domenAfin[i, j]));
                }

            domenBright = changeBright(domenAfin, rang.getS(), rang.getO());
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(width + 5 + i, (5 + R * 5 + 30) + j, Color.FromArgb(domenBright[i, j]));
                }

            //rang block
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(width + 5 + i, (5 + R * 6 + 40) + j, Color.FromArgb(rangMatr[i, j]));
                }

            //difference
            int argbcolor;
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    //argbcolor = Math.Abs(Color.FromArgb(rangMatr[i, j]).R - Color.FromArgb(domenBright[i, j]).R);
                    argbcolor = Math.Abs(Color.FromArgb(domenBright[i, j]).R - Color.FromArgb(rangMatr[i, j]).R);
                    bitmap.SetPixel(width + 5 + i, (5 + R * 7 + 50) + j, Color.FromArgb(argbcolor, argbcolor, argbcolor));
                }

            try
            {
                String name = k + "___k="+rang.getK()+"__" + "E="+epsilon + "____e=" + rang.getEpsilon() + "__";
                bitmap.Save("D:\\университет\\диплом\\bloks\\"+name+".jpg");
                //Button5.Text = "Saved file.";
            }
            catch (Exception)
            {
                //MessageBox.Show("There was a problem saving the file." +
                    //"Check the file permissions.");
            }

            if (k == 0)
                printRang(k);
        }

        public double[] getSO(int[,] rang, int[,] domen)
        {
            int N = rang.GetLength(0);
            double s = 0, o = 0, D = 0, R = 0, a = 0, b = 0;
            Color colorDomen, colorRang;

            for (int i = 0; i< N; i++)
                for (int j = 0; j < N; j++)
                {
                    colorDomen = Color.FromArgb(domen[i, j]);
                    colorRang = Color.FromArgb(rang[i, j]);
                    R = R + colorRang.R;
                    D = D + colorDomen.R;
                }

            R = R / (N * N);
            D = D / (N * N);

            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    colorDomen = Color.FromArgb(domen[i, j]);
                    colorRang = Color.FromArgb(rang[i, j]);
                    a = a + (colorDomen.R - D) * (colorRang.R - R);
                    b = b + (colorDomen.R - D) * (colorDomen.R - D);
                }

            s = a / b;
            if (b == 0 && a == 0)
                s = 0;
            o = R - s * D;

            return new double[] {s,o};
        }
    }

    /*int h = 0;
                    while ((h < 6) && (b == false))
                    {
                        //применяем афинное преобразование                           
                        domenAfin = setAfinnInt(domen, h);
                        

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
                    }*/

}
