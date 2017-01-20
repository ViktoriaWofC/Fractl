﻿using System;
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
            int[,] domen = new int[r, r];
            int[,] domenBig = new int[r * 2, r * 2];

            Color color;

            for (int g = 0; g < k; g++)
            {
                foreach (Rang rang in rangList)
                {
                    //выделяем доменный блок
                    for (int i = 0; i < r * 2; i++)
                        for (int j = 0; j < r * 2; j++)
                        {
                            //color = new Color(bi.getRGB(rang.getX() + j, rang.getY() + i));
                            //int f = color.getRed();
                            //domenBig[i][j] = f;//bi.getRGB(rang.getX() + j, rang.getY() + i);
                            //domenBig[i][j] = bi.getRGB(rang.getX() + j, rang.getY() + i);
                            domenBig[i, j] = bi.GetPixel(rang.getX() + j, rang.getY() + i).ToArgb(); 
                        }

                    int d = 0, sum = 0;
                    //и уменьшаем его усреднением
                    for (int i = 0; i < r; i++)
                        for (int j = 0; j < r; j++)
                        {
                            sum = 0;
                            for (int ii = 0; ii < 2 * rang.getK(); ii++)
                                for (int jj = 0; jj < 2 * rang.getK(); jj++)
                                {
                                    //color = new Color(domenBig[i * 2 * rang.getK() + ii][j * 2 * rang.getK() + jj]);
                                    //sum += color.getRed();
                                    sum += domenBig[i * 2 * rang.getK() + ii, j * 2 * rang.getK() + jj];
                                }


                            d = (int)(sum / Math.Pow(4, rang.getK()));

                            //color = new Color(d,d,d);
                            //domen[i][j] = color.getRGB();
                            domen[i, j] = d;
                        }

                    domen = setAfinnInt(domen, rang.getK());

                    //преобразование яркости

                    //;
                    for (int i = 0; i < r; i++)
                        for (int j = 0; j < r; j++)
                        {
                            //bi.setRGB(rang.getX0() + j, rang.getY0() + i, domen[i, j] + (domen[i, j] << 8) + (domen[i, j] << 16));
                            color = Color.FromArgb(domen[i, j]);
                            bi.SetPixel(rang.getX0() + j, rang.getY0() + i,color);

                            //bi.setRGB(rang.getX0()+j,rang.getY0()+i,domen[i][j]);

                            //

                            /*int dd = domen[i][j];//color.getRed();
                            if(dd<0)
                                dd = 0;
                            else if(dd>255)
                                dd = 255;
                            color = new Color(dd,dd,dd);
                            bi.setRGB(rang.getX0()+j,rang.getY0() + i,color.getRGB());*/

                            //color = new Color(domen[i][j]);
                            //int red = color.getRed();
                            //int green = color.getGreen();
                            //int blue = color.getBlue();
                            //bi.setRGB(rang.getX0()+j,rang.getY0() + i,color.getRGB());


                            //bi.setRGB(rang.getX0()+j,rang.getY0() + i,blue + (green << 8) + (red << 16));
                            //bi.setRGB(rang.getX0()+j,rang.getY0() + i,color.getRGB() + (color.getRGB() << 8) + (color.getRGB() << 16));

                        }
                }
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
            }

            if (k == 0)
                return pix;
            else return p;
        }
    }
}