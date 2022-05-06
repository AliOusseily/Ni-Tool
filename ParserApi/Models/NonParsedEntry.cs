using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParserApi.Models
{
    public class NonParsedEntry
    {

        public string NodeName { get; set; }
        public float NeId { get; set; }

        public string Object { get; set; }

        public string Time { get; set; }

        public int Interval { get; set; }

        public string Direction { get; set; }

        public string NeAlias { get; set; }

        public string NeType { get; set; }
        public string Position { get; set; }

        public string RxLevelBelowTS1 { get; set; }

        public string RxLevelBelowTS2 { get; set; }

        public string MinRxLevel { get; set; }

        public string MaxRxLevel { get; set; }

        public string TxLevelAboveTS1 { get; set; }

        public string MinTxLevel { get; set; }
        public string MaxTxLevel { get; set; }

        public  int IdLogNum { get; set; }

        public string FailureDescription { get; set; }



    }
}
