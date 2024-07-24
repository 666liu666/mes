using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using System.Xml.Linq;
using DevComponents;
using System.Xml;

namespace QueryReport
{
    public partial class fDataConfig :DevComponents.DotNetBar.Office2007Form
    {
        public fDataConfig()
        {
            InitializeComponent();
        }


        private void BindingTreeView(DataTable dt, TreeView advtree,string roottext,string roottype,int imgindex)
        {
            TreeNode rootNode = new TreeNode();
            rootNode.Text = roottext;
            rootNode.Name = roottype;
            rootNode.Tag = "0";
            rootNode.ImageIndex = imgindex;
            foreach (DataRow itemrow in dt.Rows) 
            {
                TreeNode newTreeNode = new TreeNode();
                newTreeNode.Text = itemrow[1].ToString();
                newTreeNode.Name = roottype;
                newTreeNode.Tag = itemrow[0].ToString();
                newTreeNode.ImageIndex = imgindex;
                rootNode.Nodes.Add(newTreeNode);
            }
            advtree.Nodes.Add(rootNode);
        }

        private void BindingTreeView(string xmlfilepath, TreeView advtree)
        {

            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(xmlfilepath);
            XmlNodeList nodelist = xdoc.SelectNodes("Models");

            bindXmlToTreeView(nodelist, advtree.Nodes);


        }

        /// <summary>
        ///   绑定xml数据到treeview中区
        /// </summary>
        public void bindXmlToTreeView(XmlNodeList nodeList, TreeNodeCollection advtreeNode)
        {

            foreach (XmlNode node in nodeList)
            {
                XmlElement xe = (XmlElement)node;

                TreeNode newTreeNode = new TreeNode();

                switch (xe.Name) 
                {
                    case "Model": newTreeNode.ImageIndex = 0;  break;
                    case "Pdlines": newTreeNode.ImageIndex = 1;  break;
                    case "Pdline": newTreeNode.ImageIndex = 1; break;
                    case "Processs": newTreeNode.ImageIndex = 2; break;
                    case "Process": newTreeNode.ImageIndex = 2; break;
                    case "Routes": newTreeNode.ImageIndex = 3;  break;
                    case "Route": newTreeNode.ImageIndex = 3;  break;
                    default: break;
                }
                newTreeNode.Text = xe.GetAttribute("name");
                newTreeNode.Tag = xe.GetAttribute("id");
                newTreeNode.Name = xe.Name;
                advtreeNode.Add(newTreeNode);

                if (node.HasChildNodes)
                {
                    bindXmlToTreeView(node.ChildNodes, newTreeNode.Nodes);
                }
            }
        }

        private void fDataConfig_Load(object sender, EventArgs e)
        {
            BindingTreeView(CommData.dtModel, treeViewModel, "--所有机种--","Model", 0);
            BindingTreeView(CommData.dtPdline, treeViewPdline, "--所有线别--","Pdline", 1);
            BindingTreeView(CommData.dtProcess, treeViewProcess, "--所有制程--","Process", 2);
            BindingTreeView(CommData.dtRoute, treeViewRoute, "--所有工艺流程--","Route", 3);

            treeViewModel.ExpandAll();
            treeViewModel.Nodes[0].EnsureVisible();
            treeViewPdline.ExpandAll();
            treeViewPdline.Nodes[0].EnsureVisible();
            treeViewProcess.ExpandAll();
            treeViewProcess.Nodes[0].EnsureVisible();
            treeViewRoute.ExpandAll();
            treeViewRoute.Nodes[0].EnsureVisible();
            BindingTreeView(Application.StartupPath + @"\Query\ModelData.xml", treeViewConfig);
            treeViewConfig.ExpandAll();
            treeViewConfig.Nodes[0].EnsureVisible();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent=true;
            settings.NewLineOnAttributes=false;
            XmlWriter writer = XmlWriter.Create(Application.StartupPath + @"\Query\ModelData.xml", settings);
            writer.WriteStartElement("Models");
            writer.WriteAttributeString("name", "机种");
            WriteXmlFromTreeNods(writer, treeViewConfig.Nodes[0].Nodes);
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
            if (DialogResult.Yes== MessageBox.Show("保存成功,是否关闭？", "消息提示：", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) 
            {
                this.Close();
                this.Dispose();
            }
            
        }

        private void WriteXmlFromTreeNods(XmlWriter writer, TreeNodeCollection treenodes)
        {
            foreach (TreeNode item in treenodes) 
            {
                writer.WriteStartElement(item.Name);
                writer.WriteAttributeString("id", item.Tag.ToString());
                writer.WriteAttributeString("name", item.Text);
               
                if (item.Nodes.Count > 0) 
                {
                    WriteXmlFromTreeNods(writer, item.Nodes);
                }
                writer.WriteEndElement();
            }
        }


        private void treeViewModel_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeViewModel.SelectedNode.SelectedImageIndex = treeViewModel.SelectedNode.ImageIndex;
        }

