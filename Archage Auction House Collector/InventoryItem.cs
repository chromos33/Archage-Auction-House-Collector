using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archage_Auction_House_Collector
{
    public class InventoryItem
    {
        public int id
        {
            get;
            set;
        }
        public string itemname
        {
            get;
            set;
        }
        public int amount
        {
            get;
            set;
        }
        public int gold
        {
            get;
            set;
        }
        public int silver
        {
            get; set;
        }
        public int copper
        {
            get; set;
        }
        public int priceincopper()
        {
            return gold * 100 * 100 + silver * 100 + copper;
        }
        public override string ToString()
        {
            if(amount > 0)
            {
                int copperprice = gold * 100 * 100 + silver * 100 + copper;
                int priceper = copperprice / amount;
                double newCopper = priceper % 100;
                double newSilver = Convert.ToInt32((priceper / 100)) % 100;
                double newGold = Convert.ToInt32((priceper / 100 / 100));
                return itemname + " " + amount + " pieces @ " + newGold + "g " + newSilver + "s " + newCopper + "c ";
            }
            return "";
            
        }
        public void converttotalcopper(int totalcopper)
        {
            copper = totalcopper % 100;
            silver = Convert.ToInt32((totalcopper / 100)) % 100;
            gold = Convert.ToInt32((totalcopper / 100 / 100));
        }
    }
}
