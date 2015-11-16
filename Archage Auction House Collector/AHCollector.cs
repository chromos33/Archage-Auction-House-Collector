using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using LiteDB;
using System.IO;
using System.Diagnostics;

namespace Archage_Auction_House_Collector
{
    public partial class Archage_AH_DataCollector : Form
    {
        LiteDatabase DB;
        LiteDatabase InventoryDB;
        LiteDatabase ConversionDB;
        LiteDatabase RecipeDB;
        string path;
        string inventoryPath;
        string conversionPath;
        string recipeDBPath;
        public Archage_AH_DataCollector()
        {
            InitializeComponent();
            path = Directory.GetCurrentDirectory() + "\\ItemDB.db";
            DB = new LiteDatabase(@path);
            inventoryPath = Directory.GetCurrentDirectory() + "\\InventoryDB.db";
            InventoryDB = new LiteDatabase(@inventoryPath);

            conversionPath = Directory.GetCurrentDirectory() + "\\ConversionDB.db";
            ConversionDB = new LiteDatabase(@conversionPath);

            recipeDBPath = Directory.GetCurrentDirectory() + "\\RecipesDB.db";
            RecipeDB = new LiteDatabase(@recipeDBPath);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Duration.SelectedIndex = 0;

            //Crafting_Recipes_Recipe
            //Crafting_Recipes_RecipeItem
            var col = RecipeDB.GetCollection<RecipeItem>("Recipe");
            var Recipes = col.FindAll();
            foreach(RecipeItem item in Recipes)
            {
                Crafting_Recipes_Recipe.Items.Add(item);
                Crafting_Recipes_RecipeItem.Items.Add(item);
            }


            IEnumerable<string> CollectionNames = DB.GetCollectionNames();
            foreach (string name in CollectionNames)
            {

                ItemNameComboBox.Items.Add(name);
                Measurment.Items.Add(name);
                MeasurementCorection.Items.Add(name);
                Conversion_BaseItem.Items.Add(name);
                Conversion_Result_Item.Items.Add(name);
                Conversion_Correction_BaseItem.Items.Add(name);
                Conversion_Correction_ResultITem.Items.Add(name);
                Crafting_Recipes_RecipeItemName.AutoCompleteCustomSource.Add(name);
                ItemName.AutoCompleteCustomSource.Add(name);
            }
            var conversioncol = ConversionDB.GetCollection<Conversion>("Conversion");
            var conversions = conversioncol.FindAll();
            foreach(Conversion conversionitem in conversions)
            {
                Conversion_Correction_ItemSelect.Items.Add(conversionitem);
            }
            IEnumerable<string> Inventory_CollectionNames = InventoryDB.GetCollectionNames();
            foreach (string name in Inventory_CollectionNames)
            {
                Inventory_ExistingItem.Items.Add(name);
            }
            if (Measurment.Items.Count > 0)
            {
                Measurment.SelectedIndex = 0;
                ItemNameComboBox.SelectedIndex = 0;
                MeasurementCorection.SelectedIndex = 0;
            }
            timespanselect.SelectedIndex = 0;
        }

