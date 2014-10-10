namespace Check_up.forms
{
    partial class frmCrystalReportViewer
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
            this.CrystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.arInvoice1 = new Check_up.crystal_reports.arInvoice();
            this.SuspendLayout();
            // 
            // CrystalReportViewer1
            // 
            this.CrystalReportViewer1.ActiveViewIndex = -1;
            this.CrystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CrystalReportViewer1.DisplayGroupTree = false;
            this.CrystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CrystalReportViewer1.Location = new System.Drawing.Point(0, 0);
            this.CrystalReportViewer1.Name = "CrystalReportViewer1";
            this.CrystalReportViewer1.SelectionFormula = "";
            this.CrystalReportViewer1.ShowGroupTreeButton = false;
            this.CrystalReportViewer1.ShowRefreshButton = false;
            this.CrystalReportViewer1.Size = new System.Drawing.Size(869, 453);
            this.CrystalReportViewer1.TabIndex = 1;
            this.CrystalReportViewer1.ViewTimeSelectionFormula = "";
            // 
            // frmCrystalReportViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 453);
            this.Controls.Add(this.CrystalReportViewer1);
            this.Name = "frmCrystalReportViewer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Report";
            this.ResumeLayout(false);

        }

        #endregion

        private Check_up.crystal_reports.arInvoice arInvoice1;
        protected internal CrystalDecisions.Windows.Forms.CrystalReportViewer CrystalReportViewer1;






    }
}