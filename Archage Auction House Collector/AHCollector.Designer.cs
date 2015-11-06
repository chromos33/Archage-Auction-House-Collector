using System;

namespace Archage_Auction_House_Collector
{
    partial class Archage_AH_DataCollector
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ItemNameLabel = new System.Windows.Forms.Label();
            this.ItemName = new System.Windows.Forms.TextBox();
            this.BidInCopper = new System.Windows.Forms.TextBox();
            this.BidInCopperLabel = new System.Windows.Forms.Label();
            this.BuyoutinCopper = new System.Windows.Forms.TextBox();
            this.BuyInCopperLabel = new System.Windows.Forms.Label();
            this.ItemAmount = new System.Windows.Forms.TextBox();
            this.AmountLabel = new System.Windows.Forms.Label();
            this.Save_btn = new System.Windows.Forms.Button();
            this.Measurment = new System.Windows.Forms.ComboBox();
            this.MeasurementLabel = new System.Windows.Forms.Label();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.DataEntry = new System.Windows.Forms.TabPage();
            this.ItemNameComboBox = new System.Windows.Forms.ComboBox();
            this.DataExploration = new System.Windows.Forms.TabPage();
            this.updatechart = new System.Windows.Forms.Button();
            this.Chartbrowser = new System.Windows.Forms.WebBrowser();
            this.timespanlabel = new System.Windows.Forms.Label();
            this.timespanselect = new System.Windows.Forms.ComboBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.Asume = new System.Windows.Forms.CheckBox();
            this.PointTab = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.TimeStampList = new System.Windows.Forms.CheckedListBox();
            this.MeasurementCorection = new System.Windows.Forms.ComboBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.deletepoint = new System.Windows.Forms.Button();
            this.Tabs.SuspendLayout();
            this.DataEntry.SuspendLayout();
            this.DataExploration.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.PointTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // ItemNameLabel
            // 
            this.ItemNameLabel.AutoSize = true;
            this.ItemNameLabel.Location = new System.Drawing.Point(3, 3);
            this.ItemNameLabel.Name = "ItemNameLabel";
            this.ItemNameLabel.Size = new System.Drawing.Size(58, 13);
            this.ItemNameLabel.TabIndex = 0;
            this.ItemNameLabel.Text = "Item Name";
            // 
            // ItemName
            // 
            this.ItemName.Location = new System.Drawing.Point(6, 19);
            this.ItemName.Name = "ItemName";
            this.ItemName.Size = new System.Drawing.Size(147, 20);
            this.ItemName.TabIndex = 1;
            // 
            // BidInCopper
            // 
            this.BidInCopper.Location = new System.Drawing.Point(159, 19);
            this.BidInCopper.Name = "BidInCopper";
            this.BidInCopper.Size = new System.Drawing.Size(100, 20);
            this.BidInCopper.TabIndex = 3;
            // 
            // BidInCopperLabel
            // 
            this.BidInCopperLabel.AutoSize = true;
            this.BidInCopperLabel.Location = new System.Drawing.Point(156, 3);
            this.BidInCopperLabel.Name = "BidInCopperLabel";
            this.BidInCopperLabel.Size = new System.Drawing.Size(70, 13);
            this.BidInCopperLabel.TabIndex = 2;
            this.BidInCopperLabel.Text = "Bid in Copper";
            // 
            // BuyoutinCopper
            // 
            this.BuyoutinCopper.Location = new System.Drawing.Point(265, 19);
            this.BuyoutinCopper.Name = "BuyoutinCopper";
            this.BuyoutinCopper.Size = new System.Drawing.Size(85, 20);
            this.BuyoutinCopper.TabIndex = 5;
            // 
            // BuyInCopperLabel
            // 
            this.BuyInCopperLabel.AutoSize = true;
            this.BuyInCopperLabel.Location = new System.Drawing.Point(262, 3);
            this.BuyInCopperLabel.Name = "BuyInCopperLabel";
            this.BuyInCopperLabel.Size = new System.Drawing.Size(88, 13);
            this.BuyInCopperLabel.TabIndex = 4;
            this.BuyInCopperLabel.Text = "Buyout in Copper";
            // 
            // ItemAmount
            // 
            this.ItemAmount.Location = new System.Drawing.Point(356, 19);
            this.ItemAmount.Name = "ItemAmount";
            this.ItemAmount.Size = new System.Drawing.Size(63, 20);
            this.ItemAmount.TabIndex = 7;
            // 
            // AmountLabel
            // 
            this.AmountLabel.AutoSize = true;
            this.AmountLabel.Location = new System.Drawing.Point(353, 3);
            this.AmountLabel.Name = "AmountLabel";
            this.AmountLabel.Size = new System.Drawing.Size(66, 13);
            this.AmountLabel.TabIndex = 6;
            this.AmountLabel.Text = "Item Amount";
            // 
            // Save_btn
            // 
            this.Save_btn.Location = new System.Drawing.Point(196, 59);
            this.Save_btn.Name = "Save_btn";
            this.Save_btn.Size = new System.Drawing.Size(75, 23);
            this.Save_btn.TabIndex = 8;
            this.Save_btn.Text = "Save Item";
            this.Save_btn.UseVisualStyleBackColor = true;
            this.Save_btn.Click += new System.EventHandler(this.Save_btn_Click);
            // 
            // Measurment
            // 
            this.Measurment.FormattingEnabled = true;
            this.Measurment.Location = new System.Drawing.Point(9, 32);
            this.Measurment.Name = "Measurment";
            this.Measurment.Size = new System.Drawing.Size(121, 21);
            this.Measurment.TabIndex = 9;
            this.Measurment.SelectedIndexChanged += new System.EventHandler(this.Measurment_SelectedIndexChanged);
            // 
            // MeasurementLabel
            // 
            this.MeasurementLabel.AutoSize = true;
            this.MeasurementLabel.Location = new System.Drawing.Point(6, 16);
            this.MeasurementLabel.Name = "MeasurementLabel";
            this.MeasurementLabel.Size = new System.Drawing.Size(27, 13);
            this.MeasurementLabel.TabIndex = 10;
            this.MeasurementLabel.Text = "Item";
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.DataEntry);
            this.Tabs.Controls.Add(this.DataExploration);
            this.Tabs.Controls.Add(this.tabPage1);
            this.Tabs.Controls.Add(this.PointTab);
            this.Tabs.Location = new System.Drawing.Point(-2, -1);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(1027, 532);
            this.Tabs.TabIndex = 11;
            // 
            // DataEntry
            // 
            this.DataEntry.Controls.Add(this.ItemNameComboBox);
            this.DataEntry.Controls.Add(this.ItemNameLabel);
            this.DataEntry.Controls.Add(this.Save_btn);
            this.DataEntry.Controls.Add(this.ItemName);
            this.DataEntry.Controls.Add(this.BidInCopperLabel);
            this.DataEntry.Controls.Add(this.BidInCopper);
            this.DataEntry.Controls.Add(this.ItemAmount);
            this.DataEntry.Controls.Add(this.BuyInCopperLabel);
            this.DataEntry.Controls.Add(this.AmountLabel);
            this.DataEntry.Controls.Add(this.BuyoutinCopper);
            this.DataEntry.Location = new System.Drawing.Point(4, 22);
            this.DataEntry.Name = "DataEntry";
            this.DataEntry.Padding = new System.Windows.Forms.Padding(3);
            this.DataEntry.Size = new System.Drawing.Size(1019, 506);
            this.DataEntry.TabIndex = 0;
            this.DataEntry.Text = "Data entry";
            this.DataEntry.UseVisualStyleBackColor = true;
            // 
            // ItemNameComboBox
            // 
            this.ItemNameComboBox.FormattingEnabled = true;
            this.ItemNameComboBox.Location = new System.Drawing.Point(6, 45);
            this.ItemNameComboBox.Name = "ItemNameComboBox";
            this.ItemNameComboBox.Size = new System.Drawing.Size(147, 21);
            this.ItemNameComboBox.TabIndex = 9;
            // 
            // DataExploration
            // 
            this.DataExploration.Controls.Add(this.updatechart);
            this.DataExploration.Controls.Add(this.Chartbrowser);
            this.DataExploration.Controls.Add(this.timespanlabel);
            this.DataExploration.Controls.Add(this.timespanselect);
            this.DataExploration.Controls.Add(this.MeasurementLabel);
            this.DataExploration.Controls.Add(this.Measurment);
            this.DataExploration.Location = new System.Drawing.Point(4, 22);
            this.DataExploration.Name = "DataExploration";
            this.DataExploration.Padding = new System.Windows.Forms.Padding(3);
            this.DataExploration.Size = new System.Drawing.Size(1019, 506);
            this.DataExploration.TabIndex = 1;
            this.DataExploration.Text = "Data exploration";
            this.DataExploration.UseVisualStyleBackColor = true;
            this.DataExploration.Click += new System.EventHandler(this.DataExploration_Click);
            // 
            // updatechart
            // 
            this.updatechart.Location = new System.Drawing.Point(10, 108);
            this.updatechart.Name = "updatechart";
            this.updatechart.Size = new System.Drawing.Size(120, 23);
            this.updatechart.TabIndex = 15;
            this.updatechart.Text = "Update Chart";
            this.updatechart.UseVisualStyleBackColor = true;
            this.updatechart.Click += new System.EventHandler(this.updatechart_Click);
            // 
            // Chartbrowser
            // 
            this.Chartbrowser.Location = new System.Drawing.Point(136, 3);
            this.Chartbrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.Chartbrowser.Name = "Chartbrowser";
            this.Chartbrowser.Size = new System.Drawing.Size(883, 503);
            this.Chartbrowser.TabIndex = 14;
            // 
            // timespanlabel
            // 
            this.timespanlabel.AutoSize = true;
            this.timespanlabel.Location = new System.Drawing.Point(6, 56);
            this.timespanlabel.Name = "timespanlabel";
            this.timespanlabel.Size = new System.Drawing.Size(53, 13);
            this.timespanlabel.TabIndex = 12;
            this.timespanlabel.Text = "Timespan";
            // 
            // timespanselect
            // 
            this.timespanselect.FormattingEnabled = true;
            this.timespanselect.Items.AddRange(new object[] {
            "Last 7 Days",
            "Last 30 Days",
            "Last 365 Days"});
            this.timespanselect.Location = new System.Drawing.Point(9, 72);
            this.timespanselect.Name = "timespanselect";
            this.timespanselect.Size = new System.Drawing.Size(121, 21);
            this.timespanselect.TabIndex = 11;
            this.timespanselect.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.Asume);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1019, 506);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // Asume
            // 
            this.Asume.AutoSize = true;
            this.Asume.Location = new System.Drawing.Point(21, 17);
            this.Asume.Name = "Asume";
            this.Asume.Size = new System.Drawing.Size(187, 17);
            this.Asume.TabIndex = 1;
            this.Asume.Text = "assume 1 if Amount not specified?";
            this.Asume.UseVisualStyleBackColor = true;
            // 
            // PointTab
            // 
            this.PointTab.Controls.Add(this.deletepoint);
            this.PointTab.Controls.Add(this.label1);
            this.PointTab.Controls.Add(this.TimeStampList);
            this.PointTab.Controls.Add(this.MeasurementCorection);
            this.PointTab.Location = new System.Drawing.Point(4, 22);
            this.PointTab.Name = "PointTab";
            this.PointTab.Padding = new System.Windows.Forms.Padding(3);
            this.PointTab.Size = new System.Drawing.Size(1019, 506);
            this.PointTab.TabIndex = 3;
            this.PointTab.Text = "Point Correction";
            this.PointTab.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Item Auswahl";
            // 
            // TimeStampList
            // 
            this.TimeStampList.FormattingEnabled = true;
            this.TimeStampList.Location = new System.Drawing.Point(208, 33);
            this.TimeStampList.Name = "TimeStampList";
            this.TimeStampList.Size = new System.Drawing.Size(399, 439);
            this.TimeStampList.TabIndex = 2;
            // 
            // MeasurementCorection
            // 
            this.MeasurementCorection.FormattingEnabled = true;
            this.MeasurementCorection.Location = new System.Drawing.Point(35, 33);
            this.MeasurementCorection.Name = "MeasurementCorection";
            this.MeasurementCorection.Size = new System.Drawing.Size(121, 21);
            this.MeasurementCorection.TabIndex = 0;
            this.MeasurementCorection.SelectedIndexChanged += new System.EventHandler(this.MeasurementCorection_SelectedIndexChanged);
            // 
            // deletepoint
            // 
            this.deletepoint.Location = new System.Drawing.Point(35, 433);
            this.deletepoint.Name = "deletepoint";
            this.deletepoint.Size = new System.Drawing.Size(146, 39);
            this.deletepoint.TabIndex = 4;
            this.deletepoint.Text = "Delete Point";
            this.deletepoint.UseVisualStyleBackColor = true;
            this.deletepoint.Click += new System.EventHandler(this.deletepoint_Click);
            // 
            // Archage_AH_DataCollector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 527);
            this.Controls.Add(this.Tabs);
            this.Name = "Archage_AH_DataCollector";
            this.Text = "           ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Tabs.ResumeLayout(false);
            this.DataEntry.ResumeLayout(false);
            this.DataEntry.PerformLayout();
            this.DataExploration.ResumeLayout(false);
            this.DataExploration.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.PointTab.ResumeLayout(false);
            this.PointTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ItemNameLabel;
        private System.Windows.Forms.TextBox ItemName;
        private System.Windows.Forms.TextBox BidInCopper;
        private System.Windows.Forms.Label BidInCopperLabel;
        private System.Windows.Forms.TextBox BuyoutinCopper;
        private System.Windows.Forms.Label BuyInCopperLabel;
        private System.Windows.Forms.TextBox ItemAmount;
        private System.Windows.Forms.Label AmountLabel;
        private System.Windows.Forms.Button Save_btn;
        private System.Windows.Forms.ComboBox Measurment;
        private System.Windows.Forms.Label MeasurementLabel;
        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage DataEntry;
        private System.Windows.Forms.TabPage DataExploration;
        private System.Windows.Forms.Label timespanlabel;
        private System.Windows.Forms.ComboBox timespanselect;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox Asume;
        private System.Windows.Forms.WebBrowser Chartbrowser;
        private System.Windows.Forms.Button updatechart;
        private System.Windows.Forms.ComboBox ItemNameComboBox;
        private System.Windows.Forms.TabPage PointTab;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox TimeStampList;
        private System.Windows.Forms.ComboBox MeasurementCorection;
        private System.Windows.Forms.Button deletepoint;
    }
}

