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

namespace Archage_Auction_House_Collector
{
    public partial class Archage_AH_DataCollector : Form
    {
        LiteDatabase DB;
        public Archage_AH_DataCollector()
        {
            InitializeComponent();
            string path = Directory.GetCurrentDirectory()+"\\ItemDB.db";
            DB = new LiteDatabase(@path);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IEnumerable<string> CollectionNames = DB.GetCollectionNames();
            foreach (string name in CollectionNames)
            {
                Measurment.Items.Add(name);
            }
            if (Measurment.Items.Count > 0)
            {
                Measurment.SelectedIndex = 0;
            }
            timespanselect.SelectedIndex = 0;
        }

        private void Save_btn_Click(object sender, EventArgs e)
        {
            if(ItemName.Text == "")
            {
                MessageBox.Show("Item Name missing");
                return;
            }
            int Bid = 0;
            int Buyout = 0;
            int Amount = 0;
            if(!(int.TryParse(BidInCopper.Text,out Bid)))
            {
                MessageBox.Show("Bid in Copper missing or not a Number");
                return;
            }
            if (!(int.TryParse(BuyoutinCopper.Text, out Buyout)))
            {
                MessageBox.Show("Buyout in Copper missing or not a Number");
                return;
            }
            if(ItemAmount.Text=="" && Asume.Checked)
            {
                Amount = 1;   
            }
            else
            {
                if(!int.TryParse(ItemAmount.Text,out Amount))
                {
                    MessageBox.Show("Item Amount missing or not a Number. (Check Setting to assume 1 if left empty)");
                    return;
                }
            }
            Item newItemEntry = new Item(ItemName.Text, Buyout, Bid,Amount);
            newItemEntry.setTimeStamp(DateTime.Now);
            if(!Measurment.Items.Contains(newItemEntry.ItemName))
            {
                Measurment.Items.Add(newItemEntry.ItemName);
            }
            saveObjecttoDB(newItemEntry);
        }
        private void saveObjecttoDB(Item insert)
        {
            var col = DB.GetCollection<Item>(insert.ItemName);
            col.Insert(insert);
        }

        private void Measurment_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private IEnumerable<Item> ReadDatabase(string _table, int starttimestamp = 0)
        {
            int endtimestamp = (int)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var col = DB.GetCollection<Item>(_table);
            var results = col.Find(x => x.ItemName == _table).Where(x=>x.TimeStamp > starttimestamp).Where(x=>x.TimeStamp < endtimestamp);
            return results;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
        private string HTMLString(IEnumerable<Item> items)
        {
            string result = "<!DOCTYPE html>";
            result =  "<html>" + System.Environment.NewLine;
            result += "<head>" + System.Environment.NewLine;
            result += "<meta http-equiv='X-UA-Compatible' content='IE=Edge'>";
            result += "</head>" + System.Environment.NewLine;
            result += "<body>" + System.Environment.NewLine;
            string curDir = "file:///" + Directory.GetCurrentDirectory()+@"\Scripts\Chart.js";
            result += "<script type='text/javascript' src='"+ curDir + "'></script>" + System.Environment.NewLine;
            result += "<canvas ID='datachart' css='width:100%;height:100%;' width='800' height='400'></canvas>" + System.Environment.NewLine;
            result += "<script>" + System.Environment.NewLine;
            result += "var ctx = document.getElementById('datachart').getContext('2d');" + System.Environment.NewLine;
            result += "var data = {" + System.Environment.NewLine;
            result += "labels: [" + System.Environment.NewLine;
            foreach (var item in items)
            {
                result += "'" + item.TimeStamp + "',";
            }
            result = result.Remove(result.Length - 1);
            result += "],"+ System.Environment.NewLine;
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
            foreach (var item in items)
            {
                result += item.BuyoutPerPiece() + ",";
            }
            result = result.Remove(result.Length - 1);
            result += "]"+ System.Environment.NewLine;
            result += "},"+ System.Environment.NewLine;
            result += "{" + System.Environment.NewLine;
            result += "label: '_BidPerPiece'," + System.Environment.NewLine;
            result += "fillColor: 'rgba(255,255,255,0)'," + System.Environment.NewLine;
            result += "strokeColor: 'rgba(0,0,255,1)'," + System.Environment.NewLine;
            result += "pointColor: 'rgba(0,0,255,1)'," + System.Environment.NewLine;
            result += "pointStrokeColor: '#fff'," + System.Environment.NewLine;
            result += "pointHighlightFill: '#fff'," + System.Environment.NewLine;
            result += "pointHighlightStroke: 'rgba(0,0,255,1)'," + System.Environment.NewLine;
            result += "data: [" + System.Environment.NewLine;
            foreach (var item in items)
            {
                result += item.BidPerPiece() + ",";
            }
            result = result.Remove(result.Length - 1);
            result += "]" + System.Environment.NewLine;
            result += "}" + System.Environment.NewLine;
            result += "]" + System.Environment.NewLine;
            result += "}" + System.Environment.NewLine;


            result += "var linechart = new Chart(ctx).Line(data);" + System.Environment.NewLine;
            result += "</script>" + System.Environment.NewLine;

            result += "</body>" + System.Environment.NewLine;
            result += "</html>" + System.Environment.NewLine;
            System.Windows.Forms.Clipboard.SetText(result);

            return result;
        }

        private void updatechart_Click(object sender, EventArgs e)
        {
            if(Measurment.Items.Count>0)
            {
                int starttime = (int)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                switch (timespanselect.SelectedIndex)
                {
                    case 0: starttime -= 604800; break;
                    case 1: starttime -= 2592000; break;
                    case 2: starttime -= 31536000; break;
                }
                var result = ReadDatabase(Measurment.Text.ToLower(), starttime);
                if (result.Count() > 0)
                {
                    Chartbrowser.Navigate("about:blank");

                    if (Chartbrowser.Document != null)
                    {
                        Chartbrowser.Document.Write(string.Empty);
                    }
                    Chartbrowser.DocumentText = HTMLString(result);
                }
            }
        }
    }
}
