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
        Decompress decompress;

        public Form1()
        {
            InitializeComponent();
            comboBoxClassif.SelectedIndex = 0;
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
                bitEnd = new Bitmap(Image.FromFile(fileName));
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
            if (radioButtonBase.Checked)
                r = Convert.ToInt32(numberRangSize.Value);
            else {
                r = 8;
                numberRangSize.Value = 8;
            }


            decompress = new Decompress(rangList, bitEnd, r);

            int col = Convert.ToInt32(numberIteracDecompr.Value);

            DateTime t1 = DateTime.Now;
            bitEnd = decompress.decompressImage(col);
            DateTime t2 = DateTime.Now;

            String ss = "decompress time :" + (t2 - t1);

            String s = ss + " ";


            //String str = "sdf";

            pictureBoxEndImage.Image = bitEnd;
            pictureBoxEndImage.Width = bitStart.Width;
            pictureBoxEndImage.Height = bitStart.Height;

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

            if (radioButtonBase.Checked)
            {
                r = Convert.ToInt32(numberRangSize.Value);                
            }
            else {
                r = 8;
                numberRangSize.Value = 8;
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
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    //argb = bi.getRGB(j,i);
                    //color = new Color(argb);
                    //f = color.getRed();
                    //pixels[i][j] = f;
                    //pixels[i][j] = getRGBValue(bi, j, i);
                    pixels[i, j] = bitStart.GetPixel(j, i).ToArgb();//bi.getRGB(j, i);

                }



            String s = "";

            //проверем, использовать классификацию или нет
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

            DateTime t1 = DateTime.Now;//System.currentTimeMillis();
            //compr.compressImage();
            compress.compressImage();
            //rangList = compr.getRangList();
            rangList = compress.getRangList();
            DateTime t2 = DateTime.Now;//System.currentTimeMillis();
            String ss = "compress time :" + (t2 - t1);
            s += ss + " ";

            String str = "!!!";
            labelCompressCharacteristic.Text = s;


            ///
            long[] longList = new long[rangList.Count];
            string[] stringList = new string[rangList.Count];
            string lon = "", lonTest = "";
            int ii = 0;
            long d = 0;
            foreach (Rang rang in rangList)
            {
                //преобразование из Rang в число long
                d = Convert.ToInt32(rang.getBright()) + (rang.getY0() << 10) + (rang.getX0() << 21) + (rang.getK() << 25) + (rang.getAfinn() << 29) + (rang.getY() << 40) + (rang.getX() << 51);
                //longList.add(d);
                longList[ii] = d;
                stringList[ii] = Convert.ToString(d);
                lon += d + " ";
                //
                lonTest += rang.getX() + " "+rang.getY() + " " + rang.getAfinn() + " " + rang.getK() + " " + rang.getX0() + " "+ rang.getY0() + " " +rang.getBright()+ "\r\n";
                ii++;
            }

            //запись в файл
            //File.WriteAllLines(@"bat.txt", stringList);
            File.WriteAllText(@"bat.txt", lon);
            File.WriteAllText(@"batTest.txt", lonTest);
            //using (FileStream fstream = new FileStream(@"bat.txt", FileMode.Create))
            //{
            //    // преобразуем строку в байты
            //    byte[] array = System.Text.Encoding.Default.GetBytes(lon);
            //    // запись массива байтов в файл
            //    fstream.Write(array, 0, array.Length);
            //    //Console.WriteLine("Текст записан в файл");
            //}
            using (BinaryWriter writer = new BinaryWriter(File.Open(@"bat.bat", FileMode.Create)))
            {
                writer.Write(lon);
            }

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

        private void buttonTest_Click(object sender, EventArgs e)
        {

            string test = "";
            m = bitStart.Width;
            n = bitStart.Height;
            pixels = new int[n, m];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    //argb = bi.getRGB(j,i);
                    //color = new Color(argb);
                    //f = color.getRed();
                    //pixels[i][j] = f;
                    //pixels[i][j] = getRGBValue(bi, j, i);
                    pixels[i, j] = bitStart.GetPixel(j, i).ToArgb();//bi.getRGB(j, i);

                }

            int r;// = Convert.ToInt32(numberRangSize.Value);
            int eps = Convert.ToInt32(numberCoefCompress.Value);

            r = Convert.ToInt32(numberRangSize.Value);
            int nDom = n / r - 1;
            int mDom = m / r - 1;

            int[,] rang = new int[r, r];
            int[,] domen = new int[r*2, r*2];
            int[,] classRang = new int[n / r, m / r];
            int[,] classDomen = new int[nDom, mDom];

            if (comboBoxClassif.SelectedIndex == 1)
                classification = new ClassificationCentrMass(Convert.ToString(comboBoxClassif.SelectedItem));

            //перебор ранговых блоков
            for (int i = 0; i < n / r; i++)
                for (int j = 0; j < m / r; j++)
                {
                    //выделяем ранговый блок
                    for (int ii = 0; ii < r; ii++)
                        for (int jj = 0; jj < r; jj++)
                            rang[ii, jj] = pixels[r * i + ii, r * j + jj];

                    classRang[i, j] = classification.getClass(rang);
                }

            //перебор доменных блоков
            for (int i = 0; i < nDom; i++)
                for (int j = 0; j < mDom; j++)
                {
                    //выделяем доменных блок
                    for (int ii = 0; ii < r*2; ii++)
                        for (int jj = 0; jj < r*2; jj++)
                            domen[ii, jj] = pixels[r * i + ii, r * j + jj];

                    classDomen[i, j] = classification.getClass(domen);
                }


            test += classification.getName();
            //pictureBoxEndImage.Image = bitTest;
            textBoxTest.Text = test;
            //////////////////////////////////////////


            //for (int i = 0; i < hh; i++)
            //    for (int j = 0; j < hh; j++)
            //    {
            //        color = Color.FromArgb(domenBig[i, j]);
            //        bitTest.SetPixel(x + j, y + i, color);
            //    }

            //for (int i = 0; i < z; i++)
            //    for (int j = 0; j < z; j++)
            //    {
            //        color = Color.FromArgb(domenBr[i, j]);
            //        bitTest.SetPixel(x + j + 40, y + i, color);
            //    }

            //for (int i = 0; i < z; i++)
            //    for (int j = 0; j < z; j++)
            //    {
            //        color = Color.FromArgb(rang[i, j]);
            //        bitTest.SetPixel(x + j+70, y + i, color);
            //    }

            //pictureBoxEndImage.Image = bitTest;
            //textBoxTest.Text = test;
            //////////////////////////////////////////////////////////

            //bool rf = compareBlocs(rang, domen, 200);

            //domen = setAfinnInt(domen, 2);

            //rf = compareBlocs(rang, domen, 200);



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

                bitStart = getGrey(bitStart);

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
