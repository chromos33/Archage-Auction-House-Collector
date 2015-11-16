using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
