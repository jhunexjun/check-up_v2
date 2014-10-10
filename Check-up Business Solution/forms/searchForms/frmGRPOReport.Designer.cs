namespace Check_up.forms
{
    partial class frmGRPOReport
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
            this.dtPickerDate = new System.Windows.Forms.DateTimePicker();
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.txtDateTo = new System.Windows.Forms.TextBox();
            this.txtDateFrm = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.chkVendor = new System.Windows.Forms.CheckBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.chkGRPO = new System.Windows.Forms.CheckBox();
            this.linkLabelVendor = new System.Windows.Forms.LinkLabel();
            this.txtVendorTo = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtVendorFrm = new System.Windows.Forms.TextBox();
            this.linkLabelGRPO = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGRPONoTo = new System.Windows.Forms.TextBox();
            this.txtGRPONoFrm = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(504, 88);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dtPickerDate
            // 
            this.dtPickerDate.Enabled = false;
            this.dtPickerDate.Location = new System.Drawing.Point(440, 83);
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
            this.chkDate.CheckedChanged += new System.EventHandler(this.chkDate_CheckedChanged);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Pur. price/pc";
            this.columnHeader3.Width = 103;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Description";
            this.columnHeader2.Width = 73;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Item code";
            this.columnHeader1.Width = 77;
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
            // btnExport
            // 
            this.btnExport.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(504, 59);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 19;
            this.btnExport.Text = "&Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Total";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Qty";
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
            this.chkVendor.CheckedChanged += new System.EventHandler(this.chkVendor_CheckedChanged);
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
            this.listView1.TabIndex = 21;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // chkGRPO
            // 
            this.chkGRPO.AutoSize = true;
            this.chkGRPO.Location = new System.Drawing.Point(24, 38);
            this.chkGRPO.Name = "chkGRPO";
            this.chkGRPO.Size = new System.Drawing.Size(74, 17);
            this.chkGRPO.TabIndex = 1;
            this.chkGRPO.Text = "GRPO No";
            this.chkGRPO.UseVisualStyleBackColor = true;
            this.chkGRPO.CheckedChanged += new System.EventHandler(this.chkGRPO_CheckedChanged);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtPickerDate);
            this.groupBox1.Controls.Add(this.chkDate);
            this.groupBox1.Controls.Add(this.txtDateTo);
            this.groupBox1.Controls.Add(this.txtDateFrm);
            this.groupBox1.Controls.Add(this.chkVendor);
            this.groupBox1.Controls.Add(this.chkGRPO);
            this.groupBox1.Controls.Add(this.linkLabelVendor);
            this.groupBox1.Controls.Add(this.txtVendorTo);
            this.groupBox1.Controls.Add(this.txtVendorFrm);
            this.groupBox1.Controls.Add(this.linkLabelGRPO);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtGRPONoTo);
            this.groupBox1.Controls.Add(this.txtGRPONoFrm);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(486, 118);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
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
            // linkLabelGRPO
            // 
            this.linkLabelGRPO.AutoSize = true;
            this.linkLabelGRPO.Enabled = false;
            this.linkLabelGRPO.Location = new System.Drawing.Point(440, 35);
            this.linkLabelGRPO.Name = "linkLabelGRPO";
            this.linkLabelGRPO.Size = new System.Drawing.Size(19, 13);
            this.linkLabelGRPO.TabIndex = 4;
            this.linkLabelGRPO.TabStop = true;
            this.linkLabelGRPO.Text = ">>";
            this.linkLabelGRPO.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelGRPO_LinkClicked);
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
            // txtGRPONoTo
            // 
            this.txtGRPONoTo.Enabled = false;
            this.txtGRPONoTo.Location = new System.Drawing.Point(284, 33);
            this.txtGRPONoTo.Name = "txtGRPONoTo";
            this.txtGRPONoTo.Size = new System.Drawing.Size(150, 20);
            this.txtGRPONoTo.TabIndex = 3;
            this.txtGRPONoTo.TextChanged += new System.EventHandler(this.txtGRPONoTo_TextChanged);
            // 
            // txtGRPONoFrm
            // 
            this.txtGRPONoFrm.Enabled = false;
            this.txtGRPONoFrm.Location = new System.Drawing.Point(128, 33);
            this.txtGRPONoFrm.Name = "txtGRPONoFrm";
            this.txtGRPONoFrm.Size = new System.Drawing.Size(150, 20);
            this.txtGRPONoFrm.TabIndex = 2;
            this.txtGRPONoFrm.TextChanged += new System.EventHandler(this.txtGRPONoFrm_TextChanged);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(504, 30);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 18;
            this.btnGenerate.Text = "&Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // frmGRPOReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(960, 497);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnGenerate);
            this.Name = "frmGRPOReport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Goods Receipt PO Report";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DateTimePicker dtPickerDate;
        private System.Windows.Forms.CheckBox chkDate;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TextBox txtDateTo;
        private System.Windows.Forms.TextBox txtDateFrm;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.CheckBox chkVendor;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.CheckBox chkGRPO;
        private System.Windows.Forms.LinkLabel linkLabelVendor;
        private System.Windows.Forms.TextBox txtVendorTo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtVendorFrm;
        private System.Windows.Forms.LinkLabel linkLabelGRPO;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGRPONoTo;
        private System.Windows.Forms.TextBox txtGRPONoFrm;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}