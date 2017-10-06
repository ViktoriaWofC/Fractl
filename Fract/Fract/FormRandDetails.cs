using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fract
{
    public partial class FormRandDetails : Form
    {
        public FormRandDetails()
        {
            InitializeComponent();
        }

        public FormRandDetails(Rang rang, int[,] pix, int r, double epsilon)
        {
            InitializeComponent();
            this.rang = rang;
            this.pix = pix;
            this.r = r;
            this.epsilon = epsilon;
            setParameters();
        }

        Rang rang;
        int[,] pix;
        int r;
        double epsilon;

        public void setParameters()
        {
            Bitmap bitmap = new Bitmap(35,35);
            Color color;// = Color.White;
            int R = r / rang.getK();//размер рангового блока
            int D = R * 2;//размер доменного блока
            int[,] rangMatr = new int[R, R];
            int[,] domen = new int[D, D], domenAfin, domenBright;
            int[,] domenMin = new int[R, R];
                       

            for (int i = 0; i < D; i++)
                for (int j = 0; j < D; j++)
                {
                    bitmap.SetPixel(i,j, Color.FromArgb(pix[rang.getX() + i, rang.getY() + j]));
                    domen[i, j] = pix[rang.getX() + i, rang.getY() + j];
                }
            pictureBoxDomen.Image = bitmap;

            //////////////////////////////////////////            

            bitmap = new Bitmap(35, 35);
            domenMin = reduceBlock(domen);
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(i, j, Color.FromArgb(domenMin[i, j]));
                }
            pictureBoxDomenReduce.Image = bitmap;

            bitmap = new Bitmap(35, 35);
            domenAfin = setAfinnInt(domenMin, rang.getAfinn());
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(i, j, Color.FromArgb(domenAfin[i, j]));
                }
            pictureBoxDomenAfin.Image = bitmap;

            bitmap = new Bitmap(35, 35);
            domenBright = changeBright(domenAfin, rang.getS(), rang.getO());
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(i, j, Color.FromArgb(domenBright[i, j]));
                }
            pictureBoxDomenBright.Image = bitmap;

            //rang
            bitmap = new Bitmap(35, 35);
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(i, j, Color.FromArgb(pix[rang.getX0() + i, rang.getY0() + j]));
                    rangMatr[i, j] = pix[rang.getX0() + i, rang.getY0() + j];
                }
            pictureBoxRang.Image = bitmap;

            //difference
            bitmap = new Bitmap(35, 35);
            int argbcolor;
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    argbcolor = Math.Abs(Color.FromArgb(domenBright[i, j]).R - Color.FromArgb(rangMatr[i, j]).R);
                    bitmap.SetPixel(i, j, Color.FromArgb(argbcolor, argbcolor, argbcolor));
                }
            pictureBoxDifference.Image = bitmap;

            /////////////////////////////////////////////////////////////////

            label7.Text = "Епсилон = " + epsilon;
            label8.Text = "СКО = " + rang.getEpsilon();
            label9.Text = "k = " + rang.getK();

            label10.Text = "";
            label10.Text = "СКО = " + getSKO(rangMatr, domenMin);
            label11.Text = "СКО = " + getSKO(rangMatr, domenAfin);
            label12.Text = "СКО = " + getSKO(rangMatr, domenBright);
        }

        public double getSKO(int[,] ran, int[,] domen)
        {
            int R = ran.GetLength(0);
            double[] so = getSO(ran, domen);
            double s = so[0];
            double o = so[1];
            double sko = 0;
            Color colorRang, colorDomen;

            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    colorDomen = Color.FromArgb(domen[i, j]);
                    colorRang = Color.FromArgb(ran[i, j]);
                    double per = (s * colorDomen.R + o) - colorRang.R;
                    sko = sko + (per) * (per);
                    //sko = sko + ((s*domen[i, j] + o)-rang[i,j]) * ((s * domen[i, j] + o) - rang[i, j]);
                }

            return sko;
        }

        public double[] getSO(int[,] rang, int[,] domen)
        {
            int N = rang.GetLength(0);
            double s = 0, o = 0, D = 0, R = 0, a = 0, b = 0;
            Color colorDomen, colorRang;

            for (int i = 0; i < N; i++)
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

            return new double[] { s, o };
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
    }
}
