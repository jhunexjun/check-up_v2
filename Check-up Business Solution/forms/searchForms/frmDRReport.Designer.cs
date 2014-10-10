namespace Check_up.forms
{
    partial class frmDRReport
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
            this.chkVendor = new System.Windows.Forms.CheckBox();
            this.chkDR = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.linkLabelVendor = new System.Windows.Forms.LinkLabel();
            this.txtVendorTo = new System.Windows.Forms.TextBox();
            this.txtVendorFrm = new System.Windows.Forms.TextBox();
            this.linkLabelDeliveryReceipt = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDRNoTo = new System.Windows.Forms.TextBox();
            this.txtDRNoFrm = new System.Windows.Forms.TextBox();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.dtPickerDate = new System.Windows.Forms.DateTimePicker();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.txtDateTo = new System.Windows.Forms.TextBox();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.btnExport = new System.Windows.Forms.Button();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.txtDateFrm = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkVendor
            // 
            this.chkVendor.AccessibleDescription = "";
            this.chkVendor.AutoSize = true;
            this.chkVendor.Location = new System.Drawing.Point(24, 60);
            this.chkVendor.Name = "chkVendor";
            this.chkVendor.Size = new System.Drawing.Size(88, 17);
            this.chkVendor.TabIndex = 5;
            this.chkVendor.Text = "Vendor Code";
            this.chkVendor.UseVisualStyleBackColor = true;
            this.chkVendor.CheckedChanged += new System.EventHandler(this.chkVendor_CheckedChanged);
            // 
            // chkDR
            // 
            this.chkDR.AutoSize = true;
            this.chkDR.Location = new System.Drawing.Point(24, 35);
            this.chkDR.Name = "chkDR";
            this.chkDR.Size = new System.Drawing.Size(118, 17);
            this.chkDR.TabIndex = 1;
            this.chkDR.Text = "DeliveryReceipt No";
            this.chkDR.UseVisualStyleBackColor = true;
            this.chkDR.CheckedChanged += new System.EventHandler(this.chkDeliveryReceipt_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(574, 90);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // linkLabelVendor
            // 
            this.linkLabelVendor.AutoSize = true;
            this.linkLabelVendor.Enabled = false;
            this.linkLabelVendor.Location = new System.Drawing.Point(462, 61);
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
            this.txtVendorTo.Location = new System.Drawing.Point(306, 60);
            this.txtVendorTo.Name = "txtVendorTo";
            this.txtVendorTo.Size = new System.Drawing.Size(150, 20);
            this.txtVendorTo.TabIndex = 7;
            this.txtVendorTo.TextChanged += new System.EventHandler(this.txtVendorTo_TextChanged);
            // 
            // txtVendorFrm
            // 
            this.txtVendorFrm.Enabled = false;
            this.txtVendorFrm.Location = new System.Drawing.Point(150, 60);
            this.txtVendorFrm.Name = "txtVendorFrm";
            this.txtVendorFrm.Size = new System.Drawing.Size(150, 20);
            this.txtVendorFrm.TabIndex = 6;
            this.txtVendorFrm.TextChanged += new System.EventHandler(this.txtVendorFrm_TextChanged);
            // 
            // linkLabelDeliveryReceipt
            // 
            this.linkLabelDeliveryReceipt.AutoSize = true;
            this.linkLabelDeliveryReceipt.Enabled = false;
            this.linkLabelDeliveryReceipt.Location = new System.Drawing.Point(462, 36);
            this.linkLabelDeliveryReceipt.Name = "linkLabelDeliveryReceipt";
            this.linkLabelDeliveryReceipt.Size = new System.Drawing.Size(19, 13);
            this.linkLabelDeliveryReceipt.TabIndex = 4;
            this.linkLabelDeliveryReceipt.TabStop = true;
            this.linkLabelDeliveryReceipt.Text = ">>";
            this.linkLabelDeliveryReceipt.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDeliveryReceipt_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(373, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "To";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(215, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "From";
            // 
            // txtDRNoTo
            // 
            this.txtDRNoTo.Enabled = false;
            this.txtDRNoTo.Location = new System.Drawing.Point(306, 34);
            this.txtDRNoTo.Name = "txtDRNoTo";
            this.txtDRNoTo.Size = new System.Drawing.Size(150, 20);
            this.txtDRNoTo.TabIndex = 3;
            this.txtDRNoTo.TextChanged += new System.EventHandler(this.txtGRPONoTo_TextChanged);
            // 
            // txtDRNoFrm
            // 
            this.txtDRNoFrm.Enabled = false;
            this.txtDRNoFrm.Location = new System.Drawing.Point(150, 34);
            this.txtDRNoFrm.Name = "txtDRNoFrm";
            this.txtDRNoFrm.Size = new System.Drawing.Size(150, 20);
            this.txtDRNoFrm.TabIndex = 2;
            this.txtDRNoFrm.TextChanged += new System.EventHandler(this.txtGRPONoFrm_TextChanged);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Qty";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Pur. price/pc";
            this.columnHeader3.Width = 103;
            // 
            // dtPickerDate
            // 
            this.dtPickerDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtPickerDate.Enabled = false;
            this.dtPickerDate.Location = new System.Drawing.Point(464, 84);
            this.dtPickerDate.Name = "dtPickerDate";
            this.dtPickerDate.Size = new System.Drawing.Size(15, 20);
            this.dtPickerDate.TabIndex = 140;
            this.dtPickerDate.ValueChanged += new System.EventHandler(this.dtPickerDate_ValueChanged);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(574, 32);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 23;
            this.btnGenerate.Text = "&Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Description";
            this.columnHeader2.Width = 73;
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
            this.chkDate.CheckedChanged += new System.EventHandler(this.chkDate_CheckedChanged);
            // 
            // txtDateTo
            // 
            this.txtDateTo.Enabled = false;
            this.txtDateTo.Location = new System.Drawing.Point(306, 84);
            this.txtDateTo.Name = "txtDateTo";
            this.txtDateTo.Size = new System.Drawing.Size(150, 20);
            this.txtDateTo.TabIndex = 11;
            this.txtDateTo.TextChanged += new System.EventHandler(this.txtDateTo_TextChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Item code";
            this.columnHeader1.Width = 77;
            // 
            // btnExport
            // 
            this.btnExport.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(574, 61);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 24;
            this.btnExport.Text = "&Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Total";
            // 
            // txtDateFrm
            // 
            this.txtDateFrm.Enabled = false;
            this.txtDateFrm.Location = new System.Drawing.Point(150, 84);
            this.txtDateFrm.Name = "txtDateFrm";
            this.txtDateFrm.Size = new System.Drawing.Size(150, 20);
            this.txtDateFrm.TabIndex = 10;
            this.txtDateFrm.TextChanged += new System.EventHandler(this.txtDateFrm_TextChanged);
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
            this.listView1.Location = new System.Drawing.Point(12, 148);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(936, 336);
            this.listView1.TabIndex = 26;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtPickerDate);
            this.groupBox1.Controls.Add(this.chkDate);
            this.groupBox1.Controls.Add(this.txtDateTo);
            this.groupBox1.Controls.Add(this.txtDateFrm);
            this.groupBox1.Controls.Add(this.chkVendor);
            this.groupBox1.Controls.Add(this.chkDR);
            this.groupBox1.Controls.Add(this.linkLabelVendor);
            this.groupBox1.Controls.Add(this.txtVendorTo);
            this.groupBox1.Controls.Add(this.txtVendorFrm);
            this.groupBox1.Controls.Add(this.linkLabelDeliveryReceipt);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtDRNoTo);
            this.groupBox1.Controls.Add(this.txtDRNoFrm);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(556, 118);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            // 
            // frmDRReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(960, 497);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmDRReport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Delivery Receipt Report";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkVendor;
        private System.Windows.Forms.CheckBox chkDR;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.LinkLabel linkLabelVendor;
        private System.Windows.Forms.TextBox txtVendorTo;
        private System.Windows.Forms.TextBox txtVendorFrm;
        private System.Windows.Forms.LinkLabel linkLabelDeliveryReceipt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDRNoTo;
        private System.Windows.Forms.TextBox txtDRNoFrm;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.DateTimePicker dtPickerDate;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.CheckBox chkDate;
        private System.Windows.Forms.TextBox txtDateTo;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.TextBox txtDateFrm;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}