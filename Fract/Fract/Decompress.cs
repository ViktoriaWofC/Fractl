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

        private List<Rang> rangListR = new List<Rang>();
        private List<Rang> rangListG = new List<Rang>();
        private List<Rang> rangListB = new List<Rang>();

        private List<Rang> rangListY = new List<Rang>();
        private List<Rang> rangListI = new List<Rang>();
        private List<Rang> rangListQ = new List<Rang>();

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

        public Decompress(List<Rang> rangListR, List<Rang> rangListG, List<Rang> rangListB, List<Rang> rangListY, List<Rang> rangListI, List<Rang> rangListQ, Bitmap bi, int r)//Decompress(List<Rang> rangList, BufferedImage bi, int r)
        {
            this.rangListR = rangListR;
            this.rangListG = rangListG;
            this.rangListB = rangListB;
            this.rangListY = rangListY;
            this.rangListI = rangListI;
            this.rangListQ = rangListQ;
            this.r = r;
            this.bi = bi;
            this.width = bi.Width;
            this.height = bi.Height;
            //this.m = width/r;
            //this.n = height/r;
        }

        public Bitmap decompressImage(int k, string imageColor)
        {
            if (imageColor.Equals("gray"))
                return decompressImage(k);
            else if (imageColor.Equals("rgb"))
                return decompressImageColorRGB(k);
            else if (imageColor.Equals("yiq"))
                return decompressImageColorYIQ(k);
            else return decompressImage(k);
        }

        public Bitmap decompressImage(int k)
        {
            int[,] domen;// = new int[r, r];
            int[,] domenBig;// = new int[r * 2, r * 2];
            int R, D;

            Color color;
            int tk = 0;

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

                tk = 0;
                foreach (Rang rang in rangList)
                {    

                    //domen = new int[r, r];
                    R = r / rang.getK();
                    D = R * 2;
                    domen = new int[R,R];
                    domenBig = new int[D,D];

                    //выделяем доменный блок
                    for (int i = 0; i < D; i++)
                      for (int j = 0; j < D; j++)
                        {
                            domenBig[i, j] = bi.GetPixel(rang.getX() + j, rang.getY() + i).ToArgb();
                        }


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
                            color = Color.FromArgb(domen[i, j]);
                            bi.SetPixel(rang.getX0() + j, rang.getY0() + i,color);
                        }

                    tk++;
                }
                printDecompression(g);
            }

            return bi;
        }

        public Bitmap decompressImageColorRGB(int k)
        {
            int[,] domen_Red, domenBig_Red;
            int[,] domen_Green, domenBig_Green;
            int[,] domen_Blue, domenBig_Blue;
            int R, D;

            Color color;
            int tk = 0;

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

                tk = 0;                

                for (int l = 0; l < rangListR.Count; l++)
                {
                    //domen = new int[r, r];
                    R = r / rangListR[l].getK();
                    D = R * 2;
                    domen_Red = new int[R, R];
                    domenBig_Red = new int[D, D];
                    int d = 0, sum = 0;

                    //выделяем доменный блок
                    for (int i = 0; i < D; i++)
                        for (int j = 0; j < D; j++)
                        {
                            domenBig_Red[i, j] = bi.GetPixel(rangListR[l].getX() + j, rangListR[l].getY() + i).ToArgb();
                        }
                    //и уменьшаем его усреднением
                    domen_Red = reduceBlockColor(domenBig_Red);

                    //афинное преобразование
                    domen_Red = setAfinnInt(domen_Red, rangListR[l].getAfinn());

                    //преобразование яркости - получаем компоненту одного цвета
                    //domen = changeBright(domen, rang.getBright());
                    domen_Red = changeBrightColorRGB(domen_Red, rangListR[l].getS(), rangListR[l].getO(), "R");

                    //;
                    for (int i = 0; i < R; i++)
                        for (int j = 0; j < R; j++)
                        {
                            color = Color.FromArgb(domen_Red[i, j],
                                bi.GetPixel(rangListR[l].getX0() + j, rangListR[l].getY0() + i).G,
                                bi.GetPixel(rangListR[l].getX0() + j, rangListR[l].getY0() + i).B);
                            bi.SetPixel(rangListR[l].getX0() + j, rangListR[l].getY0() + i, color);
                        }

                    tk++;
                }
                for (int l = 0; l < rangListG.Count; l++)
                {

                    //domen = new int[r, r];
                    R = r / rangListG[l].getK();
                    D = R * 2;
                    domen_Green = new int[R, R];
                    domenBig_Green = new int[D, D];

                    int d = 0, sum = 0;

                    //выделяем доменный блок
                    for (int i = 0; i < D; i++)
                        for (int j = 0; j < D; j++)
                        {
                            domenBig_Green[i, j] = bi.GetPixel(rangListG[l].getX() + j, rangListG[l].getY() + i).ToArgb();
                        }
                    //и уменьшаем его усреднением
                    domen_Green = reduceBlockColor(domenBig_Green);

                    //афинное преобразование
                    domen_Green = setAfinnInt(domen_Green, rangListG[l].getAfinn());

                    //преобразование яркости - получаем компоненту одного цвета
                    domen_Green = changeBrightColorRGB(domen_Green, rangListG[l].getS(), rangListG[l].getO(), "G");

                    //;
                    for (int i = 0; i < R; i++)
                        for (int j = 0; j < R; j++)
                        {
                            color = Color.FromArgb(bi.GetPixel(rangListG[l].getX0() + j, rangListG[l].getY0() + i).R,
                                domen_Green[i, j],
                                bi.GetPixel(rangListG[l].getX0() + j, rangListG[l].getY0() + i).B);
                            bi.SetPixel(rangListG[l].getX0() + j, rangListG[l].getY0() + i, color);
                        }

                    tk++;
                }
                for (int l = 0; l < rangListB.Count; l++)
                {

                    //domen = new int[r, r];
                    R = r / rangListB[l].getK();
                    D = R * 2;
                    domen_Blue = new int[R, R];
                    domenBig_Blue = new int[D, D];

                    int d = 0, sum = 0;

                    //выделяем доменный блок
                    for (int i = 0; i < D; i++)
                        for (int j = 0; j < D; j++)
                        {
                            domenBig_Blue[i, j] = bi.GetPixel(rangListB[l].getX() + j, rangListB[l].getY() + i).ToArgb();
                        }
                    //и уменьшаем его усреднением
                    domen_Blue = reduceBlockColor(domenBig_Blue);

                    //афинное преобразование
                    domen_Blue = setAfinnInt(domen_Blue, rangListB[l].getAfinn());

                    //преобразование яркости - получаем компоненту одного цвета
                    //domen = changeBright(domen, rang.getBright());
                    domen_Blue = changeBrightColorRGB(domen_Blue, rangListB[l].getS(), rangListB[l].getO(), "B");

                    //;
                    for (int i = 0; i < R; i++)
                        for (int j = 0; j < R; j++)
                        {
                            color = Color.FromArgb(bi.GetPixel(rangListB[l].getX0() + j, rangListB[l].getY0() + i).R,
                                bi.GetPixel(rangListB[l].getX0() + j, rangListB[l].getY0() + i).G,
                                domen_Blue[i, j]);
                            bi.SetPixel(rangListB[l].getX0() + j, rangListB[l].getY0() + i, color);
                        }

                    tk++;
                }
                printDecompression(g);
                
            }

            return bi;
        }

        public Bitmap decompressImageColorYIQ(int k)
        {
            int[,] domen_Y, domenBig_Y;
            int[,] domen_I, domenBig_I;
            int[,] domen_Q, domenBig_Q;
            int R, D;

            Color color;
            int tk = 0;

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

                tk = 0;                
                for (int l = 0; l < rangListY.Count; l++)
                {

                    //domen = new int[r, r];
                    R = r / rangListY[l].getK();
                    D = R * 2;
                    domen_Y = new int[R, R];
                    domenBig_Y = new int[D, D];

                    int d = 0, sum = 0;

                    //выделяем доменный блок
                    for (int i = 0; i < D; i++)
                        for (int j = 0; j < D; j++)
                        {
                            domenBig_Y[i, j] = bi.GetPixel(rangListY[l].getX() + j, rangListY[l].getY() + i).ToArgb();
                        }
                    //и уменьшаем его усреднением
                    domen_Y = reduceBlockColor(domenBig_Y);

                    //афинное преобразование
                    domen_Y = setAfinnInt(domen_Y, rangListY[l].getAfinn());

                    //преобразование яркости - получаем компоненту одного цвета
                    //domen = changeBright(domen, rang.getBright());
                    double[,] d_domen_Y = changeBrightColorYIQ(domen_Y, rangListY[l].getS(), rangListY[l].getO(), "Y");

                    //;
                    for (int i = 0; i < R; i++)
                        for (int j = 0; j < R; j++)
                        {
                            int Red = Convert.ToInt32(d_domen_Y[i, j]
                                + 0.956 * getI(bi.GetPixel(rangListY[l].getX0() + j, rangListY[l].getY0() + i))
                                + 0.623 * getQ(bi.GetPixel(rangListY[l].getX0() + j, rangListY[l].getY0() + i)));
                            int Green = Convert.ToInt32(d_domen_Y[i, j] 
                                - 0.272 * getI(bi.GetPixel(rangListY[l].getX0() + j, rangListY[l].getY0() + i))
                                - 0.648 * getQ(bi.GetPixel(rangListY[l].getX0() + j, rangListY[l].getY0() + i)));
                            int Blue = Convert.ToInt32(d_domen_Y[i, j] 
                                - 1.105 * getI(bi.GetPixel(rangListY[l].getX0() + j, rangListY[l].getY0() + i))
                                + 1.705 * getQ(bi.GetPixel(rangListY[l].getX0() + j, rangListY[l].getY0() + i)));

                            if (Red < 0) Red = 0;
                            if (Green < 0) Green = 0;
                            if (Blue < 0) Blue = 0;
                            if (Red > 255) Red = 255;
                            if (Green > 255) Green = 255;
                            if (Blue > 255) Blue = 255;

                            color = Color.FromArgb(Red, Green, Blue);
                            bi.SetPixel(rangListY[l].getX0() + j, rangListY[l].getY0() + i, color);
                        }

                    tk++;
                }
                for (int l = 0; l < rangListI.Count; l++)
                {

                    //domen = new int[r, r];
                    R = r / rangListI[l].getK();
                    D = R * 2;
                    domen_I = new int[R, R];
                    domenBig_I = new int[D, D];

                    int d = 0, sum = 0;

                    //выделяем доменный блок
                    for (int i = 0; i < D; i++)
                        for (int j = 0; j < D; j++)
                        {
                            domenBig_I[i, j] = bi.GetPixel(rangListI[l].getX() + j, rangListI[l].getY() + i).ToArgb();
                        }
                    //и уменьшаем его усреднением
                    domen_I = reduceBlockColor(domenBig_I);

                    //афинное преобразование
                    domen_I = setAfinnInt(domen_I, rangListI[l].getAfinn());

                    //преобразование яркости - получаем компоненту одного цвета
                    //domen = changeBright(domen, rang.getBright());
                    double[,] d_domen_I = changeBrightColorYIQ(domen_I, rangListI[l].getS(), rangListI[l].getO(), "I");

                    //;
                    for (int i = 0; i < R; i++)
                        for (int j = 0; j < R; j++)
                        {
                            int Red = Convert.ToInt32(getY(bi.GetPixel(rangListI[l].getX0() + j, rangListI[l].getY0() + i))
                                + 0.956 * d_domen_I[i, j] 
                                + 0.623 * getQ(bi.GetPixel(rangListI[l].getX0() + j, rangListI[l].getY0() + i)));
                            int Green = Convert.ToInt32(getY(bi.GetPixel(rangListI[l].getX0() + j, rangListI[l].getY0() + i))
                                - 0.272 * d_domen_I[i, j] 
                                - 0.648 * getQ(bi.GetPixel(rangListI[l].getX0() + j, rangListI[l].getY0() + i)));
                            int Blue = Convert.ToInt32(getY(bi.GetPixel(rangListI[l].getX0() + j, rangListI[l].getY0() + i))
                                - 1.105 * d_domen_I[i, j] 
                                + 1.705 * getQ(bi.GetPixel(rangListI[l].getX0() + j, rangListI[l].getY0() + i)));

                            if (Red < 0) Red = 0;
                            if (Green < 0) Green = 0;
                            if (Blue < 0) Blue = 0;
                            if (Red > 255) Red = 255;
                            if (Green > 255) Green = 255;
                            if (Blue > 255) Blue = 255;

                            color = Color.FromArgb(Red, Green, Blue);
                            bi.SetPixel(rangListI[l].getX0() + j, rangListI[l].getY0() + i, color);
                        }

                    tk++;
                }
                for (int l = 0; l < rangListQ.Count; l++)
                {

                    //domen = new int[r, r];
                    R = r / rangListQ[l].getK();
                    D = R * 2;
                    domen_Q = new int[R, R];
                    domenBig_Q = new int[D, D];

                    int d = 0, sum = 0;

                    //выделяем доменный блок
                    for (int i = 0; i < D; i++)
                        for (int j = 0; j < D; j++)
                        {
                            domenBig_Q[i, j] = bi.GetPixel(rangListQ[l].getX() + j, rangListQ[l].getY() + i).ToArgb();
                        }
                    //и уменьшаем его усреднением
                    domen_Q = reduceBlockColor(domenBig_Q);

                    //афинное преобразование
                    domen_Q = setAfinnInt(domen_Q, rangListQ[l].getAfinn());

                    //преобразование яркости - получаем компоненту одного цвета
                    //domen = changeBright(domen, rang.getBright());
                    double[,] d_domen_Q = changeBrightColorYIQ(domen_Q, rangListQ[l].getS(), rangListQ[l].getO(), "Q");

                    //;
                    for (int i = 0; i < R; i++)
                        for (int j = 0; j < R; j++)
                        {
                            int Red = Convert.ToInt32(getY(bi.GetPixel(rangListQ[l].getX0() + j, rangListQ[l].getY0() + i))
                                + 0.956 * getI(bi.GetPixel(rangListQ[l].getX0() + j, rangListQ[l].getY0() + i))
                                + 0.623 * d_domen_Q[i, j]);
                            int Green = Convert.ToInt32(getY(bi.GetPixel(rangListQ[l].getX0() + j, rangListQ[l].getY0() + i))
                                - 0.272 * getI(bi.GetPixel(rangListQ[l].getX0() + j, rangListQ[l].getY0() + i))
                                - 0.648 * d_domen_Q[i, j]);
                            int Blue = Convert.ToInt32(getY(bi.GetPixel(rangListQ[l].getX0() + j, rangListQ[l].getY0() + i))
                                - 1.105 * getI(bi.GetPixel(rangListQ[l].getX0() + j, rangListQ[l].getY0() + i))
                                + 1.705 * d_domen_Q[i, j]);

                            if (Red < 0) Red = 0;
                            if (Green < 0) Green = 0;
                            if (Blue < 0) Blue = 0;
                            if (Red > 255) Red = 255;
                            if (Green > 255) Green = 255;
                            if (Blue > 255) Blue = 255;

                            color = Color.FromArgb(Red, Green, Blue);
                            bi.SetPixel(rangListQ[l].getX0() + j, rangListQ[l].getY0() + i, color);
                        }

                    tk++;
                }
                printDecompression(g);
            }

            return bi;
        }

        /*for (int l = 0; l<rangListY.Count; l++)
                {

                    //domen = new int[r, r];
                    R = r / rangListY[l].getK();
        D = R* 2;
                    domen_Y = new int[R, R];
                    domenBig_Y = new int[D, D];
                    domen_I = new int[R, R];
                    domenBig_I = new int[D, D];
                    domen_Q = new int[R, R];
                    domenBig_Q = new int[D, D];

                    int d = 0, sum = 0;

                    //выделяем доменный блок
                    for (int i = 0; i<D; i++)
                        for (int j = 0; j<D; j++)
                        {
                            domenBig_Y[i, j] = bi.GetPixel(rangListY[l].getX() + j, rangListY[l].getY() + i).ToArgb();
        domenBig_I[i, j] = bi.GetPixel(rangListI[l].getX() + j, rangListI[l].getY() + i).ToArgb();
        domenBig_Q[i, j] = bi.GetPixel(rangListQ[l].getX() + j, rangListQ[l].getY() + i).ToArgb();
    }
    //и уменьшаем его усреднением
    domen_Y = reduceBlockColor(domenBig_Y);
    domen_I = reduceBlockColor(domenBig_I);
    domen_Q = reduceBlockColor(domenBig_Q);

    //афинное преобразование
    domen_Y = setAfinnInt(domen_Y, rangListY[l].getAfinn());
                    domen_I = setAfinnInt(domen_I, rangListI[l].getAfinn());
                    domen_Q = setAfinnInt(domen_Q, rangListQ[l].getAfinn());

                    //преобразование яркости - получаем компоненту одного цвета
                    //domen = changeBright(domen, rang.getBright());
                    double[,] d_domen_Y = changeBrightColorYIQ(domen_Y, rangListY[l].getS(), rangListY[l].getO(), "Y");
    double[,] d_domen_I = changeBrightColorYIQ(domen_I, rangListI[l].getS(), rangListI[l].getO(), "I");
    double[,] d_domen_Q = changeBrightColorYIQ(domen_Q, rangListQ[l].getS(), rangListQ[l].getO(), "Q");

                    //;
                    for (int i = 0; i<R; i++)
                        for (int j = 0; j<R; j++)
                        {
                            int Red = Convert.ToInt32(d_domen_Y[i, j] + 0.956 * d_domen_I[i, j] + 0.623 * d_domen_Q[i, j]);
    int Green = Convert.ToInt32(d_domen_Y[i, j] - 0.272 * d_domen_I[i, j] - 0.648 * d_domen_Q[i, j]);
    int Blue = Convert.ToInt32(d_domen_Y[i, j] - 1.105 * d_domen_I[i, j] + 1.705 * d_domen_Q[i, j]);

                            if (Red< 0) Red = 0;
                            if (Green< 0) Green = 0;
                            if (Blue< 0) Blue = 0;
                            if (Red > 255) Red = 255;
                            if (Green > 255) Green = 255;
                            if (Blue > 255) Blue = 255;

                            color = Color.FromArgb(Red, Green, Blue);
                            bi.SetPixel(rangListY[l].getX0() + j, rangListY[l].getY0() + i, color);
                        }

tk++;
                }     */

        public int[,] reduceBlockColor(int[,] blockBig)
        {
            int n = blockBig.GetLength(0);
            int[,] block = new int[n / 2, n / 2];
            Color color;// = new Color(domen[i][j]);
            int colR = 0, colG = 0, colB = 0, sumR, sumG, sumB;
            for (int i = 0; i < n; i = i + 2)
                for (int j = 0; j < n; j = j + 2)
                {
                    sumR = 0;
                    sumR += Color.FromArgb(blockBig[i, j]).R;
                    sumR += Color.FromArgb(blockBig[i + 1, j]).R;
                    sumR += Color.FromArgb(blockBig[i, j + 1]).R;
                    sumR += Color.FromArgb(blockBig[i + 1, j + 1]).R;
                    colR = (int)(sumR / 4);

                    sumG = 0;
                    sumG += Color.FromArgb(blockBig[i, j]).G;
                    sumG += Color.FromArgb(blockBig[i + 1, j]).G;
                    sumG += Color.FromArgb(blockBig[i, j + 1]).G;
                    sumG += Color.FromArgb(blockBig[i + 1, j + 1]).G;
                    colG = (int)(sumG / 4);

                    sumB = 0;
                    sumB += Color.FromArgb(blockBig[i, j]).B;
                    sumB += Color.FromArgb(blockBig[i + 1, j]).B;
                    sumB += Color.FromArgb(blockBig[i, j + 1]).B;
                    sumB += Color.FromArgb(blockBig[i + 1, j + 1]).B;
                    colB = (int)(sumB / 4);

                    color = Color.FromArgb(colR, colG, colB);
                    block[i / 2, j / 2] = color.ToArgb();
                    //block[i / (2 * k), j / (2 * k)] = color.ToArgb();
                }
            return block;
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

        public int[,] changeBrightColorRGB(int[,] pix, double s, double o, string imageColor)
        {
            int n = pix.GetLength(0);
            double x;
            int[,] p = new int[n, n];
            Color color;
            bool red = false;
            bool green = false;
            bool blue = false;

            if (imageColor.Equals("R"))
                red = true;
            else if (imageColor.Equals("G"))
                green = true;
            else if (imageColor.Equals("B"))
                blue = true;

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    color = Color.FromArgb(pix[i, j]);
                    if(red) x = s * color.R + o;
                    else if(green) x = s * color.G + o;
                    else if (blue) x = s * color.B + o;
                    else x = s * color.R + o;

                    if (x > 255)
                        x = 255;
                    if (x < 0)
                        x = 0;

                    //Color newColor;
                    //if (red) newColor = Color.FromArgb((int)x, color.G, color.B);
                    //else if (green) newColor = Color.FromArgb(color.R, (int)x, color.B);
                    //else newColor = Color.FromArgb(color.R, color.G, (int)x);
                    //else color = Color.FromArgb((int)x, (int)x, (int)x);


                    //p[i, j] = newColor.ToArgb();
                    p[i, j] = (int)x;
                }
            return p;
        }

        public double[,] changeBrightColorYIQ(int[,] pix, double s, double o, string imageColor)
        {
            int n = pix.GetLength(0);
            double x;
            double[,] p = new double[n, n];
            Color color;
            bool Y = false;
            bool I = false;
            bool Q = false;

            if (imageColor.Equals("Y"))
                Y = true;
            else if (imageColor.Equals("I"))
                I = true;
            else if (imageColor.Equals("Q"))
                Q = true;

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    color = Color.FromArgb(pix[i, j]);
                    if (Y) x = s * getY(color) + o;
                    else if (I) x = s * getI(color) + o;
                    else if (Q) x = s * getQ(color) + o;
                    else x = s * color.R + o;

                    if (x > 255)
                        x = 255;
                    if (x < 0)
                        x = 0;

                    //if (red) color = Color.FromArgb((int)x, 0, 0);
                    //else if (green) color = Color.FromArgb(0, (int)x, 0);
                    //else if (blue) color = Color.FromArgb(0, 0, (int)x);
                    //else color = Color.FromArgb((int)x, (int)x, (int)x);


                    //p[i, j] = color.ToArgb();
                    p[i, j] = x;
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

        public double getY(Color color)
        {
            return 0.299 * color.R + 0.587 * color.G + 0.114 * color.B;
        }

        public double getI(Color color)
        {
            return 0.596 * color.R - 0.274 * color.G - 0.322 * color.B;
        }

        public double getQ(Color color)
        {
            return 0.211 * color.R - 0.522 * color.G + 0.311 * color.B;
        }

        public void printBlock(Rang rang, int k, int g)
        {
            int m = bi.Width;
            int n = bi.Height;
            int[,] pix = new int[n, m];
            //получаем массив интовых чисел из изображения
            for (int i = 0; i < n; i++)//строки
                for (int j = 0; j < m; j++)//столбцы
                {
                    pix[i, j] = bi.GetPixel(j, i).ToArgb();//bi.getRGB(j, i);

                }

            int otst = r * 2 + 10;
            int width = pix.GetLength(1);
            Bitmap bitmap = new Bitmap(width + otst, pix.GetLength(0));
            Color color;// = Color.White;
            int R = r / rang.getK();//размер рангового блока
            int D = R * 2;//размер доменного блока
            int[,] rangMatr = new int[R, R];
            //bi.SetPixel(rang.getX0() + j, rang.getY0() + i, color);

            for (int i = 0; i < bitmap.Width - otst; i++)
                for (int j = 0; j < bitmap.Height; j++)
                {
                    if (i % r != 0 && j % r != 0)
                        bitmap.SetPixel(i, j, Color.White);
                    else bitmap.SetPixel(i, j, Color.Black);
                }

            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(rang.getX0() + i, rang.getY0() + j, Color.FromArgb(pix[rang.getY0() + j, rang.getX0() + i]));
                    rangMatr[j, i] = pix[rang.getY0() + j, rang.getX0() + i];
                }

            for (int i = 0; i < D; i++)
                for (int j = 0; j < D; j++)
                {
                    bitmap.SetPixel(rang.getX() + i, rang.getY() + j, Color.FromArgb(pix[rang.getY() + j, rang.getX() + i]));
                }

            //////////////////////////////////////////
            int[,] domen = new int[D, D], domenAfin, domenBright;
            int[,] domenMin = new int[R, R];
            for (int i = 0; i < D; i++)
                for (int j = 0; j < D; j++)
                {
                    bitmap.SetPixel(width + 5 + i, 5 + j, Color.FromArgb(pix[rang.getY() + j, rang.getX() + i]));
                    domen[j, i] = pix[rang.getY() + j, rang.getX() + i];
                }

            domenMin = reduceBlock(domen);
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(width + 5 + i, (5 + R * 2 + 10) + j, Color.FromArgb(domenMin[j, i]));
                }

            domenAfin = setAfinnInt(domenMin, rang.getAfinn());
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(width + 5 + i, (5 + R * 3 + 20) + j, Color.FromArgb(domenAfin[j, i]));
                }

            domenBright = changeBright(domenAfin, rang.getS(), rang.getO());
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(width + 5 + i, (5 + R * 5 + 30) + j, Color.FromArgb(domenBright[j, i]));
                }

            //rang block
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(width + 5 + i, (5 + R * 6 + 40) + j, Color.FromArgb(rangMatr[j, i]));
                }

            //difference
            int argbcolor;
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    //argbcolor = Math.Abs(Color.FromArgb(rangMatr[i, j]).R - Color.FromArgb(domenBright[i, j]).R);
                    argbcolor = Math.Abs(Color.FromArgb(domenBright[j, i]).R - Color.FromArgb(rangMatr[j, i]).R);
                    bitmap.SetPixel(width + 5 + i, (5 + R * 7 + 50) + j, Color.FromArgb(argbcolor, argbcolor, argbcolor));
                }

            try
            {
                String name = k + "___k=" + rang.getK() + "__decompress__g="+g+"__print";
                bitmap.Save("D:\\университет\\диплом\\bloks\\" + name + ".jpg");
                //Button5.Text = "Saved file.";
            }
            catch (Exception)
            {
                //MessageBox.Show("There was a problem saving the file." +
                //"Check the file permissions.");
            }


        }

        public void printBlock(Rang rang, int[,] domen, int k, int g,string tag)
        {
            int m = bi.Width;
            int n = bi.Height;
            int[,] pix = new int[n, m];
            //получаем массив интовых чисел из изображения
            for (int i = 0; i < n; i++)//строки
                for (int j = 0; j < m; j++)//столбцы
                {
                    pix[i, j] = bi.GetPixel(j, i).ToArgb();//bi.getRGB(j, i);
                }

            int otst = r * 2 + 10;
            //int width = pix.GetLength(1);
            Bitmap bitmap = new Bitmap(width + otst, pix.GetLength(0));
            Color color;// = Color.White;
            int R = r / rang.getK();//размер рангового блока
            //int D = R * 2;//размер доменного блока
            //int[,] rangMatr = new int[R, R];
            //bi.SetPixel(rang.getX0() + j, rang.getY0() + i, color);

            for (int i = 0; i < bitmap.Width - otst; i++)
                for (int j = 0; j < bitmap.Height; j++)
                {
                    if (i % r != 0 && j % r != 0)
                        bitmap.SetPixel(i, j, Color.White);
                    else bitmap.SetPixel(i, j, Color.Black);
                }

            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    //bitmap.SetPixel(rang.getX0() + i, rang.getY0() + j, Color.FromArgb(rangMatr[j, i]));
                    bitmap.SetPixel(rang.getX0() + i, rang.getY0() + j, Color.FromArgb(pix[rang.getY0() + j, rang.getX0() + i]));
                    //rangMatr[j, i] = pix[rang.getY0() + j, rang.getX0() + i];
                }


            for (int i = 0; i < domen.GetLength(0); i++)
                for (int j = 0; j < domen.GetLength(0); j++)
                {
                    //bitmap.SetPixel(width + 5 + i, 5 + j, Color.FromArgb(domen[j, i]));
                    bitmap.SetPixel(rang.getX() + i, rang.getY() + j, Color.FromArgb(domen[j, i]));
                    //bitmap.SetPixel(width + 5 + i, 5 + j, Color.FromArgb(pix[rang.getY() + j, rang.getX() + i]));
                    //domen[j, i] = pix[rang.getY() + j, rang.getX() + i];
                }



            try
            {
                String name = k + "___k=" + rang.getK() + "__decompress__g=" + g + "__tag="+tag;
                bitmap.Save("D:\\университет\\диплом\\bloks\\" + name + ".jpg");
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
