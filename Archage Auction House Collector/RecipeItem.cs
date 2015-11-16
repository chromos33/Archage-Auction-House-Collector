using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using System.IO;

namespace Archage_Auction_House_Collector
{
    public class RecipeItem
    {
        LiteDatabase DB;
        LiteDatabase InventoryDB;
        string path;
        string inventoryPath;
        public RecipeItem()
        {
            path = Directory.GetCurrentDirectory() + "\\ItemDB.db";
            DB = new LiteDatabase(@path);
            inventoryPath = Directory.GetCurrentDirectory() + "\\InventoryDB.db";
            InventoryDB = new LiteDatabase(@inventoryPath);
            SubItems = new List<RecipeItem>();
            saved = false;
        }
        public int ID
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public int LaborCost
        {
            get;
            set;
        }
        public List<RecipeItem> SubItems
        {
            get;
        }
        public int ItemType
        {
            get;
            set;
        }
        public void AddSubItem(RecipeItem insert)
        {
            SubItems.Add(insert);
        }
        public int Amount
        {
            get;
            set;
        }
        public int vendorcost
        {
            get;
            set;
        }
        public bool isBaseItem()
        {
            if(SubItems.Count() == 0)
            {
                return true;
            }
            return false;
        }
        public override string ToString()
        {
            return Name;
        }

        public bool saved
        {
            get;
            set;
        }
        public LaborPriceDataPoint TotalPrice()
        {
            int TotalCost = 0;
            int TotalLabor = 0;
            if(isBaseItem())
            {
                if (vendorcost > 0)
                {
                    TotalCost += vendorcost;
                }
                var loc = InventoryDB.GetCollection<InventoryItem>(Name);
                var inventoryItems = loc.FindAll();
                int inventoryPrice = 999999999;
                foreach (var inventoryitem in inventoryItems)
                {
                    int tempprice = (inventoryitem.gold * 100 * 100 + inventoryitem.silver * 100 + inventoryitem.copper);
                    if (inventoryPrice > tempprice)
                    {
                        inventoryPrice = tempprice;
                    }
                }
                var auctionloc = DB.GetCollection<AuctionItem>(Name);
                var auctionItems = auctionloc.FindAll().Where(x => x.TimeStamp > ((int)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds - 60 * 60 * 24));
                int auctionprice = 999999999;
                foreach (AuctionItem auctionitem in auctionItems)
                {
                    if (auctionitem.BuyoutPrice < auctionprice)
                    {
                        auctionprice = auctionitem.BuyoutPrice;
                    }
                }
                if(auctionprice<inventoryPrice)
                {
                    TotalCost += auctionprice;
                }
                else
                {
                    TotalCost += inventoryPrice;
                }
            }
            else
            {
                //do the same as aboth only difference is you also call subitems TotalPrice() Function
                // Factor in LaborCost in Crafting Price
            }
            LaborPriceDataPoint laborpricepoint = new LaborPriceDataPoint();
            laborpricepoint.Cost = TotalCost;
            laborpricepoint.Labor = TotalLabor;
            return laborpricepoint;
        }
    }
}
