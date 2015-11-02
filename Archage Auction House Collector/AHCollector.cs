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

        private void UpdateChartBtn_Click(object sender, EventArgs e)
        {
            ReadDatabase("test");
        }

        private void Measurment_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        private void ReadDatabase(string _table,int starttimestamp = 0,int endtimestamp = 0)
        {
            var col = DB.GetCollection<Item>(_table);
            var results = col.FindAll();
            foreach(var result in results)
            {
                MessageBox.Show(result.Amount.ToString());
            }

        }
    }
}
