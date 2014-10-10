namespace Check_up.forms
{
    partial class frmExportDBRecords
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Users"}, -1, System.Drawing.Color.Black, System.Drawing.Color.Empty, null);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Warehouse"}, -1, System.Drawing.Color.Black, System.Drawing.Color.Empty, null);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Item Master Data"}, -1, System.Drawing.Color.Black, System.Drawing.Color.Empty, null);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "Price Lists"}, -1, System.Drawing.Color.Black, System.Drawing.Color.Empty, null);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "Price Lists History"}, -1, System.Drawing.Color.Black, System.Drawing.Color.Empty, null);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "Barcodes"}, -1, System.Drawing.Color.Black, System.Drawing.Color.Empty, null);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] {
            "Barcodes History"}, -1, System.Drawing.Color.Black, System.Drawing.Color.Empty, null);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] {
            "Purchase Orders"}, -1, System.Drawing.Color.Black, System.Drawing.Color.Empty, null);
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new string[] {
            "Goods Receipt PO\'s"}, -1, System.Drawing.Color.Black, System.Drawing.Color.Empty, null);
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem("Inventory Transfer");
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem(new string[] {
            "Goods Returns"}, -1, System.Drawing.Color.Black, System.Drawing.Color.Empty, null);
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem(new string[] {
            "Sales Invoices"}, -1, System.Drawing.Color.Black, System.Drawing.Color.Empty, null);
            System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem(new string[] {
            "Sales Returns"}, -1, System.Drawing.Color.Black, System.Drawing.Color.Empty, null);
            System.Windows.Forms.ListViewItem listViewItem14 = new System.Windows.Forms.ListViewItem("Delivery Receipt");
            System.Windows.Forms.ListViewItem listViewItem15 = new System.Windows.Forms.ListViewItem("Inventory Posting");
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            listViewItem1.Checked = true;
            listViewItem1.StateImageIndex = 1;
            listViewItem1.Tag = "DefaultRecords";
            listViewItem2.Checked = true;
            listViewItem2.StateImageIndex = 1;
            listViewItem2.Tag = "DefaultRecords";
            listViewItem3.Checked = true;
            listViewItem3.StateImageIndex = 1;
            listViewItem3.Tag = "DefaultRecords";
            listViewItem4.Checked = true;
            listViewItem4.StateImageIndex = 1;
            listViewItem4.Tag = "DefaultRecords";
            listViewItem5.Checked = true;
            listViewItem5.StateImageIndex = 1;
            listViewItem5.Tag = "DefaultRecords";
            listViewItem6.Checked = true;
            listViewItem6.StateImageIndex = 1;
            listViewItem6.Tag = "DefaultRecords";
            listViewItem7.Checked = true;
            listViewItem7.StateImageIndex = 1;
            listViewItem7.Tag = "DefaultRecords";
            listViewItem8.Checked = true;
            listViewItem8.StateImageIndex = 1;
            listViewItem9.Checked = true;
            listViewItem9.StateImageIndex = 1;
            listViewItem10.Checked = true;
            listViewItem10.StateImageIndex = 1;
            listViewItem11.Checked = true;
            listViewItem11.StateImageIndex = 1;
            listViewItem12.Checked = true;
            listViewItem12.StateImageIndex = 1;
            listViewItem13.Checked = true;
            listViewItem13.StateImageIndex = 1;
            listViewItem14.Checked = true;
            listViewItem14.StateImageIndex = 1;
            listViewItem15.Checked = true;
            listViewItem15.StateImageIndex = 1;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10,
            listViewItem11,
            listViewItem12,
            listViewItem13,
            listViewItem14,
            listViewItem15});
            this.listView1.Location = new System.Drawing.Point(12, 49);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(362, 335);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Records";
            this.columnHeader1.Width = 241;
            // 
            // btnUpload
            // 
            this.btnUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpload.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnUpload.Location = new System.Drawing.Point(218, 410);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 1;
            this.btnUpload.Text = "&Export";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(299, 410);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select the records you want to export.";
            // 
            // frmExportDBRecords
            // 
            this.AcceptButton = this.btnUpload;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(386, 457);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.listView1);
            this.Name = "frmExportDBRecords";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Export Database Records";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
    }
}