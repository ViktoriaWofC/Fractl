using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fract
{
    public class Decompress
    {
        private List<Rang> rangList = new List<Rang>();
        private int r;//размер рангового блока
        private Bitmap bi;//BufferedImage bi;
        private int width;//ширина картинки
        private int height;//высота картинки
                           //private int n;//количество блоков по высоте
                           //private int m;//количество блоков по ширине        

        public Decompress(List<Rang> rangList, Bitmap bi, int r)//Decompress(List<Rang> rangList, BufferedImage bi, int r)
        {
            this.rangList = rangList;
            this.r = r;
            this.bi = bi;
            this.width = bi.Width;
            this.height = bi.Height;
            //this.m = width/r;
            //this.n = height/r;
        }

        public Bitmap decompressImage(int k)
        {
            int[,] domen;// = new int[r, r];
            int[,] domenBig = new int[r * 2, r * 2];
            int R, D;

            Color color;

            for (int g = 0; g < k; g++)
            {
                int m = bi.Width;
                int n = bi.Height;
                int[,] pixels = new int[n, m];

                //получаем массив интовых чисел из изображения
                for (int i = 0; i < n; i++)//строки
                    for (int j = 0; j < m; j++)//столбцы
                    {
                        pixels[i, j] = bi.GetPixel(j, i).ToArgb();//bi.getRGB(j, i);

                    }

                foreach (Rang rang in rangList)
                {
                    //domen = new int[r, r];
                    R = r / rang.getK();
                    D = R * 2;
                    domen = new int[R,R];

                    //выделяем доменный блок
                    for (int i = 0; i < D; i++)
                      for (int j = 0; j < D; j++)
                        {
                            //color = new Color(bi.getRGB(rang.getX() + j, rang.getY() + i));
                            //int f = color.getRed();
                            //domenBig[i][j] = f;//bi.getRGB(rang.getX() + j, rang.getY() + i);
                            //domenBig[i][j] = bi.getRGB(rang.getX() + j, rang.getY() + i);
                            //domenBig[i, j] = bitTest.GetPixel(j, i).ToArgb();

                            domenBig[i, j] = bi.GetPixel(rang.getX() + j, rang.getY() + i).ToArgb();
                            //domenBig[i, j] = bi.GetPixel(rang.getX() + i, rang.getY() + j).ToArgb();
                            //domenBig[j, i] = pixels[rang.getY() + j, rang.getX() + i];

                            //bitmap.SetPixel(width + 5 + i, 5 + j, Color.FromArgb(pix[rang.getY() + j, rang.getX() + i]));
                            //domen[j, i] = pix[rang.getY() + j, rang.getX() + i];
                        }

                    //pixels[i, j] = bitStart.GetPixel(j, i).ToArgb();
                    //domen[j, i] = pix[rang.getY() + j, rang.getX() + i];
                    //bitmap.SetPixel(rang.getX() + i, rang.getY() + j, Color.FromArgb(pix[rang.getY() + j, rang.getX() + i]));

                    int d = 0, sum = 0;
                    //и уменьшаем его усреднением
                    domen = reduceBlock(domenBig);

                    //афинное преобразование
                    domen = setAfinnInt(domen, rang.getAfinn());

                    //преобразование яркости
                    //domen = changeBright(domen, rang.getBright());
                    domen = changeBright(domen, rang.getS(), rang.getO());

                    //;
                    for (int i = 0; i < R; i++)
                        for (int j = 0; j < R; j++)
                        {
                            //bi.setRGB(rang.getX0() + j, rang.getY0() + i, domen[i, j] + (domen[i, j] << 8) + (domen[i, j] << 16));
                            color = Color.FromArgb(domen[i, j]);
                            bi.SetPixel(rang.getX0() + j, rang.getY0() + i,color);
                            //color = Color.FromArgb(domen[j, i]);
                            //bi.SetPixel(rang.getX0() + i, rang.getY0() + j, color);
                            //pixels[i, j] = bi.GetPixel(j, i).ToArgb();

                            //bitmap.SetPixel(rang.getX0() + i, rang.getY0() + j, Color.FromArgb(pix[rang.getY0() + j, rang.getX0() + i]));
                            //rangMatr[j, i] = pix[rang.getY0() + j, rang.getX0() + i];

                        }
                }
                printDecompression(g);
            }
            //
            //Color color = new Color(domen[0][0]);
            //System.out.println(""+color.getRed()+" "+color.getGreen()+" "+color.getBlue());


            return bi;
        }

        public int[,] setAfinnInt(int[,] pix, int k)
        {
            //int argb;
            int n = pix.GetLength(0);
            int x, y;
            int[,] p = new int[n, n];

            if (k < 4)
            {

                if (k == 1)
                {//поворот на 90

                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                        {
                            x = n - 1 - i;
                            y = j;
                            p[y, x] = pix[i, j];
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
                            p[y, x] = pix[i, j];
                        }
                }
                else if (k == 3)
                {//поворот на 270
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                        {
                            x = i;
                            y = n - 1 - j;
                            p[y, x] = pix[i, j];
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
                            p[y, x] = pix[i, j];
                        }
                }
                else if (k == 5)
                {//отражение по горизонтали
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                        {
                            x = j;
                            y = n - 1 - i;
                            p[y, x] = pix[i, j];
                        }
                }
                else if (k == 6)//k=1 + k=4
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

        public int[,] changeBright(int[,] pix, double k)
        {
            int n = pix.GetLength(0);
            int x;
            int[,] p = new int[n, n];
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
                    //x = (color.R-o) / s;
                    x = s * color.R + o;
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

        public int[,] reduceBlock(int[,] blockBig)
        {
            int n = blockBig.GetLength(0);
            int[,] block = new int[n / 2, n / 2];
            Color color;// = new Color(domen[i][j]);
            int d = 0, sum;
            for (int i = 0; i < n; i = i + 2)
                for (int j = 0; j < n; j = j + 2)
                {
                    sum = 0;

                    //color = Color.FromArgb(blockBig[i + ii, j + jj]);
                    sum += Color.FromArgb(blockBig[i, j]).R;
                    sum += Color.FromArgb(blockBig[i + 1, j]).R;
                    sum += Color.FromArgb(blockBig[i, j + 1]).R;
                    sum += Color.FromArgb(blockBig[i + 1, j + 1]).R;

                    //d = (int)(sum / Math.Pow(4, k));
                    d = (int)(sum / 4);


                    color = Color.FromArgb(d, d, d);
                    block[i / 2, j / 2] = color.ToArgb();
                    //block[i / (2 * k), j / (2 * k)] = color.ToArgb();
                }
            return block;
        }

        public void printDecompression(int k)
        { 
            try
            {

                bi.Save("D:\\университет\\диплом\\bloks\\Decompression_" + (k+1) + ".jpg");
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
