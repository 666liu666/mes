namespace ProcessOutputDetialReport
{
    partial class fMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            this.cmbModle = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbEndTime = new System.Windows.Forms.ComboBox();
            this.cmbStartTime = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtWorkOrder = new System.Windows.Forms.TextBox();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbProcess = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbPdline = new System.Windows.Forms.ComboBox();
            this.btnQuery = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvMainTable = new System.Windows.Forms.DataGridView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dgvDefectStatistics = new System.Windows.Forms.DataGridView();
            this.dgvDetial = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ExportExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.ckbSortByRoute = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMainTable)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefectStatistics)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetial)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbModle
            // 
            this.cmbModle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModle.FormattingEnabled = true;
            resources.ApplyResources(this.cmbModle, "cmbModle");
            this.cmbModle.Name = "cmbModle";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ckbSortByRoute);
            this.panel1.Controls.Add(this.cmbEndTime);
            this.panel1.Controls.Add(this.cmbStartTime);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtWorkOrder);
            this.panel1.Controls.Add(this.dtpEndDate);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.dtpStartDate);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cmbProcess);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cmbPdline);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmbModle);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // cmbEndTime
            // 
            this.cmbEndTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEndTime.FormattingEnabled = true;
            this.cmbEndTime.Items.AddRange(new object[] {
            resources.GetString("cmbEndTime.Items"),
            resources.GetString("cmbEndTime.Items1"),
            resources.GetString("cmbEndTime.Items2"),
            resources.GetString("cmbEndTime.Items3"),
            resources.GetString("cmbEndTime.Items4"),
            resources.GetString("cmbEndTime.Items5"),
            resources.GetString("cmbEndTime.Items6"),
            resources.GetString("cmbEndTime.Items7"),
            resources.GetString("cmbEndTime.Items8"),
            resources.GetString("cmbEndTime.Items9"),
            resources.GetString("cmbEndTime.Items10"),
            resources.GetString("cmbEndTime.Items11"),
            resources.GetString("cmbEndTime.Items12"),
            resources.GetString("cmbEndTime.Items13"),
            resources.GetString("cmbEndTime.Items14"),
            resources.GetString("cmbEndTime.Items15"),
            resources.GetString("cmbEndTime.Items16"),
            resources.GetString("cmbEndTime.Items17"),
            resources.GetString("cmbEndTime.Items18"),
            resources.GetString("cmbEndTime.Items19"),
            resources.GetString("cmbEndTime.Items20"),
            resources.GetString("cmbEndTime.Items21"),
            resources.GetString("cmbEndTime.Items22"),
            resources.GetString("cmbEndTime.Items23")});
            resources.ApplyResources(this.cmbEndTime, "cmbEndTime");
            this.cmbEndTime.Name = "cmbEndTime";
            // 
            // cmbStartTime
            // 
            this.cmbStartTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartTime.FormattingEnabled = true;
            this.cmbStartTime.Items.AddRange(new object[] {
            resources.GetString("cmbStartTime.Items"),
            resources.GetString("cmbStartTime.Items1"),
            resources.GetString("cmbStartTime.Items2"),
            resources.GetString("cmbStartTime.Items3"),
            resources.GetString("cmbStartTime.Items4"),
            resources.GetString("cmbStartTime.Items5"),
            resources.GetString("cmbStartTime.Items6"),
            resources.GetString("cmbStartTime.Items7"),
            resources.GetString("cmbStartTime.Items8"),
            resources.GetString("cmbStartTime.Items9"),
            resources.GetString("cmbStartTime.Items10"),
            resources.GetString("cmbStartTime.Items11"),
            resources.GetString("cmbStartTime.Items12"),
            resources.GetString("cmbStartTime.Items13"),
            resources.GetString("cmbStartTime.Items14"),
            resources.GetString("cmbStartTime.Items15"),
            resources.GetString("cmbStartTime.Items16"),
            resources.GetString("cmbStartTime.Items17"),
            resources.GetString("cmbStartTime.Items18"),
            resources.GetString("cmbStartTime.Items19"),
            resources.GetString("cmbStartTime.Items20"),
            resources.GetString("cmbStartTime.Items21"),
            resources.GetString("cmbStartTime.Items22"),
            resources.GetString("cmbStartTime.Items23")});
            resources.ApplyResources(this.cmbStartTime, "cmbStartTime");
            this.cmbStartTime.Name = "cmbStartTime";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // txtWorkOrder
            // 
            resources.ApplyResources(this.txtWorkOrder, "txtWorkOrder");
            this.txtWorkOrder.Name = "txtWorkOrder";
            // 
            // dtpEndDate
            // 
            resources.ApplyResources(this.dtpEndDate, "dtpEndDate");
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Name = "dtpEndDate";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // dtpStartDate
            // 
            resources.ApplyResources(this.dtpStartDate, "dtpStartDate");
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDate.Name = "dtpStartDate";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbProcess
            // 
            this.cmbProcess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProcess.FormattingEnabled = true;
            resources.ApplyResources(this.cmbProcess, "cmbProcess");
            this.cmbProcess.Name = "cmbProcess";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbPdline
            // 
            this.cmbPdline.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPdline.FormattingEnabled = true;
            resources.ApplyResources(this.cmbPdline, "cmbPdline");
            this.cmbPdline.Name = "cmbPdline";
            // 
            // btnQuery
            // 
            resources.ApplyResources(this.btnQuery, "btnQuery");
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvMainTable);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            // 
            // dgvMainTable
            // 
            this.dgvMainTable.AllowUserToAddRows = false;
            this.dgvMainTable.AllowUserToDeleteRows = false;
            this.dgvMainTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvMainTable.BackgroundColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.dgvMainTable, "dgvMainTable");
            this.dgvMainTable.Name = "dgvMainTable";
            this.dgvMainTable.ReadOnly = true;
            this.dgvMainTable.RowTemplate.Height = 23;
            this.dgvMainTable.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgwMainTable_CellDoubleClick);
            this.dgvMainTable.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvMainTable_CellMouseDown);
            this.dgvMainTable.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvMainTable_RowPostPaint);
            // 
            // splitContainer2
            // 
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dgvDefectStatistics);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dgvDetial);
            // 
            // dgvDefectStatistics
            // 
            this.dgvDefectStatistics.AllowUserToAddRows = false;
            this.dgvDefectStatistics.AllowUserToDeleteRows = false;
            this.dgvDefectStatistics.BackgroundColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.dgvDefectStatistics, "dgvDefectStatistics");
            this.dgvDefectStatistics.Name = "dgvDefectStatistics";
            this.dgvDefectStatistics.ReadOnly = true;
            this.dgvDefectStatistics.RowTemplate.Height = 23;
            this.dgvDefectStatistics.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDefectStatistics_CellDoubleClick);
            this.dgvDefectStatistics.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDefectStatistics_CellMouseDown);
            this.dgvDefectStatistics.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvDefectStatistics_RowPostPaint);
            // 
            // dgvDetial
            // 
            this.dgvDetial.AllowUserToAddRows = false;
            this.dgvDetial.AllowUserToDeleteRows = false;
            this.dgvDetial.BackgroundColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.dgvDetial, "dgvDetial");
            this.dgvDetial.Name = "dgvDetial";
            this.dgvDetial.ReadOnly = true;
            this.dgvDetial.RowTemplate.Height = 23;
            this.dgvDetial.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDetial_CellMouseDown);
            this.dgvDetial.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvDetial_RowPostPaint);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExportExcelToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // ExportExcelToolStripMenuItem
            // 
            this.ExportExcelToolStripMenuItem.Name = "ExportExcelToolStripMenuItem";
            resources.ApplyResources(this.ExportExcelToolStripMenuItem, "ExportExcelToolStripMenuItem");
            this.ExportExcelToolStripMenuItem.Click += new System.EventHandler(this.ExportExcelToolStripMenuItem_Click);
            // 
            // ckbSortByRoute
            // 
            resources.ApplyResources(this.ckbSortByRoute, "ckbSortByRoute");
            this.ckbSortByRoute.Name = "ckbSortByRoute";
            this.ckbSortByRoute.UseVisualStyleBackColor = true;
            // 
            // fMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Name = "fMain";
            this.Load += new System.EventHandler(this.fMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMainTable)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefectStatistics)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetial)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbModle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbProcess;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbPdline;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtWorkOrder;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.ComboBox cmbEndTime;
        private System.Windows.Forms.ComboBox cmbStartTime;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvMainTable;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dgvDefectStatistics;
        private System.Windows.Forms.DataGridView dgvDetial;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ExportExcelToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox ckbSortByRoute;
    }
}