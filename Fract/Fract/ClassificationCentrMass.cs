using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Fract
{
    public class ClassificationCentrMass : Classification
    {
        private string name;

        public ClassificationCentrMass(string name)
        {
            this.name = name;
        }

        public string getName()
        {
            return name;
        }
        //5 classes
        public int getClass(int[,] block)
        {
            int k = -1;
            int sum = 0;
            double res; ;
            Color color;

            double count = block.GetLength(0) * block.GetLength(1);
            for (int i = 0; i < block.GetLength(0); i++)
                for (int j = 0; j < block.GetLength(1); j++)
                {
                    color = Color.FromArgb(block[i, j]);
                    sum += color.R;
                }
                    
            res = sum / count;

            if (res < 50) k = 0;
            else if(res < 100) k = 1;
            else if (res < 150) k = 2;
            else if (res < 200) k = 3;
            else k = 4;

            return k;
        }

        public int[] getClassColor(int[,] block)
        {
            int[] k = new int[3];
            int kRed = -1, kGreen = -1, kBlue = -1;
            int sumRed = 0, sumGreen = 0, sumBlue = 0;
            double resRed, resGreen, resBlue ;
            Color color;

            double count = block.GetLength(0) * block.GetLength(1);
            for (int i = 0; i < block.GetLength(0); i++)
                for (int j = 0; j < block.GetLength(1); j++)
                {
                    color = Color.FromArgb(block[i, j]);
                    sumRed += color.R;
                    sumGreen += color.G;
                    sumBlue += color.B;
                }

            resRed = sumRed / count;
            resGreen = sumGreen / count;
            resBlue = sumBlue / count;

            if (resRed < 50) kRed = 0;
            else if (resRed < 100) kRed = 1;
            else if (resRed < 150) kRed = 2;
            else if (resRed < 200) kRed = 3;
            else kRed = 4;

            if (resGreen < 50) kGreen = 0;
            else if (resGreen < 100) kGreen = 1;
            else if (resGreen < 150) kGreen = 2;
            else if (resGreen < 200) kGreen = 3;
            else kGreen = 4;

            if (resBlue < 50) kBlue = 0;
            else if (resBlue < 100) kBlue = 1;
            else if (resBlue < 150) kBlue = 2;
            else if (resBlue < 200) kBlue = 3;
            else kBlue = 4;

            k[0] = kRed;
            k[1] = kGreen;
            k[2] = kBlue;
            return k;
        }

    }
}
