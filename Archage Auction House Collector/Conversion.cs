using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archage_Auction_House_Collector
{
    public class Conversion
    {
        public int Id
        {
            get;
            set;
        }
        public int fixcost
        {
            get;
            set;
        }
        public string name
        {
            get;
            set;
        }
        public string SourceItem
        {
            get;
            set;
        }
        public int SourceItemCount
        {
            get;
            set;
        }
        public string ResultItem
        {
            get;
            set;
        }
        public int ResultItemCount
        {
            get;
            set;
        }
        public int LaborCost
        {
            get;
            set;
        }
        public override string ToString()
        {
            return SourceItem + " -> " + ResultItem;
        }
    }
}
