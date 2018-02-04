using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fract
{
    public class BlockSCO
    {
        private List<double> scoList = new List<double>();

        public BlockSCO()
        {
            this.scoList = new List<double>();
        }

        public List<double> getSCOList()
        {
            return scoList;
        }
    }
}