        private void Save_btn_Click(object sender, EventArgs e)
        {
            if (ItemName.Text == "" && ItemNameComboBox.Items.Count == 0)
            {
                MessageBox.Show("Item Name missing");
                return;
            }
            int Bid = 0;
            int Buyout = 0;
            if (!(int.TryParse(BidInCopper.Text, out Bid)) && !BidInCopper.Text.Contains(','))
            {
                MessageBox.Show("Bid in Copper missing or not a Number");
                return;
            }
            if (!(int.TryParse(BuyoutinCopper.Text, out Buyout)) && !BuyoutinCopper.Text.Contains(','))
            {
                MessageBox.Show("Buyout in Copper missing or not a Number");
                return;
            }
            if(BuyoutinCopper.Text.Contains(','))
            {
                int buycount = BuyoutinCopper.Text.Count(f => f == ',');
                if(buycount == 1)
                {
                    int silver = 0;
                    int copper = 0;
                    string[] money = BuyoutinCopper.Text.Split(',');
                    int.TryParse(money[1], out silver);
                    int.TryParse(money[2], out copper);
                    Buyout = silver * 100 + copper;
                }
                if(buycount == 2)
                {
                    int gold = 0;
                    int silver = 0;
                    int copper = 0;
                    string[] money = BuyoutinCopper.Text.Split(',');
                    int.TryParse(money[0], out gold);
                    int.TryParse(money[1], out silver);
                    int.TryParse(money[2], out copper);
                    Buyout = gold * 100 * 100 + silver * 100 + copper;
                }
            }
            if (BidInCopper.Text.Contains(','))
            {
                int bidcount = BidInCopper.Text.Count(f => f == ',');
                if (bidcount == 1)
                {
                    int silver = 0;
                    int copper = 0;
                    string[] money = BidInCopper.Text.Split(',');
                    int.TryParse(money[1], out silver);
                    int.TryParse(money[2], out copper);
                    Bid = silver * 100 + copper;
                }
                if (bidcount == 2)
                {
                    int gold = 0;
                    int silver = 0;
                    int copper = 0;
                    string[] money = BidInCopper.Text.Split(',');
                    int.TryParse(money[0], out gold);
                    int.TryParse(money[1], out silver);
                    int.TryParse(money[2], out copper);
                    Bid = gold * 100 * 100 + silver * 100 + copper;
                }
            }
            string name = "";
            if (ItemName.Text != "")
            {
                name = ItemName.Text;
                ItemName.Text = "";
            }
            else
            {
                name = ItemNameComboBox.SelectedItem.ToString();
            }
            name = name.Replace(" ", "_");
            AuctionItem newItemEntry = new AuctionItem();
            newItemEntry.itemName = name;
            newItemEntry.BuyoutPrice = Buyout;
            newItemEntry.BidPrice = Bid;
            newItemEntry.setTimeStamp(DateTime.Now);
            if (!Measurment.Items.Contains(name))
            {
                Measurment.Items.Add(name);
                ItemNameComboBox.Items.Add(name);
                MeasurementCorection.Items.Add(name);
                Conversion_BaseItem.Items.Add(name);
                Conversion_Result_Item.Items.Add(name);
                Conversion_Correction_BaseItem.Items.Add(name);
                Conversion_Correction_ResultITem.Items.Add(name);
                ItemNameComboBox.SelectedIndex = ItemNameComboBox.Items.Count -1;
                Crafting_Recipes_RecipeItemName.AutoCompleteCustomSource.Add(name);
                ItemName.AutoCompleteCustomSource.Add(name);
            }
            saveObjecttoDB(newItemEntry);
            BuyoutinCopper.Text = "";
            BidInCopper.Text = "";
        }
        private void saveObjecttoDB(AuctionItem insert)
        {
            var col = DB.GetCollection<AuctionItem>(insert.itemName);
            col.Insert(insert);
        }

