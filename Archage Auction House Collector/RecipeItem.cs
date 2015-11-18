using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if(SubItems.Count() != 0)
            {
                foreach(RecipeItem item in SubItems)
                {
                    if (item.Name == Name)
                    {
                        return true;
                    }
                }
            }
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
        public RecipeItem CheapestWayObtaining(List<AuctionItem> DB, List<InventoryItem> InventoryDB,int _laborcost,ref Dictionary<string,int> Materials,ref int _totalprice)
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
                var inventoryItems = InventoryDB.Where(x => x.itemname == Name);
                int inventoryPrice = 999999999;
                foreach (var inventoryitem in inventoryItems)
                {
                    int tempprice = (inventoryitem.gold * 100 * 100 + inventoryitem.silver * 100 + inventoryitem.copper);
                    if (inventoryPrice > tempprice)
                    {
                        inventoryPrice = tempprice;
                    }
                }
                var auctionItems = DB.Where(x => x.TimeStamp > ((int)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds - 60 * 60 * 24));
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
                    TotalCost += auctionprice*Amount;
                }
                else
                {
                    TotalCost += inventoryPrice*Amount;
                }
                _totalprice += TotalCost;
                upwarditem.Cost = TotalCost;
                upwarditem.Amount = Amount;
                if (Materials.Keys.Contains(Name))
                {
                    Materials[Name] += Amount;
                }
                else
                {
                    Materials[Name] = Amount;
                }
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
                    var inventoryItems = InventoryDB.Where(x => x.itemname == subitem.Name);
                    foreach(InventoryItem inventoryItem in inventoryItems)
                    {
                        InventoryPrice = inventoryItem.amount * inventoryItem.priceincopper();
                    }
                    var auctionItems = DB.Where(x => x.itemName == subitem.Name).Where(x => x.TimeStamp > ((int)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds - 60 * 60 * 24));
                    AuctionPrice = 999999999;
                    foreach(AuctionItem _auctionItem in auctionItems)
                    {
                        if(_auctionItem.BuyoutPrice < AuctionPrice)
                        {
                            AuctionPrice = _auctionItem.BuyoutPrice;
                        }
                    }
                    CraftingPrice = subitem.CheapestWayObtaining(DB, InventoryDB, _laborcost, ref Materials, ref _totalprice).Cost;
                    int _Price = 0;
                    int[] Price = new int[3] { InventoryPrice, AuctionPrice, CraftingPrice };
                    _Price = Price.Min();
                    if(_Price == InventoryPrice)
                    {
                        RecipeItem newitem = new RecipeItem();
                        newitem.Name = subitem.Name;
                        newitem.Cost = _Price*subitem.Amount;
                        _totalprice += _Price * subitem.Amount;
                        newitem.Amount = subitem.Amount;
                        if (Materials.Keys.Contains(Name))
                        {
                            Materials[Name] += Amount;
                        }
                        else
                        {
                            Materials[Name] = Amount;
                        }
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
                        _totalprice += _Price * subitem.Amount;
                        newitem.Amount = subitem.Amount;
                        if (Materials.Keys.Contains(Name))
                        {
                            Materials[Name] += Amount;
                        }
                        else
                        {
                            Materials[Name] = Amount;
                        }
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
                        newitem.AddSubItem(subitem.CheapestWayObtaining(DB, InventoryDB, _laborcost, ref Materials, ref _totalprice));
                        upwarditem.AddSubItem(newitem);
                    }
                }
            }
            return upwarditem;
        }
    }
}
