using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

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
        public RecipeSummaryItem CheapestWayObtaining(List<AuctionItem> DB, List<InventoryItem> InventoryDB,int _laborcost)
        {
            RecipeSummaryItem upwarditem = new RecipeSummaryItem();
            upwarditem.totallabor += LaborCost * Amount;
            
            if(this.isBaseItem())
            {
                int aquirement = 0;
                var auctionresult = DB.Where(x => x.itemName == Name).Where(x => x.TimeStamp > Timestamp(60 * 60 * 24 * 14));
                int auctionprice = 999999999;
                bool auctionfound = false;
                foreach(AuctionItem _auctionitem in auctionresult)
                {
                    auctionfound = true;
                    if(_auctionitem.BuyoutPrice < auctionprice)
                    {
                        auctionprice = _auctionitem.BuyoutPrice;
                    }
                }
                var inventoryresult = InventoryDB.Where(x => x.itemname == Name);
                int inventoryprice = 999999999;
                bool inventoryfound = false;
                foreach(InventoryItem _inventory in inventoryresult)
                {
                    if(_inventory.amount > Amount)
                    {
                        inventoryfound = true;
                        if (_inventory.priceincopper() < auctionprice)
                        {
                            auctionprice = _inventory.priceincopper();
                        }
                    }
                }
                if(!inventoryfound && !auctionfound)
                {
                    throw new System.ArgumentException(Name+" not found in AuctionData or InventoryData","original");
                }
                if (inventoryfound && auctionfound)
                {
                    if(auctionprice > inventoryprice)
                    {
                        upwarditem.totalprice += auctionprice;
                        aquirement = 1;
                    }
                    else
                    {
                        upwarditem.totalprice += inventoryprice;
                        aquirement = 2;
                    }
                }
                else
                {
                    if (inventoryfound)
                    {
                        upwarditem.totalprice += inventoryprice;
                        aquirement = 2;
                    }
                    if (auctionfound)
                    {
                        upwarditem.totalprice += auctionprice;
                        aquirement = 1;
                    }
                }
                RecipeCraftingItems _craftingitem = new RecipeCraftingItems();
                _craftingitem.amount += Amount;
                _craftingitem.name = Name;
                _craftingitem.aquirementtype = aquirement;
                upwarditem.CraftingItems.Add(_craftingitem);
                
            }
            else
            {
                // do the rest
            }
            return upwarditem;
        }
        public int Timestamp(int deduct = 0)
        {
            return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds - deduct;
        }
    }
    public class RecipeSummaryItem
    {
        public RecipeSummaryItem()
        {
            CraftingItems = new List<RecipeCraftingItems>();
        }
        public int totalprice
        {
            get;
            set;
        }
        public int totallabor
        {
            get;
            set;
        }
        public List<RecipeCraftingItems> CraftingItems
        {
            get;
            set;
        }

    }
    public class RecipeCraftingItems
    {
        public string name
        {
            get;
            set;
        }
        public int amount
        {
            get;
            set;
        }
        // 1 = Auction
        // 2 = Inventory
        // 3 = Crafting
        public int aquirementtype
        {
            set;
            get;
        }
    }
}
