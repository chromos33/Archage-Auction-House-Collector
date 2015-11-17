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
        public RecipeItem()
        {
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
        public int Cost
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
        public RecipeItem CheapestWayObtaining(LiteDatabase DB, LiteDatabase InventoryDB,int _laborcost)
        {
            RecipeItem upwarditem = new RecipeItem();
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
                upwarditem.Cost = TotalCost;
                upwarditem.Amount = Amount;
                upwarditem.LaborCost = 0;
                upwarditem.saved = false;
                upwarditem.vendorcost = 0;
                upwarditem.Name = Name;
            }
            else
            {
                foreach(RecipeItem subitem in SubItems)
                {
                    int InventoryPrice = 0;
                    int AuctionPrice = 0;
                    int CraftingPrice = 0;
                    var inventory = InventoryDB.GetCollection<InventoryItem>(subitem.Name);
                    var inventoryItems = inventory.FindAll();
                    foreach(InventoryItem inventoryItem in inventoryItems)
                    {
                        InventoryPrice = inventoryItem.amount * inventoryItem.priceincopper();
                    }
                    var auction = DB.GetCollection<AuctionItem>(subitem.Name);
                    var auctionItems = auction.FindAll().Where(x => x.TimeStamp > ((int)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds - 60 * 60 * 24));
                    AuctionPrice = 999999999;
                    foreach(AuctionItem _auctionItem in auctionItems)
                    {
                        if(_auctionItem.BuyoutPrice < AuctionPrice)
                        {
                            AuctionPrice = _auctionItem.BuyoutPrice;
                        }
                    }
                    CraftingPrice = subitem.CheapestWayObtaining(DB, InventoryDB, _laborcost).Cost;
                    int _Price = 0;
                    int[] Price = new int[3] { InventoryPrice, AuctionPrice, CraftingPrice };
                    _Price = Price.Min();
                    if(_Price == InventoryPrice)
                    {
                        RecipeItem newitem = new RecipeItem();
                        newitem.Name = subitem.Name;
                        newitem.Cost = _Price*subitem.Amount;
                        newitem.Amount = subitem.Amount;
                        newitem.LaborCost = 0;
                        newitem.saved = false;
                        newitem.vendorcost = 0;
                        upwarditem.AddSubItem(newitem);
                    }
                    if (_Price == AuctionPrice)
                    {
                        RecipeItem newitem = new RecipeItem();
                        newitem.Name = subitem.Name;
                        newitem.Cost = _Price * subitem.Amount;
                        newitem.Amount = subitem.Amount;
                        newitem.LaborCost = 0;
                        newitem.saved = false;
                        newitem.vendorcost = 0;
                        upwarditem.AddSubItem(newitem);
                    }
                    if (_Price == CraftingPrice)
                    {
                        RecipeItem newitem = new RecipeItem();
                        newitem.Name = subitem.Name;
                        newitem.Cost = _Price * subitem.Amount;
                        newitem.Amount = subitem.Amount;
                        newitem.LaborCost = 0;
                        newitem.saved = false;
                        newitem.vendorcost = 0;
                        newitem.AddSubItem(subitem.CheapestWayObtaining(DB, InventoryDB, _laborcost));
                        upwarditem.AddSubItem(newitem);
                    }
                }
            }
            return upwarditem;
        }
    }
}