        private void treeViewModel_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void treeViewModel_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeViewConfig_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeViewConfig.SelectedNode.SelectedImageIndex = treeViewConfig.SelectedNode.ImageIndex;
        }

        private void treeViewConfig_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode SrcNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode"); 
            if (SrcNode == null||SrcNode.Level==0)
            { return; }

            switch (SrcNode.Name) 
            {
                case "Model":
                    if (treeViewConfig.Nodes[0].Nodes.Cast<TreeNode>().Count(q => q.Tag.ToString() == SrcNode.Tag.ToString()) > 0) return;
                    treeViewConfig.Nodes[0].Nodes.Add((TreeNode)SrcNode.Clone());
                    break;
                case "Pdline":

                    if (treeViewConfig.SelectedNode.Level != 1) return;
                    TreeNode[] tPdlineNodes = treeViewConfig.SelectedNode.Nodes.Find("Pdlines",false);
                    TreeNode tPdlineNode = null;
                    if (tPdlineNodes.Length ==0) 
                    {
                        TreeNode tPdlinesNodeTemp = new TreeNode();
                        tPdlinesNodeTemp.Text = "线别";
                        tPdlinesNodeTemp.Tag = "0";
                        tPdlinesNodeTemp.Name = "Pdlines";
                        tPdlinesNodeTemp.ImageIndex = 1;
                        tPdlinesNodeTemp.SelectedImageIndex = tPdlinesNodeTemp.ImageIndex;

                        TreeNode tAllLineNode = new TreeNode();
                        tAllLineNode.Text = "--所有线别--";
                        tAllLineNode.Tag = "0";
                        tAllLineNode.Name = "Pdline";
                        tAllLineNode.ImageIndex = 1;
                        tAllLineNode.SelectedImageIndex = tAllLineNode.ImageIndex;
                        tPdlinesNodeTemp.Nodes.Add(tAllLineNode);
                        treeViewConfig.SelectedNode.Nodes.Add(tPdlinesNodeTemp);
                        tPdlineNode = tPdlinesNodeTemp;
                    }
                    else 
                    {
                        tPdlineNode = tPdlineNodes[0];
                    }

                    if (tPdlineNode.Nodes.Cast<TreeNode>().Count(q => q.Tag == SrcNode.Tag) > 0) return;

                    
                    TreeNode tpdlinenode=(TreeNode)SrcNode.Clone();
                    tPdlineNode.Nodes.Add(tpdlinenode);
            
                    break;
                case "Process":

                     if (treeViewConfig.SelectedNode.Level != 1) return;
                    TreeNode[] tProcessNodes = treeViewConfig.SelectedNode.Nodes.Find("Processs",false);
                    TreeNode tProcessNode = null;
                    if (tProcessNodes.Length == 0) 
                    {
                        TreeNode tProcessNodeTemp = new TreeNode();
                        tProcessNodeTemp.Text = "制程";
                        tProcessNodeTemp.Tag = "0";
                        tProcessNodeTemp.Name = "Processs";
                        tProcessNodeTemp.ImageIndex = 1;
                        tProcessNodeTemp.SelectedImageIndex = tProcessNodeTemp.ImageIndex;

                        TreeNode tAllProcessNode = new TreeNode();
                        tAllProcessNode.Text = "--所有制程--";
                        tAllProcessNode.Tag = "0";
                        tAllProcessNode.Name = "Process";
                        tAllProcessNode.ImageIndex = 1;
                        tAllProcessNode.SelectedImageIndex = tAllProcessNode.ImageIndex;
                        tProcessNodeTemp.Nodes.Add(tAllProcessNode);

                        treeViewConfig.SelectedNode.Nodes.Add(tProcessNodeTemp);
                        tProcessNode = tProcessNodeTemp; ;
                    }
                    else 
                    {
                        tProcessNode = tProcessNodes[0];
                    }

                    if (tProcessNode.Nodes.Cast<TreeNode>().Count(q => q.Tag == SrcNode.Tag) > 0) return;
                    TreeNode tprocessnode = (TreeNode)SrcNode.Clone();
                    tProcessNode.Nodes.Add(tprocessnode);
                    break;
                case "Route":
                     if (treeViewConfig.SelectedNode.Level != 1) return;
                    TreeNode[] tRouteNodes = treeViewConfig.SelectedNode.Nodes.Find("Routes",false);
                    TreeNode tRouteNode = null;
                    if (tRouteNodes.Length == 0) 
                    {
                        TreeNode tRouteNodeTemp = new TreeNode();
                        tRouteNodeTemp.Text = "工艺流程";
                        tRouteNodeTemp.Tag = "0";
                        tRouteNodeTemp.Name = "Routes";
                        tRouteNodeTemp.ImageIndex = 1;
                        tRouteNodeTemp.SelectedImageIndex = tRouteNodeTemp.ImageIndex;

                        TreeNode tAllRouteNode = new TreeNode();
                        tAllRouteNode.Text = "--所有工艺流程--";
                        tAllRouteNode.Tag = "0";
                        tAllRouteNode.Name = "Route";
                        tAllRouteNode.ImageIndex = 1;
                        tAllRouteNode.SelectedImageIndex = tAllRouteNode.ImageIndex;
                        tRouteNodeTemp.Nodes.Add(tAllRouteNode);


                        treeViewConfig.SelectedNode.Nodes.Add(tRouteNodeTemp);
                        tRouteNode = tRouteNodeTemp; ;
                    }
                    else 
                    {
                        tRouteNode = tRouteNodes[0];
                    }

                    if (tRouteNode.Nodes.Cast<TreeNode>().Count(q => q.Tag == SrcNode.Tag) > 0) return;

                    tRouteNode.Nodes.Add((TreeNode)SrcNode.Clone());
                    break;
                default: break;
            }


        }

        private void treeViewConfig_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void treeViewConfig_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeViewPdline_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeViewPdline.SelectedNode.SelectedImageIndex = treeViewPdline.SelectedNode.ImageIndex;
        }

        private void treeViewPdline_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void treeViewPdline_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeViewProcess_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeViewProcess.SelectedNode.SelectedImageIndex = treeViewProcess.SelectedNode.ImageIndex;
        }

        private void treeViewProcess_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void treeViewProcess_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeViewRoute_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeViewRoute.SelectedNode.SelectedImageIndex = treeViewRoute.SelectedNode.ImageIndex;
        }

        private void treeViewRoute_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void treeViewRoute_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }
        TreeView currTreeView = null;
        private void expandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currTreeView.ExpandAll();
            currTreeView.Nodes[0].EnsureVisible();
        }

        private void collapseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currTreeView.CollapseAll();
        }


        private void delToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currTreeView.SelectedNode.Level == 0) return;
            currTreeView.SelectedNode.Remove();
        }

        private void treeViewConfig_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                currTreeView = treeViewConfig;
                delToolStripMenuItem.Visible = true;
                currTreeView.SelectedNode = currTreeView.GetNodeAt(e.X, e.Y);
            }

        }

        private void treeViewModel_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                currTreeView = treeViewModel;
                delToolStripMenuItem.Visible = false;
                currTreeView.SelectedNode = currTreeView.GetNodeAt(e.X, e.Y);
            }
        }

        private void treeViewPdline_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                currTreeView = treeViewPdline;
                delToolStripMenuItem.Visible = false;
                currTreeView.SelectedNode = currTreeView.GetNodeAt(e.X, e.Y);
            }
        }

        private void treeViewProcess_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                currTreeView = treeViewProcess;
                delToolStripMenuItem.Visible = false;
                currTreeView.SelectedNode = currTreeView.GetNodeAt(e.X, e.Y);
            }
        }

        private void treeViewRoute_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                currTreeView = treeViewRoute;
                delToolStripMenuItem.Visible = false;
                currTreeView.SelectedNode = currTreeView.GetNodeAt(e.X, e.Y);
            }
        }


      



    }
}
