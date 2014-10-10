namespace Check_up.forms
{
    partial class frmSalesReturnReport
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
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.chkCustomer = new System.Windows.Forms.CheckBox();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.txtDateTo = new System.Windows.Forms.TextBox();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.txtDateFrm = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.dtPickerDate = new System.Windows.Forms.DateTimePicker();
            this.linkLabelCustomer = new System.Windows.Forms.LinkLabel();
            this.txtCustomerTo = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkSalesReturnInvoiceNo = new System.Windows.Forms.CheckBox();
            this.txtCustomerFrm = new System.Windows.Forms.TextBox();
            this.linkLabelSalesReturnInvoiceNo = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSalesReturnInvoiceNoTo = new System.Windows.Forms.TextBox();
            this.txtSalesReturnInvoiceNoFrm = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
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
            // chkCustomer
            // 
            this.chkCustomer.AccessibleDescription = "";
            this.chkCustomer.AutoSize = true;
            this.chkCustomer.Location = new System.Drawing.Point(24, 61);
            this.chkCustomer.Name = "chkCustomer";
            this.chkCustomer.Size = new System.Drawing.Size(98, 17);
            this.chkCustomer.TabIndex = 5;
            this.chkCustomer.Text = "Customer Code";
            this.chkCustomer.UseVisualStyleBackColor = true;
            this.chkCustomer.CheckedChanged += new System.EventHandler(this.chkCustomer_CheckedChanged);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Description";
            this.columnHeader2.Width = 73;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(557, 30);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 28;
            this.btnGenerate.Text = "&Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtDateTo
            // 
            this.txtDateTo.Enabled = false;
            this.txtDateTo.Location = new System.Drawing.Point(333, 85);
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
            this.listView1.TabIndex = 31;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
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
            // txtDateFrm
            // 
            this.txtDateFrm.Enabled = false;
            this.txtDateFrm.Location = new System.Drawing.Point(177, 85);
            this.txtDateFrm.Name = "txtDateFrm";
            this.txtDateFrm.Size = new System.Drawing.Size(150, 20);
            this.txtDateFrm.TabIndex = 10;
            this.txtDateFrm.TextChanged += new System.EventHandler(this.txtDateFrm_TextChanged);
            // 
            // btnExport
            // 
            this.btnExport.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(557, 59);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 29;
            this.btnExport.Text = "&Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // dtPickerDate
            // 
            this.dtPickerDate.Enabled = false;
            this.dtPickerDate.Location = new System.Drawing.Point(492, 85);
            this.dtPickerDate.Name = "dtPickerDate";
            this.dtPickerDate.Size = new System.Drawing.Size(15, 20);
            this.dtPickerDate.TabIndex = 140;
            this.dtPickerDate.ValueChanged += new System.EventHandler(this.dtPickerDate_ValueChanged);
            // 
            // linkLabelCustomer
            // 
            this.linkLabelCustomer.AutoSize = true;
            this.linkLabelCustomer.Enabled = false;
            this.linkLabelCustomer.Location = new System.Drawing.Point(489, 62);
            this.linkLabelCustomer.Name = "linkLabelCustomer";
            this.linkLabelCustomer.Size = new System.Drawing.Size(19, 13);
            this.linkLabelCustomer.TabIndex = 8;
            this.linkLabelCustomer.TabStop = true;
            this.linkLabelCustomer.Text = ">>";
            this.linkLabelCustomer.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelCustomer_LinkClicked);
            // 
            // txtCustomerTo
            // 
            this.txtCustomerTo.Enabled = false;
            this.txtCustomerTo.Location = new System.Drawing.Point(333, 61);
            this.txtCustomerTo.Name = "txtCustomerTo";
            this.txtCustomerTo.Size = new System.Drawing.Size(150, 20);
            this.txtCustomerTo.TabIndex = 7;
            this.txtCustomerTo.TextChanged += new System.EventHandler(this.txtCustomerTo_TextChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(557, 88);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkSalesReturnInvoiceNo
            // 
            this.chkSalesReturnInvoiceNo.AutoSize = true;
            this.chkSalesReturnInvoiceNo.Location = new System.Drawing.Point(24, 38);
            this.chkSalesReturnInvoiceNo.Name = "chkSalesReturnInvoiceNo";
            this.chkSalesReturnInvoiceNo.Size = new System.Drawing.Size(142, 17);
            this.chkSalesReturnInvoiceNo.TabIndex = 1;
            this.chkSalesReturnInvoiceNo.Text = "Sales Return Invoice No";
            this.chkSalesReturnInvoiceNo.UseVisualStyleBackColor = true;
            this.chkSalesReturnInvoiceNo.CheckedChanged += new System.EventHandler(this.chkSalesInvoiceNo_CheckedChanged);
            // 
            // txtCustomerFrm
            // 
            this.txtCustomerFrm.Enabled = false;
            this.txtCustomerFrm.Location = new System.Drawing.Point(177, 61);
            this.txtCustomerFrm.Name = "txtCustomerFrm";
            this.txtCustomerFrm.Size = new System.Drawing.Size(150, 20);
            this.txtCustomerFrm.TabIndex = 6;
            this.txtCustomerFrm.TextChanged += new System.EventHandler(this.txtCustomerFrm_TextChanged);
            // 
            // linkLabelSalesReturnInvoiceNo
            // 
            this.linkLabelSalesReturnInvoiceNo.AutoSize = true;
            this.linkLabelSalesReturnInvoiceNo.Enabled = false;
            this.linkLabelSalesReturnInvoiceNo.Location = new System.Drawing.Point(489, 37);
            this.linkLabelSalesReturnInvoiceNo.Name = "linkLabelSalesReturnInvoiceNo";
            this.linkLabelSalesReturnInvoiceNo.Size = new System.Drawing.Size(19, 13);
            this.linkLabelSalesReturnInvoiceNo.TabIndex = 4;
            this.linkLabelSalesReturnInvoiceNo.TabStop = true;
            this.linkLabelSalesReturnInvoiceNo.Text = ">>";
            this.linkLabelSalesReturnInvoiceNo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSalesInvoiceNo_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(361, 18);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtPickerDate);
            this.groupBox1.Controls.Add(this.chkDate);
            this.groupBox1.Controls.Add(this.txtDateTo);
            this.groupBox1.Controls.Add(this.txtDateFrm);
            this.groupBox1.Controls.Add(this.chkCustomer);
            this.groupBox1.Controls.Add(this.chkSalesReturnInvoiceNo);
            this.groupBox1.Controls.Add(this.linkLabelCustomer);
            this.groupBox1.Controls.Add(this.txtCustomerTo);
            this.groupBox1.Controls.Add(this.txtCustomerFrm);
            this.groupBox1.Controls.Add(this.linkLabelSalesReturnInvoiceNo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtSalesReturnInvoiceNoTo);
            this.groupBox1.Controls.Add(this.txtSalesReturnInvoiceNoFrm);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(539, 118);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // txtSalesReturnInvoiceNoTo
            // 
            this.txtSalesReturnInvoiceNoTo.Enabled = false;
            this.txtSalesReturnInvoiceNoTo.Location = new System.Drawing.Point(333, 35);
            this.txtSalesReturnInvoiceNoTo.Name = "txtSalesReturnInvoiceNoTo";
            this.txtSalesReturnInvoiceNoTo.Size = new System.Drawing.Size(150, 20);
            this.txtSalesReturnInvoiceNoTo.TabIndex = 3;
            this.txtSalesReturnInvoiceNoTo.TextChanged += new System.EventHandler(this.txtSalesInvoiceNoTo_TextChanged);
            // 
            // txtSalesReturnInvoiceNoFrm
            // 
            this.txtSalesReturnInvoiceNoFrm.Enabled = false;
            this.txtSalesReturnInvoiceNoFrm.Location = new System.Drawing.Point(177, 36);
            this.txtSalesReturnInvoiceNoFrm.Name = "txtSalesReturnInvoiceNoFrm";
            this.txtSalesReturnInvoiceNoFrm.Size = new System.Drawing.Size(150, 20);
            this.txtSalesReturnInvoiceNoFrm.TabIndex = 2;
            this.txtSalesReturnInvoiceNoFrm.TextChanged += new System.EventHandler(this.txtSalesInvoiceNoFrm_TextChanged);
            // 
            // frmSalesReturnReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(960, 497);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmSalesReturnReport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sales Return Report";
            this.Load += new System.EventHandler(this.frmSearchSalesReturn_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkDate;
        private System.Windows.Forms.CheckBox chkCustomer;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.TextBox txtDateTo;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.TextBox txtDateFrm;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.DateTimePicker dtPickerDate;
        private System.Windows.Forms.LinkLabel linkLabelCustomer;
        private System.Windows.Forms.TextBox txtCustomerTo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkSalesReturnInvoiceNo;
        private System.Windows.Forms.TextBox txtCustomerFrm;
        private System.Windows.Forms.LinkLabel linkLabelSalesReturnInvoiceNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtSalesReturnInvoiceNoTo;
        private System.Windows.Forms.TextBox txtSalesReturnInvoiceNoFrm;
    }
}