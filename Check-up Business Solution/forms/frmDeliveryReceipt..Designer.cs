﻿namespace Check_up.forms
{
    partial class frmDeliveryReceipt
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDeliveryReceipt));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.lblitemsCount = new System.Windows.Forms.Label();
            this.removeRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label11 = new System.Windows.Forms.Label();
            this.cboWarehouse = new System.Windows.Forms.ComboBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.txtRemarks2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRemarks1 = new System.Windows.Forms.TextBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.txtPostingDate = new System.Windows.Forms.TextBox();
            this.txtTotalAmtDscnt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtTotalPrcntDscnt = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDeliveryReceiptNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGrossTotal = new System.Windows.Forms.TextBox();
            this.txtNetTotal = new System.Windows.Forms.TextBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.colVendorCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVendorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vatable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehouse = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.baseUoM = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.qtyPrPrchsUoM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.realBsNetPrchsPrc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.realBsGrossPrchsPrc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.realNetPrchsPrc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.realGrossPrchsPrc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.netPrchsPrc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grossPrchsPrc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prcntDscnt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amtDscnt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rowNetTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rowGrossTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label12 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.SuspendLayout();
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker1.Location = new System.Drawing.Point(957, 40);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(13, 20);
            this.dateTimePicker1.TabIndex = 171;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // lblitemsCount
            // 
            this.lblitemsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblitemsCount.AutoSize = true;
            this.lblitemsCount.Location = new System.Drawing.Point(25, 314);
            this.lblitemsCount.Name = "lblitemsCount";
            this.lblitemsCount.Size = new System.Drawing.Size(13, 13);
            this.lblitemsCount.TabIndex = 168;
            this.lblitemsCount.Text = "0";
            // 
            // removeRowToolStripMenuItem
            // 
            this.removeRowToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removeRowToolStripMenuItem.Image")));
            this.removeRowToolStripMenuItem.Name = "removeRowToolStripMenuItem";
            this.removeRowToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.removeRowToolStripMenuItem.Text = "&Remove row";
            this.removeRowToolStripMenuItem.Click += new System.EventHandler(this.removeRowToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.editToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(97, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addToolStripMenuItem.Image")));
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.addToolStripMenuItem.Text = "&Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("editToolStripMenuItem.Image")));
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.editToolStripMenuItem.Text = "&Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(21, 38);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 13);
            this.label11.TabIndex = 169;
            this.label11.Text = "Warehouse";
            // 
            // cboWarehouse
            // 
            this.cboWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWarehouse.FormattingEnabled = true;
            this.cboWarehouse.Location = new System.Drawing.Point(89, 35);
            this.cboWarehouse.Name = "cboWarehouse";
            this.cboWarehouse.Size = new System.Drawing.Size(131, 21);
            this.cboWarehouse.TabIndex = 147;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeRowToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(141, 26);
            this.contextMenuStrip2.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip2_Opening);
            // 
            // txtRemarks2
            // 
            this.txtRemarks2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtRemarks2.Location = new System.Drawing.Point(89, 396);
            this.txtRemarks2.MaxLength = 500;
            this.txtRemarks2.Multiline = true;
            this.txtRemarks2.Name = "txtRemarks2";
            this.txtRemarks2.Size = new System.Drawing.Size(131, 49);
            this.txtRemarks2.TabIndex = 150;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 399);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 162;
            this.label4.Text = "Remarks 2";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(25, 344);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 161;
            this.label10.Text = "Remarks 1";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(752, 382);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 163;
            this.label5.Text = "Net Total";
            // 
            // txtRemarks1
            // 
            this.txtRemarks1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtRemarks1.Location = new System.Drawing.Point(89, 341);
            this.txtRemarks1.MaxLength = 500;
            this.txtRemarks1.Multiline = true;
            this.txtRemarks1.Name = "txtRemarks1";
            this.txtRemarks1.Size = new System.Drawing.Size(131, 49);
            this.txtRemarks1.TabIndex = 149;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnPrint.Enabled = false;
            this.btnPrint.Location = new System.Drawing.Point(897, 455);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 157;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // txtPostingDate
            // 
            this.txtPostingDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPostingDate.Location = new System.Drawing.Point(889, 40);
            this.txtPostingDate.Name = "txtPostingDate";
            this.txtPostingDate.Size = new System.Drawing.Size(67, 20);
            this.txtPostingDate.TabIndex = 142;
            // 
            // txtTotalAmtDscnt
            // 
            this.txtTotalAmtDscnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalAmtDscnt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtTotalAmtDscnt.ForeColor = System.Drawing.Color.Blue;
            this.txtTotalAmtDscnt.Location = new System.Drawing.Point(857, 357);
            this.txtTotalAmtDscnt.Name = "txtTotalAmtDscnt";
            this.txtTotalAmtDscnt.ReadOnly = true;
            this.txtTotalAmtDscnt.Size = new System.Drawing.Size(100, 20);
            this.txtTotalAmtDscnt.TabIndex = 152;
            this.txtTotalAmtDscnt.Text = "0.00";
            this.txtTotalAmtDscnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(752, 360);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 13);
            this.label7.TabIndex = 166;
            this.label7.Text = "Total Discount Amt";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(781, 43);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 13);
            this.label9.TabIndex = 167;
            this.label9.Text = "Posting Date";
            // 
            // txtTotalPrcntDscnt
            // 
            this.txtTotalPrcntDscnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalPrcntDscnt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtTotalPrcntDscnt.ForeColor = System.Drawing.Color.Blue;
            this.txtTotalPrcntDscnt.Location = new System.Drawing.Point(857, 334);
            this.txtTotalPrcntDscnt.Name = "txtTotalPrcntDscnt";
            this.txtTotalPrcntDscnt.ReadOnly = true;
            this.txtTotalPrcntDscnt.Size = new System.Drawing.Size(100, 20);
            this.txtTotalPrcntDscnt.TabIndex = 151;
            this.txtTotalPrcntDscnt.Text = "0.00";
            this.txtTotalPrcntDscnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(752, 337);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 13);
            this.label8.TabIndex = 165;
            this.label8.Text = "Total Discount %";
            // 
            // txtDeliveryReceiptNo
            // 
            this.txtDeliveryReceiptNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDeliveryReceiptNo.Location = new System.Drawing.Point(889, 17);
            this.txtDeliveryReceiptNo.MaxLength = 21;
            this.txtDeliveryReceiptNo.Name = "txtDeliveryReceiptNo";
            this.txtDeliveryReceiptNo.Size = new System.Drawing.Size(81, 20);
            this.txtDeliveryReceiptNo.TabIndex = 141;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(781, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 159;
            this.label2.Text = "Delivery Receipt No";
            // 
            // txtGrossTotal
            // 
            this.txtGrossTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGrossTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtGrossTotal.ForeColor = System.Drawing.Color.Blue;
            this.txtGrossTotal.Location = new System.Drawing.Point(857, 402);
            this.txtGrossTotal.Name = "txtGrossTotal";
            this.txtGrossTotal.ReadOnly = true;
            this.txtGrossTotal.Size = new System.Drawing.Size(100, 20);
            this.txtGrossTotal.TabIndex = 154;
            this.txtGrossTotal.Text = "0.00";
            this.txtGrossTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtNetTotal
            // 
            this.txtNetTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNetTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtNetTotal.ForeColor = System.Drawing.Color.Blue;
            this.txtNetTotal.Location = new System.Drawing.Point(857, 379);
            this.txtNetTotal.Name = "txtNetTotal";
            this.txtNetTotal.ReadOnly = true;
            this.txtNetTotal.Size = new System.Drawing.Size(100, 20);
            this.txtNetTotal.TabIndex = 153;
            this.txtNetTotal.Text = "0.00";
            this.txtNetTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.Location = new System.Drawing.Point(735, 455);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 23);
            this.btnFind.TabIndex = 155;
            this.btnFind.Text = "&Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(752, 405);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 164;
            this.label6.Text = "Gross Total";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(816, 455);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 156;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToAddRows = false;
            this.dgvItems.AllowUserToResizeRows = false;
            this.dgvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItems.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colVendorCode,
            this.colVendorName,
            this.itemCode,
            this.description,
            this.vatable,
            this.warehouse,
            this.Qty,
            this.baseUoM,
            this.qtyPrPrchsUoM,
            this.realBsNetPrchsPrc,
            this.realBsGrossPrchsPrc,
            this.realNetPrchsPrc,
            this.realGrossPrchsPrc,
            this.netPrchsPrc,
            this.grossPrchsPrc,
            this.prcntDscnt,
            this.amtDscnt,
            this.rowNetTotal,
            this.rowGrossTotal});
            this.dgvItems.ContextMenuStrip = this.contextMenuStrip2;
            this.dgvItems.Location = new System.Drawing.Point(21, 70);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.RowHeadersVisible = false;
            this.dgvItems.Size = new System.Drawing.Size(950, 241);
            this.dgvItems.TabIndex = 148;
            // 
            // colVendorCode
            // 
            this.colVendorCode.HeaderText = "Vendor Code";
            this.colVendorCode.Name = "colVendorCode";
            this.colVendorCode.Width = 87;
            // 
            // colVendorName
            // 
            this.colVendorName.HeaderText = "Vendor Name";
            this.colVendorName.Name = "colVendorName";
            this.colVendorName.ReadOnly = true;
            this.colVendorName.Width = 89;
            // 
            // itemCode
            // 
            this.itemCode.HeaderText = "Item code";
            this.itemCode.Name = "itemCode";
            this.itemCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.itemCode.Width = 73;
            // 
            // description
            // 
            this.description.HeaderText = "Description";
            this.description.Name = "description";
            this.description.Width = 85;
            // 
            // vatable
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.vatable.DefaultCellStyle = dataGridViewCellStyle1;
            this.vatable.HeaderText = "Vatable";
            this.vatable.MaxInputLength = 1;
            this.vatable.Name = "vatable";
            this.vatable.ReadOnly = true;
            this.vatable.Width = 5;
            // 
            // warehouse
            // 
            this.warehouse.HeaderText = "Warehouse";
            this.warehouse.Name = "warehouse";
            this.warehouse.Visible = false;
            this.warehouse.Width = 68;
            // 
            // Qty
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N6";
            dataGridViewCellStyle2.NullValue = "0.00";
            this.Qty.DefaultCellStyle = dataGridViewCellStyle2;
            this.Qty.HeaderText = "Qty";
            this.Qty.Name = "Qty";
            this.Qty.Width = 48;
            // 
            // baseUoM
            // 
            this.baseUoM.HeaderText = "Base UoM";
            this.baseUoM.Items.AddRange(new object[] {
            "Y",
            "N"});
            this.baseUoM.Name = "baseUoM";
            this.baseUoM.Width = 57;
            // 
            // qtyPrPrchsUoM
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.qtyPrPrchsUoM.DefaultCellStyle = dataGridViewCellStyle3;
            this.qtyPrPrchsUoM.HeaderText = "Qty/Prchs UoM";
            this.qtyPrPrchsUoM.Name = "qtyPrPrchsUoM";
            this.qtyPrPrchsUoM.ReadOnly = true;
            this.qtyPrPrchsUoM.Width = 97;
            // 
            // realBsNetPrchsPrc
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle4.Format = "N6";
            dataGridViewCellStyle4.NullValue = "0.00";
            this.realBsNetPrchsPrc.DefaultCellStyle = dataGridViewCellStyle4;
            this.realBsNetPrchsPrc.HeaderText = "Real Bsc Net Pur.Price";
            this.realBsNetPrchsPrc.Name = "realBsNetPrchsPrc";
            this.realBsNetPrchsPrc.ReadOnly = true;
            this.realBsNetPrchsPrc.Width = 5;
            // 
            // realBsGrossPrchsPrc
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle5.Format = "N6";
            dataGridViewCellStyle5.NullValue = "0.00";
            this.realBsGrossPrchsPrc.DefaultCellStyle = dataGridViewCellStyle5;
            this.realBsGrossPrchsPrc.HeaderText = "Real Bsc Gross Prchs Price";
            this.realBsGrossPrchsPrc.Name = "realBsGrossPrchsPrc";
            this.realBsGrossPrchsPrc.ReadOnly = true;
            this.realBsGrossPrchsPrc.Width = 5;
            // 
            // realNetPrchsPrc
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle6.Format = "N6";
            dataGridViewCellStyle6.NullValue = "0.00";
            this.realNetPrchsPrc.DefaultCellStyle = dataGridViewCellStyle6;
            this.realNetPrchsPrc.HeaderText = "Real Net Prchs Price";
            this.realNetPrchsPrc.Name = "realNetPrchsPrc";
            this.realNetPrchsPrc.ReadOnly = true;
            this.realNetPrchsPrc.Width = 5;
            // 
            // realGrossPrchsPrc
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle7.Format = "N6";
            dataGridViewCellStyle7.NullValue = "0.00";
            this.realGrossPrchsPrc.DefaultCellStyle = dataGridViewCellStyle7;
            this.realGrossPrchsPrc.HeaderText = "Real Gross Prchs Price";
            this.realGrossPrchsPrc.Name = "realGrossPrchsPrc";
            this.realGrossPrchsPrc.ReadOnly = true;
            this.realGrossPrchsPrc.Width = 5;
            // 
            // netPrchsPrc
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle8.Format = "N6";
            dataGridViewCellStyle8.NullValue = "0.00";
            this.netPrchsPrc.DefaultCellStyle = dataGridViewCellStyle8;
            this.netPrchsPrc.HeaderText = "Net Pur.Price";
            this.netPrchsPrc.Name = "netPrchsPrc";
            this.netPrchsPrc.ReadOnly = true;
            this.netPrchsPrc.Width = 5;
            // 
            // grossPrchsPrc
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle9.Format = "N6";
            dataGridViewCellStyle9.NullValue = "0.00";
            this.grossPrchsPrc.DefaultCellStyle = dataGridViewCellStyle9;
            this.grossPrchsPrc.HeaderText = "Gross Pur.Price";
            this.grossPrchsPrc.Name = "grossPrchsPrc";
            this.grossPrchsPrc.ReadOnly = true;
            this.grossPrchsPrc.Width = 97;
            // 
            // prcntDscnt
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.Format = "N6";
            dataGridViewCellStyle10.NullValue = "0.00";
            this.prcntDscnt.DefaultCellStyle = dataGridViewCellStyle10;
            this.prcntDscnt.HeaderText = "Discount %";
            this.prcntDscnt.Name = "prcntDscnt";
            this.prcntDscnt.Width = 79;
            // 
            // amtDscnt
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.Format = "N6";
            dataGridViewCellStyle11.NullValue = "0.00";
            this.amtDscnt.DefaultCellStyle = dataGridViewCellStyle11;
            this.amtDscnt.HeaderText = "Amt Dscnt";
            this.amtDscnt.Name = "amtDscnt";
            this.amtDscnt.Width = 75;
            // 
            // rowNetTotal
            // 
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle12.Format = "N6";
            dataGridViewCellStyle12.NullValue = "0.00";
            this.rowNetTotal.DefaultCellStyle = dataGridViewCellStyle12;
            this.rowNetTotal.HeaderText = "Row Net Total";
            this.rowNetTotal.Name = "rowNetTotal";
            this.rowNetTotal.ReadOnly = true;
            this.rowNetTotal.Width = 5;
            // 
            // rowGrossTotal
            // 
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle13.Format = "N6";
            dataGridViewCellStyle13.NullValue = "0.00";
            this.rowGrossTotal.DefaultCellStyle = dataGridViewCellStyle13;
            this.rowGrossTotal.HeaderText = "Row Gross Total";
            this.rowGrossTotal.Name = "rowGrossTotal";
            this.rowGrossTotal.ReadOnly = true;
            this.rowGrossTotal.Width = 102;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label12.Location = new System.Drawing.Point(736, 323);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(236, 112);
            this.label12.TabIndex = 170;
            // 
            // frmDeliveryReceipt
            // 
            this.AcceptButton = this.btnFind;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(989, 498);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.lblitemsCount);
            this.Controls.Add(this.txtRemarks2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtRemarks1);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.cboWarehouse);
            this.Controls.Add(this.txtPostingDate);
            this.Controls.Add(this.txtTotalAmtDscnt);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtTotalPrcntDscnt);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtDeliveryReceiptNo);
            this.Controls.Add(this.txtGrossTotal);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtNetTotal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.dgvItems);
            this.Controls.Add(this.label12);
            this.Name = "frmDeliveryReceipt";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Delivery Receipt";
            this.Load += new System.EventHandler(this.frmDeliveryReceipt_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label lblitemsCount;
        private System.Windows.Forms.ToolStripMenuItem removeRowToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cboWarehouse;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.TextBox txtRemarks2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRemarks1;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.TextBox txtPostingDate;
        private System.Windows.Forms.TextBox txtTotalAmtDscnt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtTotalPrcntDscnt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDeliveryReceiptNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtGrossTotal;
        private System.Windows.Forms.TextBox txtNetTotal;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVendorCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVendorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn vatable;
        private System.Windows.Forms.DataGridViewComboBoxColumn warehouse;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        private System.Windows.Forms.DataGridViewComboBoxColumn baseUoM;
        private System.Windows.Forms.DataGridViewTextBoxColumn qtyPrPrchsUoM;
        private System.Windows.Forms.DataGridViewTextBoxColumn realBsNetPrchsPrc;
        private System.Windows.Forms.DataGridViewTextBoxColumn realBsGrossPrchsPrc;
        private System.Windows.Forms.DataGridViewTextBoxColumn realNetPrchsPrc;
        private System.Windows.Forms.DataGridViewTextBoxColumn realGrossPrchsPrc;
        private System.Windows.Forms.DataGridViewTextBoxColumn netPrchsPrc;
        private System.Windows.Forms.DataGridViewTextBoxColumn grossPrchsPrc;
        private System.Windows.Forms.DataGridViewTextBoxColumn prcntDscnt;
        private System.Windows.Forms.DataGridViewTextBoxColumn amtDscnt;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowNetTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowGrossTotal;
    }
}