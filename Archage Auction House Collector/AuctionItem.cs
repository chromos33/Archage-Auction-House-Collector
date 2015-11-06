using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archage_Auction_House_Collector
{
    public class AuctionItem
    {
        public int Id
        {
            get;
            set;
        }
        public string itemName
        {
            get;
            set;
        }
        public int BuyoutPrice
        {
            get;
            set;
        }
        public int BidPrice
        {
            get;
            set;
        }
        public int TimeStamp
        {
            get;
            set;
        }
        public override string ToString()
        {
            return itemName;
        }
        public void setTimeStamp(DateTime Date)
        {
            TimeStamp = (Int32)(Date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}
