namespace Check_up.forms
{
    partial class frmInventoryPosting
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInventoryPosting));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.colItemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVatable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCurrentQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCountedQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVariance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrchsPrc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRetailPrc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.txtPostingDate = new System.Windows.Forms.TextBox();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.txtCountDateTime = new System.Windows.Forms.TextBox();
            this.txtInvPostingNo = new System.Windows.Forms.TextBox();
            this.txtRemarks1 = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblitemsCount = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cboWarehouse = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Posting Date";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(810, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "No.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Count Date && Time";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 444);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Remarks";
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToAddRows = false;
            this.dgvItems.AllowUserToResizeRows = false;
            this.dgvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvItems.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colItemCode,
            this.colItemDescription,
            this.colVatable,
            this.colCurrentQty,
            this.colCountedQty,
            this.colVariance,
            this.colPrchsPrc,
            this.colRetailPrc});
            this.dgvItems.ContextMenuStrip = this.contextMenuStrip2;
            this.dgvItems.Location = new System.Drawing.Point(12, 85);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.RowHeadersVisible = false;
            this.dgvItems.Size = new System.Drawing.Size(936, 350);
            this.dgvItems.TabIndex = 6;
            // 
            // colItemCode
            // 
            this.colItemCode.HeaderText = "Item Code";
            this.colItemCode.Name = "colItemCode";
            this.colItemCode.Width = 74;
            // 
            // colItemDescription
            // 
            this.colItemDescription.HeaderText = "Description";
            this.colItemDescription.Name = "colItemDescription";
            this.colItemDescription.Width = 85;
            // 
            // colVatable
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colVatable.DefaultCellStyle = dataGridViewCellStyle1;
            this.colVatable.HeaderText = "Vatable";
            this.colVatable.Name = "colVatable";
            this.colVatable.ReadOnly = true;
            this.colVatable.Width = 68;
            // 
            // colCurrentQty
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colCurrentQty.DefaultCellStyle = dataGridViewCellStyle2;
            this.colCurrentQty.HeaderText = "Current Qty (pc)";
            this.colCurrentQty.Name = "colCurrentQty";
            this.colCurrentQty.ReadOnly = true;
            this.colCurrentQty.Width = 81;
            // 
            // colCountedQty
            // 
            this.colCountedQty.HeaderText = "CountedQty (pc)";
            this.colCountedQty.Name = "colCountedQty";
            // 
            // colVariance
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colVariance.DefaultCellStyle = dataGridViewCellStyle3;
            this.colVariance.HeaderText = "Variance (pc)";
            this.colVariance.Name = "colVariance";
            this.colVariance.ReadOnly = true;
            this.colVariance.Width = 87;
            // 
            // colPrchsPrc
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colPrchsPrc.DefaultCellStyle = dataGridViewCellStyle4;
            this.colPrchsPrc.HeaderText = "Pur. Price";
            this.colPrchsPrc.Name = "colPrchsPrc";
            this.colPrchsPrc.ReadOnly = true;
            this.colPrchsPrc.Width = 72;
            // 
            // colRetailPrc
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colRetailPrc.DefaultCellStyle = dataGridViewCellStyle5;
            this.colRetailPrc.HeaderText = "Retail Price";
            this.colRetailPrc.Name = "colRetailPrc";
            this.colRetailPrc.ReadOnly = true;
            this.colRetailPrc.Width = 79;
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
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnPrint.Enabled = false;
            this.btnPrint.Location = new System.Drawing.Point(873, 497);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 10;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.Location = new System.Drawing.Point(711, 497);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 23);
            this.btnFind.TabIndex = 8;
            this.btnFind.Text = "&Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(792, 497);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(199, 21);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(17, 20);
            this.dateTimePicker1.TabIndex = 2;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // txtPostingDate
            // 
            this.txtPostingDate.Location = new System.Drawing.Point(131, 21);
            this.txtPostingDate.Name = "txtPostingDate";
            this.txtPostingDate.Size = new System.Drawing.Size(67, 20);
            this.txtPostingDate.TabIndex = 1;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "MM/dd/yyyy HH:mm tt";
            this.dateTimePicker2.Location = new System.Drawing.Point(256, 47);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(17, 20);
            this.dateTimePicker2.TabIndex = 5;
            this.dateTimePicker2.ValueChanged += new System.EventHandler(this.dateTimePicker2_ValueChanged);
            // 
            // txtCountDateTime
            // 
            this.txtCountDateTime.Location = new System.Drawing.Point(131, 47);
            this.txtCountDateTime.Name = "txtCountDateTime";
            this.txtCountDateTime.Size = new System.Drawing.Size(125, 20);
            this.txtCountDateTime.TabIndex = 4;
            // 
            // txtInvPostingNo
            // 
            this.txtInvPostingNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInvPostingNo.Location = new System.Drawing.Point(840, 24);
            this.txtInvPostingNo.MaxLength = 21;
            this.txtInvPostingNo.Name = "txtInvPostingNo";
            this.txtInvPostingNo.Size = new System.Drawing.Size(108, 20);
            this.txtInvPostingNo.TabIndex = 0;
            // 
            // txtRemarks1
            // 
            this.txtRemarks1.AcceptsReturn = true;
            this.txtRemarks1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtRemarks1.Location = new System.Drawing.Point(91, 441);
            this.txtRemarks1.Multiline = true;
            this.txtRemarks1.Name = "txtRemarks1";
            this.txtRemarks1.Size = new System.Drawing.Size(173, 79);
            this.txtRemarks1.TabIndex = 7;
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
            // lblitemsCount
            // 
            this.lblitemsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblitemsCount.AutoSize = true;
            this.lblitemsCount.Location = new System.Drawing.Point(810, 438);
            this.lblitemsCount.Name = "lblitemsCount";
            this.lblitemsCount.Size = new System.Drawing.Size(13, 13);
            this.lblitemsCount.TabIndex = 180;
            this.lblitemsCount.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(319, 24);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 13);
            this.label11.TabIndex = 182;
            this.label11.Text = "Warehouse";
            // 
            // cboWarehouse
            // 
            this.cboWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWarehouse.FormattingEnabled = true;
            this.cboWarehouse.Location = new System.Drawing.Point(405, 20);
            this.cboWarehouse.Name = "cboWarehouse";
            this.cboWarehouse.Size = new System.Drawing.Size(121, 21);
            this.cboWarehouse.TabIndex = 3;
            this.cboWarehouse.SelectedIndexChanged += new System.EventHandler(this.cboWarehouse_SelectedIndexChanged);
            // 
            // frmInventoryPosting
            // 
            this.AcceptButton = this.btnFind;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(960, 544);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cboWarehouse);
            this.Controls.Add(this.lblitemsCount);
            this.Controls.Add(this.txtRemarks1);
            this.Controls.Add(this.txtInvPostingNo);
            this.Controls.Add(this.txtCountDateTime);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.txtPostingDate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.dgvItems);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmInventoryPosting";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inventory Posting";
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.TextBox txtPostingDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.TextBox txtCountDateTime;
        private System.Windows.Forms.TextBox txtInvPostingNo;
        private System.Windows.Forms.TextBox txtRemarks1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem removeRowToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.Label lblitemsCount;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cboWarehouse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVatable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCurrentQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCountedQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVariance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrchsPrc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRetailPrc;
    }
}