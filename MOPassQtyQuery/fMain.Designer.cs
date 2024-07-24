namespace MOPassQtyQuery
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.btnQuery = new System.Windows.Forms.Button();
            this.cmbRoute = new System.Windows.Forms.ComboBox();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.cmbEndTime = new System.Windows.Forms.ComboBox();
            this.cmbStartTime = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtWorkOrder = new System.Windows.Forms.TextBox();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbPdline = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbModle = new System.Windows.Forms.ComboBox();
            this.dgvMainTable = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.导出数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabItem1 = new DevComponents.DotNetBar.TabItem(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.导出数据ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabItem2 = new DevComponents.DotNetBar.TabItem(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.mProcessID = new System.Windows.Forms.CheckedListBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMainTable)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvMainTable);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.mProcessID);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.cmbRoute);
            this.panel1.Controls.Add(this.groupPanel1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Name = "label7";
            // 
            // btnQuery
            // 
            resources.ApplyResources(this.btnQuery, "btnQuery");
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // cmbRoute
            // 
            this.cmbRoute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRoute.FormattingEnabled = true;
            resources.ApplyResources(this.cmbRoute, "cmbRoute");
            this.cmbRoute.Name = "cmbRoute";
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.cmbEndTime);
            this.groupPanel1.Controls.Add(this.cmbStartTime);
            this.groupPanel1.Controls.Add(this.label6);
            this.groupPanel1.Controls.Add(this.txtWorkOrder);
            this.groupPanel1.Controls.Add(this.dtpEndDate);
            this.groupPanel1.Controls.Add(this.label5);
            this.groupPanel1.Controls.Add(this.dtpStartDate);
            this.groupPanel1.Controls.Add(this.label4);
            this.groupPanel1.Controls.Add(this.label3);
            this.groupPanel1.Controls.Add(this.label2);
            this.groupPanel1.Controls.Add(this.cmbPdline);
            this.groupPanel1.Controls.Add(this.label1);
            this.groupPanel1.Controls.Add(this.cmbModle);
            this.groupPanel1.DisabledBackColor = System.Drawing.Color.Empty;
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Name = "groupPanel1";
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
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
            this.label6.BackColor = System.Drawing.Color.Transparent;
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
            this.label5.BackColor = System.Drawing.Color.Transparent;
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
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // cmbPdline
            // 
            this.cmbPdline.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPdline.FormattingEnabled = true;
            resources.ApplyResources(this.cmbPdline, "cmbPdline");
            this.cmbPdline.Name = "cmbPdline";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // cmbModle
            // 
            this.cmbModle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModle.FormattingEnabled = true;
            resources.ApplyResources(this.cmbModle, "cmbModle");
            this.cmbModle.Name = "cmbModle";
            // 
            // dgvMainTable
            // 
            this.dgvMainTable.AllowUserToAddRows = false;
            this.dgvMainTable.AllowUserToDeleteRows = false;
            this.dgvMainTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMainTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvMainTable.BackgroundColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.dgvMainTable, "dgvMainTable");
            this.dgvMainTable.ContextMenuStrip = this.contextMenuStrip1;
            this.dgvMainTable.Name = "dgvMainTable";
            this.dgvMainTable.ReadOnly = true;
            this.dgvMainTable.RowTemplate.Height = 23;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导出数据ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // 导出数据ToolStripMenuItem
            // 
            this.导出数据ToolStripMenuItem.Name = "导出数据ToolStripMenuItem";
            resources.ApplyResources(this.导出数据ToolStripMenuItem, "导出数据ToolStripMenuItem");
            this.导出数据ToolStripMenuItem.Click += new System.EventHandler(this.导出数据ToolStripMenuItem_Click);
            // 
            // tabItem1
            // 
            this.tabItem1.Name = "tabItem1";
            resources.ApplyResources(this.tabItem1, "tabItem1");
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导出数据ToolStripMenuItem1});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            resources.ApplyResources(this.contextMenuStrip2, "contextMenuStrip2");
            // 
            // 导出数据ToolStripMenuItem1
            // 
            this.导出数据ToolStripMenuItem1.Name = "导出数据ToolStripMenuItem1";
            resources.ApplyResources(this.导出数据ToolStripMenuItem1, "导出数据ToolStripMenuItem1");
            // 
            // tabItem2
            // 
            this.tabItem2.Name = "tabItem2";
            resources.ApplyResources(this.tabItem2, "tabItem2");
            this.tabItem2.Visible = false;
            // 
            // mProcessID
            // 
            this.mProcessID.FormattingEnabled = true;
            resources.ApplyResources(this.mProcessID, "mProcessID");
            this.mProcessID.Name = "mProcessID";
            // 
            // fMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "fMain";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.fMain_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMainTable)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.DotNetBar.TabItem tabItem1;
        private DevComponents.DotNetBar.TabItem tabItem2;
        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private System.Windows.Forms.ComboBox cmbEndTime;
        private System.Windows.Forms.ComboBox cmbStartTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtWorkOrder;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbPdline;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbModle;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 导出数据ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 导出数据ToolStripMenuItem1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbRoute;
        private System.Windows.Forms.DataGridView dgvMainTable;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckedListBox mProcessID;

    }
}