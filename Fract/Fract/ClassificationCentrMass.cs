﻿using System;
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
            if (sum < 50) k = 0;
            else if(sum < 100) k = 1;
            else if (sum < 150) k = 2;
            else if (sum < 200) k = 3;
            else k = 4;

            return k;
        }
    }
}