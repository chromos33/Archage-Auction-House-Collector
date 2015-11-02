using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archage_Auction_House_Collector
{
    public class Item
    {
        public Item(String _itemName,int _copperBuyoutPrice,int _copperBidPrice,int _amount)
        {
            itemName            = _itemName;
            copperBuyoutPrice   = _copperBuyoutPrice;
            copperBidPrice      = _copperBidPrice;
            amount              = _amount;
        }
        private int timeStamp;
        private String itemName;
        private int copperBuyoutPrice;
        private int copperBidPrice;
        private int amount;
        public Item()
        {

        }
        public float BidPerPiece()
        {
            return CopperBidPrice / Amount;
        }
        public float BuyoutPerPiece()
        {
            return CopperBuyoutPrice / Amount;
        }
        public override string ToString()
        {
            return ItemName;
        }

        public int TimeStamp
        {
            get
            {
                return timeStamp;
            }
            set
            {
                timeStamp = value;
            }
        }
        public void setTimeStamp(DateTime _timestamp)
        {
            TimeStamp = (int)(_timestamp.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
        public String ItemName
        {
            get
            {
                return itemName;
            }
            set
            {
                itemName = value;
            }
        }
        public int CopperBuyoutPrice
        {
            get
            {
                return copperBuyoutPrice;
            }
            set
            {
                copperBuyoutPrice = value;
            }
        }
        public int Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
            }
        }
        public int CopperBidPrice
        {
            get
            {
                return copperBidPrice;
            }
            set
            {
                copperBidPrice = value;
            }
        }



    }
}