        private void Measurment_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private AuctionItem ReadDatabase(string _table, int starttimestamp,int endtimestamp)
        {
            var col = DB.GetCollection<AuctionItem>(_table);
            var results = col.Find(x => x.itemName == _table).Where(x => x.TimeStamp > starttimestamp).Where(x => x.TimeStamp < endtimestamp);
            AuctionItem returnItem = new AuctionItem();
            if (results.Count() > 0)
            {
                returnItem.TimeStamp = results.ElementAt(0).TimeStamp;
                returnItem.itemName = results.ElementAt(0).itemName;
                
                int bidsum = 0;
                int buysum = 0;
                for (int i = 0;i< results.Count();i++)
                {
                    bidsum += results.ElementAt(i).BidPrice;
                    buysum += results.ElementAt(i).BuyoutPrice;
                }
                returnItem.BuyoutPrice = Convert.ToInt32(buysum / results.Count());
                returnItem.BidPrice = Convert.ToInt32(bidsum / results.Count());
            }
            else
            {
                returnItem.BuyoutPrice = 0;
                returnItem.BidPrice = 0;
                returnItem.TimeStamp = starttimestamp;
                returnItem.itemName = _table;
            }
            return returnItem;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
        private string HTMLString(List<AuctionItem> items)
        {
            items.Reverse();
            string result = "<!DOCTYPE html>";
            result = "<html>" + System.Environment.NewLine;
            result += "<head>" + System.Environment.NewLine;
            result += "<meta http-equiv='X-UA-Compatible' content='IE=Edge'>";
            result += "</head>" + System.Environment.NewLine;
            result += "<body>" + System.Environment.NewLine;
            string curDir = "file:///" + Directory.GetCurrentDirectory() + @"\Scripts\Chart.js";
            result += "<script type='text/javascript' src='" + curDir + "'></script>" + System.Environment.NewLine;
            result += "<canvas ID='datachart' css='width:" + (Chartbrowser.Width - 20) + "px;height:" + (Chartbrowser.Height - 20) + "px;' width='" + (Chartbrowser.Width - 20) + "' height='" + (Chartbrowser.Height - 20) + "'></canvas>" + System.Environment.NewLine;
            result += "<script>" + System.Environment.NewLine;
            result += "var ctx = document.getElementById('datachart').getContext('2d');" + System.Environment.NewLine;
            result += "var data = {" + System.Environment.NewLine;
            result += "labels: [" + System.Environment.NewLine;
            foreach (var item in items)
            {
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                dateTime = dateTime.AddSeconds(item.TimeStamp);
                result += "'" +dateTime.Hour+"th Hour "+ dateTime.Day+"."+dateTime.Month+"."+dateTime.Year + "',";
            }
            result = result.Remove(result.Length - 1);
            result += "]," + System.Environment.NewLine;
            result += "datasets: [" + System.Environment.NewLine;

            result += "{" + System.Environment.NewLine;
            result += ("label: '_BuyoutPerPiece'," + System.Environment.NewLine);
            result += "fillColor: 'rgba(255,0,0,0)'," + System.Environment.NewLine;
            result += "strokeColor: 'rgba(255,0,0,1)'," + System.Environment.NewLine;
            result += "pointColor: 'rgba(255,0,0,1)'," + System.Environment.NewLine;
            result += "pointStrokeColor: '#fff'," + System.Environment.NewLine;
            result += "pointHighlightFill: '#fff'," + System.Environment.NewLine;
            result += "pointHighlightStroke: 'rgba(255,0,0,1)'," + System.Environment.NewLine;
            result += "data: [" + System.Environment.NewLine;
            for(int i =0;i < items.Count;i++)
            {
                var item = items.ElementAt(i);
                
                if (item.BuyoutPrice != 0)
                {
                    result += item.BuyoutPrice + ",";
                }
                else
                {
                    int j = i;
                    while (items.ElementAt(j).BuyoutPrice == 0 && j > 0)
                    {
                        j--;
                    }
                    result += items.ElementAt(j).BuyoutPrice + ",";
                }
            }
            result = result.Remove(result.Length - 1);
            result += "]" + System.Environment.NewLine;
            result += "}," + System.Environment.NewLine;
            result += "{" + System.Environment.NewLine;
            result += "label: '_BidPerPiece'," + System.Environment.NewLine;
            result += "fillColor: 'rgba(255,255,255,0)'," + System.Environment.NewLine;
            result += "strokeColor: 'rgba(0,0,255,1)'," + System.Environment.NewLine;
            result += "pointColor: 'rgba(0,0,255,1)'," + System.Environment.NewLine;
            result += "pointStrokeColor: '#fff'," + System.Environment.NewLine;
            result += "pointHighlightFill: '#fff'," + System.Environment.NewLine;
            result += "pointHighlightStroke: 'rgba(0,0,255,1)'," + System.Environment.NewLine;
            result += "data: [" + System.Environment.NewLine;
            for (int i = 0; i < items.Count; i++)
            {
                var item = items.ElementAt(i);

                if (item.BidPrice != 0)
                {
                    result += item.BidPrice + ",";
                }
                else
                {
                    int j = i;
                    while (items.ElementAt(j).BidPrice == 0 && j > 0)
                    {
                        j--;
                    }
                    result += items.ElementAt(j).BidPrice + ",";
                }
            }

            result = result.Remove(result.Length - 1);
            result += "]" + System.Environment.NewLine;
            result += "}" + System.Environment.NewLine;
            result += "]" + System.Environment.NewLine;
            result += "}" + System.Environment.NewLine;

            result += "var options = {bezierCurve:false};";
            result += "var linechart = new Chart(ctx).Line(data,options);" + System.Environment.NewLine;
            result += "</script>" + System.Environment.NewLine;

            result += "</body>" + System.Environment.NewLine;
            result += "</html>" + System.Environment.NewLine;
            System.Windows.Forms.Clipboard.SetText(result);

            return result;
        }

        private void updatechart_Click(object sender, EventArgs e)
        {
            if (Measurment.Items.Count > 0)
            {
                int finalstartime = 0;
                int starttime = (int)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                int endtime = 0;
                List<AuctionItem> ResultSet = new List<AuctionItem>();
                int timesteps = 3600;
                switch (timespanselect.SelectedIndex)
                {
                    case 0: finalstartime = starttime - 604800*1; timesteps = 3600*6; break;
                    case 1: finalstartime = starttime - 604800*2; timesteps = 3600*6; break;
                    case 2: finalstartime = starttime - 604800*3; timesteps = 3600*24; break;
                    case 3: finalstartime = starttime - 604800*4; timesteps = 3600*24; break;
                }
                while(starttime >= finalstartime)
                {
                    starttime -= timesteps;
                    endtime = starttime + timesteps;
                    var auctionitem = ReadDatabase(Measurment.Text.ToLower(), starttime, endtime);
                    if(auctionitem != null)
                    {
                        ResultSet.Add(auctionitem);
                    }
                }
                
                if (ResultSet.Count() > 0)
                {
                    Chartbrowser.Navigate("about:blank");

                    if (Chartbrowser.Document != null)
                    {
                        Chartbrowser.Document.Write(string.Empty);
                    }
                    Chartbrowser.DocumentText = HTMLString(ResultSet);
                }
            }
        }

        private void DataExploration_Click(object sender, EventArgs e)
        {

        }

        private void MeasurementCorection_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            TimeStampList.Items.Clear();
            var col = DB.GetCollection<AuctionItem>(MeasurementCorection.SelectedItem.ToString());
            var results = col.FindAll();
            foreach (var result in results)
            {
                TimeStampList.Items.Add(result.Id + "," + result.itemName + "," + result.TimeStamp + ",Buyout: " + result.BuyoutPrice + ",Bid: " + result.BidPrice);
            }
        }

