namespace Check_up.forms
{
    partial class frmItemMasterData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemMasterData));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.txtItemCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtShortName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUnitPrice = new System.Windows.Forms.TextBox();
            this.chkVatable = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboPriceList = new System.Windows.Forms.ComboBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label8 = new System.Windows.Forms.Label();
            this.txtVendor = new System.Windows.Forms.TextBox();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.chkDeactivate = new System.Windows.Forms.CheckBox();
            this.dgvBarcode = new System.Windows.Forms.DataGridView();
            this.Barcodes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.linkVendor = new System.Windows.Forms.LinkLabel();
            this.dgvInventory = new System.Windows.Forms.DataGridView();
            this.whCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.whName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label9 = new System.Windows.Forms.Label();
            this.txtQtyPrPrchsUoM = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtQtyPrSalesUoM = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label13 = new System.Windows.Forms.Label();
            this.txtMinStock = new System.Windows.Forms.TextBox();
            this.txtMaxStock = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.chkVarWeightItm = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPurchaseUoM = new System.Windows.Forms.TextBox();
            this.txtSaleUoM = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBarcode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventory)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(674, 452);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.Location = new System.Drawing.Point(593, 452);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 23);
            this.btnFind.TabIndex = 20;
            this.btnFind.Text = "&Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // txtItemCode
            // 
            this.txtItemCode.Location = new System.Drawing.Point(78, 16);
            this.txtItemCode.MaxLength = 15;
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.Size = new System.Drawing.Size(100, 20);
            this.txtItemCode.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Item code";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Description";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(78, 42);
            this.txtDescription.MaxLength = 250;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(216, 20);
            this.txtDescription.TabIndex = 1;
            this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(315, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Short name";
            // 
            // txtShortName
            // 
            this.txtShortName.Location = new System.Drawing.Point(382, 42);
            this.txtShortName.MaxLength = 100;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(151, 20);
            this.txtShortName.TabIndex = 3;
            this.txtShortName.TextChanged += new System.EventHandler(this.txtShortName_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Barcode";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Unit price";
            // 
            // txtUnitPrice
            // 
            this.txtUnitPrice.Location = new System.Drawing.Point(70, 70);
            this.txtUnitPrice.MaxLength = 20;
            this.txtUnitPrice.Name = "txtUnitPrice";
            this.txtUnitPrice.Size = new System.Drawing.Size(121, 20);
            this.txtUnitPrice.TabIndex = 5;
            this.txtUnitPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chkVatable
            // 
            this.chkVatable.AutoSize = true;
            this.chkVatable.Location = new System.Drawing.Point(269, 70);
            this.chkVatable.Name = "chkVatable";
            this.chkVatable.Size = new System.Drawing.Size(62, 17);
            this.chkVatable.TabIndex = 17;
            this.chkVatable.Text = "Vatable";
            this.chkVatable.UseVisualStyleBackColor = true;
            this.chkVatable.CheckedChanged += new System.EventHandler(this.chkVatable_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cboPriceList);
            this.groupBox1.Controls.Add(this.txtUnitPrice);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(318, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(215, 108);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pricing";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "Price list";
            // 
            // cboPriceList
            // 
            this.cboPriceList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPriceList.FormattingEnabled = true;
            this.cboPriceList.Items.AddRange(new object[] {
            "Purchase price",
            "Retail price"});
            this.cboPriceList.Location = new System.Drawing.Point(70, 26);
            this.cboPriceList.Name = "cboPriceList";
            this.cboPriceList.Size = new System.Drawing.Size(121, 21);
            this.cboPriceList.TabIndex = 4;
            this.cboPriceList.SelectedIndexChanged += new System.EventHandler(this.cboPriceList_SelectedIndexChanged);
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
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lockToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(100, 26);
            // 
            // lockToolStripMenuItem
            // 
            this.lockToolStripMenuItem.Name = "lockToolStripMenuItem";
            this.lockToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.lockToolStripMenuItem.Text = "Lock";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(554, 44);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 33;
            this.label8.Text = "Vendor";
            // 
            // txtVendor
            // 
            this.txtVendor.Location = new System.Drawing.Point(620, 41);
            this.txtVendor.MaxLength = 15;
            this.txtVendor.Name = "txtVendor";
            this.txtVendor.Size = new System.Drawing.Size(131, 20);
            this.txtVendor.TabIndex = 7;
            this.txtVendor.TextChanged += new System.EventHandler(this.txtVendor_TextChanged);
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(620, 67);
            this.txtRemarks.MaxLength = 500;
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(131, 59);
            this.txtRemarks.TabIndex = 8;
            this.txtRemarks.TextChanged += new System.EventHandler(this.txtRemarks_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(554, 70);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 13);
            this.label10.TabIndex = 35;
            this.label10.Text = "Remarks";
            // 
            // chkDeactivate
            // 
            this.chkDeactivate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkDeactivate.AutoSize = true;
            this.chkDeactivate.Location = new System.Drawing.Point(10, 458);
            this.chkDeactivate.Name = "chkDeactivate";
            this.chkDeactivate.Size = new System.Drawing.Size(78, 17);
            this.chkDeactivate.TabIndex = 19;
            this.chkDeactivate.Text = "Deactivate";
            this.chkDeactivate.UseVisualStyleBackColor = true;
            // 
            // dgvBarcode
            // 
            this.dgvBarcode.AllowUserToAddRows = false;
            this.dgvBarcode.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvBarcode.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBarcode.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Barcodes});
            this.dgvBarcode.Location = new System.Drawing.Point(78, 68);
            this.dgvBarcode.Name = "dgvBarcode";
            this.dgvBarcode.RowHeadersVisible = false;
            this.dgvBarcode.Size = new System.Drawing.Size(216, 108);
            this.dgvBarcode.TabIndex = 2;
            this.dgvBarcode.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBarcode_CellContentClick);
            // 
            // Barcodes
            // 
            this.Barcodes.HeaderText = "Barcodes";
            this.Barcodes.Name = "Barcodes";
            this.Barcodes.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Barcodes.Width = 205;
            // 
            // linkVendor
            // 
            this.linkVendor.AutoSize = true;
            this.linkVendor.Enabled = false;
            this.linkVendor.Location = new System.Drawing.Point(597, 44);
            this.linkVendor.Name = "linkVendor";
            this.linkVendor.Size = new System.Drawing.Size(19, 13);
            this.linkVendor.TabIndex = 6;
            this.linkVendor.TabStop = true;
            this.linkVendor.Text = ">>";
            this.linkVendor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkVendor_LinkClicked);
            // 
            // dgvInventory
            // 
            this.dgvInventory.AllowUserToAddRows = false;
            this.dgvInventory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvInventory.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInventory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.whCode,
            this.whName,
            this.inStock});
            this.dgvInventory.Location = new System.Drawing.Point(6, 6);
            this.dgvInventory.Name = "dgvInventory";
            this.dgvInventory.RowHeadersVisible = false;
            this.dgvInventory.Size = new System.Drawing.Size(716, 193);
            this.dgvInventory.TabIndex = 10;
            // 
            // whCode
            // 
            this.whCode.HeaderText = "Warehouse";
            this.whCode.Name = "whCode";
            this.whCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // whName
            // 
            this.whName.HeaderText = "Warehouse name";
            this.whName.Name = "whName";
            this.whName.Width = 200;
            // 
            // inStock
            // 
            this.inStock.HeaderText = "In Stock";
            this.inStock.Name = "inStock";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 43);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(116, 13);
            this.label9.TabIndex = 37;
            this.label9.Text = "Qty Per Purchase UoM";
            // 
            // txtQtyPrPrchsUoM
            // 
            this.txtQtyPrPrchsUoM.Location = new System.Drawing.Point(139, 40);
            this.txtQtyPrPrchsUoM.MaxLength = 15;
            this.txtQtyPrPrchsUoM.Name = "txtQtyPrPrchsUoM";
            this.txtQtyPrPrchsUoM.Size = new System.Drawing.Size(69, 20);
            this.txtQtyPrPrchsUoM.TabIndex = 12;
            this.txtQtyPrPrchsUoM.Text = "1";
            this.txtQtyPrPrchsUoM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 94);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(97, 13);
            this.label11.TabIndex = 39;
            this.label11.Text = "Qty Per Sales UoM";
            // 
            // txtQtyPrSalesUoM
            // 
            this.txtQtyPrSalesUoM.Location = new System.Drawing.Point(139, 92);
            this.txtQtyPrSalesUoM.MaxLength = 20;
            this.txtQtyPrSalesUoM.Name = "txtQtyPrSalesUoM";
            this.txtQtyPrSalesUoM.Size = new System.Drawing.Size(69, 20);
            this.txtQtyPrSalesUoM.TabIndex = 14;
            this.txtQtyPrSalesUoM.Text = "1";
            this.txtQtyPrSalesUoM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(15, 197);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(736, 231);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvInventory);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(728, 205);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Inventory";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.txtMinStock);
            this.tabPage2.Controls.Add(this.txtMaxStock);
            this.tabPage2.Controls.Add(this.label14);
            this.tabPage2.Controls.Add(this.chkVarWeightItm);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.txtPurchaseUoM);
            this.tabPage2.Controls.Add(this.txtSaleUoM);
            this.tabPage2.Controls.Add(this.chkVatable);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.txtQtyPrPrchsUoM);
            this.tabPage2.Controls.Add(this.txtQtyPrSalesUoM);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(728, 205);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Purchasing and Sales";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(266, 43);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(61, 13);
            this.label13.TabIndex = 48;
            this.label13.Text = "Max. Stock";
            // 
            // txtMinStock
            // 
            this.txtMinStock.Location = new System.Drawing.Point(333, 16);
            this.txtMinStock.MaxLength = 15;
            this.txtMinStock.Name = "txtMinStock";
            this.txtMinStock.Size = new System.Drawing.Size(69, 20);
            this.txtMinStock.TabIndex = 15;
            this.txtMinStock.Text = "0";
            this.txtMinStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtMaxStock
            // 
            this.txtMaxStock.Location = new System.Drawing.Point(333, 40);
            this.txtMaxStock.MaxLength = 15;
            this.txtMaxStock.Name = "txtMaxStock";
            this.txtMaxStock.Size = new System.Drawing.Size(69, 20);
            this.txtMaxStock.TabIndex = 16;
            this.txtMaxStock.Text = "0";
            this.txtMaxStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(266, 19);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(58, 13);
            this.label14.TabIndex = 47;
            this.label14.Text = "Min. Stock";
            // 
            // chkVarWeightItm
            // 
            this.chkVarWeightItm.AutoSize = true;
            this.chkVarWeightItm.Location = new System.Drawing.Point(269, 90);
            this.chkVarWeightItm.Name = "chkVarWeightItm";
            this.chkVarWeightItm.Size = new System.Drawing.Size(124, 17);
            this.chkVarWeightItm.TabIndex = 18;
            this.chkVarWeightItm.Text = "Variable Weight Item";
            this.chkVarWeightItm.UseVisualStyleBackColor = true;
            this.chkVarWeightItm.CheckedChanged += new System.EventHandler(this.chkVarWeightItm_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 43;
            this.label7.Text = "Sale UoM";
            // 
            // txtPurchaseUoM
            // 
            this.txtPurchaseUoM.Location = new System.Drawing.Point(139, 14);
            this.txtPurchaseUoM.MaxLength = 20;
            this.txtPurchaseUoM.Name = "txtPurchaseUoM";
            this.txtPurchaseUoM.Size = new System.Drawing.Size(69, 20);
            this.txtPurchaseUoM.TabIndex = 11;
            this.txtPurchaseUoM.Text = "PC";
            this.txtPurchaseUoM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtSaleUoM
            // 
            this.txtSaleUoM.Location = new System.Drawing.Point(139, 66);
            this.txtSaleUoM.MaxLength = 15;
            this.txtSaleUoM.Name = "txtSaleUoM";
            this.txtSaleUoM.Size = new System.Drawing.Size(69, 20);
            this.txtSaleUoM.TabIndex = 13;
            this.txtSaleUoM.Text = "PC";
            this.txtSaleUoM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 17);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(78, 13);
            this.label12.TabIndex = 42;
            this.label12.Text = "Purchase UoM";
            // 
            // frmItemMasterData
            // 
            this.AcceptButton = this.btnFind;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(761, 487);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.dgvBarcode);
            this.Controls.Add(this.chkDeactivate);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.linkVendor);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtShortName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtItemCode);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.txtVendor);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmItemMasterData";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Item Master Data";
            this.Load += new System.EventHandler(this.frmItemMasterData_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBarcode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventory)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TextBox txtItemCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtShortName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtUnitPrice;
        private System.Windows.Forms.CheckBox chkVatable;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboPriceList;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem lockToolStripMenuItem;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtVendor;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkDeactivate;
        private System.Windows.Forms.DataGridView dgvBarcode;
        private System.Windows.Forms.LinkLabel linkVendor;
        private System.Windows.Forms.DataGridView dgvInventory;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtQtyPrPrchsUoM;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtQtyPrSalesUoM;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPurchaseUoM;
        private System.Windows.Forms.TextBox txtSaleUoM;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox chkVarWeightItm;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtMinStock;
        private System.Windows.Forms.TextBox txtMaxStock;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DataGridViewTextBoxColumn whCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn whName;
        private System.Windows.Forms.DataGridViewTextBoxColumn inStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn Barcodes;
    }
}