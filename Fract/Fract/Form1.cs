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
        Compress compress;
        List<Rang> rangList;
        Decompress decompress;

        public Form1()
        {
            InitializeComponent();
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
            int r = Convert.ToInt32(numberRangSize.Value);
            decompress = new Decompress(rangList, bitEnd, r);

            int col = Convert.ToInt32(numberIteracDecompr.Value);

            DateTime t1 = DateTime.Now;
            bitEnd = decompress.decompressImage(col);
            DateTime t2 = DateTime.Now;

            String ss = "decompress time :" + (t2 - t1);

            String s = ss + " ";


            //String str = "sdf";

            pictureBoxEndImage.Image = bitEnd;

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
            //
            int argb = 0;
            Color color;
            int f;

            m = bitStart.Width;
            n = bitStart.Height;
            pixels = new int[n,m];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    //argb = bi.getRGB(j,i);
                    //color = new Color(argb);
                    //f = color.getRed();
                    //pixels[i][j] = f;
                    //pixels[i][j] = getRGBValue(bi, j, i);
                    pixels[i,j] = bitStart.GetPixel(j, i).ToArgb();//bi.getRGB(j, i);

                }

            int r = Convert.ToInt32(numberRangSize.Value);
            int eps = Convert.ToInt32(numberCoefCompress.Value);

            String s = "";

            compress = new Compress(pixels, r, eps);

            DateTime t1 = DateTime.Now;//System.currentTimeMillis();
            compress.compressImage();
            rangList = compress.getRangList();
            DateTime t2 = DateTime.Now;//System.currentTimeMillis();
            String ss = "compress time :" + (t2 - t1);
            s += ss + " ";

            String str = "!!!";
            labelCompressCharacteristic.Text = s;

            ///
            long[] longList = new long[rangList.Count];
            string[] stringList = new string[rangList.Count];
            string lon = "";
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
                ii++;
            }

            //запись в файл
            //File.WriteAllLines(@"bat.txt", stringList);
            File.WriteAllText(@"bat.txt", lon);
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
            
        }


        /// ////////////////////////////////

        private void buttonTest_Click(object sender, EventArgs e)
        {
            String test = "";

            //тест сжатия домен блока до рангового
            Bitmap bitTest = bitStart;
            //int hh = 32, z = hh / 2;
            int hh = bitTest.Width, z = hh / 2;
            int[,] domenBig = new int[hh, hh];
            int[,] domen = new int[z, z];
            Color color;

            for (int i = 0; i < hh; i++)
                for (int j = 0; j < hh; j++)
                {
                    domenBig[i, j] = bitTest.GetPixel(j, i).ToArgb();//bi.getRGB(j, i);
                }

            int x = bitTest.Width - z;
            int y = bitTest.Height - z;
            for (int i = 0; i < z; i++)
                for (int j = 0; j < z; j++)
                {
                    color = Color.FromArgb(domenBig[i, j]);
                    bitTest.SetPixel(x+ j, y + i, color);
                }

            pictureBoxEndImage.Image = bitTest;

            /*int d, sum,k = 1;
            for (int i = 0; i < z; i++)
                for (int j = 0; j < z; j++)
                {
                    sum = 0;
                    //sumR = 0; sumG = 0; sumB = 0;
                    for (int ii = 0; ii < 2 * k; ii++)
                        for (int jj = 0; jj < 2 * k; jj++)
                        {
                            sum += domenBig[i * 2 * k + ii, j * 2 * k + jj];

                            //color = new Color(domenBig[i * 2 * k + ii][j * 2 * k + jj]);
                            //sum += color.getRed();


                        }
                    d = (int)(sum / Math.Pow(4, k));

                    //Color color = new Color(d);
                    //dR = color.getRed();
                    //dG = color.getGreen();
                    //dB = color.getBlue();
                    //int grey = (int) (0.3*dR + 0.59*dG + 0.11 *dB);
                    //d = grey + (grey << 8) + (grey << 16);

                    //color = new Color(d,d,d);
                    //domen[i][j] = color.getRGB();
                    domen[i, j] = d;

                    //dR = (int) (sumR/Math.pow(4,k));
                    //dG = (int) (sumG/Math.pow(4,k));
                    //dB = (int) (sumB/Math.pow(4,k));
                    //domen[i][j] = dB + (dG << 8) + (dR << 16);
                }*/



            textBoxTest.Text = test;
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