        private void deletepoint_Click(object sender, EventArgs e)
        {
            List<object> itemstodelete = new List<object>();
            foreach (var item in TimeStampList.CheckedItems)
            {
                string[] _string = item.ToString().Split(',');
                var loc = DB.GetCollection<AuctionItem>(_string[1]);
                if (loc.Delete(Int32.Parse(_string[0])))
                {
                    itemstodelete.Add(item);
                }
            }
            foreach (var item in itemstodelete)
            {
                TimeStampList.Items.Remove(item);
            }
        }
        private void Form_Resize(object sender, System.EventArgs e)
        {
            Control control = (Control)sender;

            Tabs.Width = control.Size.Width;
            if(Tabs.SelectedTab.Name == "DataExploration")
            {
                Chartbrowser.Width = this.Width - 150;
                Chartbrowser.Height = this.Height-50;
                Tabs.Width = this.Width;
                Tabs.Height = this.Height;
            }
        }
        private void Form1_ResizeEnd(Object sender, EventArgs e)
        {

            MessageBox.Show("You are in the Form.ResizeEnd event.");

        }
        private void TabSelectIndexChanged(object sender, EventArgs e)
        {
            if (Tabs.SelectedTab.Name == "DataExploration")
            {
                Tabs.Width = 1000;
                Tabs.Height = 600;
                this.Width = 1000;
                this.Height = 600;
                Chartbrowser.Width = 1000 - 150;
                Chartbrowser.Height = 600 - 231;
            }
            else
            {
                Tabs.Width = 558;
                Tabs.Height = 265;
                this.Width = 558;
                this.Height = 265;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Calculate_Click(object sender, EventArgs e)
        {
            try
            {
                string output = "";
                double _Gold = Double.Parse(Gold.Text);
                double _Silver = Double.Parse(Silver.Text);
                double _Copper = Double.Parse(Copper.Text);
                int _tries = Int32.Parse(numberofauctions.Text);
                double _BuyPrice = _Gold * 100 * 100 + _Silver * 100 + _Copper;
                int _Duration = Duration.SelectedIndex;
                _Duration++;
                int limit = 3;
                if(_tries > 0)
                {
                    limit = _tries;
                }
                for(int i = 0; i< limit; i++)
                {
                    
                    double BreakEvenPoint = _BuyPrice + (_BuyPrice * (0.05 +0.01* _Duration));
                    double newCopper = BreakEvenPoint % 100;
                    double newSilver = Convert.ToInt32((BreakEvenPoint / 100)) % 100;
                    double newGold = Convert.ToInt32((BreakEvenPoint / 100 / 100));

                    output += i+"th Auction Break Even Point:"+Convert.ToInt32(newGold) +" Gold "+ Convert.ToInt32(newSilver) + " Silver " + Convert.ToInt32(newCopper) + " Copper " + System.Environment.NewLine;
                    _BuyPrice = _BuyPrice + (_BuyPrice * (0.01 * _Duration));
                }
                BreakEvenPointsText.Text = output;

            }
            catch(Exception ex)
            {
                MessageBox.Show("Only numbers accepted");
            }
            
        }

        private void Inventory_SellBtn_Click(object sender, EventArgs e)
        {
            List<InventoryItem> itemstodelete = new List<InventoryItem>();
            foreach(InventoryItem item in Inventory_ItemList.CheckedItems)
            {
                itemstodelete.Add(item);
            }
            foreach(InventoryItem item in itemstodelete)
            {
                Inventory_ItemList.Items.Remove(item);
                var loc = InventoryDB.GetCollection<InventoryItem>(item.itemname);
                loc.Delete(item.id);
            }
            Inventory_Recalculate();

        }

        private void Inventory_BuyBtn_Click(object sender, EventArgs e)
        {
            string searchname = "";
            if(Inventory_NewItem.Text != "")
            {
                searchname = Inventory_NewItem.Text;
            }
            if(Inventory_ExistingItem.SelectedItem != null)
            {
                searchname = Inventory_ExistingItem.SelectedItem.ToString();
            }
            if(searchname != "")
            {
                try
                {
                    if(Inventory_Silver.Text.Length>2 || Inventory_Copper.Text.Length >2)
                    {
                        MessageBox.Show("Error in Input Silver/Copper cant be bigger than 99");
                        return;
                    }
                    int gold = 0;
                    int silver = 0;
                    int copper = 0;
                    if(Inventory_Gold.Text.Length > 0)
                    {
                        gold = Int32.Parse(Inventory_Gold.Text);
                    }
                    if (Inventory_Silver.Text.Length > 0)
                    {
                        silver = Int32.Parse(Inventory_Silver.Text);
                    }
                    if (Inventory_Copper.Text.Length > 0)
                    {
                        copper = Int32.Parse(Inventory_Copper.Text);
                    }
                    if(gold == 0 && silver == 0 && copper == 0)
                    {
                        MessageBox.Show("No Price entered or Price is 0");
                        return;
                    }
                    if(Inventory_Amount.Text.Length == 0)
                    {
                        MessageBox.Show("Enter an amount");
                        return;
                    }

                    InventoryItem insert = new InventoryItem();
                    insert.gold = gold;
                    insert.silver = silver;
                    insert.copper = copper;
                    insert.itemname = searchname.Replace(" ","_");
                    insert.amount = Int32.Parse(Inventory_Amount.Text);
                    var col = InventoryDB.GetCollection<InventoryItem>(insert.itemname);
                    col.Insert(insert);
                    Inventory_ItemList.Items.Add(insert);
                    Inventory_Recalculate();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    MessageBox.Show("Gold/Silver/Copper must be entered as a Number");
                }
                
            }
            else
            {
                MessageBox.Show("Itemname must be either entered or selected");
                return;
            }
        }

        private void Inventory_ExistingItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = Inventory_ExistingItem.Text;
            var col = InventoryDB.GetCollection<InventoryItem>(selected);
            var results = col.FindAll();
            Inventory_ItemList.Items.Clear();
            foreach (InventoryItem result in results)
            {
                Inventory_ItemList.Items.Add(result);
            }
            Inventory_Recalculate();
        }
        public void Inventory_Recalculate()
        {
            if(Inventory_ItemList.Items.Count > 0)
            {
                int totalamount = 0;
                int totalprice = 0;
                foreach (InventoryItem item in Inventory_ItemList.Items)
                {
                    totalamount += item.amount;
                    totalprice += item.priceincopper();
                }
                double averagepriceperpiece = totalprice / totalamount;
                double newCopper = averagepriceperpiece % 100;
                double newSilver = Convert.ToInt32((averagepriceperpiece / 100)) % 100;
                double newGold = Convert.ToInt32((averagepriceperpiece / 100 / 100));
                Inventory_TotalAmount.Text = totalamount.ToString();
                Inventory_AveragePrice.Text = newGold + "g " + newSilver + "s " + newCopper + "c";
            }
            else
            {
                Inventory_TotalAmount.Text = "NAN";
                Inventory_AveragePrice.Text = "NAN";
            }
        }

        private void Conversion_Save_Btn_Click(object sender, EventArgs e)
        {
            //ConversionDB
            int baseitemcount = 0;
            int resultitemcount = 0;
            string baseitem = "";
            string resultitem = "";
            int laborcost = 0;
            int fixcost = 0;
            if (!int.TryParse(Conversion_Base_Count.Text, out baseitemcount))
            {
                MessageBox.Show("Item Count must be a whole number");
                return;
            }
            int.TryParse(Conversion_Fix_Cost.Text, out fixcost);
            if (!int.TryParse(Conversion_Result_Count.Text, out resultitemcount))
            {
                MessageBox.Show("Item Count must be a whole number");
                return;
            }
            if(Conversion_BaseItem.SelectedItem.ToString() == "")
            {
                MessageBox.Show("Base Item must be selected");
                return;
            }
            if (Conversion_Result_Item.SelectedItem.ToString() == "")
            {
                MessageBox.Show("Result Item must be selected");
                return;
            }
            Conversion newitem = new Conversion();
            newitem.LaborCost = laborcost;
            newitem.ResultItem = Conversion_Result_Item.SelectedItem.ToString();
            newitem.SourceItem = Conversion_BaseItem.SelectedItem.ToString();
            newitem.SourceItemCount = baseitemcount;
            newitem.ResultItemCount = resultitemcount;
            newitem.fixcost = fixcost;
            newitem.name = "Conversion";

            var col = ConversionDB.GetCollection<Conversion>(newitem.name);
            col.Insert(newitem);
        }

        private void Conversion_Check_Click(object sender, EventArgs e)
        {
            Conversion_Rule_Applicableg.Items.Clear();
            int workerscopensation = 0;
            double percent = 0;
            
            if (!double.TryParse(Conversion_Profit_Percent.Text, out percent))
            {
                MessageBox.Show("Profit % must be a number > 0 (0.0% -> xxx.xx%)");
                return;
            }
            percent += 100;
            percent = percent / 100;
            if (!int.TryParse(Conversion_Workers_Compensation_Potion.Text, out workerscopensation))
            {
                MessageBox.Show("Workers Compensation Price must be given in Copper");
                return;
            }
            var col = ConversionDB.GetCollection<Conversion>("Conversion");
            var results = col.FindAll();
            int starttimestamp = (int)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds - 86400/2;
            foreach(Conversion item in results)
            {
                var resultpricecol = DB.GetCollection<AuctionItem>(item.ResultItem);
                var resultpriceitems = resultpricecol.FindAll().Where(x => x.TimeStamp > starttimestamp);
                var sourcepricecol = DB.GetCollection<AuctionItem>(item.SourceItem);
                var sourcepriceitems = sourcepricecol.FindAll().Where(x => x.TimeStamp > starttimestamp);
                double minbuyprice = 9999999999;
                foreach(var resultitem in resultpriceitems)
                {
                    if(resultitem.BuyoutPrice < minbuyprice)
                    {
                        minbuyprice = resultitem.BuyoutPrice;
                    }
                }
                foreach (var sourceitem in sourcepriceitems)
                {
                    //preis = sourceitemprice * sourcepriceamount + 70* potionpreis / 1000
                    double buyprice = (item.fixcost + sourceitem.BuyoutPrice * item.SourceItemCount + item.LaborCost * workerscopensation / 1000);
                    double bidprice = (item.fixcost + sourceitem.BidPrice * item.SourceItemCount + item.LaborCost * workerscopensation / 1000);

                    double newbuyCopper = Convert.ToInt32(buyprice % 100);
                    double newbuySilver = Convert.ToInt32((buyprice / 100)) % 100;
                    double newbuyGold = Convert.ToInt32((buyprice / 100 / 100));

                    double newbidCopper = Convert.ToInt32(bidprice % 100);
                    double newbidSilver = Convert.ToInt32((bidprice / 100)) % 100;
                    double newbidGold = Convert.ToInt32((bidprice / 100 / 100));
                    double saleprice = minbuyprice * 0.94;
                    bool skip = false;
                    if ((buyprice * percent) <= saleprice)
                    {
                        double profit = saleprice - buyprice;
                        double profitCopper = Convert.ToInt32(profit % 100);
                        double profitSilver = Convert.ToInt32((profit / 100)) % 100;
                        double profitGold = Convert.ToInt32((profit / 100 / 100));
                        Conversion_Rule_Applicableg.Items.Add("Buy " + item.SourceItemCount + " " + sourceitem.itemName + " " + newbuyGold + "g " + newbuySilver + "s " + newbuyCopper + "c " + " / " + item.ResultItemCount + " " + item.ResultItem + " = " + "Profit:" + profitGold + "g " + profitSilver + "s " + profitCopper + "c ");
                        skip = true;
                    }
                    if ((bidprice * percent) <= saleprice && !skip)
                    {
                        double profit = saleprice - bidprice;
                        double profitCopper = Convert.ToInt32(profit % 100);
                        double profitSilver = Convert.ToInt32((profit / 100)) % 100;
                        double profitGold = Convert.ToInt32((profit / 100 / 100));
                        Conversion_Rule_Applicableg.Items.Add("Bid " + item.SourceItemCount + " " + sourceitem.itemName + " " + newbidGold + "g " + newbidSilver + "s " + newbidCopper + "c " + " / " + item.ResultItemCount + " " + item.ResultItem + " = " + "Profit:" + profitGold + "g " + profitSilver + "s " + profitCopper + "c "); 
                    }
                    
                    
                }
            }
        }

        private void Conversion_Correction_ItemSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            Conversion selecteditem = (Conversion) Conversion_Correction_ItemSelect.SelectedItem;
            Conversion_Correction_BaseItem.SelectedItem = selecteditem.SourceItem;
            Conversion_Correction_ResultITem.SelectedItem = selecteditem.ResultItem;
            Conversion_Correction_BaseItemCount.Text = selecteditem.SourceItemCount.ToString();
            Conversion_Correction_ResultItemCount.Text = selecteditem.ResultItemCount.ToString();

            if(selecteditem.LaborCost != null)
            {
                Conversion_Correction_LaborCost.Text = selecteditem.LaborCost.ToString();
            }
            else
            {
                Conversion_Correction_LaborCost.Text = "0";
            }
            if (selecteditem.fixcost != null)
            {
                Conversion_Correction_FixCost.Text = selecteditem.fixcost.ToString();
            }
            else
            {
                Conversion_Correction_FixCost.Text = "0";
            }

        }

