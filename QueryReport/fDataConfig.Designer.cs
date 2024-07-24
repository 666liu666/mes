namespace QueryReport
{
    partial class fDataConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fDataConfig));
            this.sideNav1 = new DevComponents.DotNetBar.Controls.SideNav();
            this.sideNavPanel4 = new DevComponents.DotNetBar.Controls.SideNavPanel();
            this.treeViewRoute = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.collapseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.delToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.sideNavPanel3 = new DevComponents.DotNetBar.Controls.SideNavPanel();
            this.treeViewProcess = new System.Windows.Forms.TreeView();
            this.sideNavPanel2 = new DevComponents.DotNetBar.Controls.SideNavPanel();
            this.treeViewPdline = new System.Windows.Forms.TreeView();
            this.sideNavPanel1 = new DevComponents.DotNetBar.Controls.SideNavPanel();
            this.treeViewModel = new System.Windows.Forms.TreeView();
            this.sideNavItemMenu = new DevComponents.DotNetBar.Controls.SideNavItem();
            this.separator1 = new DevComponents.DotNetBar.Separator();
            this.sideNavItemModel = new DevComponents.DotNetBar.Controls.SideNavItem();
            this.sideNavItemPdline = new DevComponents.DotNetBar.Controls.SideNavItem();
            this.sideNavItemProcess = new DevComponents.DotNetBar.Controls.SideNavItem();
            this.sideNavItemRoute = new DevComponents.DotNetBar.Controls.SideNavItem();
            this.elementStyle2 = new DevComponents.DotNetBar.ElementStyle();
            this.elementStyle5 = new DevComponents.DotNetBar.ElementStyle();
            this.elementStyle4 = new DevComponents.DotNetBar.ElementStyle();
            this.elementStyle3 = new DevComponents.DotNetBar.ElementStyle();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.treeViewConfig = new System.Windows.Forms.TreeView();
            this.panelEx3 = new DevComponents.DotNetBar.PanelEx();
            this.panelEx4 = new DevComponents.DotNetBar.PanelEx();
            this.btnQuit = new DevComponents.DotNetBar.ButtonX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.treeView = new System.Windows.Forms.TreeView();
            this.sideNav1.SuspendLayout();
            this.sideNavPanel4.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.sideNavPanel3.SuspendLayout();
            this.sideNavPanel2.SuspendLayout();
            this.sideNavPanel1.SuspendLayout();
            this.panelEx1.SuspendLayout();
            this.panelEx3.SuspendLayout();
            this.panelEx4.SuspendLayout();
            this.SuspendLayout();
            // 
            // sideNav1
            // 
            this.sideNav1.Controls.Add(this.sideNavPanel4);
            this.sideNav1.Controls.Add(this.sideNavPanel2);
            this.sideNav1.Controls.Add(this.sideNavPanel3);
            this.sideNav1.Controls.Add(this.sideNavPanel1);
            this.sideNav1.Dock = System.Windows.Forms.DockStyle.Left;
            this.sideNav1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.sideNavItemMenu,
            this.separator1,
            this.sideNavItemModel,
            this.sideNavItemPdline,
            this.sideNavItemProcess,
            this.sideNavItemRoute});
            this.sideNav1.Location = new System.Drawing.Point(0, 0);
            this.sideNav1.Name = "sideNav1";
            this.sideNav1.Padding = new System.Windows.Forms.Padding(1);
            this.sideNav1.Size = new System.Drawing.Size(358, 464);
            this.sideNav1.TabIndex = 3;
            // 
            // sideNavPanel4
            // 
            this.sideNavPanel4.Controls.Add(this.treeViewRoute);
            this.sideNavPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sideNavPanel4.Location = new System.Drawing.Point(111, 38);
            this.sideNavPanel4.Name = "sideNavPanel4";
            this.sideNavPanel4.Size = new System.Drawing.Size(242, 425);
            this.sideNavPanel4.TabIndex = 14;
            // 
            // treeViewRoute
            // 
            this.treeViewRoute.AllowDrop = true;
            this.treeViewRoute.ContextMenuStrip = this.contextMenuStrip1;
            this.treeViewRoute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewRoute.FullRowSelect = true;
            this.treeViewRoute.ImageIndex = 0;
            this.treeViewRoute.ImageList = this.imageList1;
            this.treeViewRoute.Location = new System.Drawing.Point(0, 0);
            this.treeViewRoute.Name = "treeViewRoute";
            this.treeViewRoute.SelectedImageIndex = 0;
            this.treeViewRoute.Size = new System.Drawing.Size(242, 425);
            this.treeViewRoute.TabIndex = 1;
            this.treeViewRoute.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewRoute_ItemDrag);
            this.treeViewRoute.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewRoute_AfterSelect);
            this.treeViewRoute.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewRoute_NodeMouseClick);
            this.treeViewRoute.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewRoute_DragEnter);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.collapseToolStripMenuItem,
            this.expandToolStripMenuItem,
            this.toolStripSeparator1,
            this.delToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 76);
            // 
            // collapseToolStripMenuItem
            // 
            this.collapseToolStripMenuItem.Name = "collapseToolStripMenuItem";
            this.collapseToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.collapseToolStripMenuItem.Text = "折叠所有节点";
            this.collapseToolStripMenuItem.Click += new System.EventHandler(this.collapseToolStripMenuItem_Click);
            // 
            // expandToolStripMenuItem
            // 
            this.expandToolStripMenuItem.Name = "expandToolStripMenuItem";
            this.expandToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.expandToolStripMenuItem.Text = "展开所有节点";
            this.expandToolStripMenuItem.Click += new System.EventHandler(this.expandToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
            // 
            // delToolStripMenuItem
            // 
            this.delToolStripMenuItem.Name = "delToolStripMenuItem";
            this.delToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.delToolStripMenuItem.Text = "删除选中节点";
            this.delToolStripMenuItem.Click += new System.EventHandler(this.delToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "model.PNG");
            this.imageList1.Images.SetKeyName(1, "pdline.PNG");
            this.imageList1.Images.SetKeyName(2, "process.PNG");
            this.imageList1.Images.SetKeyName(3, "Route.PNG");
            // 
            // sideNavPanel3
            // 
            this.sideNavPanel3.Controls.Add(this.treeViewProcess);
            this.sideNavPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sideNavPanel3.Location = new System.Drawing.Point(111, 38);
            this.sideNavPanel3.Name = "sideNavPanel3";
            this.sideNavPanel3.Size = new System.Drawing.Size(242, 425);
            this.sideNavPanel3.TabIndex = 10;
            this.sideNavPanel3.Visible = false;
            // 
            // treeViewProcess
            // 
            this.treeViewProcess.ContextMenuStrip = this.contextMenuStrip1;
            this.treeViewProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewProcess.FullRowSelect = true;
            this.treeViewProcess.ImageIndex = 0;
            this.treeViewProcess.ImageList = this.imageList1;
            this.treeViewProcess.Location = new System.Drawing.Point(0, 0);
            this.treeViewProcess.Name = "treeViewProcess";
            this.treeViewProcess.SelectedImageIndex = 0;
            this.treeViewProcess.Size = new System.Drawing.Size(242, 425);
            this.treeViewProcess.TabIndex = 1;
            this.treeViewProcess.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewProcess_ItemDrag);
            this.treeViewProcess.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewProcess_AfterSelect);
            this.treeViewProcess.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewProcess_NodeMouseClick);
            this.treeViewProcess.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewProcess_DragEnter);
            // 
            // sideNavPanel2
            // 
            this.sideNavPanel2.Controls.Add(this.treeViewPdline);
            this.sideNavPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sideNavPanel2.Location = new System.Drawing.Point(111, 38);
            this.sideNavPanel2.Name = "sideNavPanel2";
            this.sideNavPanel2.Size = new System.Drawing.Size(242, 425);
            this.sideNavPanel2.TabIndex = 6;
            this.sideNavPanel2.Visible = false;
            // 
            // treeViewPdline
            // 
            this.treeViewPdline.AllowDrop = true;
            this.treeViewPdline.ContextMenuStrip = this.contextMenuStrip1;
            this.treeViewPdline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewPdline.FullRowSelect = true;
            this.treeViewPdline.ImageIndex = 0;
            this.treeViewPdline.ImageList = this.imageList1;
            this.treeViewPdline.Location = new System.Drawing.Point(0, 0);
            this.treeViewPdline.Name = "treeViewPdline";
            this.treeViewPdline.SelectedImageIndex = 0;
            this.treeViewPdline.Size = new System.Drawing.Size(242, 425);
            this.treeViewPdline.TabIndex = 1;
            this.treeViewPdline.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewPdline_ItemDrag);
            this.treeViewPdline.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewPdline_AfterSelect);
            this.treeViewPdline.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewPdline_NodeMouseClick);
            this.treeViewPdline.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewPdline_DragEnter);
            // 
            // sideNavPanel1
            // 
            this.sideNavPanel1.Controls.Add(this.treeViewModel);
            this.sideNavPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sideNavPanel1.Location = new System.Drawing.Point(111, 38);
            this.sideNavPanel1.Name = "sideNavPanel1";
            this.sideNavPanel1.Size = new System.Drawing.Size(242, 425);
            this.sideNavPanel1.TabIndex = 2;
            this.sideNavPanel1.Visible = false;
            // 
            // treeViewModel
            // 
            this.treeViewModel.AllowDrop = true;
            this.treeViewModel.ContextMenuStrip = this.contextMenuStrip1;
            this.treeViewModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewModel.FullRowSelect = true;
            this.treeViewModel.ImageIndex = 0;
            this.treeViewModel.ImageList = this.imageList1;
            this.treeViewModel.Location = new System.Drawing.Point(0, 0);
            this.treeViewModel.Name = "treeViewModel";
            this.treeViewModel.SelectedImageIndex = 0;
            this.treeViewModel.Size = new System.Drawing.Size(242, 425);
            this.treeViewModel.TabIndex = 1;
            this.treeViewModel.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewModel_ItemDrag);
            this.treeViewModel.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewModel_AfterSelect);
            this.treeViewModel.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewModel_NodeMouseClick);
            this.treeViewModel.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewModel_DragEnter);
            // 
            // sideNavItemMenu
            // 
            this.sideNavItemMenu.IsSystemMenu = true;
            this.sideNavItemMenu.Name = "sideNavItemMenu";
            this.sideNavItemMenu.Symbol = "";
            this.sideNavItemMenu.Text = "Menu";
            // 
            // separator1
            // 
            this.separator1.FixedSize = new System.Drawing.Size(3, 1);
            this.separator1.Name = "separator1";
            this.separator1.Padding.Bottom = 2;
            this.separator1.Padding.Left = 6;
            this.separator1.Padding.Right = 6;
            this.separator1.Padding.Top = 2;
            this.separator1.SeparatorOrientation = DevComponents.DotNetBar.eDesignMarkerOrientation.Vertical;
            // 
            // sideNavItemModel
            // 
            this.sideNavItemModel.Name = "sideNavItemModel";
            this.sideNavItemModel.Panel = this.sideNavPanel1;
            this.sideNavItemModel.Symbol = "";
            this.sideNavItemModel.Text = "机种";
            // 
            // sideNavItemPdline
            // 
            this.sideNavItemPdline.Name = "sideNavItemPdline";
            this.sideNavItemPdline.Panel = this.sideNavPanel2;
            this.sideNavItemPdline.Symbol = "";
            this.sideNavItemPdline.Text = "线别";
            // 
            // sideNavItemProcess
            // 
            this.sideNavItemProcess.Name = "sideNavItemProcess";
            this.sideNavItemProcess.Panel = this.sideNavPanel3;
            this.sideNavItemProcess.Symbol = "";
            this.sideNavItemProcess.Text = "制程";
            // 
            // sideNavItemRoute
            // 
            this.sideNavItemRoute.Checked = true;
            this.sideNavItemRoute.Name = "sideNavItemRoute";
            this.sideNavItemRoute.Panel = this.sideNavPanel4;
            this.sideNavItemRoute.Symbol = "";
            this.sideNavItemRoute.Text = "工艺流程";
            // 
            // elementStyle2
            // 
            this.elementStyle2.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle2.Name = "elementStyle2";
            this.elementStyle2.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle5
            // 
            this.elementStyle5.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle5.Name = "elementStyle5";
            this.elementStyle5.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle4
            // 
            this.elementStyle4.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle4.Name = "elementStyle4";
            this.elementStyle4.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle3
            // 
            this.elementStyle3.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle3.Name = "elementStyle3";
            this.elementStyle3.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.treeViewConfig);
            this.panelEx1.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(358, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(436, 464);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 4;
            // 
            // treeViewConfig
            // 
            this.treeViewConfig.AllowDrop = true;
            this.treeViewConfig.ContextMenuStrip = this.contextMenuStrip1;
            this.treeViewConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewConfig.ImageIndex = 0;
            this.treeViewConfig.ImageList = this.imageList1;
            this.treeViewConfig.Location = new System.Drawing.Point(0, 0);
            this.treeViewConfig.Name = "treeViewConfig";
            this.treeViewConfig.SelectedImageIndex = 0;
            this.treeViewConfig.Size = new System.Drawing.Size(436, 464);
            this.treeViewConfig.TabIndex = 1;
            this.treeViewConfig.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewConfig_ItemDrag);
            this.treeViewConfig.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewConfig_AfterSelect);
            this.treeViewConfig.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewConfig_NodeMouseClick);
            this.treeViewConfig.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewConfig_DragDrop);
            this.treeViewConfig.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewConfig_DragEnter);
            // 
            // panelEx3
            // 
            this.panelEx3.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx3.Controls.Add(this.panelEx1);
            this.panelEx3.Controls.Add(this.sideNav1);
            this.panelEx3.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelEx3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx3.Location = new System.Drawing.Point(0, 0);
            this.panelEx3.Name = "panelEx3";
            this.panelEx3.Size = new System.Drawing.Size(794, 464);
            this.panelEx3.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx3.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx3.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx3.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx3.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx3.Style.GradientAngle = 90;
            this.panelEx3.TabIndex = 1;
            // 
            // panelEx4
            // 
            this.panelEx4.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx4.Controls.Add(this.btnQuit);
            this.panelEx4.Controls.Add(this.btnSave);
            this.panelEx4.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelEx4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelEx4.Location = new System.Drawing.Point(0, 464);
            this.panelEx4.Name = "panelEx4";
            this.panelEx4.Size = new System.Drawing.Size(794, 47);
            this.panelEx4.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx4.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx4.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx4.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx4.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx4.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx4.Style.GradientAngle = 90;
            this.panelEx4.TabIndex = 5;
            // 
            // btnQuit
            // 
            this.btnQuit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnQuit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnQuit.Location = new System.Drawing.Point(471, 12);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(75, 23);
            this.btnQuit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnQuit.TabIndex = 1;
            this.btnQuit.Text = "退出";
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(224, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // treeView
            // 
            this.treeView.LineColor = System.Drawing.Color.Empty;
            this.treeView.Location = new System.Drawing.Point(29, 68);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(121, 97);
            this.treeView.TabIndex = 0;
            // 
            // fDataConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 511);
            this.Controls.Add(this.panelEx3);
            this.Controls.Add(this.panelEx4);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "fDataConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "QueryConfig";
            this.Load += new System.EventHandler(this.fDataConfig_Load);
            this.sideNav1.ResumeLayout(false);
            this.sideNav1.PerformLayout();
            this.sideNavPanel4.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.sideNavPanel3.ResumeLayout(false);
            this.sideNavPanel2.ResumeLayout(false);
            this.sideNavPanel1.ResumeLayout(false);
            this.panelEx1.ResumeLayout(false);
            this.panelEx3.ResumeLayout(false);
            this.panelEx4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.SideNav sideNav1;
        private DevComponents.DotNetBar.Controls.SideNavPanel sideNavPanel2;
        private DevComponents.DotNetBar.Controls.SideNavPanel sideNavPanel1;
        private DevComponents.DotNetBar.Controls.SideNavPanel sideNavPanel3;
        private DevComponents.DotNetBar.Controls.SideNavItem sideNavItemMenu;
        private DevComponents.DotNetBar.Separator separator1;
        private DevComponents.DotNetBar.Controls.SideNavItem sideNavItemModel;
        private DevComponents.DotNetBar.Controls.SideNavItem sideNavItemPdline;
        private DevComponents.DotNetBar.Controls.SideNavItem sideNavItemProcess;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.Controls.SideNavPanel sideNavPanel4;
        private DevComponents.DotNetBar.Controls.SideNavItem sideNavItemRoute;
        private DevComponents.DotNetBar.PanelEx panelEx3;
        private DevComponents.DotNetBar.PanelEx panelEx4;
        private DevComponents.DotNetBar.ButtonX btnQuit;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private System.Windows.Forms.ImageList imageList1;
        private DevComponents.DotNetBar.ElementStyle elementStyle3;
        private DevComponents.DotNetBar.ElementStyle elementStyle4;
        private DevComponents.DotNetBar.ElementStyle elementStyle5;
        private DevComponents.DotNetBar.ElementStyle elementStyle2;
        private System.Windows.Forms.TreeView treeViewModel;
        private System.Windows.Forms.TreeView treeViewRoute;
        private System.Windows.Forms.TreeView treeViewProcess;
        private System.Windows.Forms.TreeView treeViewPdline;
        private System.Windows.Forms.TreeView treeViewConfig;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem collapseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem delToolStripMenuItem;


    }
}