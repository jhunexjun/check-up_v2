namespace Check_up.forms
{
    partial class frmSalesInvoice
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSalesInvoice));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCustomerCode = new System.Windows.Forms.TextBox();
            this.txtSalesInvoiceNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCustomerName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.itemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vatable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.baseUoM = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.qtyPrSaleUoM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saleUoM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.realBsNetSalePrc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.realBsGrossSalePrc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.netPrchsPrc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grossPrchsPrc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.netSalePrc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grossSalePrc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prcntDscnt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amtDscnt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rowNetTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rowGrossTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label10 = new System.Windows.Forms.Label();
            this.txtRemarks1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRemarks2 = new System.Windows.Forms.TextBox();
            this.txtNetTotal = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtGrossTotal = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.linkCustomerCode = new System.Windows.Forms.LinkLabel();
            this.linkCustomerName = new System.Windows.Forms.LinkLabel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.txtTotalAmtDscnt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTotalPrcntDscnt = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblitemsCount = new System.Windows.Forms.Label();
            this.cboWarehouse = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.txtPostingDate = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(784, 509);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.Location = new System.Drawing.Point(703, 509);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 23);
            this.btnFind.TabIndex = 13;
            this.btnFind.Text = "&Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Customer Code";
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.Location = new System.Drawing.Point(126, 27);
            this.txtCustomerCode.MaxLength = 15;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Size = new System.Drawing.Size(100, 20);
            this.txtCustomerCode.TabIndex = 2;
            this.txtCustomerCode.TextChanged += new System.EventHandler(this.txtCustomerCode_TextChanged);
            // 
            // txtSalesInvoiceNo
            // 
            this.txtSalesInvoiceNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSalesInvoiceNo.Location = new System.Drawing.Point(856, 30);
            this.txtSalesInvoiceNo.MaxLength = 21;
            this.txtSalesInvoiceNo.Name = "txtSalesInvoiceNo";
            this.txtSalesInvoiceNo.Size = new System.Drawing.Size(81, 20);
            this.txtSalesInvoiceNo.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(779, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Sales Invoice";
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Location = new System.Drawing.Point(126, 50);
            this.txtCustomerName.MaxLength = 60;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Size = new System.Drawing.Size(302, 20);
            this.txtCustomerName.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Customer Name";
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToAddRows = false;
            this.dgvItems.AllowUserToDeleteRows = false;
            this.dgvItems.AllowUserToResizeRows = false;
            this.dgvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItems.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.itemCode,
            this.description,
            this.vatable,
            this.Qty,
            this.baseUoM,
            this.qtyPrSaleUoM,
            this.saleUoM,
            this.realBsNetSalePrc,
            this.realBsGrossSalePrc,
            this.netPrchsPrc,
            this.grossPrchsPrc,
            this.netSalePrc,
            this.grossSalePrc,
            this.prcntDscnt,
            this.amtDscnt,
            this.rowNetTotal,
            this.rowGrossTotal});
            this.dgvItems.ContextMenuStrip = this.contextMenuStrip2;
            this.dgvItems.Location = new System.Drawing.Point(18, 105);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.RowHeadersVisible = false;
            this.dgvItems.Size = new System.Drawing.Size(921, 260);
            this.dgvItems.TabIndex = 6;
            this.dgvItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItems_CellContentClick);
            // 
            // itemCode
            // 
            this.itemCode.FillWeight = 135.1852F;
            this.itemCode.HeaderText = "Item code";
            this.itemCode.Name = "itemCode";
            this.itemCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.itemCode.Width = 79;
            // 
            // description
            // 
            this.description.FillWeight = 149.9931F;
            this.description.HeaderText = "Description";
            this.description.Name = "description";
            this.description.Width = 85;
            // 
            // vatable
            // 
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.vatable.DefaultCellStyle = dataGridViewCellStyle15;
            this.vatable.FillWeight = 112.9661F;
            this.vatable.HeaderText = "Vatable";
            this.vatable.MaxInputLength = 1;
            this.vatable.Name = "vatable";
            this.vatable.ReadOnly = true;
            this.vatable.Width = 68;
            // 
            // Qty
            // 
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle16.Format = "N6";
            dataGridViewCellStyle16.NullValue = "0.00";
            this.Qty.DefaultCellStyle = dataGridViewCellStyle16;
            this.Qty.FillWeight = 59.72825F;
            this.Qty.HeaderText = "Qty";
            this.Qty.Name = "Qty";
            this.Qty.Width = 48;
            // 
            // baseUoM
            // 
            this.baseUoM.FillWeight = 70.64058F;
            this.baseUoM.HeaderText = "Base UoM";
            this.baseUoM.Items.AddRange(new object[] {
            "Y",
            "N"});
            this.baseUoM.Name = "baseUoM";
            this.baseUoM.Width = 63;
            // 
            // qtyPrSaleUoM
            // 
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.qtyPrSaleUoM.DefaultCellStyle = dataGridViewCellStyle17;
            this.qtyPrSaleUoM.FillWeight = 130.6474F;
            this.qtyPrSaleUoM.HeaderText = "Qty/Sale UoM";
            this.qtyPrSaleUoM.Name = "qtyPrSaleUoM";
            this.qtyPrSaleUoM.ReadOnly = true;
            // 
            // saleUoM
            // 
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.saleUoM.DefaultCellStyle = dataGridViewCellStyle18;
            this.saleUoM.FillWeight = 107.8605F;
            this.saleUoM.HeaderText = "Sale UoM";
            this.saleUoM.Name = "saleUoM";
            this.saleUoM.ReadOnly = true;
            this.saleUoM.Width = 79;
            // 
            // realBsNetSalePrc
            // 
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle19.Format = "N6";
            dataGridViewCellStyle19.NullValue = "0.00";
            this.realBsNetSalePrc.DefaultCellStyle = dataGridViewCellStyle19;
            this.realBsNetSalePrc.FillWeight = 93.64573F;
            this.realBsNetSalePrc.HeaderText = "Real Base Net Sale Price";
            this.realBsNetSalePrc.Name = "realBsNetSalePrc";
            this.realBsNetSalePrc.ReadOnly = true;
            this.realBsNetSalePrc.Width = 5;
            // 
            // realBsGrossSalePrc
            // 
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle20.Format = "N6";
            dataGridViewCellStyle20.NullValue = "0.00";
            this.realBsGrossSalePrc.DefaultCellStyle = dataGridViewCellStyle20;
            this.realBsGrossSalePrc.FillWeight = 102.3697F;
            this.realBsGrossSalePrc.HeaderText = "Real Base Gross Sale Price";
            this.realBsGrossSalePrc.Name = "realBsGrossSalePrc";
            this.realBsGrossSalePrc.ReadOnly = true;
            this.realBsGrossSalePrc.Width = 126;
            // 
            // netPrchsPrc
            // 
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle21.Format = "N6";
            dataGridViewCellStyle21.NullValue = "0.00";
            this.netPrchsPrc.DefaultCellStyle = dataGridViewCellStyle21;
            this.netPrchsPrc.FillWeight = 110.2788F;
            this.netPrchsPrc.HeaderText = "Net Pur. Price";
            this.netPrchsPrc.Name = "netPrchsPrc";
            this.netPrchsPrc.ReadOnly = true;
            this.netPrchsPrc.Width = 5;
            // 
            // grossPrchsPrc
            // 
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle22.Format = "N6";
            dataGridViewCellStyle22.NullValue = "0.00";
            this.grossPrchsPrc.DefaultCellStyle = dataGridViewCellStyle22;
            this.grossPrchsPrc.FillWeight = 110.7466F;
            this.grossPrchsPrc.HeaderText = "Gross Pur. Price";
            this.grossPrchsPrc.Name = "grossPrchsPrc";
            this.grossPrchsPrc.ReadOnly = true;
            this.grossPrchsPrc.Width = 78;
            // 
            // netSalePrc
            // 
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle23.Format = "N6";
            dataGridViewCellStyle23.NullValue = "0.00";
            this.netSalePrc.DefaultCellStyle = dataGridViewCellStyle23;
            this.netSalePrc.FillWeight = 79.63597F;
            this.netSalePrc.HeaderText = "Net Sale Price";
            this.netSalePrc.Name = "netSalePrc";
            this.netSalePrc.ReadOnly = true;
            this.netSalePrc.Width = 5;
            // 
            // grossSalePrc
            // 
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle24.Format = "N6";
            dataGridViewCellStyle24.NullValue = "0.00";
            this.grossSalePrc.DefaultCellStyle = dataGridViewCellStyle24;
            this.grossSalePrc.FillWeight = 87.7643F;
            this.grossSalePrc.HeaderText = "Gross Sale Price";
            this.grossSalePrc.Name = "grossSalePrc";
            this.grossSalePrc.ReadOnly = true;
            this.grossSalePrc.Width = 101;
            // 
            // prcntDscnt
            // 
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle25.Format = "N6";
            dataGridViewCellStyle25.NullValue = "0.00";
            this.prcntDscnt.DefaultCellStyle = dataGridViewCellStyle25;
            this.prcntDscnt.FillWeight = 83.12262F;
            this.prcntDscnt.HeaderText = "% Discnt";
            this.prcntDscnt.Name = "prcntDscnt";
            this.prcntDscnt.Width = 68;
            // 
            // amtDscnt
            // 
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle26.Format = "N6";
            dataGridViewCellStyle26.NullValue = "0.00";
            this.amtDscnt.DefaultCellStyle = dataGridViewCellStyle26;
            this.amtDscnt.FillWeight = 104.8852F;
            this.amtDscnt.HeaderText = "Amt Discount";
            this.amtDscnt.Name = "amtDscnt";
            this.amtDscnt.Width = 88;
            // 
            // rowNetTotal
            // 
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle27.Format = "N6";
            dataGridViewCellStyle27.NullValue = "0.00";
            this.rowNetTotal.DefaultCellStyle = dataGridViewCellStyle27;
            this.rowNetTotal.FillWeight = 76.35129F;
            this.rowNetTotal.HeaderText = "Row Net Total";
            this.rowNetTotal.Name = "rowNetTotal";
            this.rowNetTotal.ReadOnly = true;
            this.rowNetTotal.Width = 5;
            // 
            // rowGrossTotal
            // 
            dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle28.Format = "N6";
            dataGridViewCellStyle28.NullValue = "0.00";
            this.rowGrossTotal.DefaultCellStyle = dataGridViewCellStyle28;
            this.rowGrossTotal.FillWeight = 84.17857F;
            this.rowGrossTotal.HeaderText = "Row Gross Total";
            this.rowGrossTotal.Name = "rowGrossTotal";
            this.rowGrossTotal.ReadOnly = true;
            this.rowGrossTotal.Width = 102;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeRowToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(141, 26);
            this.contextMenuStrip2.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip2_Opening);
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
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(22, 398);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 37;
            this.label10.Text = "Remarks 1";
            // 
            // txtRemarks1
            // 
            this.txtRemarks1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtRemarks1.Location = new System.Drawing.Point(86, 395);
            this.txtRemarks1.MaxLength = 500;
            this.txtRemarks1.Multiline = true;
            this.txtRemarks1.Name = "txtRemarks1";
            this.txtRemarks1.Size = new System.Drawing.Size(131, 49);
            this.txtRemarks1.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 453);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 39;
            this.label4.Text = "Remarks 2";
            // 
            // txtRemarks2
            // 
            this.txtRemarks2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtRemarks2.Location = new System.Drawing.Point(86, 450);
            this.txtRemarks2.MaxLength = 500;
            this.txtRemarks2.Multiline = true;
            this.txtRemarks2.Name = "txtRemarks2";
            this.txtRemarks2.Size = new System.Drawing.Size(131, 49);
            this.txtRemarks2.TabIndex = 8;
            // 
            // txtNetTotal
            // 
            this.txtNetTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNetTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtNetTotal.ForeColor = System.Drawing.Color.Blue;
            this.txtNetTotal.Location = new System.Drawing.Point(824, 433);
            this.txtNetTotal.Name = "txtNetTotal";
            this.txtNetTotal.ReadOnly = true;
            this.txtNetTotal.Size = new System.Drawing.Size(100, 20);
            this.txtNetTotal.TabIndex = 11;
            this.txtNetTotal.Text = "0.00";
            this.txtNetTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(721, 436);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 40;
            this.label5.Text = "Net Total";
            // 
            // txtGrossTotal
            // 
            this.txtGrossTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGrossTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtGrossTotal.ForeColor = System.Drawing.Color.Blue;
            this.txtGrossTotal.Location = new System.Drawing.Point(824, 456);
            this.txtGrossTotal.Name = "txtGrossTotal";
            this.txtGrossTotal.ReadOnly = true;
            this.txtGrossTotal.Size = new System.Drawing.Size(100, 20);
            this.txtGrossTotal.TabIndex = 12;
            this.txtGrossTotal.Text = "0.00";
            this.txtGrossTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtGrossTotal.TextChanged += new System.EventHandler(this.txtGrossTotal_TextChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(721, 459);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 42;
            this.label6.Text = "Gross Total";
            // 
            // linkCustomerCode
            // 
            this.linkCustomerCode.AutoSize = true;
            this.linkCustomerCode.Enabled = false;
            this.linkCustomerCode.Location = new System.Drawing.Point(100, 30);
            this.linkCustomerCode.Name = "linkCustomerCode";
            this.linkCustomerCode.Size = new System.Drawing.Size(19, 13);
            this.linkCustomerCode.TabIndex = 1;
            this.linkCustomerCode.TabStop = true;
            this.linkCustomerCode.Text = ">>";
            this.linkCustomerCode.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCustomerCode_LinkClicked);
            // 
            // linkCustomerName
            // 
            this.linkCustomerName.AutoSize = true;
            this.linkCustomerName.Enabled = false;
            this.linkCustomerName.Location = new System.Drawing.Point(100, 54);
            this.linkCustomerName.Name = "linkCustomerName";
            this.linkCustomerName.Size = new System.Drawing.Size(19, 13);
            this.linkCustomerName.TabIndex = 3;
            this.linkCustomerName.TabStop = true;
            this.linkCustomerName.Text = ">>";
            this.linkCustomerName.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCustomerName_LinkClicked);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnPrint.Enabled = false;
            this.btnPrint.Location = new System.Drawing.Point(865, 509);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 15;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // txtTotalAmtDscnt
            // 
            this.txtTotalAmtDscnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalAmtDscnt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtTotalAmtDscnt.ForeColor = System.Drawing.Color.Blue;
            this.txtTotalAmtDscnt.Location = new System.Drawing.Point(824, 411);
            this.txtTotalAmtDscnt.Name = "txtTotalAmtDscnt";
            this.txtTotalAmtDscnt.ReadOnly = true;
            this.txtTotalAmtDscnt.Size = new System.Drawing.Size(100, 20);
            this.txtTotalAmtDscnt.TabIndex = 10;
            this.txtTotalAmtDscnt.Text = "0.00";
            this.txtTotalAmtDscnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(721, 414);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 13);
            this.label7.TabIndex = 47;
            this.label7.Text = "Total Discount Amt";
            // 
            // txtTotalPrcntDscnt
            // 
            this.txtTotalPrcntDscnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalPrcntDscnt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtTotalPrcntDscnt.ForeColor = System.Drawing.Color.Blue;
            this.txtTotalPrcntDscnt.Location = new System.Drawing.Point(824, 388);
            this.txtTotalPrcntDscnt.Name = "txtTotalPrcntDscnt";
            this.txtTotalPrcntDscnt.ReadOnly = true;
            this.txtTotalPrcntDscnt.Size = new System.Drawing.Size(100, 20);
            this.txtTotalPrcntDscnt.TabIndex = 9;
            this.txtTotalPrcntDscnt.Text = "0.00";
            this.txtTotalPrcntDscnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(721, 391);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 13);
            this.label8.TabIndex = 46;
            this.label8.Text = "Total Discount %";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(779, 57);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 13);
            this.label9.TabIndex = 49;
            this.label9.Text = "Posting Date";
            // 
            // lblitemsCount
            // 
            this.lblitemsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblitemsCount.AutoSize = true;
            this.lblitemsCount.Location = new System.Drawing.Point(22, 368);
            this.lblitemsCount.Name = "lblitemsCount";
            this.lblitemsCount.Size = new System.Drawing.Size(13, 13);
            this.lblitemsCount.TabIndex = 50;
            this.lblitemsCount.Text = "0";
            // 
            // cboWarehouse
            // 
            this.cboWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWarehouse.FormattingEnabled = true;
            this.cboWarehouse.Location = new System.Drawing.Point(126, 76);
            this.cboWarehouse.Name = "cboWarehouse";
            this.cboWarehouse.Size = new System.Drawing.Size(121, 21);
            this.cboWarehouse.TabIndex = 5;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(18, 79);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 13);
            this.label11.TabIndex = 126;
            this.label11.Text = "Warehouse";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker1.Location = new System.Drawing.Point(924, 54);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(13, 20);
            this.dateTimePicker1.TabIndex = 141;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // txtPostingDate
            // 
            this.txtPostingDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPostingDate.Location = new System.Drawing.Point(856, 54);
            this.txtPostingDate.Name = "txtPostingDate";
            this.txtPostingDate.Size = new System.Drawing.Size(67, 20);
            this.txtPostingDate.TabIndex = 140;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label12.Location = new System.Drawing.Point(704, 377);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(236, 112);
            this.label12.TabIndex = 142;
            // 
            // frmARInvoice
            // 
            this.AcceptButton = this.btnFind;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(960, 544);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.txtPostingDate);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lblitemsCount);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtTotalAmtDscnt);
            this.Controls.Add(this.cboWarehouse);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtTotalPrcntDscnt);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.txtGrossTotal);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtNetTotal);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtRemarks2);
            this.Controls.Add(this.linkCustomerName);
            this.Controls.Add(this.dgvItems);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtRemarks1);
            this.Controls.Add(this.linkCustomerCode);
            this.Controls.Add(this.txtSalesInvoiceNo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCustomerName);
            this.Controls.Add(this.txtCustomerCode);
            this.Controls.Add(this.label12);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmARInvoice";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sales Invoice";
            this.Load += new System.EventHandler(this.frmARInvoice_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCustomerCode;
        private System.Windows.Forms.TextBox txtSalesInvoiceNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCustomerName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtRemarks1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRemarks2;
        private System.Windows.Forms.TextBox txtNetTotal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtGrossTotal;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel linkCustomerCode;
        private System.Windows.Forms.LinkLabel linkCustomerName;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.TextBox txtTotalAmtDscnt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTotalPrcntDscnt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblitemsCount;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem removeRowToolStripMenuItem;
        private System.Windows.Forms.ComboBox cboWarehouse;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.TextBox txtPostingDate;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn vatable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        private System.Windows.Forms.DataGridViewComboBoxColumn baseUoM;
        private System.Windows.Forms.DataGridViewTextBoxColumn qtyPrSaleUoM;
        private System.Windows.Forms.DataGridViewTextBoxColumn saleUoM;
        private System.Windows.Forms.DataGridViewTextBoxColumn realBsNetSalePrc;
        private System.Windows.Forms.DataGridViewTextBoxColumn realBsGrossSalePrc;
        private System.Windows.Forms.DataGridViewTextBoxColumn netPrchsPrc;
        private System.Windows.Forms.DataGridViewTextBoxColumn grossPrchsPrc;
        private System.Windows.Forms.DataGridViewTextBoxColumn netSalePrc;
        private System.Windows.Forms.DataGridViewTextBoxColumn grossSalePrc;
        private System.Windows.Forms.DataGridViewTextBoxColumn prcntDscnt;
        private System.Windows.Forms.DataGridViewTextBoxColumn amtDscnt;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowNetTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowGrossTotal;
    }
}