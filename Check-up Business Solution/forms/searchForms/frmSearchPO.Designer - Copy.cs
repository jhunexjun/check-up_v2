namespace Check_up.forms
{
    partial class frmSearchGRPO
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.txtPONoFrm = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtPickerDate = new System.Windows.Forms.DateTimePicker();
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.txtDateTo = new System.Windows.Forms.TextBox();
            this.txtDateFrm = new System.Windows.Forms.TextBox();
            this.chkVendor = new System.Windows.Forms.CheckBox();
            this.chkPO = new System.Windows.Forms.CheckBox();
            this.linkLabelVendor = new System.Windows.Forms.LinkLabel();
            this.txtVendorTo = new System.Windows.Forms.TextBox();
            this.txtVendorFrm = new System.Windows.Forms.TextBox();
            this.linkLabelPO = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPONoTo = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.btnExport = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(504, 79);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(504, 21);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 23);
            this.btnFind.TabIndex = 13;
            this.btnFind.Text = "&Search";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // txtPONoFrm
            // 
            this.txtPONoFrm.Enabled = false;
            this.txtPONoFrm.Location = new System.Drawing.Point(128, 33);
            this.txtPONoFrm.Name = "txtPONoFrm";
            this.txtPONoFrm.Size = new System.Drawing.Size(150, 20);
            this.txtPONoFrm.TabIndex = 2;
            this.txtPONoFrm.TextChanged += new System.EventHandler(this.txtPONoFrm_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtPickerDate);
            this.groupBox1.Controls.Add(this.chkDate);
            this.groupBox1.Controls.Add(this.txtDateTo);
            this.groupBox1.Controls.Add(this.txtDateFrm);
            this.groupBox1.Controls.Add(this.chkVendor);
            this.groupBox1.Controls.Add(this.chkPO);
            this.groupBox1.Controls.Add(this.linkLabelVendor);
            this.groupBox1.Controls.Add(this.txtVendorTo);
            this.groupBox1.Controls.Add(this.txtVendorFrm);
            this.groupBox1.Controls.Add(this.linkLabelPO);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtPONoTo);
            this.groupBox1.Controls.Add(this.txtPONoFrm);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(486, 118);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // dtPickerDate
            // 
            this.dtPickerDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtPickerDate.Enabled = false;
            this.dtPickerDate.Location = new System.Drawing.Point(433, 83);
            this.dtPickerDate.Name = "dtPickerDate";
            this.dtPickerDate.Size = new System.Drawing.Size(15, 20);
            this.dtPickerDate.TabIndex = 140;
            this.dtPickerDate.ValueChanged += new System.EventHandler(this.dtPickerDate_ValueChanged);
            // 
            // chkDate
            // 
            this.chkDate.AccessibleDescription = "";
            this.chkDate.AutoSize = true;
            this.chkDate.Location = new System.Drawing.Point(24, 82);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(49, 17);
            this.chkDate.TabIndex = 9;
            this.chkDate.Text = "Date";
            this.chkDate.UseVisualStyleBackColor = true;
            this.chkDate.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // txtDateTo
            // 
            this.txtDateTo.Enabled = false;
            this.txtDateTo.Location = new System.Drawing.Point(284, 83);
            this.txtDateTo.Name = "txtDateTo";
            this.txtDateTo.Size = new System.Drawing.Size(150, 20);
            this.txtDateTo.TabIndex = 11;
            this.txtDateTo.TextChanged += new System.EventHandler(this.txtDateTo_TextChanged);
            // 
            // txtDateFrm
            // 
            this.txtDateFrm.Enabled = false;
            this.txtDateFrm.Location = new System.Drawing.Point(128, 83);
            this.txtDateFrm.Name = "txtDateFrm";
            this.txtDateFrm.Size = new System.Drawing.Size(150, 20);
            this.txtDateFrm.TabIndex = 10;
            this.txtDateFrm.TextChanged += new System.EventHandler(this.txtDateFrm_TextChanged);
            // 
            // chkVendor
            // 
            this.chkVendor.AccessibleDescription = "";
            this.chkVendor.AutoSize = true;
            this.chkVendor.Location = new System.Drawing.Point(24, 61);
            this.chkVendor.Name = "chkVendor";
            this.chkVendor.Size = new System.Drawing.Size(88, 17);
            this.chkVendor.TabIndex = 5;
            this.chkVendor.Text = "Vendor Code";
            this.chkVendor.UseVisualStyleBackColor = true;
            this.chkVendor.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // chkPO
            // 
            this.chkPO.AutoSize = true;
            this.chkPO.Location = new System.Drawing.Point(24, 38);
            this.chkPO.Name = "chkPO";
            this.chkPO.Size = new System.Drawing.Size(58, 17);
            this.chkPO.TabIndex = 1;
            this.chkPO.Text = "PO No";
            this.chkPO.UseVisualStyleBackColor = true;
            this.chkPO.CheckedChanged += new System.EventHandler(this.chkPO_CheckedChanged);
            // 
            // linkLabelVendor
            // 
            this.linkLabelVendor.AutoSize = true;
            this.linkLabelVendor.Enabled = false;
            this.linkLabelVendor.Location = new System.Drawing.Point(440, 60);
            this.linkLabelVendor.Name = "linkLabelVendor";
            this.linkLabelVendor.Size = new System.Drawing.Size(19, 13);
            this.linkLabelVendor.TabIndex = 8;
            this.linkLabelVendor.TabStop = true;
            this.linkLabelVendor.Text = ">>";
            this.linkLabelVendor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelVendor_LinkClicked);
            // 
            // txtVendorTo
            // 
            this.txtVendorTo.Enabled = false;
            this.txtVendorTo.Location = new System.Drawing.Point(284, 59);
            this.txtVendorTo.Name = "txtVendorTo";
            this.txtVendorTo.Size = new System.Drawing.Size(150, 20);
            this.txtVendorTo.TabIndex = 7;
            this.txtVendorTo.TextChanged += new System.EventHandler(this.txtVendorTo_TextChanged);
            // 
            // txtVendorFrm
            // 
            this.txtVendorFrm.Enabled = false;
            this.txtVendorFrm.Location = new System.Drawing.Point(128, 59);
            this.txtVendorFrm.Name = "txtVendorFrm";
            this.txtVendorFrm.Size = new System.Drawing.Size(150, 20);
            this.txtVendorFrm.TabIndex = 6;
            this.txtVendorFrm.TextChanged += new System.EventHandler(this.txtVendorFrm_TextChanged);
            // 
            // linkLabelPO
            // 
            this.linkLabelPO.AutoSize = true;
            this.linkLabelPO.Enabled = false;
            this.linkLabelPO.Location = new System.Drawing.Point(440, 35);
            this.linkLabelPO.Name = "linkLabelPO";
            this.linkLabelPO.Size = new System.Drawing.Size(19, 13);
            this.linkLabelPO.TabIndex = 4;
            this.linkLabelPO.TabStop = true;
            this.linkLabelPO.Text = ">>";
            this.linkLabelPO.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(352, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "To";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(174, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "From";
            // 
            // txtPONoTo
            // 
            this.txtPONoTo.Enabled = false;
            this.txtPONoTo.Location = new System.Drawing.Point(284, 33);
            this.txtPONoTo.Name = "txtPONoTo";
            this.txtPONoTo.Size = new System.Drawing.Size(150, 20);
            this.txtPONoTo.TabIndex = 3;
            this.txtPONoTo.TextChanged += new System.EventHandler(this.txtPONoTo_TextChanged);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(12, 139);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(936, 336);
            this.listView1.TabIndex = 16;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Item code";
            this.columnHeader1.Width = 77;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Description";
            this.columnHeader2.Width = 73;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Pur. price/pc";
            this.columnHeader3.Width = 103;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Qty";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Total";
            // 
            // btnExport
            // 
            this.btnExport.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(504, 50);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 14;
            this.btnExport.Text = "&Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // frmSearchPO
            // 
            this.AcceptButton = this.btnFind;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(960, 497);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnFind);
            this.Name = "frmSearchPO";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search Purchase Order";
            this.Load += new System.EventHandler(this.frmRepPurchaseOrder_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TextBox txtPONoFrm;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPONoTo;
        private System.Windows.Forms.LinkLabel linkLabelPO;
        private System.Windows.Forms.LinkLabel linkLabelVendor;
        private System.Windows.Forms.TextBox txtVendorTo;
        private System.Windows.Forms.TextBox txtVendorFrm;
        private System.Windows.Forms.CheckBox chkVendor;
        private System.Windows.Forms.CheckBox chkPO;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox chkDate;
        private System.Windows.Forms.TextBox txtDateTo;
        private System.Windows.Forms.TextBox txtDateFrm;
        private System.Windows.Forms.DateTimePicker dtPickerDate;
    }
}