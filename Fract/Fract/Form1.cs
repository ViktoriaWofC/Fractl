using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fract
{
    public partial class Form1 : Form
    {
        Bitmap bitStart, bitEnd;

        int[,] pixels;
        int n, m;
        //Compress compress;
        Classification classification;
        Compression compress;
        List<Rang> rangList;
        List<Rang> rangListR;
        List<Rang> rangListG;
        List<Rang> rangListB;
        List<Rang> rangListY;
        List<Rang> rangListI;
        List<Rang> rangListQ;
        Decompress decompress;
        

        public Form1()
        {
            InitializeComponent();
            comboBoxClassif.SelectedIndex = 0;
            comboBoxSearchDomen.SelectedIndex = 0;
            comboBoxColor.SelectedIndex = 0;
        }

        private void buttonDecompress_Click(object sender, EventArgs e)
        {
            String fileName = Environment.CurrentDirectory;
            switch (Convert.ToString(comboBoxBaseImage.SelectedItem))
            {
                case "Белое":
                    {
                        fileName += @"\baseWhite.jpg";
                    }
                    break;
                case "Черное":
                    {
                        fileName += @"\baseBlack.jpg";
                    }
                    break;
                case "Клеточка большая":
                    {
                        fileName += @"\baseCletBig.jpg";
                    }
                    break;
                case "Клеточка маленькая":
                    {
                        fileName += @"\baseCletLittle.jpg";
                    }
                    break;

            }

            try {
                Bitmap bEnd = new Bitmap(Image.FromFile(fileName));
                //bitEnd = new Bitmap(Image.FromFile(fileName));
                bitEnd = new Bitmap(bitStart.Width, bitStart.Height);
                int width = bitStart.Width, height = bitStart.Height;
                for (int i = 0; i < width; i++)
                    for (int j = 0; j < height; j++)
                        bitEnd.SetPixel(i, j, bEnd.GetPixel(i, j));

                pictureBoxEndImage.Image = bitEnd;

                DecompressFunc();

            }
            catch (System.IO.FileNotFoundException ec)
            {
                string message = "Выберите базовое изображение";
                string caption = "Error base image";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons);
                //if (result == System.Windows.Forms.DialogResult.OK)
                //{
                //    this.Close();
                //}
            }
        }              

        public void DecompressFunc()
        {
            int r;// = Convert.ToInt32(numberRangSize.Value);
            string imageColor = "";
            if (radioButtonGray.Checked)
            {
                r = Convert.ToInt32(numberRangSize.Value);
                decompress = new Decompress(rangList, bitEnd, r);
                imageColor = "gray";
            }
            else {
                r = Convert.ToInt32(numberRangSize.Value);
                decompress = new Decompress(rangListR, rangListG, rangListB, rangListY, rangListI, rangListQ, bitEnd, r);
                if (comboBoxColor.SelectedIndex == 0)
                    imageColor = "rgb";
                else if (comboBoxColor.SelectedIndex == 1)
                    imageColor = "yiq";
            }            

            int col = Convert.ToInt32(numberIteracDecompr.Value);

            DateTime t1 = DateTime.Now;
            bitEnd = decompress.decompressImage(col,imageColor);
            DateTime t2 = DateTime.Now;

            String ss = "decompress time :" + (t2 - t1);

            String s = ss + " ";


            //String str = "sdf";

            pictureBoxEndImage.Image = bitEnd;
            pictureBoxEndImage.Width = bitStart.Width;
            pictureBoxEndImage.Height = bitStart.Height;

            double sko = 0;
            if (imageColor.Equals("gray"))
                sko = getSKO();
            else if (imageColor.Equals("rgb"))
                sko = getSKOColor();
            else if (imageColor.Equals("yiq"))
                sko = getSKOColor();

            s = s + "  sko: " + sko;

            labelDecompressCharacteristic.Text = s;


            // чтение из файла
            string lon;
            

            //File.WriteAllLines(@"bat.txt", stringList);
            lon = File.ReadAllText(@"bat.txt");
            //using (FileStream fstream = File.OpenRead(@"C:\SomeDir\noname\note.txt"))
            //{
            //    // преобразуем строку в байты
            //    byte[] array = new byte[fstream.Length];
            //    // считываем данные
            //    fstream.Read(array, 0, array.Length);
            //    // декодируем байты в строку
            //    string textFromFile = System.Text.Encoding.Default.GetString(array);
            //    Console.WriteLine("Текст из файла: {0}", textFromFile);
            //}
            using (BinaryReader reader = new BinaryReader(File.Open(@"bat.bat", FileMode.Open)))
            {
                lon = reader.ReadString();
            }

        }

        private void buttonCompress_Click(object sender, EventArgs e)
        {
            CompressFunc();
        }

        public void CompressFunc()
        {
            int r;// = Convert.ToInt32(numberRangSize.Value);
            int eps = Convert.ToInt32(numberCoefCompress.Value);

            string imageColor = "";            

            if (radioButtonGray.Checked)
            {
                r = Convert.ToInt32(numberRangSize.Value);
                imageColor = "gray";
            }
            else {
                r = Convert.ToInt32(numberRangSize.Value);
                if (comboBoxColor.SelectedIndex == 0)
                    imageColor = "rgb";
                else if (comboBoxColor.SelectedIndex == 1)
                    imageColor = "yiq";
            }

            //if Classification
            
            //
            int argb = 0;
            Color color;
            int f;

            m = bitStart.Width;
            n = bitStart.Height;
            pixels = new int[n, m];

            //получаем массив интовых чисел из изображения
            for (int i = 0; i < n; i++)//строки
                for (int j = 0; j < m; j++)//столбцы   
                    pixels[i, j] = bitStart.GetPixel(j, i).ToArgb();//bi.getRGB(j, i);




            String s = "";

            //проверем, использовать классификацию или нет
            //compress = new Compress(pixels, r, eps);
            if (comboBoxClassif.SelectedIndex == 0)
                compress = new Compress(pixels, r, eps);
            else if (comboBoxClassif.SelectedIndex == 1)
            {
                classification = new ClassificationCentrMass(Convert.ToString(comboBoxClassif.SelectedItem));
                compress = new CompressClassification(pixels, r, eps, classification);
            }
            else if (comboBoxClassif.SelectedIndex == 2)
            {
                classification = new ClassificationDifference(Convert.ToString(comboBoxClassif.SelectedItem));
                compress = new CompressClassification(pixels, r, eps, classification);
            }
            //CompressQuadro compr = new CompressQuadro(pixels, r, eps);

            //проверем способ поиска доменного блока
            string searcDomen = "";
            if (comboBoxSearchDomen.SelectedIndex == 0)
                searcDomen = "first<eps";
            else if (comboBoxSearchDomen.SelectedIndex == 1)
                searcDomen = "min";
            else if (comboBoxSearchDomen.SelectedIndex == 2)
                searcDomen = "min and <eps";
            else if (comboBoxSearchDomen.SelectedIndex == 3)
                searcDomen = "test";
            else if (comboBoxSearchDomen.SelectedIndex == 4)
                searcDomen = "etalons";


            DateTime t1 = DateTime.Now;//System.currentTimeMillis();
            compress.compressImage(searcDomen,imageColor);            
            DateTime t2 = DateTime.Now;//System.currentTimeMillis();
            rangList = compress.getRangList();
            rangListR = compress.getRangListComponent("R");
            rangListG = compress.getRangListComponent("G");
            rangListB = compress.getRangListComponent("B");
            rangListY = compress.getRangListComponent("Y");
            rangListI = compress.getRangListComponent("I");
            rangListQ = compress.getRangListComponent("Q");


            String ss = "compress time :" + (t2 - t1);
            s += "Размер изображения: "+bitStart.Width+" x "+bitStart.Height+"\n";
            s += ss + " ";

            String str = "!!!";
            labelCompressCharacteristic.Text = s;




            ///
            //
            if (imageColor.Equals("gray"))
                saveResult();
            else if (imageColor.Equals("rgb"))
                saveResultRGB();
            else if (imageColor.Equals("yiq"))
                saveResultYIQ();



            //

            compress.SaveSumCompare();

        }

        private void numberCoefCompress_ValueChanged(object sender, EventArgs e)
        {
            numberCoefCompressBar.Value = Convert.ToInt32(numberCoefCompress.Value);
        }

        private void numberCoefCompressBar_ValueChanged(object sender, EventArgs e)
        {
            numberCoefCompress.Value = Convert.ToDecimal(numberCoefCompressBar.Value);
        }

        /// ////////////////////////////////
        /// 

        public double getSKO()
        {
            double sko,val;

            m = bitStart.Width;
            n = bitStart.Height;
            int[,] pixelsStart = new int[n, m];
            int[,] pixelsEnd = new int[n, m];

            //получаем массив интовых чисел из изображения
            for (int i = 0; i < n; i++)//строки
                for (int j = 0; j < m; j++)//столбцы  
                {
                    pixelsStart[i, j] = bitStart.GetPixel(j, i).ToArgb();//bi.getRGB(j, i);
                    pixelsEnd[i, j] = bitEnd.GetPixel(j, i).ToArgb();//bi.getRGB(j, i);
                }

            sko = 0;
            int k = 0;
            Color colorRang, colorDomen;
            for (int i = 0; i < n; i++)//строки
            {
                val = 0;
                for (int j = 0; j < m; j++)//столбцы  
                {
                    colorDomen = Color.FromArgb(pixelsStart[i, j]);
                    colorRang = Color.FromArgb(pixelsEnd[i, j]);
                    val = colorDomen.R - colorRang.R;
                    sko = sko + val * val;
                }
            }
            sko = sko / (n * m);

            return sko;
        }

        public double getSKOColor()
        {
            double skoR,skoG,skoB, val;

            m = bitStart.Width;
            n = bitStart.Height;
            int[,] pixelsStart = new int[n, m];
            int[,] pixelsEnd = new int[n, m];

            //получаем массив интовых чисел из изображения
            for (int i = 0; i < n; i++)//строки
                for (int j = 0; j < m; j++)//столбцы  
                {
                    pixelsStart[i, j] = bitStart.GetPixel(j, i).ToArgb();//bi.getRGB(j, i);
                    pixelsEnd[i, j] = bitEnd.GetPixel(j, i).ToArgb();//bi.getRGB(j, i);
                }

            skoR = 0;
            skoG = 0;
            skoB = 0;
            int k = 0;
            Color colorRang, colorDomen;
            for (int i = 0; i < n; i++)//строки
            {
                val = 0;
                for (int j = 0; j < m; j++)//столбцы  
                {
                    colorDomen = Color.FromArgb(pixelsStart[i, j]);
                    colorRang = Color.FromArgb(pixelsEnd[i, j]);
                    val = colorDomen.R - colorRang.R;
                    skoR = skoR + val * val;
                    val = colorDomen.G - colorRang.G;
                    skoG = skoG + val * val;
                    val = colorDomen.B - colorRang.B;
                    skoB = skoB + val * val;
                }
            }
            skoR = skoR / (n * m);
            skoG = skoG / (n * m);
            skoB = skoB / (n * m);

            double sko = (skoR + skoG + skoB) / 3;
            return sko;
        }

        public void saveResult()
        {
            long[] longList = new long[rangList.Count];
            string[] stringList = new string[rangList.Count];
            string lon = "", lonTest = "";
            int ii = 0;
            long d = 0;
            int R = Convert.ToInt32(numberRangSize.Value);
            int epsilon = Convert.ToInt32(numberCoefCompress.Value);
            foreach (Rang rang in rangList)
            {
                //преобразование из Rang в число long
                //d = Convert.ToInt32(rang.getBright()) + (rang.getY0() << 10) + (rang.getX0() << 21) + (rang.getK() << 25) + (rang.getAfinn() << 29) + (rang.getY() << 40) + (rang.getX() << 51);
                d =  (rang.getY0() << 10) + (rang.getX0() << 21) + (rang.getK() << 25) + (rang.getAfinn() << 29) + (rang.getY() << 40) + (rang.getX() << 51);
                //longList.add(d);
                longList[ii] = d;
                stringList[ii] = Convert.ToString(d);
                lon += d + " ";
                //
                //lonTest += rang.getX() + " " + rang.getY() + " " + rang.getAfinn() + " " + rang.getK() + " " + rang.getX0() + " " + rang.getY0() + " " + rang.getBright() + "\r\n";
                lonTest += ii+")   "+ rang.getX() + " " + rang.getY() + " " + rang.getAfinn() + " " + rang.getK() + " " + rang.getS() + " " + rang.getO() + " " + "\r\n";
                ii++;
            }

            DateTime date = DateTime.Now;
            string dateString = date.Day.ToString() + "."
                                + date.Month.ToString() + "." 
                                + date.Year.ToString() + ""
                                + "   "
                                + date.Hour.ToString() + "."
                                + date.Minute.ToString() + "."
                                + date.Second.ToString() + "";

            String name = dateString + "___size=" + bitStart.Height + "___R=" + R + "___E="+epsilon;

            System.IO.File.WriteAllText(@"D:\\университет\\диплом\\bloks_files\\" + name + ".txt", lon);
            //System.IO.File.WriteAllText(@"D:\\университет\\диплом\\bloks_files\\" + name + "Test.txt", lonTest);

            //запись в файл
            //File.WriteAllLines(@"bat.txt", stringList);
            //File.WriteAllText(@"D:\\университет\\диплом\\bloks_files\\" + name+".txt", lon);
            //File.WriteAllText(@"D:\\университет\\диплом\\bloks_files\\" + name + "Test.txt", lonTest);
            //using (FileStream fstream = new FileStream(@"bat.txt", FileMode.Create))
            //{
            //    // преобразуем строку в байты
            //    byte[] array = System.Text.Encoding.Default.GetBytes(lon);
            //    // запись массива байтов в файл
            //    fstream.Write(array, 0, array.Length);
            //    //Console.WriteLine("Текст записан в файл");
            //}

            //using (BinaryWriter writer = new BinaryWriter(File.Open(@"D:\\университет\\диплом\\bloks_files\\" + name + ".bat", FileMode.Create)))
            //{
            //    writer.Write(lon);
            //}

        }

        public void saveResultRGB()
        {
            //long[] longList = new long[rangList.Count*3];
            //string[] stringList = new string[rangList.Count*3];
            string lon = "", lonTest = "";
            int ii = 0;
            long d = 0;
            int R = Convert.ToInt32(numberRangSize.Value);
            int epsilon = Convert.ToInt32(numberCoefCompress.Value);
            foreach (Rang rang in rangListR)
            {
                //преобразование из Rang в число long
                //d = Convert.ToInt32(rang.getBright()) + (rang.getY0() << 10) + (rang.getX0() << 21) + (rang.getK() << 25) + (rang.getAfinn() << 29) + (rang.getY() << 40) + (rang.getX() << 51);
                d = (rang.getY0() << 10) + (rang.getX0() << 21) + (rang.getK() << 25) + (rang.getAfinn() << 29) + (rang.getY() << 40) + (rang.getX() << 51);
                //longList.add(d);
                //longList[ii] = d;
                //stringList[ii] = Convert.ToString(d);
                lon += d + " ";
                //
                //lonTest += rang.getX() + " " + rang.getY() + " " + rang.getAfinn() + " " + rang.getK() + " " + rang.getX0() + " " + rang.getY0() + " " + rang.getBright() + "\r\n";
                lonTest += ii + ")   " + rang.getX() + " " + rang.getY() + " " + rang.getAfinn() + " " + rang.getK() + " " + rang.getS() + " " + rang.getO() + " " + "\r\n";
                ii++;
            }

            foreach (Rang rang in rangListG)
            {
                //преобразование из Rang в число long
                //d = Convert.ToInt32(rang.getBright()) + (rang.getY0() << 10) + (rang.getX0() << 21) + (rang.getK() << 25) + (rang.getAfinn() << 29) + (rang.getY() << 40) + (rang.getX() << 51);
                d = (rang.getY0() << 10) + (rang.getX0() << 21) + (rang.getK() << 25) + (rang.getAfinn() << 29) + (rang.getY() << 40) + (rang.getX() << 51);
                //longList.add(d);
                //longList[ii] = d;
                //stringList[ii] = Convert.ToString(d);
                lon += d + " ";
                //
                //lonTest += rang.getX() + " " + rang.getY() + " " + rang.getAfinn() + " " + rang.getK() + " " + rang.getX0() + " " + rang.getY0() + " " + rang.getBright() + "\r\n";
                lonTest += ii + ")   " + rang.getX() + " " + rang.getY() + " " + rang.getAfinn() + " " + rang.getK() + " " + rang.getS() + " " + rang.getO() + " " + "\r\n";
                ii++;
            }

            foreach (Rang rang in rangListB)
            {
                //преобразование из Rang в число long
                //d = Convert.ToInt32(rang.getBright()) + (rang.getY0() << 10) + (rang.getX0() << 21) + (rang.getK() << 25) + (rang.getAfinn() << 29) + (rang.getY() << 40) + (rang.getX() << 51);
                d = (rang.getY0() << 10) + (rang.getX0() << 21) + (rang.getK() << 25) + (rang.getAfinn() << 29) + (rang.getY() << 40) + (rang.getX() << 51);
                //longList.add(d);
                //longList[ii] = d;
                //stringList[ii] = Convert.ToString(d);
                lon += d + " ";
                //
                //lonTest += rang.getX() + " " + rang.getY() + " " + rang.getAfinn() + " " + rang.getK() + " " + rang.getX0() + " " + rang.getY0() + " " + rang.getBright() + "\r\n";
                lonTest += ii + ")   " + rang.getX() + " " + rang.getY() + " " + rang.getAfinn() + " " + rang.getK() + " " + rang.getS() + " " + rang.getO() + " " + "\r\n";
                ii++;
            }

            DateTime date = DateTime.Now;
            string dateString = date.Day.ToString() + "."
                                + date.Month.ToString() + "."
                                + date.Year.ToString() + ""
                                + "   "
                                + date.Hour.ToString() + "."
                                + date.Minute.ToString() + "."
                                + date.Second.ToString() + "";

            String name = dateString + "_RGB___size=" + bitStart.Height + "___R=" + R + "___E=" + epsilon;

            System.IO.File.WriteAllText(@"D:\\университет\\диплом\\bloks_files\\" + name + ".txt", lon);
            //System.IO.File.WriteAllText(@"D:\\университет\\диплом\\bloks_files\\" + name + "Test.txt", lonTest);

            //запись в файл
            //File.WriteAllLines(@"bat.txt", stringList);
            //File.WriteAllText(@"D:\\университет\\диплом\\bloks_files\\" + name+".txt", lon);
            //File.WriteAllText(@"D:\\университет\\диплом\\bloks_files\\" + name + "Test.txt", lonTest);
            //using (FileStream fstream = new FileStream(@"bat.txt", FileMode.Create))
            //{
            //    // преобразуем строку в байты
            //    byte[] array = System.Text.Encoding.Default.GetBytes(lon);
            //    // запись массива байтов в файл
            //    fstream.Write(array, 0, array.Length);
            //    //Console.WriteLine("Текст записан в файл");
            //}
            //using (BinaryWriter writer = new BinaryWriter(File.Open(@"D:\\университет\\диплом\\bloks_files\\" + name + ".bat", FileMode.Create)))
            //{
            //    writer.Write(lon);
            //}

        }

        public void saveResultYIQ()
        {
            //long[] longList = new long[rangList.Count*3];
            //string[] stringList = new string[rangList.Count*3];
            string lon = "", lonTest = "";
            int ii = 0;
            long d = 0;
            int R = Convert.ToInt32(numberRangSize.Value);
            int epsilon = Convert.ToInt32(numberCoefCompress.Value);
            foreach (Rang rang in rangListY)
            {
                //преобразование из Rang в число long
                //d = Convert.ToInt32(rang.getBright()) + (rang.getY0() << 10) + (rang.getX0() << 21) + (rang.getK() << 25) + (rang.getAfinn() << 29) + (rang.getY() << 40) + (rang.getX() << 51);
                d = (rang.getY0() << 10) + (rang.getX0() << 21) + (rang.getK() << 25) + (rang.getAfinn() << 29) + (rang.getY() << 40) + (rang.getX() << 51);
                //longList.add(d);
                //longList[ii] = d;
                //stringList[ii] = Convert.ToString(d);
                lon += d + " ";
                //
                //lonTest += rang.getX() + " " + rang.getY() + " " + rang.getAfinn() + " " + rang.getK() + " " + rang.getX0() + " " + rang.getY0() + " " + rang.getBright() + "\r\n";
                lonTest += ii + ")   " + rang.getX() + " " + rang.getY() + " " + rang.getAfinn() + " " + rang.getK() + " " + rang.getS() + " " + rang.getO() + " " + "\r\n";
                ii++;
            }

            foreach (Rang rang in rangListI)
            {
                //преобразование из Rang в число long
                //d = Convert.ToInt32(rang.getBright()) + (rang.getY0() << 10) + (rang.getX0() << 21) + (rang.getK() << 25) + (rang.getAfinn() << 29) + (rang.getY() << 40) + (rang.getX() << 51);
                d = (rang.getY0() << 10) + (rang.getX0() << 21) + (rang.getK() << 25) + (rang.getAfinn() << 29) + (rang.getY() << 40) + (rang.getX() << 51);
                //longList.add(d);
                //longList[ii] = d;
                //stringList[ii] = Convert.ToString(d);
                lon += d + " ";
                //
                //lonTest += rang.getX() + " " + rang.getY() + " " + rang.getAfinn() + " " + rang.getK() + " " + rang.getX0() + " " + rang.getY0() + " " + rang.getBright() + "\r\n";
                lonTest += ii + ")   " + rang.getX() + " " + rang.getY() + " " + rang.getAfinn() + " " + rang.getK() + " " + rang.getS() + " " + rang.getO() + " " + "\r\n";
                ii++;
            }

            foreach (Rang rang in rangListQ)
            {
                //преобразование из Rang в число long
                //d = Convert.ToInt32(rang.getBright()) + (rang.getY0() << 10) + (rang.getX0() << 21) + (rang.getK() << 25) + (rang.getAfinn() << 29) + (rang.getY() << 40) + (rang.getX() << 51);
                d = (rang.getY0() << 10) + (rang.getX0() << 21) + (rang.getK() << 25) + (rang.getAfinn() << 29) + (rang.getY() << 40) + (rang.getX() << 51);
                //longList.add(d);
                //longList[ii] = d;
                //stringList[ii] = Convert.ToString(d);
                lon += d + " ";
                //
                //lonTest += rang.getX() + " " + rang.getY() + " " + rang.getAfinn() + " " + rang.getK() + " " + rang.getX0() + " " + rang.getY0() + " " + rang.getBright() + "\r\n";
                lonTest += ii + ")   " + rang.getX() + " " + rang.getY() + " " + rang.getAfinn() + " " + rang.getK() + " " + rang.getS() + " " + rang.getO() + " " + "\r\n";
                ii++;
            }

            DateTime date = DateTime.Now;
            string dateString = date.Day.ToString() + "."
                                + date.Month.ToString() + "."
                                + date.Year.ToString() + ""
                                + "   "
                                + date.Hour.ToString() + "."
                                + date.Minute.ToString() + "."
                                + date.Second.ToString() + "";

            String name = dateString + "_YIQ___size=" + bitStart.Height + "___R=" + R + "___E=" + epsilon;

            System.IO.File.WriteAllText(@"D:\\университет\\диплом\\bloks_files\\" + name + ".txt", lon);
            //System.IO.File.WriteAllText(@"D:\\университет\\диплом\\bloks_files\\" + name + "Test.txt", lonTest);

            //запись в файл
            //File.WriteAllLines(@"bat.txt", stringList);
            //File.WriteAllText(@"D:\\университет\\диплом\\bloks_files\\" + name+".txt", lon);
            //File.WriteAllText(@"D:\\университет\\диплом\\bloks_files\\" + name + "Test.txt", lonTest);
            //using (FileStream fstream = new FileStream(@"bat.txt", FileMode.Create))
            //{
            //    // преобразуем строку в байты
            //    byte[] array = System.Text.Encoding.Default.GetBytes(lon);
            //    // запись массива байтов в файл
            //    fstream.Write(array, 0, array.Length);
            //    //Console.WriteLine("Текст записан в файл");
            //}
            //using (BinaryWriter writer = new BinaryWriter(File.Open(@"D:\\университет\\диплом\\bloks_files\\" + name + ".bat", FileMode.Create)))
            //{
            //    writer.Write(lon);
            //}

        }

        public int[] getClassesCounts(int R)
        {
            classification = new ClassificationCentrMass(Convert.ToString(comboBoxClassif.SelectedItem));
            //classification = new ClassificationDifference(Convert.ToString(comboBoxClassif.SelectedItem));


            m = bitStart.Width;
            n = bitStart.Height;
            int[,] pixelsStart = new int[n, m];

            //получаем массив интовых чисел из изображения
            for (int i = 0; i < n; i++)//строки
                for (int j = 0; j < m; j++)//столбцы  
                {
                    pixelsStart[i, j] = bitStart.GetPixel(j, i).ToArgb();//bi.getRGB(j, i);
                }

            int id = 0;
            int jd = 0;


            int D = R * 2;//размер доменного блока
            int N = pixelsStart.GetLength(0) / R;//количество блоков по высоте
            int M = pixelsStart.GetLength(1) / R;//количество блоков по ширине
            int[,] domenBig = new int[D, D];//доменный блок

            int c1 = 0,c2=0,c3=0,c4=0,c5 = 0;
            while (id < N - 1)
            {
                jd = 0;
                while (jd < M - 1) 
                {

                    for (int i = 0; i < D; i++)
                        for (int j = 0; j < D; j++)
                        {
                            domenBig[i, j] = pixelsStart[R * id + i, R * jd + j];
                            //domenBig[i, j] = pix[id + i, jd + j];
                        }

                    int res =classification.getClass(domenBig);
                    if (res == 0) c1++;
                    else if (res == 1) c2++;
                    else if (res == 2) c3++;
                    else if (res == 3) c4++;
                    else c5++;
                    jd++;
                }
                id++;
            }

            return new int[] {c1,c2,c3,c4,c5};
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            int[] classes = getClassesCounts(Convert.ToInt32(numberRangSize.Value));

            string text = "";
            for (int i = 0; i < classes.Length; i++)
                text += classes[i]+ "  \n  ";

            textBoxTest.Text =text;

            //m = bitStart.Width;
            //n = bitStart.Height;
            //pixels = new int[n, m];

            ////получаем массив интовых чисел из изображения
            //for (int i = 0; i < n; i++)//строки
            //    for (int j = 0; j < m; j++)//столбцы   
            //        pixels[i, j] = bitStart.GetPixel(j, i).ToArgb();//bi.getRGB(j, i);


            //int[,] pix = new int[pixels.GetLength(0), pixels.GetLength(0)];
            //for (int i = 0; i<8; i++)
            //{
            //    pix = setAfinnInt(pixels, i);
            //    printBlockExample(pix, i);
            //}

            //int number = Convert.ToInt32(textBoxRangNumber.Text);

            ////ShortCompressFunc();
            //Color col;
            //string text = "";

            //Rang rrr = rangList[number];
            //text = textBoxTest.Text+"\n x="+rrr.getX()+" y="+rrr.getY();

            //textBoxTest.Text = text;



            //////////////////////////////////////////////////////////л
            //long x = 1005, y = 10, af = 3, k = 2, x0 = 15, y0 = 23, br = 123;
            //test += x + " " + y + " " + af + " " + k + " " + x0 + " " + y0 + " " + br + " \r\n";
            ////преобразование из Rang в число long
            //long d = 0;
            //d = 0;
            ////d = y0 + (x0 << 10) + (k << 21) + (af << 25) + (y << 29) + (x << 40) +(br << 51);
            //d = br + (y0 << 10) + (x0 << 21) + (k << 25) + (af << 29) + (y << 40) + (x << 51);
            //test += "" + d;
            //test += "\r\n" + (d >> 51)
            //        + " " + ((d - (x << 51)) >> 40)
            //        + " " + ((d - (x << 51) - (y << 40)) >> 29)
            //        + " " + ((d - (x << 51) - (y << 40) - (af << 29)) >> 25)
            //        + " " + ((d - (x << 51) - (y << 40) - (af << 29) - (k << 25)) >> 21)
            //        + " " + ((d - (x << 51) - (y << 40) - (af << 29) - (k << 25) - (x0 << 21)) >> 10)
            //        + " " + ((d - (x << 51) - (y << 40) - (af << 29) - (k << 25) - (x0 << 21) - (y0 << 10)));

            //test += Environment.NewLine;


            //long i1 = Convert.ToInt32(d >> 51);
            //long i2 = Convert.ToInt32((d - (i1 << 51)) >> 40);
            //long i3 = Convert.ToInt32((d - (i1 << 51) - (i2 << 40)) >> 29);
            //long i4 = Convert.ToInt32((d - (i1 << 51) - (i2 << 40) - (i3 << 29)) >> 25);
            //long i5 = Convert.ToInt32((d - (i1 << 51) - (i2 << 40) - (i3 << 29) - (i4 << 25)) >> 21);
            //long i6 = Convert.ToInt32((d - (i1 << 51) - (i2 << 40) - (i3 << 29) - (i4 << 25) - (i5 << 21)) >> 10);
            //long i7 = Convert.ToInt32((d - (i1 << 51) - (i2 << 40) - (i3 << 29) - (i4 << 25) - (i5 << 21) - (i6 << 10)));
            //test += "end " + " " + i1 + " " + i2 + " " + i3 + " " + i4 + " " + i5 + " " + i6 + " " + i7;

            //textBoxTest.Text = test;
        }

        public void printBlockExample(int[,] example, int afin)
        {
            int width = example.GetLength(1);
            Bitmap bitmap = new Bitmap(width, example.GetLength(0));
            Color color;// = Color.White;


            for (int i = 0; i < bitmap.Width; i++)
                //for (int j = 0; j < bitmap.Height; j++)
                for (int j = 0; j < example.GetLength(1); j++)
                {
                    bitmap.SetPixel(i, j, Color.FromArgb(example[j, i]));
                }

            try
            {
                String name = "Afin___" + afin;
                bitmap.Save("D:\\университет\\диплом\\bloks\\" + name + ".jpg");
                //Button5.Text = "Saved file.";
            }
            catch (Exception)
            {
                //MessageBox.Show("There was a problem saving the file." +
                //"Check the file permissions.");
            }
        }

        public bool compareBlocs(int[,] rang, int[,] domen, double epsilon)
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

                    h = (colorDomen.R - colorRang.R);

                    sum += h * h;
                }

            sum = sum / 1000;


            if (sum < epsilon)
                b = true;
            else b = false;

            return b;
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

        private void button1_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(textBoxRangNumber.Text);
            int r;// = Convert.ToInt32(numberRangSize.Value);
            int eps = Convert.ToInt32(numberCoefCompress.Value);

            if (radioButtonGray.Checked)
            {
                r = Convert.ToInt32(numberRangSize.Value);
            }
            else {
                r = 8;
                numberRangSize.Value = 8;
            }
            FormRandDetails formRandDetails = new FormRandDetails(rangList[number], pixels, r, eps);
            formRandDetails.Show();
        }


        public void ShortCompressFunc()
        {
            int number = Convert.ToInt32(textBoxTest.Text);

            Rang rang = rangList[number];

            double[] so;
            double s, o, sko;
            List<double> skoMass = new List<double>();
            int R = compress.getR() / rang.getK();//размер рангового блока
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
                    domenBig[i, j] = pixels[rang.getX() + i, rang.getY() + j];
                }

            for (int i = 0; i < R; i++)
                for (int j = 0; j < R; j++)
                {
                    rangMatr[i, j] = pixels[rang.getX0() + i, rang.getY0() + j];
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
                        colorDomen = Color.FromArgb(domen[i, j]);
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


            textBoxTest.Text = test;

        }

        private void comboBoxSearchDomen_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSearchDomen.SelectedIndex == 1)
                numberCoefCompress.Enabled = false;
            else numberCoefCompress.Enabled = true;
        }

        private void radioButtonGray_CheckedChanged(object sender, EventArgs e)
        {           
             comboBoxColor.Enabled = false;
        }

        private void radioButtonColors_CheckedChanged(object sender, EventArgs e)
        {

            comboBoxColor.Enabled = true;
        }

        /// //////////////////////////////////////////////////////////////

        ////преобразование из Rang в число long
        //long d = 0;
        ////d = y0 + (x0 << 10) + (k << 21) + (af << 25) + (y << 29) + (x << 40) +(br << 51);
        //d = br+ (y0<<10) + (x0 << 21) + (k << 25) + (af << 29) + (y << 40) + (x << 51) ;
        //    test +="" + d;
        //    test += " ! \n" + (d >> 51) + " " + ((d - (x << 51)) >> 40)
        //            + " " + ((d - (x << 51) - (y << 40)) >> 29) + " " + ((d - (x << 51) - (y << 40) - (af << 29)) >> 25)
        //            + " " + ((d - (x << 51) - (y << 40) - (af << 29) - (k << 25)) >> 21) 
        //            + " " + ((d - (x << 51) - (y << 40) - (af << 29) - (k << 25) - (x0 << 21)) >> 10) 
        //            + " " + ((d - (x << 51) - (y << 40) - (af << 29) - (k << 25) - (x0 << 21) - (y0 << 10)));
        //test += Environment.NewLine;

        //    long i1 = Convert.ToInt32(d >> 51);
        //long i2 = Convert.ToInt32((d - (i1 << 51)) >> 40);
        //long i3 = Convert.ToInt32((d - (i1 << 51) - (i2 << 40)) >> 29);
        //long i4 = Convert.ToInt32((d - (i1 << 51) - (i2 << 40) - (i3 << 29)) >> 25);
        //long i5 = Convert.ToInt32((d - (i1 << 51) - (i2 << 40) - (i3 << 29) - (i4 << 25)) >> 21);
        //long i6 = Convert.ToInt32((d - (i1 << 51) - (i2 << 40) - (i3 << 29) - (i4 << 25) - (i5 << 21)) >> 10);
        //long i7 = Convert.ToInt32((d - (i1 << 51) - (i2 << 40) - (i3 << 29) - (i4 << 25) - (i5 << 21) - (i6 << 10)));
        //test += " " + i1 + " " + i2 + " " + i3 + " " + i4 + " " + i5 + " " + i6 + " " + i7;
        //    textBoxTest.Text = test;

        //long x = 1005, y = 10, af = 3, k = 2, x0 = 15, y0 = 23, br = 123;
        ////преобразование из Rang в число long
        //long d = 0;
        //d = 0;
        //    //d = y0 + (x0 << 10) + (k << 21) + (af << 25) + (y << 29) + (x << 40) +(br << 51);
        //    d = br+ (y0<<10) + (x0 << 21) + (k << 25) + (af << 29) + (y << 40) + (x << 51) ;
        //    test +="" + d;
        //    test += " ! \n" + (d >> 51) 
        //            + " " + ((d - (x << 51)) >> 40)
        //            + " " + ((d - (x << 51) - (y << 40)) >> 29) 
        //            + " " + ((d - (x << 51) - (y << 40) - (af << 29)) >> 25)
        //            + " " + ((d - (x << 51) - (y << 40) - (af << 29) - (k << 25)) >> 21) 
        //            + " " + ((d - (x << 51) - (y << 40) - (af << 29) - (k << 25) - (x0 << 21)) >> 10) 
        //            + " " + ((d - (x << 51) - (y << 40) - (af << 29) - (k << 25) - (x0 << 21) - (y0 << 10)));

        //    test += Environment.NewLine;

        //    d = Convert.ToInt32(rangList[0].getBright())
        //        + (rangList[100].getY0() << 10)
        //        + (rangList[100].getX0() << 21)
        //        + (rangList[100].getK() << 25)
        //        + (rangList[100].getAfinn() << 29)
        //        + (rangList[100].getY() << 40)
        //        + (rangList[100].getX() << 51);

        //    test = "start " + Convert.ToInt32(rangList[0].getBright()) + " "
        //        + (rangList[100].getY0()) + " "
        //        + (rangList[100].getX0()) + " "
        //        + (rangList[100].getK()) + " "
        //        + (rangList[100].getAfinn()) + " "
        //        + (rangList[100].getY()) + " "
        //        + (rangList[100].getX());

        //    test += Environment.NewLine;
        //    test += "long " + d;
        //    test += Environment.NewLine;



        //    long i1 = Convert.ToInt32(d >> 51);
        //long i2 = Convert.ToInt32((d - (i1 << 51)) >> 40);
        //long i3 = Convert.ToInt32((d - (i1 << 51) - (i2 << 40)) >> 29);
        //long i4 = Convert.ToInt32((d - (i1 << 51) - (i2 << 40) - (i3 << 29)) >> 25);
        //long i5 = Convert.ToInt32((d - (i1 << 51) - (i2 << 40) - (i3 << 29) - (i4 << 25)) >> 21);
        //long i6 = Convert.ToInt32((d - (i1 << 51) - (i2 << 40) - (i3 << 29) - (i4 << 25) - (i5 << 21)) >> 10);
        //long i7 = Convert.ToInt32((d - (i1 << 51) - (i2 << 40) - (i3 << 29) - (i4 << 25) - (i5 << 21) - (i6 << 10)));
        //test += "end " + " " + i1 + " " + i2 + " " + i3 + " " + i4 + " " + i5 + " " + i6 + " " + i7;

        private void buttonOpenImage_Click(object sender, EventArgs e)
        {
            if(openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String fileName = openFileDialog.FileName;
                bitStart = new Bitmap(Image.FromFile(fileName));
                                
                if (radioButtonGray.Checked)
                {
                    bitStart = getGrey(bitStart);
                }

                pictureBoxStartImage.Image = bitStart;                
            }
        }

        //получение изображение в оттенках серого
        public Bitmap getGrey(Bitmap bit)
        {
            int grey;
            Color color;
            for (int i = 0; i < bit.Width; i++)
                for (int j = 0; j < bit.Height; j++)
                {
                    grey = Convert.ToInt32(bit.GetPixel(i, j).R * 0.3 + bit.GetPixel(i, j).G * 0.59 + bit.GetPixel(i, j).B * 0.11);
                    color = Color.FromArgb(grey, grey, grey);
                    bit.SetPixel(i, j, color);
                }
            return bit;
        }
    }
}
