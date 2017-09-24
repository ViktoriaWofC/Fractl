using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fract
{
    [Serializable]
    public class Rang
    {
        private int x;
        private int y;
        private int afinn;

        private int k;
        private int x0;
        private int y0;
        private double bright;
        private double s;
        private double o;

        //public Rang(int x, int y, int afinn, int k, int x0, int y0, double bright)
        public Rang(int x, int y, int afinn, int k, int x0, int y0, double s, double o)
        {
            this.x = x;
            this.y = y;
            this.afinn = afinn;

            this.k = k;
            this.x0 = x0;
            this.y0 = y0;

            //this.bright = bright;
            this.s = s;
            this.o = o;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public int getAfinn()
        {
            return afinn;
        }

        public int getX0()
        {
            return x0;
        }

        public int getY0()
        {
            return y0;
        }

        public int getK()
        {
            return k;
        }

        public double getBright()
        {
            return bright;
        }

        public double getS()
        {
            return s;
        }

        public double getO()
        {
            return o;
        }

    }
}
