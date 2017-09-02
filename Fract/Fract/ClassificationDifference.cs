using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Fract
{
    public class ClassificationDifference : Classification
    {
        private string name;

        public ClassificationDifference(string name)
        {
            this.name = name;
        }        

        public string getName()
        {
            return name;
        }

        //5 class
        public int getClass(int[,] block)
        {
            int k = -1;
            int sum = 0;
            int max = 0, min = 255, res;
            Color color;

            double count = block.GetLength(0) * block.GetLength(1);
            for (int i = 0; i < block.GetLength(0); i++)
                for (int j = 0; j < block.GetLength(1); j++)
                {
                    color = Color.FromArgb(block[i, j]);
                    if (max < color.R) max = color.R;
                    else if (min > color.R) min = color.R;
                }

            res = max - min;

            if (res < 50) k = 0;
            else if (res < 100) k = 1;
            else if (res < 150) k = 2;
            else if (res < 200) k = 3;
            else k = 4;

            return k;
        }

    }
}