        private void Conversion_Correction_Delete_Click(object sender, EventArgs e)
        {
            List<object> itemstodelete = new List<object>();
            Conversion selecteditem = (Conversion) Conversion_Correction_ItemSelect.SelectedItem;
            var loc = ConversionDB.GetCollection<Conversion>("Conversion");
            Conversion_Correction_ItemSelect.Items.RemoveAt(Conversion_Correction_ItemSelect.SelectedIndex);
            loc.Delete(selecteditem.Id);
        }

        private void Conversion_Correction_SaveBtn_Click(object sender, EventArgs e)
        {
            Conversion selecteditem = (Conversion)Conversion_Correction_ItemSelect.SelectedItem;
            Conversion newitem = new Conversion();
            newitem.SourceItem = Conversion_Correction_BaseItem.SelectedItem.ToString();
            newitem.ResultItem = Conversion_Correction_ResultITem.SelectedItem.ToString();
            try
            {
                newitem.SourceItemCount = Int32.Parse(Conversion_Correction_BaseItemCount.Text);
                newitem.ResultItemCount = Int32.Parse(Conversion_Correction_ResultItemCount.Text);
                newitem.LaborCost = Int32.Parse(Conversion_Correction_LaborCost.Text);
                newitem.fixcost = Int32.Parse(Conversion_Correction_FixCost.Text);
            } catch(Exception)
            {
                MessageBox.Show("No Letters in Numberfields!");
            }
            var loc = ConversionDB.GetCollection<Conversion>("Conversion");
            loc.Insert(newitem);
            loc.Delete(selecteditem.Id);
            Conversion_Correction_ItemSelect.Items.Remove(selecteditem);
            Conversion_Correction_ItemSelect.Items.Add(newitem);
            Conversion_Correction_ItemSelect.SelectedItem = newitem;
        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void Craftin_Recipes_AddRecipeItem_Click(object sender, EventArgs e)
        {

            string RecipeItemName = "";
            string RecipeName = "";
            int Amount = 0;
            int Cost = 0;
            bool newitem = false;
            int LaborCost = 0;
            bool newSubitem = false;
            
            if (Crafting_Recipes_RecipeName.Text != "")
            {
                RecipeName = Crafting_Recipes_RecipeName.Text;
                newitem = true;
            }
            else
            {
                if (Crafting_Recipes_Recipe.SelectedItem != "")
                {
                    RecipeName = Crafting_Recipes_Recipe.SelectedItem.ToString();
                }
            }
            if(!Int32.TryParse(Crafting_Recipes_LaborCost.Text,out LaborCost))
            {
                MessageBox.Show("LaborCost Must be at least 0 and a number");
                return;
            }
            Crafting_Recipes_RecipeName.Text = "";
            if (RecipeName == "")
            {
                MessageBox.Show("Recipe must be set/selected");
                return;
            }

            if (Crafting_Recipes_RecipeItemName.Text != "")
            {
                RecipeItemName = Crafting_Recipes_RecipeItemName.Text;
                newSubitem = true;
            }
            else
            {
                if(Crafting_Recipes_RecipeItem.SelectedItem != "")
                {
                    RecipeItemName = Crafting_Recipes_RecipeItem.SelectedItem.ToString();
                }
            }
            if(RecipeItemName == "")
            {
                MessageBox.Show("Recipe Item must be set/selected");
                return;
            }
            if(!Int32.TryParse(Crafting_Recipes_RecipeItemAmount.Text,out Amount))
            {
                MessageBox.Show("Amount either missing or not a number");
                return;
            }
            Int32.TryParse(Crafting_Recipes_RecipeItemCost.Text, out Cost);
            RecipeItem Item;
            if(newitem)
            {
                Item = new RecipeItem();
                Item.Amount = Amount;
                Item.Name = RecipeName.Replace(" ", "_");
                Item.vendorcost = Cost;
                Item.LaborCost = LaborCost;
                Crafting_Recipes_Recipe.Items.Add(Item);
                Crafting_Recipes_RecipeItem.Items.Add(Item);
                Crafting_Recipes_Recipe.SelectedItem = Item;
            }
            else
            {
                Item = (RecipeItem)Crafting_Recipes_Recipe.SelectedItem;
                Crafting_Recipes_RecipeItem.Items.Remove(Item);

                RecipeItem Subitem = new RecipeItem();
                if(newSubitem)
                {
                    Subitem.Amount = Amount;
                    Subitem.Name = RecipeName.Replace(" ", "_");
                    Subitem.vendorcost = Cost;
                    Subitem.LaborCost = LaborCost;
                }
                else
                {
                    Subitem = (RecipeItem) Crafting_Recipes_RecipeItem.SelectedItem;
                }
                Item.AddSubItem(Subitem);
                Crafting_Recipes_RecipeItem.Items.Add(Item);
            }
        }

        private void Crafting_Recipes_SaveRecipe_Click(object sender, EventArgs e)
        {
            foreach (RecipeItem item in Crafting_Recipes_Recipe.Items)
            {
                if(!item.saved)
                {
                    var col = RecipeDB.GetCollection<RecipeItem>("Recipe");
                    col.Insert(item);
                }
            }
        }

        private void Crafting_Profit_Check_Click(object sender, EventArgs e)
        {
            var col = RecipeDB.GetCollection<RecipeItem>("Recipe");
            var Recipes = col.FindAll();
            foreach (RecipeItem item in Recipes)
            {
                int totalcost = 0;
                int totallaborcost = 0;
                if(item.vendorcost > 0)
                {
                    totalcost += item.vendorcost;
                }
                if(item.LaborCost>0)
                {
                    totallaborcost += item.LaborCost;
                }
                // Inventory I don't give a fuck about Laborcost
                var loc = InventoryDB.GetCollection<InventoryItem>(item.Name);
                var inventoryItems = loc.FindAll();
                int inventoryPrice = 999999999;
                foreach(var inventoryitem in inventoryItems)
                {
                    int tempprice = (inventoryitem.gold * 100 * 100 + inventoryitem.silver * 100 + inventoryitem.copper);
                    if (inventoryPrice > tempprice)
                    {
                        inventoryPrice = tempprice;
                    }
                }
                //Auction
                var auctionloc = DB.GetCollection<AuctionItem>(item.Name);
                var auctionItems = auctionloc.FindAll().Where(x => x.TimeStamp > ((int)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds - 60 * 60 * 24));
                int auctionprice = 999999999;
                foreach (AuctionItem auctionitem in auctionItems)
                {
                    if(auctionitem.BuyoutPrice < auctionprice)
                    {
                        auctionprice = auctionitem.BuyoutPrice;
                    }
                }
                //Auction+Crafting
                RecipeItem temprecipeitem;
                temprecipeitem = item;
                // Build into object to calculate it's own price either through crafting / auction
                
            }
        }
    }
}