using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Converter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        String rangs = "";
        String[] rangMass;

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //String fileName = openFileDialog.FileName;
                //rangs = File.ReadAllText(fileName);
                //rangMass = rangs.Split(' ');
                //String text = "";
                //for (int i = 0; i < rangMass.Length; i++)
                //    text += i+ "|     "+rangMass[i] + "\r\n";
                //labelList.Text = text;

                String fileName = openFileDialog.FileName;
                rangMass = File.ReadAllLines(fileName);
                String text = "";
                for (int i = 0; i < rangMass.Length; i++)
                    text += i + "|     " + rangMass[i] + "\r\n";
                labelList.Text = text;
            }
        }

        public void Desh()
        {
            long d, i1, i2, i3, i4, i5, i6, i7;
            String test = "";
            for (int i = 0; i< rangMass.Length; i++)
            {
                try {
                    //d = Convert.ToInt32(rangMass[i]);
                    //i1 = Convert.ToInt32(d >> 51);
                    //i2 = Convert.ToInt32((d - (i1 << 51)) >> 40);
                    //i3 = Convert.ToInt32((d - (i1 << 51) - (i2 << 40)) >> 29);
                    //i4 = Convert.ToInt32((d - (i1 << 51) - (i2 << 40) - (i3 << 29)) >> 25);
                    //i5 = Convert.ToInt32((d - (i1 << 51) - (i2 << 40) - (i3 << 29) - (i4 << 25)) >> 21);
                    //i6 = Convert.ToInt32((d - (i1 << 51) - (i2 << 40) - (i3 << 29) - (i4 << 25) - (i5 << 21)) >> 10);
                    //i7 = Convert.ToInt32((d - (i1 << 51) - (i2 << 40) - (i3 << 29) - (i4 << 25) - (i5 << 21) - (i6 << 10)));
                    //test += i + "|     " + i1 + "   " + i2 + "      " + i3 + "   " + i4 + "      " + i5 + "   " + i6 + "      " + i7 + "\r\n";

                    String[] str = rangMass[i].Split(' ');
                    //d = Convert.ToInt32(rangMass[i]);
                    i1 = Convert.ToInt32(str[0]);
                    i2 = Convert.ToInt32(str[1]);
                    i3 = Convert.ToInt32(str[2]);
                    i4 = Convert.ToInt32(str[3]);
                    i5 = Convert.ToInt32(str[4]);
                    i6 = Convert.ToInt32(str[5]);
                    i7 = Convert.ToInt32(str[6]);
                    test += i + "|     " + i1 + "   " + i2 + "      " + i3 + "   " + i4 + "      " + i5 + "   " + i6 + "      " + i7 + "\r\n";

                    labelRang.Text = test;
                }
                catch(FormatException e)
                {

                }
            }
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            panel1.Height = this.Height - 100;
            panel2.Height = this.Height - 100;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Desh();
        }
    }
}
