using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fract
{
    interface Compression
    {
        void compressImage();
        void getDomenBloc(int[,] rang, int k, int x, int y);
        bool compareBlocs(int[,] rang, int[,] domen);
        int[,] changeBright(int[,] pix, double k);
        int[,] setAfinnInt(int[,] pix, int k);
        List<Rang> getRangList();

        int getR();

        ///test function
        void SaveSumCompare();
    }
}
