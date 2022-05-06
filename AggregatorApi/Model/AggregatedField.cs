using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AggregatorApi.Model
{
    public class AggregatedField
    {
        //public int NetworkSID { get; set; }
        public DateTime Time { get; set; }
        public string Link { get; set; }
        //public int SLOT { get; set; }
        public double MaxRxLevel { get; set; }
        public double MaxTxLevel { get; set; }
        public double RSL_DEVIATION { get; set; }
    }
}
