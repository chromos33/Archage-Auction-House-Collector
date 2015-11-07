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
        LiteDatabase newDB;
        string path;
        string newpath;
        public Archage_AH_DataCollector()
        {
            InitializeComponent();
            path = Directory.GetCurrentDirectory() + "\\ItemDB.db";
            DB = new LiteDatabase(@path);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            IEnumerable<string> CollectionNames = DB.GetCollectionNames();
            foreach (string name in CollectionNames)
            {

                ItemNameComboBox.Items.Add(name);
                Measurment.Items.Add(name);
                MeasurementCorection.Items.Add(name);
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
            if (!(int.TryParse(BidInCopper.Text, out Bid)))
            {
                MessageBox.Show("Bid in Copper missing or not a Number");
                return;
            }
            if (!(int.TryParse(BuyoutinCopper.Text, out Buyout)))
            {
                MessageBox.Show("Buyout in Copper missing or not a Number");
                return;
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
            }
            saveObjecttoDB(newItemEntry);
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
    }
}