namespace Check_up.forms
{
    partial class frmInventoryPostingReport
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
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.txtDateTo = new System.Windows.Forms.TextBox();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.listView1 = new System.Windows.Forms.ListView();
            this.colIPNo = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.txtDateFrm = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.dtPickerDate = new System.Windows.Forms.DateTimePicker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkInventoryPosting = new System.Windows.Forms.CheckBox();
            this.linkLabelIP = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtIPNoTo = new System.Windows.Forms.TextBox();
            this.txtIPNoFrm = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkDate
            // 
            this.chkDate.AccessibleDescription = "";
            this.chkDate.AutoSize = true;
            this.chkDate.Location = new System.Drawing.Point(24, 55);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(49, 17);
            this.chkDate.TabIndex = 9;
            this.chkDate.Text = "Date";
            this.chkDate.UseVisualStyleBackColor = true;
            this.chkDate.CheckedChanged += new System.EventHandler(this.chkDate_CheckedChanged);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Description";
            this.columnHeader2.Width = 73;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(574, 23);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 28;
            this.btnGenerate.Text = "&Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // txtDateTo
            // 
            this.txtDateTo.Enabled = false;
            this.txtDateTo.Location = new System.Drawing.Point(306, 57);
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
            this.colIPNo,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
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
            // colIPNo
            // 
            this.colIPNo.Text = "Inv. Posting No.";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Current Qty";
            this.columnHeader3.Width = 103;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Counted Qty";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Variance";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Pur. Price";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Retail Price";
            // 
            // txtDateFrm
            // 
            this.txtDateFrm.Enabled = false;
            this.txtDateFrm.Location = new System.Drawing.Point(150, 57);
            this.txtDateFrm.Name = "txtDateFrm";
            this.txtDateFrm.Size = new System.Drawing.Size(150, 20);
            this.txtDateFrm.TabIndex = 10;
            this.txtDateFrm.TextChanged += new System.EventHandler(this.txtDateFrm_TextChanged);
            // 
            // btnExport
            // 
            this.btnExport.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(574, 52);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 29;
            this.btnExport.Text = "&Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // dtPickerDate
            // 
            this.dtPickerDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtPickerDate.Enabled = false;
            this.dtPickerDate.Location = new System.Drawing.Point(464, 57);
            this.dtPickerDate.Name = "dtPickerDate";
            this.dtPickerDate.Size = new System.Drawing.Size(15, 20);
            this.dtPickerDate.TabIndex = 140;
            this.dtPickerDate.ValueChanged += new System.EventHandler(this.dtPickerDate_ValueChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(574, 81);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkInventoryPosting
            // 
            this.chkInventoryPosting.AutoSize = true;
            this.chkInventoryPosting.Location = new System.Drawing.Point(24, 35);
            this.chkInventoryPosting.Name = "chkInventoryPosting";
            this.chkInventoryPosting.Size = new System.Drawing.Size(125, 17);
            this.chkInventoryPosting.TabIndex = 1;
            this.chkInventoryPosting.Text = "Inventory Posting No";
            this.chkInventoryPosting.UseVisualStyleBackColor = true;
            this.chkInventoryPosting.CheckedChanged += new System.EventHandler(this.chkInventoryPosting_CheckedChanged);
            // 
            // linkLabelIP
            // 
            this.linkLabelIP.AutoSize = true;
            this.linkLabelIP.Enabled = false;
            this.linkLabelIP.Location = new System.Drawing.Point(462, 36);
            this.linkLabelIP.Name = "linkLabelIP";
            this.linkLabelIP.Size = new System.Drawing.Size(19, 13);
            this.linkLabelIP.TabIndex = 4;
            this.linkLabelIP.TabStop = true;
            this.linkLabelIP.Text = ">>";
            this.linkLabelIP.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelIP_LinkClicked);
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
            this.label3.Location = new System.Drawing.Point(207, 16);
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
            this.groupBox1.Controls.Add(this.chkInventoryPosting);
            this.groupBox1.Controls.Add(this.linkLabelIP);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtIPNoTo);
            this.groupBox1.Controls.Add(this.txtIPNoFrm);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(556, 101);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // txtIPNoTo
            // 
            this.txtIPNoTo.Enabled = false;
            this.txtIPNoTo.Location = new System.Drawing.Point(306, 34);
            this.txtIPNoTo.Name = "txtIPNoTo";
            this.txtIPNoTo.Size = new System.Drawing.Size(150, 20);
            this.txtIPNoTo.TabIndex = 3;
            this.txtIPNoTo.TextChanged += new System.EventHandler(this.txtIPNoTo_TextChanged);
            // 
            // txtIPNoFrm
            // 
            this.txtIPNoFrm.Enabled = false;
            this.txtIPNoFrm.Location = new System.Drawing.Point(150, 34);
            this.txtIPNoFrm.Name = "txtIPNoFrm";
            this.txtIPNoFrm.Size = new System.Drawing.Size(150, 20);
            this.txtIPNoFrm.TabIndex = 2;
            this.txtIPNoFrm.TextChanged += new System.EventHandler(this.txtIPNoFrm_TextChanged);
            // 
            // frmInventoryPostingReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 497);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmInventoryPostingReport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inventory Posting Report";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkDate;
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
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkInventoryPosting;
        private System.Windows.Forms.LinkLabel linkLabelIP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtIPNoTo;
        private System.Windows.Forms.TextBox txtIPNoFrm;
        private System.Windows.Forms.ColumnHeader colIPNo;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
    }
}