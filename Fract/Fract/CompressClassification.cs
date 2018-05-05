using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Fract
{
    class CompressClassification : Compression
    {

        private List<Rang> rangList = new List<Rang>();
        private int r;//размер рангового блока
        private int[,] pix;
        private int width;//ширина картинки
        private int height;//высота картинки
        private int n;//количество блоков по высоте
        private int m;//количество блоков по ширине
        private double epsilon;//коэффициент компрессии?

        private List<Rang> rangListR = new List<Rang>();
        private List<Rang> rangListG = new List<Rang>();
        private List<Rang> rangListB = new List<Rang>();

        private List<Rang> rangListY = new List<Rang>();
        private List<Rang> rangListI = new List<Rang>();
        private List<Rang> rangListQ = new List<Rang>();

        private int[,] classDomen;
        private Classification classification;

        //
        private String sss;
        private int jhgjhg = 0;

        public CompressClassification(int[,] pix, int r, double epsilon, Classification classification)
        {
            this.pix = pix;
            this.r = r;
            this.width = pix.GetLength(1);//pix[0].Length;
            this.height = pix.GetLength(0); //pix.Length;
            this.m = width / r;
            this.n = height / r;
            this.epsilon = epsilon;
            this.classDomen = new int[n - 1, n - 1];
            this.classification = classification;

            this.sss = "";
        }

        public void compressImage(string searchDomen, string imageColor)
        {
            if (imageColor.Equals("gray"))
            {
                if (searchDomen.Equals("first<eps"))
                    compressImageFirst();
                else if (searchDomen.Equals("min"))
                    compressImageMin();
                else if (searchDomen.Equals("min and <eps"))
                    compressImageDivide();
            }
            else
            {
                if (searchDomen.Equals("first<eps"))
                    compressImageFirstColor(imageColor);
                //else if (searchDomen.Equals("min"))
                //    compressImageMinColor(imageColor);
                //else if (searchDomen.Equals("min and <eps"))
                //    compressImageDivideColor(imageColor);
            }
        }
        
        public List<Rang> getRangList()
        {
            return rangList;
        }

        public List<Rang> getRangListComponent(string component)
        {
            if (component.Equals("R"))
                return rangListR;
            else if (component.Equals("G"))
                return rangListG;
            else if (component.Equals("B"))
                return rangListB;
            else if (component.Equals("Y"))
                return rangListY;
            else if (component.Equals("I"))
                return rangListI;
            else if (component.Equals("Q"))
                return rangListQ;
            else return rangList;
        }


        public int getR()
        {
            return r;
        }
      
        public void compressImageMin()
        {
            bool b = false;
            int[,] rang = new int[r, r];// ранговый блок


            int id = 0;
            int jd = 0;

            int R = r / 1;//размер рангового блока
            int D = R * 2;//размер доменного блока
            int N = height / R;//количество блоков по высоте
            int M = width / R;//количество блоков по ширине
            int[,] domenBig = new int[D, D];//доменный блок

            List<int> domenClasses = new List<int>();

            //while ((id < N) && (b == false))
            while ((id < N - 1) && (b == false))
            {
                jd = 0;
                while ((jd < M - 1) && (b == false))
                {
                    for (int i = 0; i < D; i++)
                        for (int j = 0; j < D; j++)
                        {
                            domenBig[i, j] = pix[R * id + i, R * jd + j];
                            //domenBig[i, j] = pix[id + i, jd + j];
                        }

                    domenClasses.Add(classification.getClass(domenBig));
                    jd++;
                }
                id++;
            }

            //перебор ранговых блоков
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    //выделяем ранговый блок
                    for (int ii = 0; ii < r; ii++)
                        for (int jj = 0; jj < r; jj++)
                            rang[ii, jj] = pix[r * i + ii, r * j + jj];

                    //пербор доменных блоков
                    getDomenBlocMin(rang, 1, j * r, i * r,domenClasses);

                }
        }

        public void compressImageFirst()
        {
            bool b = false;
            int[,] rang = new int[r, r];// ранговый блок


            int id = 0;
            int jd = 0;

            int R = r / 1;//размер рангового блока
            int D = R * 2;//размер доменного блока
            int N = height / R;//количество блоков по высоте
            int M = width / R;//количество блоков по ширине
            int[,] domenBig = new int[D, D];//доменный блок

            List<int> domenClasses = new List<int>();

            //while ((id < N) && (b == false))
            while ((id < N - 1) && (b == false))
            {
                jd = 0;
                while ((jd < M - 1) && (b == false))
                {
                    for (int i = 0; i < D; i++)
                        for (int j = 0; j < D; j++)
                        {
                            domenBig[i, j] = pix[R * id + i, R * jd + j];
                            //domenBig[i, j] = pix[id + i, jd + j];
                        }

                    domenClasses.Add(classification.getClass(domenBig));
                    jd++;
                }
                id++;
            }

            //перебор ранговых блоков
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    //выделяем ранговый блок
                    for (int ii = 0; ii < r; ii++)
                        for (int jj = 0; jj < r; jj++)
                            rang[ii, jj] = pix[r * i + ii, r * j + jj];

                    //пербор доменных блоков
                    getDomenBlocFirst(rang, 1, j * r, i * r,domenClasses);
                }

            //
            //SaveSumCompare();
            //return rangsList;
        }

        public void compressImageDivide()
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
                    getDomenBlocDivide(rang, 1, j * r, i * r);
                    //rangsList.add(ran);

                }

            //
            //SaveSumCompare();
            //return rangsList;
        }

        public void compressImageFirstColor(string imageColor)
        {
            bool b = false;
            int[,] rang = new int[r, r];// ранговый блок


            int id = 0;
            int jd = 0;

            int R = r / 1;//размер рангового блока
            int D = R * 2;//размер доменного блока
            int N = height / R;//количество блоков по высоте
            int M = width / R;//количество блоков по ширине
            int[,] domenBig = new int[D, D];//доменный блок

            List<int[]> domenClasses = new List<int[]>();

            //while ((id < N) && (b == false))
            while ((id < N - 1) && (b == false))
            {
                jd = 0;
                while ((jd < M - 1) && (b == false))
                {
                    for (int i = 0; i < D; i++)
                        for (int j = 0; j < D; j++)
                        {
                            domenBig[i, j] = pix[R * id + i, R * jd + j];
                            //domenBig[i, j] = pix[id + i, jd + j];
                        }

                    domenClasses.Add(classification.getClassColor(domenBig));
                    jd++;
                }
                id++;
            }

            //перебор ранговых блоков
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    //выделяем ранговый блок
                    for (int ii = 0; ii < r; ii++)
                        for (int jj = 0; jj < r; jj++)
                            rang[ii, jj] = pix[r * i + ii, r * j + jj];

                    //пербор доменных блоков
                    if (imageColor.Equals("rgb"))
                        getDomenBlocFirstColorRGB(rang, 1, j * r, i * r, domenClasses);
                    else getDomenBlocFirstColorYIQ(rang, 1, j * r, i * r, domenClasses);

                }

            //
            //SaveSumCompare();
            //return rangsList;
        }

        public void getDomenBlocMin(int[,] rang, int k, int x0, int y0, List<int> domenClasses)
        {
            //x - начальная координата блока
            //y - начальная координата блока
            //пербор доменных блоков
            Rang ran = null;
            int R = r / k;//размер рангового блока
            int D = R * 2;//размер доменного блока
            int N = height / R;//количество блоков по высоте
            int M = width / R;//количество блоков по ширине
            //int N = height - D + 1;//количество блоков по высоте
            //int M = width  - D + 1;//количество блоков по ширине

            //int domenSize = rang.GetLength(0);//размер доменного блока = размер рангового
            //int f = r * 2 / z;//кол-во усредняемых пикселей
            int[,] domen = new int[R, R];//уменьшенный доменный блок
            int[,] domenAfin = new int[R, R];//доменный блок подвергнутый афинному преобразованию
            int[,] domenBig = new int[D, D];//доменный блок

            double minSKO = 10000000;
            Rang minRang = new Rang(0, 0, 0, 1, x0, y0, 1, 1, epsilon);

            bool b = false;
            int id = 0;
            int jd = 0;

            int domenIndex = 0;

            int rangClass = classification.getClass(rang);

            //while ((id < N) && (b == false))
            while ((id < N - 1) && (b == false))
            {
                jd = 0;
                //while ((jd < M) && (b == false))
                while ((jd < M - 1) && (b == false))
                {
                    int sum = 0;                    

                    //int domenClass = classification.getClass(domenBig);
                    if (rangClass == domenClasses[domenIndex])
                    {
                        //выделяем доменный блок
                        for (int i = 0; i < D; i++)
                            for (int j = 0; j < D; j++)
                            {
                                domenBig[i, j] = pix[R * id + i, R * jd + j];
                                //domenBig[i, j] = pix[id + i, jd + j];
                            }

                        //уменьшаем его усреднением
                        domen = reduceBlock(domenBig);

                        double[] so;
                        double s, o, sko;
                        List<double> skoMass = new List<double>();

                        Color colorDomen, colorRang;
                        String test = "";

                        //вычисляемм все СКО
                        for (int h = 0; h < 8; h++)
                        {
                            sko = 0;
                            //skoMass.Clear();// = new double[8];
                            String dpr = "";

                            domenAfin = setAfinnInt(domen, h);
                            so = getSO(rang, domenAfin);
                            s = so[0];
                            o = so[1];


                            for (int i = 0; i < R; i++)
                                for (int j = 0; j < R; j++)
                                {
                                    colorDomen = Color.FromArgb(domenAfin[i, j]);
                                    colorRang = Color.FromArgb(rang[i, j]);
                                    double per = (s * colorDomen.R + o) - colorRang.R;
                                    sko = sko + (per) * (per);
                                }

                            skoMass.Add(sko);
                        }


                        //ищем минимальное СКО
                        double min = skoMass.Min();

                        if (jhgjhg < 150)
                        {
                            jhgjhg++;
                            sss += " " + min + "\r\n";
                        }
                        else if (jhgjhg == 150)
                        {
                            SaveSumCompare();
                            jhgjhg++;
                        }


                        {
                            if (min < minSKO)
                            {
                                int afin = skoMass.IndexOf(min);
                                domenAfin = setAfinnInt(domen, afin);
                                so = getSO(rang, domenAfin);
                                s = so[0];
                                o = so[1];

                                minSKO = min;
                                minRang = new Rang(jd * R, id * R, afin, k, x0, y0, s, o, minSKO);
                                //minRang = new Rang(jd, id, afin, k, x0, y0, s, o, minSKO);
                            }
                        }
                    }
                    jd++;
                    domenIndex++;
                }
                id++;
            }

            //////////////////////
            rangList.Add(minRang);
            //printBlock(minRang, rangList.Count - 1);
            
        }

        public void getDomenBlocFirst(int[,] rang, int k, int x0, int y0, List<int> domenClasses)
        {
            //x - начальная координата блока
            //y - начальная координата блока
            //пербор доменных блоков
            Rang ran = null;
            int R = r / k;//размер рангового блока
            int D = R * 2;//размер доменного блока
            int N = height / R;//количество блоков по высоте
            int M = width / R;//количество блоков по ширине
            //int N = height - D + 1;//количество блоков по высоте
            //int M = width  - D + 1;//количество блоков по ширине

            //int domenSize = rang.GetLength(0);//размер доменного блока = размер рангового
            //int f = r * 2 / z;//кол-во усредняемых пикселей
            int[,] domen = new int[R, R];//уменьшенный доменный блок
            int[,] domenAfin = new int[R, R];//доменный блок подвергнутый афинному преобразованию
            int[,] domenBig = new int[D, D];//доменный блок

            double minSKO = 10000000;
            Rang minRang = new Rang(0, 0, 0, 1, x0, y0, 1, 1, epsilon);

            bool b = false;
            int id = 0;
            int jd = 0;

            int domenIndex = 0;

            int rangClass = classification.getClass(rang);

            //while ((id < N) && (b == false))
            while ((id < N - 1) && (b == false))
            {
                jd = 0;
                //while ((jd < M) && (b == false))
                while ((jd < M - 1) && (b == false))
                {
                    int sum = 0;
                    
                    if (rangClass == domenClasses[domenIndex])
                    {
                        //выделяем доменный блок
                        for (int i = 0; i < D; i++)
                            for (int j = 0; j < D; j++)
                            {
                                domenBig[i, j] = pix[R * id + i, R * jd + j];
                                //domenBig[i, j] = pix[id + i, jd + j];
                            }

                        //уменьшаем его усреднением
                        domen = reduceBlock(domenBig);

                        //for (int afi = 0; afi < domenBig.GetLength(0); afi++)
                        //    for (int afj = 0; afj < domenBig.GetLength(0); afj++)
                        //        domenAfin[afi, afj] = domenBig[afi, afj];

                        double[] so;
                        double s, o, sko;
                        List<double> skoMass = new List<double>();

                        Color colorDomen, colorRang;
                        String test = "";

                        //вычисляемм все СКО
                        for (int h = 0; h < 8; h++)
                        {
                            sko = 0;
                            //skoMass.Clear();// = new double[8];
                            String dpr = "";

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
                                    colorDomen = Color.FromArgb(domenAfin[i, j]);
                                    colorRang = Color.FromArgb(rang[i, j]);
                                    double per = (s * colorDomen.R + o) - colorRang.R;
                                    sko = sko + (per) * (per);
                                    //sko = sko + ((s*domen[i, j] + o)-rang[i,j]) * ((s * domen[i, j] + o) - rang[i, j]);
                                }

                            skoMass.Add(sko);//skoMass[h] = sko;

                            test += "afin = " + h
                                    + "\r\n s = " + s
                                    + "\r\n o = " + o
                                    + "\r\n eps = " + sko
                                    + "\r\n ------------------------------------- \r\n";
                        }


                        //ищем минимальное СКО
                        double min = skoMass.Min();

                        if (jhgjhg < 150)
                        {
                            jhgjhg++;
                            sss += " " + min + "\r\n";
                        }
                        else if (jhgjhg == 150)
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
                            if (min < minSKO)
                            {
                                int afin = skoMass.IndexOf(min);
                                domenAfin = setAfinnInt(domen, afin);
                                so = getSO(rang, domenAfin);
                                s = so[0];
                                o = so[1];

                                minSKO = min;
                                minRang = new Rang(jd * R, id * R, afin, k, x0, y0, s, o, minSKO);
                                //minRang = new Rang(jd, id, afin, k, x0, y0, s, o, minSKO);
                            }
                        }
                    }                    
                    jd++;
                    domenIndex++;
                }
                id++;
            }
            //test
            /*if (minSKO < epsilon)
            {
                b = true;
                
                ran = new Rang(minRang.getX(), minRang.getY(), minRang.getAfinn(), minRang.getK(), minRang.getX0(), minRang.getY0(), minRang.getS(), minRang.getO(), minSKO);
            }*/
            //////////////////////

            //если для рангового блока не нашли доменного
            if (ran == null)
            {
                rangList.Add(minRang);
                //printBlock(minRang, rangList.Count - 1);
                //printAfinSO(minRang, rangList.Count - 1);

            }
            else
            {
                rangList.Add(ran);
                //printBlock(ran, rangList.Count - 1);
                //printAfinSO(ran, rangList.Count - 1);
            }

            //return ran;
        }

        public void getDomenBlocDivide(int[,] rang, int k, int x0, int y0)
        {
            //x - начальная координата блока
            //y - начальная координата блока
            //пербор доменных блоков
            Rang ran = null;
            int R = r / k;//размер рангового блока
            int D = R * 2;//размер доменного блока
            int N = height / R;//количество блоков по высоте
            int M = width / R;//количество блоков по ширине
            //int N = height - D + 1;//количество блоков по высоте
            //int M = width  - D + 1;//количество блоков по ширине

            //int domenSize = rang.GetLength(0);//размер доменного блока = размер рангового
            //int f = r * 2 / z;//кол-во усредняемых пикселей
            int[,] domen = new int[R, R];//уменьшенный доменный блок
            int[,] domenAfin = new int[R, R];//доменный блок подвергнутый афинному преобразованию
            int[,] domenBig = new int[D, D];//доменный блок

            double minSKO = 10000000;
            Rang minRang = new Rang(0, 0, 0, k, x0, y0, 1, 1, epsilon);

            bool b = false;
            int id = 0;
            int jd = 0;

            int rangClass = classification.getClass(rang);

            //while ((id < N) && (b == false))
            while ((id < N - 1) && (b == false))
            {
                jd = 0;
                //while ((jd < M) && (b == false))
                while ((jd < M - 1) && (b == false))
                {
                    int sum = 0;
                    //выделяем доменный блок
                    for (int i = 0; i < D; i++)
                        for (int j = 0; j < D; j++)
                        {
                            domenBig[i, j] = pix[R * id + i, R * jd + j];
                            //domenBig[i, j] = pix[id + i, jd + j];
                        }

                    int domenClass = classification.getClass(domenBig);
                    if (rangClass == domenClass)
                    {
                        //уменьшаем его усреднением
                        domen = reduceBlock(domenBig);


                        double[] so;
                        double s, o, sko;
                        List<double> skoMass = new List<double>();

                        Color colorDomen, colorRang;
                        String test = "";

                        //вычисляемм все СКО
                        for (int h = 0; h < 8; h++)
                        {
                            sko = 0;
                            //skoMass.Clear();// = new double[8];
                            String dpr = "";

                            domenAfin = setAfinnInt(domen, h);
                            so = getSO(rang, domenAfin);
                            s = so[0];
                            o = so[1];


                            for (int i = 0; i < R; i++)
                                for (int j = 0; j < R; j++)
                                {
                                    colorDomen = Color.FromArgb(domenAfin[i, j]);
                                    colorRang = Color.FromArgb(rang[i, j]);
                                    double per = (s * colorDomen.R + o) - colorRang.R;
                                    sko = sko + (per) * (per);
                                    //sko = sko + ((s*domen[i, j] + o)-rang[i,j]) * ((s * domen[i, j] + o) - rang[i, j]);
                                }

                            skoMass.Add(sko);//skoMass[h] = sko;
                        }


                        //ищем минимальное СКО
                        double min = skoMass.Min();

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
                            //ran = new Rang(jd, id, afin, k, x0, y0, s, o, min);

                        }
                        else
                        {
                            if (min < minSKO)
                            {
                                int afin = skoMass.IndexOf(min);
                                domenAfin = setAfinnInt(domen, afin);
                                so = getSO(rang, domenAfin);
                                s = so[0];
                                o = so[1];

                                minSKO = min;
                                minRang = new Rang(jd * R, id * R, afin, k, x0, y0, s, o, minSKO);
                                //minRang = new Rang(jd, id, afin, k, x0, y0, s, o, minSKO);
                            }
                        }
                    }
                    jd++;
                }
                id++;
            }

            //если для рангового блока не нашли доменного
            if (ran == null)
            {
                //если для рангового блока не нашли доменного
                if (minSKO > epsilon)//if (ran == null)
                {
                    k = k * 2;
                    //уменьшаем r/2 и снова ищем доменный пресуя его в 4 раза и т.д пока r>2
                    if (r / k >= 4)//(r / k >= 2)  //while (ran == null)
                    {
                        printDevide(rang, rangList.Count);

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
                                getDomenBlocDivide(rangDop, k, x0 + jr * newR, y0 + ir * newR);//x*r,y*r
                            }

                    }
                    else
                    {
                        rangList.Add(minRang);
                        //printBlock(minRang, rangList.Count - 1);
                        //printAfinSO(minRang, rangList.Count - 1);
                    }

                }
                else
                {
                    rangList.Add(minRang);
                    //printBlock(minRang, rangList.Count - 1);
                    //printAfinSO(ran, rangList.Count - 1);
                }

            }
            else
            {
                rangList.Add(ran);
                //printBlock(ran, rangList.Count - 1);
                //printAfinSO(ran, rangList.Count - 1);
            }

            //return ran;
        }

        public void getDomenBlocFirstColorRGB(int[,] rang, int k, int x0, int y0, List<int[]> domenClasses)
        {
            //x - начальная координата блока
            //y - начальная координата блока
            //пербор доменных блоков
            Rang ranRed = null, ranGreen = null, ranBlue = null;
            int R = r / k;//размер рангового блока
            int D = R * 2;//размер доменного блока
            int N = height / R;//количество блоков по высоте
            int M = width / R;//количество блоков по ширине

            int[,] domen = new int[R, R];//уменьшенный доменный блок
            int[,] domenAfin = new int[R, R];//доменный блок подвергнутый афинному преобразованию
            int[,] domenBig = new int[D, D];//доменный блок

            double minSKO_Red = 10000000;
            double minSKO_Green = 10000000;
            double minSKO_Blue = 10000000;
            Rang minRangRed = new Rang(0, 0, 0, 1, x0, y0, 1, 1, epsilon);
            Rang minRangGreen = new Rang(0, 0, 0, 1, x0, y0, 1, 1, epsilon);
            Rang minRangBlue = new Rang(0, 0, 0, 1, x0, y0, 1, 1, epsilon);
            //int minX_Red = 0, minY_Red = 0, minAfin_Red = 0;
            //int minX_Green = 0, minY_Green = 0, minAfin_Green = 0;
            //int minX_Blue = 0, minY_Blue = 0, minAfin_Blue = 0;

            bool br = false, bg = false, bb = false, b = false;
            int id = 0;
            int jd = 0;

            int domenIndex = 0;

            int[] rangClass = classification.getClassColor(rang);

            //while ((id < N) && (b == false))
            while ((id < N - 1) && (b == false))
            {
                jd = 0;
                //while ((jd < M) && (b == false))
                while ((jd < M - 1) && (b == false))
                {

                    if (rangClass[0] == domenClasses[domenIndex][0])
                    {
                        //выделяем доменный блок
                        for (int i = 0; i < D; i++)
                            for (int j = 0; j < D; j++)
                            {
                                domenBig[i, j] = pix[R * id + i, R * jd + j];
                            }

                        //уменьшаем его усреднением
                        domen = reduceBlockColor(domenBig);

                        double[] so;
                        double s_Red, o_Red, sko_Red;
                        List<double> skoMass_Red = new List<double>();

                        Color colorDomen, colorRang;

                        //вычисляемм все СКО
                        for (int h = 0; h < 8; h++)
                        {
                            sko_Red = 0;

                            domenAfin = setAfinnInt(domen, h);
                            so = getSOColors(rang, domenAfin, "rgb");
                            s_Red = so[0];
                            o_Red = so[1];

                            for (int i = 0; i < R; i++)
                                for (int j = 0; j < R; j++)
                                {
                                    colorDomen = Color.FromArgb(domenAfin[i, j]);
                                    colorRang = Color.FromArgb(rang[i, j]);
                                    double per_Red = (s_Red * colorDomen.R + o_Red) - colorRang.R;
                                    sko_Red = sko_Red + (per_Red) * (per_Red);
                                }

                            skoMass_Red.Add(sko_Red);
                        }


                        //ищем минимальное СКО
                        double min_Red = skoMass_Red.Min();

                        //сравниваем с коэффициентом компрессии
                        if (br == false)
                        {
                            if (min_Red < epsilon)
                            {
                                br = true;

                                int afin = skoMass_Red.IndexOf(min_Red);
                                domenAfin = setAfinnInt(domen, afin);
                                so = getSOColors(rang, domenAfin, "rgb");
                                s_Red = so[0];
                                o_Red = so[1];

                                ranRed = new Rang(jd * R, id * R, afin, k, x0, y0, s_Red, o_Red, min_Red);
                            }
                            else if (min_Red < minSKO_Red)
                            {
                                int afin = skoMass_Red.IndexOf(min_Red);
                                domenAfin = setAfinnInt(domen, afin);
                                so = getSOColors(rang, domenAfin, "rgb");
                                s_Red = so[0];
                                o_Red = so[1];

                                minSKO_Red = min_Red;
                                minRangRed = new Rang(jd * R, id * R, afin, k, x0, y0, s_Red, o_Red, minSKO_Red);
                                //minRang = new Rang(jd, id, afin, k, x0, y0, s, o, minSKO);
                            }

                        }                    
                    }

                    if(rangClass[1] == domenClasses[domenIndex][1])
                    {
                        //выделяем доменный блок
                        for (int i = 0; i < D; i++)
                            for (int j = 0; j < D; j++)
                            {
                                domenBig[i, j] = pix[R * id + i, R * jd + j];
                            }

                        //уменьшаем его усреднением
                        domen = reduceBlockColor(domenBig);

                        double[] so;
                        double s_Green, o_Green, sko_Green;
                        List<double> skoMass_Green = new List<double>();

                        Color colorDomen, colorRang;

                        //вычисляемм все СКО
                        for (int h = 0; h < 8; h++)
                        {
                            sko_Green = 0;

                            domenAfin = setAfinnInt(domen, h);
                            so = getSOColors(rang, domenAfin, "rgb");
                            s_Green = so[2];
                            o_Green = so[3];

                            for (int i = 0; i < R; i++)
                                for (int j = 0; j < R; j++)
                                {
                                    colorDomen = Color.FromArgb(domenAfin[i, j]);
                                    colorRang = Color.FromArgb(rang[i, j]);
                                    double per_Green = (s_Green * colorDomen.G + o_Green) - colorRang.G;
                                    sko_Green = sko_Green + (per_Green) * (per_Green);
                                }
                            skoMass_Green.Add(sko_Green);
                        }

                        //ищем минимальное СКО
                        double min_Green = skoMass_Green.Min();

                        //сравниваем с коэффициентом компрессии
                        if (bg == false)
                        {
                            if (min_Green < epsilon)
                            {
                                bg = true;

                                int afin = skoMass_Green.IndexOf(min_Green);
                                domenAfin = setAfinnInt(domen, afin);
                                so = getSOColors(rang, domenAfin, "rgb");
                                s_Green = so[2];
                                o_Green = so[3];

                                ranGreen = new Rang(jd * R, id * R, afin, k, x0, y0, s_Green, o_Green, min_Green);
                            }
                            else if (min_Green < minSKO_Green)
                            {
                                int afin = skoMass_Green.IndexOf(min_Green);
                                domenAfin = setAfinnInt(domen, afin);
                                so = getSOColors(rang, domenAfin, "rgb");
                                s_Green = so[2];
                                o_Green = so[3];

                                minSKO_Green = min_Green;
                                minRangGreen = new Rang(jd * R, id * R, afin, k, x0, y0, s_Green, o_Green, minSKO_Green);
                                //minRang = new Rang(jd, id, afin, k, x0, y0, s, o, minSKO);
                            }

                        }
                    }

                    if(rangClass[2] == domenClasses[domenIndex][2])
                    {
                        //выделяем доменный блок
                        for (int i = 0; i < D; i++)
                            for (int j = 0; j < D; j++)
                            {
                                domenBig[i, j] = pix[R * id + i, R * jd + j];
                            }

                        //уменьшаем его усреднением
                        domen = reduceBlockColor(domenBig);

                        double[] so;
                        double s_Blue, o_Blue, sko_Blue;
                        List<double> skoMass_Blue = new List<double>();

                        Color colorDomen, colorRang;

                        //вычисляемм все СКО
                        for (int h = 0; h < 8; h++)
                        {
                            sko_Blue = 0;
                            String dpr = "";

                            domenAfin = setAfinnInt(domen, h);
                            so = getSOColors(rang, domenAfin, "rgb");
                            s_Blue = so[4];
                            o_Blue = so[5];

                            for (int i = 0; i < R; i++)
                                for (int j = 0; j < R; j++)
                                {
                                    colorDomen = Color.FromArgb(domenAfin[i, j]);
                                    colorRang = Color.FromArgb(rang[i, j]);
                                    double per_Blue = (s_Blue * colorDomen.B + o_Blue) - colorRang.B;
                                    sko_Blue = sko_Blue + (per_Blue) * (per_Blue);
                                }
                            skoMass_Blue.Add(sko_Blue);
                        }

                        //ищем минимальное СКО
                        double min_Blue = skoMass_Blue.Min();

                        //сравниваем с коэффициентом компрессии
                        if (bb == false)
                        {
                            if (min_Blue < epsilon)
                            {
                                bb = true;

                                int afin = skoMass_Blue.IndexOf(min_Blue);
                                domenAfin = setAfinnInt(domen, afin);
                                so = getSOColors(rang, domenAfin, "rgb");
                                s_Blue = so[4];
                                o_Blue = so[5];

                                ranBlue = new Rang(jd * R, id * R, afin, k, x0, y0, s_Blue, o_Blue, min_Blue);
                            }
                            else if (min_Blue < minSKO_Blue)
                            {
                                int afin = skoMass_Blue.IndexOf(min_Blue);
                                domenAfin = setAfinnInt(domen, afin);
                                so = getSOColors(rang, domenAfin, "rgb");
                                s_Blue = so[4];
                                o_Blue = so[5];

                                minSKO_Blue = min_Blue;
                                minRangBlue = new Rang(jd * R, id * R, afin, k, x0, y0, s_Blue, o_Blue, minSKO_Blue);
                                //minRang = new Rang(jd, id, afin, k, x0, y0, s, o, minSKO);
                            }
                        }
                    }

                    if ((br == true) && (bg == true) && (bb == true))
                        b = true;

                    jd++;
                }
                id++;
            }

            //если для рангового блока не нашли доменного
            if (ranRed == null)
            {
                rangListR.Add(minRangRed);
                //printBlock(minRangRed, rangListR.Count - 1, "R");
                //printAfinSO(minRang, rangList.Count - 1);
            }
            else
            {
                rangListR.Add(ranRed);
                //printBlock(ranRed, rangListR.Count - 1,"R");
                //printAfinSO(ran, rangList.Count - 1);
            }

            if (ranGreen == null)
            {
                rangListG.Add(minRangGreen);
                //printBlock(minRangGreen, rangListG.Count - 1,"G");
                //printAfinSO(minRang, rangList.Count - 1);
            }
            else
            {
                rangListG.Add(ranGreen);
                //printBlock(ranGreen, rangListG.Count - 1, "G");
                //printAfinSO(ran, rangList.Count - 1);
            }

            if (ranBlue == null)
            {
                rangListB.Add(minRangBlue);
                //printBlock(minRangBlue, rangListB.Count - 1, "B");
                //printAfinSO(minRang, rangList.Count - 1);
            }
            else
            {
                rangListB.Add(ranBlue);
                //printBlock(ranBlue, rangListB.Count - 1, "B");
                //printAfinSO(ran, rangList.Count - 1);
            }

            //return ran;
        }

        public void getDomenBlocFirstColorYIQ(int[,] rang, int k, int x0, int y0, List<int[]> domenClasses)
        {
            //x - начальная координата блока
            //y - начальная координата блока
            //пербор доменных блоков
            Rang ranY = null, ranI = null, ranQ = null;
            int R = r / k;//размер рангового блока
            int D = R * 2;//размер доменного блока
            int N = height / R;//количество блоков по высоте
            int M = width / R;//количество блоков по ширине

            int[,] domen = new int[R, R];//уменьшенный доменный блок
            int[,] domenAfin = new int[R, R];//доменный блок подвергнутый афинному преобразованию
            int[,] domenBig = new int[D, D];//доменный блок

            double minSKO_Y = 10000000;
            double minSKO_I = 10000000;
            double minSKO_Q = 10000000;
            Rang minRangY = new Rang(0, 0, 0, 1, x0, y0, 1, 1, epsilon);
            Rang minRangI = new Rang(0, 0, 0, 1, x0, y0, 1, 1, epsilon);
            Rang minRangQ = new Rang(0, 0, 0, 1, x0, y0, 1, 1, epsilon);
            int minX_Y = 0, minY_Y = 0, minAfin_Y = 0;
            int minX_I = 0, minY_I = 0, minAfin_I = 0;
            int minX_Q = 0, minY_Q = 0, minAfin_Q = 0;

            bool bi = false, by = false, bq = false, b = false;
            int id = 0;
            int jd = 0;

            int domenIndex = 0;

            int[] rangClass = classification.getClassColor(rang);

            //while ((id < N) && (b == false))
            while ((id < N - 1) && (b == false))
            {
                jd = 0;
                //while ((jd < M) && (b == false))
                while ((jd < M - 1) && (b == false))
                {

                    if (rangClass[0] == domenClasses[domenIndex][0])
                    {
                        //выделяем доменный блок
                        for (int i = 0; i < D; i++)
                            for (int j = 0; j < D; j++)
                            {
                                domenBig[i, j] = pix[R * id + i, R * jd + j];
                            }

                        //уменьшаем его усреднением
                        domen = reduceBlockColor(domenBig);

                        double[] so;
                        double s_Y, o_Y, sko_Y;
                        List<double> skoMass_Y = new List<double>();

                        Color colorDomen, colorRang;

                        //вычисляемм все СКО
                        for (int h = 0; h < 8; h++)
                        {
                            sko_Y = 0;

                            domenAfin = setAfinnInt(domen, h);
                            so = getSOColors(rang, domenAfin, "yiq");
                            s_Y = so[0];
                            o_Y = so[1];

                            for (int i = 0; i < R; i++)
                                for (int j = 0; j < R; j++)
                                {
                                    colorDomen = Color.FromArgb(domenAfin[i, j]);
                                    colorRang = Color.FromArgb(rang[i, j]);
                                    double per_Y = (s_Y * getY(colorDomen) + o_Y) - getY(colorRang);
                                    sko_Y = sko_Y + (per_Y) * (per_Y);
                                }

                            skoMass_Y.Add(sko_Y);
                        }


                        //ищем минимальное СКО
                        double min_Y = skoMass_Y.Min();

                        //сравниваем с коэффициентом компрессии
                        if (by == false)
                        {
                            if (min_Y < epsilon)
                            {
                                by = true;

                                int afin = skoMass_Y.IndexOf(min_Y);
                                domenAfin = setAfinnInt(domen, afin);
                                so = getSOColors(rang, domenAfin, "yiq");
                                s_Y = so[0];
                                o_Y = so[1];

                                ranY = new Rang(jd * R, id * R, afin, k, x0, y0, s_Y, o_Y, min_Y);
                            }
                            else if (min_Y < minSKO_Y)
                            {
                                int afin = skoMass_Y.IndexOf(min_Y);
                                domenAfin = setAfinnInt(domen, afin);
                                so = getSOColors(rang, domenAfin, "yiq");
                                s_Y = so[0];
                                o_Y = so[1];

                                minSKO_Y = min_Y;
                                minRangY = new Rang(jd * R, id * R, afin, k, x0, y0, s_Y, o_Y, minSKO_Y);
                                //minRang = new Rang(jd, id, afin, k, x0, y0, s, o, minSKO);
                            }

                        }
                    }

                    if (rangClass[1] == domenClasses[domenIndex][1])
                    {
                        //выделяем доменный блок
                        for (int i = 0; i < D; i++)
                            for (int j = 0; j < D; j++)
                            {
                                domenBig[i, j] = pix[R * id + i, R * jd + j];
                            }

                        //уменьшаем его усреднением
                        domen = reduceBlockColor(domenBig);

                        double[] so;
                        double s_I, o_I, sko_I;
                        List<double> skoMass_I = new List<double>();

                        Color colorDomen, colorRang;

                        //вычисляемм все СКО
                        for (int h = 0; h < 8; h++)
                        {
                            sko_I = 0;

                            domenAfin = setAfinnInt(domen, h);
                            so = getSOColors(rang, domenAfin, "yiq");
                            s_I = so[2];
                            o_I = so[3];

                            for (int i = 0; i < R; i++)
                                for (int j = 0; j < R; j++)
                                {
                                    colorDomen = Color.FromArgb(domenAfin[i, j]);
                                    colorRang = Color.FromArgb(rang[i, j]);
                                    double per_I = (s_I * getI(colorDomen) + o_I) - getI(colorRang);
                                    sko_I = sko_I + (per_I) * (per_I);
                                }
                            skoMass_I.Add(sko_I);
                        }

                        //ищем минимальное СКО
                        double min_I = skoMass_I.Min();

                        //сравниваем с коэффициентом компрессии
                        if (bi == false)
                        {
                            if (min_I < epsilon)
                            {
                                bi = true;

                                int afin = skoMass_I.IndexOf(min_I);
                                domenAfin = setAfinnInt(domen, afin);
                                so = getSOColors(rang, domenAfin, "yiq");
                                s_I = so[2];
                                o_I = so[3];

                                ranI = new Rang(jd * R, id * R, afin, k, x0, y0, s_I, o_I, min_I);
                            }
                            else if (min_I < minSKO_I)
                            {
                                int afin = skoMass_I.IndexOf(min_I);
                                domenAfin = setAfinnInt(domen, afin);
                                so = getSOColors(rang, domenAfin, "yiq");
                                s_I = so[2];
                                o_I = so[3];

                                minSKO_I = min_I;
                                minRangI = new Rang(jd * R, id * R, afin, k, x0, y0, s_I, o_I, minSKO_I);
                                //minRang = new Rang(jd, id, afin, k, x0, y0, s, o, minSKO);
                            }

                        }
                    }

                    if (rangClass[2] == domenClasses[domenIndex][2])
                    {
                        //выделяем доменный блок
                        for (int i = 0; i < D; i++)
                            for (int j = 0; j < D; j++)
                            {
                                domenBig[i, j] = pix[R * id + i, R * jd + j];
                            }

                        //уменьшаем его усреднением
                        domen = reduceBlockColor(domenBig);

                        double[] so;
                        double s_Q, o_Q, sko_Q;
                        List<double> skoMass_Q = new List<double>();

                        Color colorDomen, colorRang;

                        //вычисляемм все СКО
                        for (int h = 0; h < 8; h++)
                        {
                            sko_Q = 0;

                            domenAfin = setAfinnInt(domen, h);
                            so = getSOColors(rang, domenAfin, "yiq");
                            s_Q = so[4];
                            o_Q = so[5];

                            for (int i = 0; i < R; i++)
                                for (int j = 0; j < R; j++)
                                {
                                    colorDomen = Color.FromArgb(domenAfin[i, j]);
                                    colorRang = Color.FromArgb(rang[i, j]);
                                    double per_Q = (s_Q * getQ(colorDomen) + o_Q) - getQ(colorRang);
                                    sko_Q = sko_Q + (per_Q) * (per_Q);
                                }
                            skoMass_Q.Add(sko_Q);
                        }

                        //ищем минимальное СКО
                        double min_Q = skoMass_Q.Min();

                        //сравниваем с коэффициентом компрессии
                        if (bq == false)
                        {
                            if (min_Q < epsilon)
                            {
                                bq = true;

                                int afin = skoMass_Q.IndexOf(min_Q);
                                domenAfin = setAfinnInt(domen, afin);
                                so = getSOColors(rang, domenAfin, "yiq");
                                s_Q = so[4];
                                o_Q = so[5];

                                ranQ = new Rang(jd * R, id * R, afin, k, x0, y0, s_Q, o_Q, min_Q);
                            }
                            else if (min_Q < minSKO_Q)
                            {
                                int afin = skoMass_Q.IndexOf(min_Q);
                                domenAfin = setAfinnInt(domen, afin);
                                so = getSOColors(rang, domenAfin, "yiq");
                                s_Q = so[4];
                                o_Q = so[5];

                                minSKO_Q = min_Q;
                                minRangQ = new Rang(jd * R, id * R, afin, k, x0, y0, s_Q, o_Q, minSKO_Q);
                                //minRang = new Rang(jd, id, afin, k, x0, y0, s, o, minSKO);
                            }
                        }
                    }

                    if ((by == true) && (bi == true) && (bq == true))
                        b = true;

                    jd++;
                }
                id++;
            }

            //если для рангового блока не нашли доменного
            if (ranY == null)
            {
                rangListY.Add(minRangY);
                //printBlock(minRangRed, rangListR.Count - 1, "R");
                //printAfinSO(minRang, rangList.Count - 1);
            }
            else
            {
                rangListY.Add(ranY);
                //printBlock(ranRed, rangListR.Count - 1,"R");
                //printAfinSO(ran, rangList.Count - 1);
            }

            if (ranI == null)
            {
                rangListI.Add(minRangI);
                //printBlock(minRangGreen, rangListG.Count - 1,"G");
                //printAfinSO(minRang, rangList.Count - 1);
            }
            else
            {
                rangListI.Add(ranI);
                //printBlock(ranGreen, rangListG.Count - 1, "G");
                //printAfinSO(ran, rangList.Count - 1);
            }

            if (ranQ == null)
            {
                rangListQ.Add(minRangQ);
                //printBlock(minRangBlue, rangListB.Count - 1, "B");
                //printAfinSO(minRang, rangList.Count - 1);
            }
            else
            {
                rangListQ.Add(ranQ);
                //printBlock(ranBlue, rangListB.Count - 1, "B");
                //printAfinSO(ran, rangList.Count - 1);
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

            sum = sum / 1000;
            // if((sum!=0)&&(sum!=1531)&&(sum!=441032))
            //   System.out.println("");

            if (jhgjhg < 150)
            {
                jhgjhg++;
                sss += " " + sum + "\r\n";
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
                + "Afin = " + rang.getAfinn() + "\r\n"
                + "k = " + rang.getK() + "\r\n"
                + "s = " + rang.getS() + "\r\n"
                + "o = " + rang.getO() + "\r\n"
                + "epsilon = " + rang.getEpsilon();
            File.WriteAllText(@"Rang_" + z + ".txt", text);
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
                    if (red) x = s * color.R + o;
                    else if (green) x = s * color.G + o;
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

        public void printBlock(Rang rang, int k)
        {
            int otst = r * 2 + 10;
            int width = pix.GetLength(1);
            Bitmap bitmap = new Bitmap(width + otst, pix.GetLength(0) + 5);
            Color color;// = Color.White;
            int R = r / rang.getK();//размер рангового блока
            int D = R * 2;//размер доменного блока
            int[,] rangMatr = new int[R, R];
            //bi.SetPixel(rang.getX0() + j, rang.getY0() + i, color);

            //for (int i = 0; i < bitmap.Width - otst; i++)
            for (int i = 0; i < pix.GetLength(0); i++)
                //for (int j = 0; j < bitmap.Height; j++)
                for (int j = 0; j < pix.GetLength(1); j++)
                {
                    //if (i % r != 0 && j % r != 0)
                    //    bitmap.SetPixel(i, j, Color.White);                       
                    //else bitmap.SetPixel(i, j, Color.Black);
                    bitmap.SetPixel(i+1, j, Color.FromArgb(pix[j, i]));
                }

            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    //bitmap.SetPixel(rang.getX0()+i, rang.getY0()+j, Color.FromArgb(pix[rang.getY0() + j, rang.getX0() + i]));
                    rangMatr[j, i] = pix[rang.getY0() + j, rang.getX0() + i];
                }

            for (int i = -1; i < R + 1; i++)
                if ((rang.getX0() + i >= 0) && (rang.getY0() - 1 >= 0))
                    bitmap.SetPixel(rang.getX0() + i, rang.getY0() - 1, Color.Yellow);
            for (int i = -1; i < R + 1; i++)
                if (rang.getX0() + i >= 0)
                    bitmap.SetPixel(rang.getX0() + i, rang.getY0() + R, Color.Yellow);
            for (int j = -1; j < R + 1; j++)
                if ((rang.getY0() + j >= 0) && (rang.getX0() - 1 >= 0))
                    bitmap.SetPixel(rang.getX0() - 1, rang.getY0() + j, Color.Yellow);
            for (int j = -1; j < R + 1; j++)
                if (rang.getY0() + j >= 0)
                    bitmap.SetPixel(rang.getX0() + R, rang.getY0() + j, Color.Yellow);

            for (int i = 0; i < D; i++)
                for (int j = 0; j < D; j++)
                {
                    //bitmap.SetPixel(rang.getX() + i, rang.getY() + j, Color.FromArgb(pix[rang.getY() + j, rang.getX() + i]));
                }

            for (int i = -1; i < D + 1; i++)
                if ((rang.getX() + i >= 0) && (rang.getY() - 1 >= 0))
                    bitmap.SetPixel(rang.getX() + i, rang.getY() - 1, Color.Yellow);
            for (int i = -1; i < D + 1; i++)
                if (rang.getX() + i >= 0)
                    bitmap.SetPixel(rang.getX() + i, rang.getY() + D, Color.Yellow);
            for (int j = -1; j < D + 1; j++)
                if ((rang.getY() + j >= 0) && (rang.getX() - 1 >= 0))
                    bitmap.SetPixel(rang.getX() - 1, rang.getY() + j, Color.Yellow);
            for (int j = -1; j < D + 1; j++)
                if (rang.getY() + j >= 0)
                    bitmap.SetPixel(rang.getX() + D, rang.getY() + j, Color.Yellow);

            //////////////////////////////////////////
            int[,] domen = new int[D, D], domenAfin, domenBright;
            int[,] domenMin = new int[R, R];
            for (int i = 0; i < D; i++)
                for (int j = 0; j < D; j++)
                {
                    bitmap.SetPixel(width + 5 + i + 1, 5 + j, Color.FromArgb(pix[rang.getY() + j, rang.getX() + i]));
                    domen[j, i] = pix[rang.getY() + j, rang.getX() + i];
                }

            domenMin = reduceBlock(domen);
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(width + 5 + i + 1, (5 + R * 2 + 10) + j, Color.FromArgb(domenMin[j, i]));
                }

            domenAfin = setAfinnInt(domenMin, rang.getAfinn());
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(width + 5 + i + 1, (5 + R * 3 + 20) + j, Color.FromArgb(domenAfin[j, i]));
                }

            domenBright = changeBright(domenAfin, rang.getS(), rang.getO());
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(width + 5 + i + 1, (5 + R * 5 + 30) + j, Color.FromArgb(domenBright[j, i]));
                }

            //rang block
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(width + 5 + i + 1, (5 + R * 6 + 40) + j, Color.FromArgb(rangMatr[j, i]));
                }

            //difference
            int argbcolor;
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    //argbcolor = Math.Abs(Color.FromArgb(rangMatr[i, j]).R - Color.FromArgb(domenBright[i, j]).R);
                    argbcolor = Math.Abs(Color.FromArgb(domenBright[j, i]).R - Color.FromArgb(rangMatr[j, i]).R);
                    bitmap.SetPixel(width + 5 + i + 1, (5 + R * 7 + 50) + j, Color.FromArgb(argbcolor, argbcolor, argbcolor));
                }

            try
            {
                String name = k + "___k=" + rang.getK() + "__" + "E=" + epsilon + "____e=" + rang.getEpsilon() + "__print";
                bitmap.Save("D:\\университет\\диплом\\bloks\\" + name + ".jpg");
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

        public void printBlock(Rang rang, int k, string imageColor)
        {
            int otst = r * 2 + 10;
            int width = pix.GetLength(1);
            Bitmap bitmap = new Bitmap(width + otst, pix.GetLength(0) + 5);
            Color color;// = Color.White;
            int R = r / rang.getK();//размер рангового блока
            int D = R * 2;//размер доменного блока
            int[,] rangMatr = new int[R, R];
            //bi.SetPixel(rang.getX0() + j, rang.getY0() + i, color);

            for (int i = 0; i < bitmap.Width - otst; i++)
                //for (int j = 0; j < bitmap.Height; j++)
                for (int j = 0; j < pix.GetLength(1); j++)
                {
                    //if (i % r != 0 && j % r != 0)
                    //    bitmap.SetPixel(i, j, Color.White);                       
                    //else bitmap.SetPixel(i, j, Color.Black);
                    bitmap.SetPixel(i, j, Color.FromArgb(pix[j, i]));
                }

            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    //bitmap.SetPixel(rang.getX0()+i, rang.getY0()+j, Color.FromArgb(pix[rang.getY0() + j, rang.getX0() + i]));
                    rangMatr[j, i] = pix[rang.getY0() + j, rang.getX0() + i];
                }

            for (int i = -1; i < R + 1; i++)
                if ((rang.getX0() + i >= 0) && (rang.getY0() - 1 >= 0))
                    bitmap.SetPixel(rang.getX0() + i, rang.getY0() - 1, Color.Yellow);
            for (int i = -1; i < R + 1; i++)
                if (rang.getX0() + i >= 0)
                    bitmap.SetPixel(rang.getX0() + i, rang.getY0() + R, Color.Yellow);
            for (int j = -1; j < R + 1; j++)
                if ((rang.getY0() + j >= 0) && (rang.getX0() - 1 >= 0))
                    bitmap.SetPixel(rang.getX0() - 1, rang.getY0() + j, Color.Yellow);
            for (int j = -1; j < R + 1; j++)
                if (rang.getY0() + j >= 0)
                    bitmap.SetPixel(rang.getX0() + R, rang.getY0() + j, Color.Yellow);

            for (int i = 0; i < D; i++)
                for (int j = 0; j < D; j++)
                {
                    //bitmap.SetPixel(rang.getX() + i, rang.getY() + j, Color.FromArgb(pix[rang.getY() + j, rang.getX() + i]));
                }

            for (int i = -1; i < D + 1; i++)
                if ((rang.getX() + i >= 0) && (rang.getY() - 1 >= 0))
                    bitmap.SetPixel(rang.getX() + i, rang.getY() - 1, Color.Yellow);
            for (int i = -1; i < D + 1; i++)
                if (rang.getX() + i >= 0)
                    bitmap.SetPixel(rang.getX() + i, rang.getY() + D, Color.Yellow);
            for (int j = -1; j < D + 1; j++)
                if ((rang.getY() + j >= 0) && (rang.getX() - 1 >= 0))
                    bitmap.SetPixel(rang.getX() - 1, rang.getY() + j, Color.Yellow);
            for (int j = -1; j < D + 1; j++)
                if (rang.getY() + j >= 0)
                    bitmap.SetPixel(rang.getX() + D, rang.getY() + j, Color.Yellow);

            //////////////////////////////////////////
            int[,] domen = new int[D, D], domenAfin, domenBright;
            int[,] domenMin = new int[R, R];
            for (int i = 0; i < D; i++)
                for (int j = 0; j < D; j++)
                {
                    bitmap.SetPixel(width + 5 + i, 5 + j, Color.FromArgb(pix[rang.getY() + j, rang.getX() + i]));
                    domen[j, i] = pix[rang.getY() + j, rang.getX() + i];
                }

            domenMin = reduceBlockColor(domen);
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

            domenBright = changeBrightColorRGB(domenAfin, rang.getS(), rang.getO(), imageColor);
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(width + 5 + i, (5 + R * 5 + 30) + j, Color.FromArgb(domenBright[j, i], domenBright[j, i], domenBright[j, i]));
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
                    //bitmap.SetPixel(width + 5 + i, (5 + R * 7 + 50) + j, Color.FromArgb(argbcolor, argbcolor, argbcolor));
                }

            try
            {
                String name = k + "_" + imageColor + "___k=" + rang.getK() + "__" + "E=" + epsilon + "____e=" + rang.getEpsilon() + "__print";
                bitmap.Save("D:\\университет\\диплом\\bloks\\" + name + ".jpg");
                //Button5.Text = "Saved file.";
            }
            catch (Exception)
            {
                //MessageBox.Show("There was a problem saving the file." +
                //"Check the file permissions.");
            }

            //if (k == 0)
            //    printRang(k);
        }

        public void printBlock(Rang rang, int[,] rangMatr, int[,] domen, int[,] domenMin, int[,] domenAfin, int k)
        {
            int otst = r * 2 + 10;
            int width = pix.GetLength(1);
            Bitmap bitmap = new Bitmap(width + otst, pix.GetLength(0));
            Color color;// = Color.White;
            int R = r / rang.getK();//размер рангового блока
            int D = R * 2;//размер доменного блока
            //int[,] rangMatr = new int[R, R];
            //bi.SetPixel(rang.getX0() + j, rang.getY0() + i, color);

            for (int i = 0; i < bitmap.Width - otst; i++)
                for (int j = 0; j < bitmap.Height; j++)
                {
                    if (i % r != 0 && j % r != 0)
                        bitmap.SetPixel(i, j, Color.White);
                    else bitmap.SetPixel(i, j, Color.Black);
                }

            for (int i = 0; i < R; i++)//столбцы
                for (int j = 0; j < R; j++)//строки
                {
                    bitmap.SetPixel(rang.getX0() + i, rang.getY0() + j, Color.FromArgb(pix[rang.getY0() + j, rang.getX0() + i]));
                    //rangMatr[i, j] = pix[rang.getX0() + i, rang.getY0() + j];
                }

            for (int i = 0; i < D; i++)
                for (int j = 0; j < D; j++)
                {
                    bitmap.SetPixel(rang.getX() + i, rang.getY() + j, Color.FromArgb(pix[rang.getY() + j, rang.getX() + i]));
                }

            ////////////////////////////////////////
            //int[,] domen = new int[D, D], domenAfin, 
            int[,] domenBright;
            //int[,] domenMin = new int[R, R];
            for (int i = 0; i < D; i++)
                for (int j = 0; j < D; j++)
                {
                    bitmap.SetPixel(width + 5 + i, 5 + j, Color.FromArgb(pix[rang.getY() + j, rang.getX() + i]));
                    //domen[i, j] = pix[rang.getX() + i, rang.getY() + j];
                }

            //domenMin = reduceBlock(domen);
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(width + 5 + i, (5 + R * 2 + 10) + j, Color.FromArgb(domenMin[j, i]));
                }

            // domenAfin = setAfinnInt(domenMin, rang.getAfinn());
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

            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    bitmap.SetPixel(rang.getX0() + i, rang.getY0() + j, Color.FromArgb(pix[rang.getY0() + j, rang.getX0() + i]));
                    //rangMatr[i, j] = pix[rang.getX0() + i, rang.getY0() + j];
                }

            //difference
            int argbcolor;
            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    argbcolor = Math.Abs(Color.FromArgb(rangMatr[i, j]).R - Color.FromArgb(domenBright[i, j]).R);
                    argbcolor = Math.Abs(Color.FromArgb(domenBright[i, j]).R - Color.FromArgb(rangMatr[i, j]).R);
                    bitmap.SetPixel(width + 5 + i, (5 + R * 7 + 50) + j, Color.FromArgb(argbcolor, argbcolor, argbcolor));
                }

            try
            {
                String name = k + "___k=" + rang.getK() + "__" + "E=" + epsilon + "____e=" + rang.getEpsilon() + "__";
                bitmap.Save("D:\\университет\\диплом\\bloks\\" + name + ".jpg");
                //Button5.Text = "Saved file.";
            }
            catch (Exception)
            {
                //MessageBox.Show("There was a problem saving the file." +
                //"Check the file permissions.");
            }

        }

        public void printAfinSO(Rang rang, int k)
        {
            double[] so;
            double s, o, sko;
            List<double> skoMass = new List<double>();
            int R = r / rang.getK();//размер рангового блока
            int D = R * 2;//размер доменного блока
            int[,] domen = new int[R, R];//уменьшенный доменный блок
            int[,] rangMatr = new int[R, R];//уменьшенный доменный блок
            int[,] domenAfin = new int[R, R];//доменный блок подвергнутый афинному преобразованию
            int[,] domenBig = new int[D, D];//доменный блок
            Color colorRang, colorDomen;
            String test = "";

            for (int i = 0; i < D; i++)
                for (int j = 0; j < D; j++)
                {
                    domenBig[i, j] = pix[rang.getX() + i, rang.getY() + j];
                }

            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    rangMatr[i, j] = pix[rang.getX0() + i, rang.getY0() + j];
                }

            //уменьшаем его усреднением
            domen = reduceBlock(domenBig);

            //вычисляемм все СКО
            for (int h = 0; h < 8; h++)
            {
                sko = 0;
                //skoMass.Clear();// = new double[8];

                domenAfin = setAfinnInt(domen, h);
                so = getSO(rangMatr, domenAfin);
                s = so[0];
                o = so[1];


                for (int i = 0; i < R; i++)
                    for (int j = 0; j < R; j++)
                    {
                        colorDomen = Color.FromArgb(domenAfin[i, j]);
                        colorRang = Color.FromArgb(rangMatr[i, j]);
                        double per = (s * colorDomen.R + o) - colorRang.R;
                        sko = sko + (per) * (per);
                        //sko = sko + ((s*domen[i, j] + o)-rang[i,j]) * ((s * domen[i, j] + o) - rang[i, j]);
                    }

                skoMass.Add(sko);//skoMass[h] = sko;

                test += "afin = " + h
                + "\r\n s = " + s
                + "\r\n o = " + o
                + "\r\n eps = " + sko
                + "\r\n ------------------------------------- \r\n";

            }

            int afin = skoMass.IndexOf(skoMass.Min());
            domenAfin = setAfinnInt(domen, afin);
            so = getSO(rangMatr, domenAfin);
            s = so[0];
            o = so[1];

            String dom = "";
            dom += "\r\n\r\n rang \r\n";

            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                    dom += rangMatr[i, j] + "\r\n";

            dom += "\r\n\r\n domen \r\n";

            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                    dom += domen[i, j] + "\r\n";


            test += "\r\n x0 = " + rang.getX0() + "    y0 = " + rang.getY0() + "    x = " + rang.getX() + "    y = " + rang.getY()
                            + "   afin = " + rang.getAfinn()
                            + "\r\ns = " + s + "\r\no = " + o
                            + "\r\n ------------------------------------- \r\n";
            test += dom;


            String name = k + "___k=" + rang.getK() + "__" + "E=" + epsilon + "____e=" + skoMass.Min() + "__print";

            System.IO.File.WriteAllText(@"D:\\университет\\диплом\\bloks_txt\\" + name + ".txt", test);
        }

        public void printDevide(int[,] rang, int k)
        {
            int n = rang.GetLength(0);
            String test = "";

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    test += rang[i, j] + "  ";
                    if (j == n / 2 - 1)
                        test += "        ";
                }
                test += "\r\n";
                if (i == n / 2 - 1)
                    test += "\r\n\r\n";
            }

            test += "\r\n----------------------------------\r\n\r\n";

            int newR = n / 2;
            int[,] rangDop = new int[newR, newR];
            for (int ir = 0; ir < 2; ir++)
            {
                for (int jr = 0; jr < 2; jr++)
                {
                    //выделяем ранговый блок
                    for (int i = 0; i < newR; i++)
                        for (int j = 0; j < newR; j++)
                        {
                            rangDop[i, j] = rang[ir * newR + i, jr * newR + j];
                            //rangDop[j, i] = rang[ir * newR + i, jr * newR + j];
                        }

                    for (int i = 0; i < newR; i++)
                    {
                        for (int j = 0; j < newR; j++)
                        {
                            test += rangDop[i, j] + "  ";
                        }
                        test += "\r\n";
                    }

                    test += "\r\n";
                }
                test += "\r\n\r\n";
            }



            String name = k + "___devide";

            System.IO.File.WriteAllText(@"D:\\университет\\диплом\\bloks_txt\\" + name + ".txt", test);
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

        public double[] getSOColors(int[,] rang, int[,] domen, string imageColor)
        {
            int N = rang.GetLength(0);
            Color colorDomen, colorRang;

            if (imageColor.Equals("rgb"))
            {
                double s_Red = 0, o_Red = 0, D_Red = 0, R_Red = 0, a_Red = 0, b_Red = 0;
                double s_Green = 0, o_Green = 0, D_Green = 0, R_Green = 0, a_Green = 0, b_Green = 0;
                double s_Blue = 0, o_Blue = 0, D_Blue = 0, R_Blue = 0, a_Blue = 0, b_Blue = 0;

                for (int i = 0; i < N; i++)
                    for (int j = 0; j < N; j++)
                    {
                        colorDomen = Color.FromArgb(domen[i, j]);
                        colorRang = Color.FromArgb(rang[i, j]);
                        R_Red = R_Red + colorRang.R;
                        D_Red = D_Red + colorDomen.R;
                        R_Green = R_Green + colorRang.G;
                        D_Green = D_Green + colorDomen.G;
                        R_Blue = R_Blue + colorRang.B;
                        D_Blue = D_Blue + colorDomen.B;
                    }

                R_Red = R_Red / (N * N);
                D_Red = D_Red / (N * N);
                R_Green = R_Green / (N * N);
                D_Green = D_Green / (N * N);
                R_Blue = R_Blue / (N * N);
                D_Blue = D_Blue / (N * N);

                for (int i = 0; i < N; i++)
                    for (int j = 0; j < N; j++)
                    {
                        colorDomen = Color.FromArgb(domen[i, j]);
                        colorRang = Color.FromArgb(rang[i, j]);
                        a_Red = a_Red + (colorDomen.R - D_Red) * (colorRang.R - R_Red);
                        b_Red = b_Red + (colorDomen.R - D_Red) * (colorDomen.R - D_Red);
                        a_Green = a_Green + (colorDomen.G - D_Green) * (colorRang.G - R_Green);
                        b_Green = b_Green + (colorDomen.G - D_Green) * (colorDomen.G - D_Green);
                        a_Blue = a_Blue + (colorDomen.B - D_Blue) * (colorRang.B - R_Blue);
                        b_Blue = b_Blue + (colorDomen.B - D_Blue) * (colorDomen.B - D_Blue);
                    }

                s_Red = a_Red / b_Red;
                if (b_Red == 0 && a_Red == 0)
                    s_Red = 0;
                o_Red = R_Red - s_Red * D_Red;

                s_Green = a_Green / b_Green;
                if (b_Green == 0 && a_Green == 0)
                    s_Green = 0;
                o_Green = R_Green - s_Green * D_Green;

                s_Blue = a_Blue / b_Blue;
                if (b_Blue == 0 && a_Blue == 0)
                    s_Blue = 0;
                o_Blue = R_Blue - s_Blue * D_Blue;

                return new double[] { s_Red, o_Red, s_Green, o_Green, s_Blue, o_Blue };
            }
            else
            {
                double s_Y = 0, o_Y = 0, D_Y = 0, R_Y = 0, a_Y = 0, b_Y = 0;
                double s_I = 0, o_I = 0, D_I = 0, R_I = 0, a_I = 0, b_I = 0;
                double s_Q = 0, o_Q = 0, D_Q = 0, R_Q = 0, a_Q = 0, b_Q = 0;

                for (int i = 0; i < N; i++)
                    for (int j = 0; j < N; j++)
                    {
                        colorDomen = Color.FromArgb(domen[i, j]);
                        colorRang = Color.FromArgb(rang[i, j]);
                        R_Y = R_Y + getY(colorRang);
                        D_Y = D_Y + getY(colorDomen);
                        R_I = R_I + getI(colorRang);
                        D_I = D_I + getI(colorDomen);
                        R_Q = R_Q + getQ(colorRang);
                        D_Q = D_Q + getQ(colorDomen);
                    }

                R_Y = R_Y / (N * N);
                D_Y = D_Y / (N * N);
                R_I = R_I / (N * N);
                D_I = D_I / (N * N);
                R_Q = R_Q / (N * N);
                D_Q = D_Q / (N * N);

                for (int i = 0; i < N; i++)
                    for (int j = 0; j < N; j++)
                    {
                        colorDomen = Color.FromArgb(domen[i, j]);
                        colorRang = Color.FromArgb(rang[i, j]);
                        a_Y = a_Y + (getY(colorDomen) - D_Y) * (getY(colorRang) - R_Y);
                        b_Y = b_Y + (getY(colorDomen) - D_Y) * (getY(colorDomen) - D_Y);
                        a_I = a_I + (getI(colorDomen) - D_I) * (getI(colorRang) - R_I);
                        b_I = b_I + (getI(colorDomen) - D_I) * (getI(colorDomen) - D_I);
                        a_Q = a_Q + (getQ(colorDomen) - D_Q) * (getQ(colorRang) - R_Q);
                        b_Q = b_Q + (getQ(colorDomen) - D_Q) * (getQ(colorDomen) - D_Q);
                    }

                s_Y = a_Y / b_Y;
                if (b_Y == 0 && a_Y == 0)
                    s_Y = 0;
                o_Y = R_Y - s_Y * D_Y;

                s_I = a_I / b_I;
                if (b_I == 0 && a_I == 0)
                    s_I = 0;
                o_I = R_I - s_I * D_I;

                s_Q = a_Q / b_Q;
                if (b_Q == 0 && a_Q == 0)
                    s_Q = 0;
                o_Q = R_Q - s_Q * D_Q;

                return new double[] { s_Y, o_Y, s_I, o_I, s_Q, o_Q };
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

        public int[,] createEtalon(int k)
        {

            Bitmap bitmap = new Bitmap(r, r);
            Color color;// = Color.White;

            /*if (k == 1)
            { 
                for (int i = 0; i < bitmap.Width; i++)
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        if (i % 2 != 0 && j % 2 != 0)
                            bitmap.SetPixel(i, j, Color.White);
                        else bitmap.SetPixel(i, j, Color.Black);
                    }
            }
            else if (k == 2)
            {
                int h = 256 / r;
                for (int i = 0; i < bitmap.Width; i++)
                    for (int j = 0; j < bitmap.Height; j++)
                    { 
                        bitmap.SetPixel(i, j, Color.FromArgb(h * j, h * j, h * j));
                    }
            }
            else if (k == 3)
            {
                int h = 256 / (2*r);
                for (int i = 0; i < bitmap.Width; i++)
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        if(j<r/2)
                            bitmap.SetPixel(i, j, Color.FromArgb(h * j, h * j, h * j));
                        else bitmap.SetPixel(i, j, Color.FromArgb(h * (r-j-1), h * (r - j - 1), h * (r - j - 1)));
                    }
            }
            else if (k == 4)
            {
                for (int i = 0; i < bitmap.Width; i++)
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        if ((i%2+j%2)%2==0)
                            bitmap.SetPixel(i, j, Color.Black);
                        else bitmap.SetPixel(i, j, Color.White);
                    }
            }*/

            if (k == 1)
            {
                for (int i = 0; i < bitmap.Width; i++)
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        if (j < r / 2)
                            bitmap.SetPixel(i, j, Color.LightGray);
                        else bitmap.SetPixel(i, j, Color.Gray);
                    }
            }
            else if (k == 2)
            {
                int h = 256 / r;
                for (int i = 0; i < bitmap.Width; i++)
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        if ((i + j) < (r - 1))
                            bitmap.SetPixel(i, j, Color.LightGray);
                        else bitmap.SetPixel(i, j, Color.Gray);
                    }
            }
            else if (k == 3)
            {
                int h = 256 / (2 * r);
                for (int i = 0; i < bitmap.Width; i++)
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        if (((i + j) < (r - 1)) || ((i + j) > r))
                            bitmap.SetPixel(i, j, Color.Gray);
                        else bitmap.SetPixel(i, j, Color.LightGray);
                    }
            }
            else if (k == 4)
            {
                for (int i = 0; i < bitmap.Width; i++)
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        if (((i + j) < (r - 1)) || ((i + j) > r))
                            bitmap.SetPixel(i, j, Color.LightGray);
                        else bitmap.SetPixel(i, j, Color.Gray);
                    }
            }
            else if (k == 5)
            {
                for (int i = 0; i < bitmap.Width; i++)
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        if ((i % 2 + j % 2) % 2 == 0)
                            bitmap.SetPixel(i, j, Color.Gray);
                        else bitmap.SetPixel(i, j, Color.LightGray);
                    }
            }
            else if (k == 101)
            {
                for (int i = 0; i < bitmap.Width; i++)
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        if (j < r / 2)
                            bitmap.SetPixel(i, j, Color.LightGray);
                        else bitmap.SetPixel(i, j, Color.Gray);
                    }
            }


            int[,] pixels = new int[r, r];

            //получаем массив интовых чисел из изображения
            for (int i = 0; i < r; i++)//строки
                for (int j = 0; j < r; j++)//столбцы
                {
                    pixels[i, j] = bitmap.GetPixel(j, i).ToArgb();//bi.getRGB(j, i);
                }


            try
            {
                String name = "Etalon " + k;
                bitmap.Save("D:\\университет\\диплом\\bloks\\" + name + ".jpg");
                //Button5.Text = "Saved file.";
            }
            catch (Exception)
            {
                //MessageBox.Show("There was a problem saving the file." +
                //"Check the file permissions.");
            }

            return pixels;
        }

    }
}
