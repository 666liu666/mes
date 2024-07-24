using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Linq;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;

namespace UpLevelQueryByKeyparts
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }

        //用于存放用户选择和填入的数据类
        public class ParamsData
        {

            public ParamsData(string modelId, string pdlineId, string processId, string workOrder, string startDatetime, string endDatetime)
            {
                this.Model_id = modelId;
                this.pdline_id = pdlineId;
                this.Process_id = processId;
                this.Work_order = workOrder;
                this.Startdatetime = startDatetime;
                this.Enddatetime = endDatetime;
            }
            private string model_id;

            public string Model_id
            {
                get { return model_id; }
                set { model_id = value; }
            }
            private string pdline_id;

            public string Pdline_id
            {
                get { return pdline_id; }
                set { pdline_id = value; }
            }
            private string process_id;

            public string Process_id
            {
                get { return process_id; }
                set { process_id = value; }
            }
            private string work_order;

            private string terminal_id;

            public string Terminal_id
            {
                get { return terminal_id; }
                set { terminal_id = value; }
            }
            private string defect_id;

            public string Defect_id
            {
                get { return defect_id; }
                set { defect_id = value; }
            }

            public string Work_order
            {
                get { return work_order; }
                set { work_order = value; }
            }
            private string startdatetime;

            public string Startdatetime
            {
                get { return startdatetime; }
                set { startdatetime = value; }
            }
            private string enddatetime;

            public string Enddatetime
            {
                get { return enddatetime; }
                set { enddatetime = value; }
            }

        };


        #region WIP
        string startSqlStr1 =
                            " SELECT A.WORK_ORDER,"
                           + " A.SERIAL_NUMBER H06_FATP_SN,"
                           + " C.PROCESS_NAME,"
                           + " D.PDLINE_NAME,"
                           + " A.OUT_PROCESS_TIME,"
                           + " B.ITEM_PART_SN"
                      + " FROM SAJET.G_SN_STATUS A,"
                           + " SAJET.G_SN_KEYPARTS B,"
                           + " SAJET.SYS_PROCESS C,"
                           + " SAJET.SYS_PDLINE D"
                     + " WHERE     A.SERIAL_NUMBER = B.SERIAL_NUMBER"
                           + " AND A.PROCESS_ID = C.PROCESS_ID"
                           + " AND A.PDLINE_ID = D.PDLINE_ID ";
        // " AND B.ITEM_PART_SN LIKE 'LNQG027N3007090%'";

        string startSqlStr2 =
                               " SELECT A.WORK_ORDER,"
                           + " A.SERIAL_NUMBER H07_MLB_SN, "
                           + " A.CUSTOMER_SN,"
                           + " A.PALLET_NO,"
                           + " A.CARTON_NO,"
                           + " I.LOT_NO,"
                           + " I.DATECODE,"
                           + " F.REEL_NO,"
                           + " G.VENDOR_NAME,"
                           + " C.PROCESS_NAME,"
                           + " D.PDLINE_NAME,"
                           + " A.OUT_PROCESS_TIME,"
                           + " B.ITEM_PART_SN H06_FATP_SN,"
                           + " B.ITEM_GROUP,"
                           + " E.ITEM_PART_SN "
                      + " FROM SAJET.G_SN_STATUS A,"
                           + " SAJET.G_SN_KEYPARTS B,"
                           + " SAJET.G_SN_KEYPARTS E,"
                           + " SAJET.SYS_PROCESS C,"
                           + " SAJET.SYS_PDLINE D,"
                           + " SAJET.G_SMT_KEYPARTS J,"
                           + " SAJET.G_MATERIAL F,"
                           + " SAJET.SYS_VENDOR G,"
                           + " SMT.G_SMT_SN_MAP  I"
                     + " WHERE     A.SERIAL_NUMBER = B.SERIAL_NUMBER"
                           + " AND B.ITEM_PART_SN = E.SERIAL_NUMBER"
                           + " AND A.PROCESS_ID = C.PROCESS_ID"
                           + " AND A.PDLINE_ID = D.PDLINE_ID"
                           + " AND B.SERIAL_NUMBER = J.ITEM_PART_SN"
                           + " AND G.VENDOR_ID = F.VENDOR_ID"
                           + " AND I.SERIAL_NUMBER = J.SERIAL_NUMBER "
                           + " AND I.REEL_NO = F.REEL_NO";
        //+ " AND E.ITEM_PART_SN LIKE 'LNQ%'"

        string startSqlStr3 =
                             " SELECT A.WORK_ORDER,"
                           + " A.SERIAL_NUMBER H06_FATP_SN,"
                           + " C.PROCESS_NAME,"
                           + " D.PDLINE_NAME,"
                           + " A.OUT_PROCESS_TIME,"
                           + " B.PART_SN"
                      + " FROM SAJET.G_SN_STATUS A,"
                           + " SAJET.G_SN_PARTSN_MULTIMAPPING B,"
                           + " SAJET.SYS_PROCESS C,"
                           + " SAJET.SYS_PDLINE D"
                     + " WHERE     A.SERIAL_NUMBER = B.SERIAL_NUMBER"
                           + " AND B.PART_TYPE=3"
                           + " AND A.PROCESS_ID = C.PROCESS_ID"
                           + " AND A.PDLINE_ID = D.PDLINE_ID";
        //+ " AND B.PART_SN LIKE '060006090001HB2%'"
        string startSqlStr4 =
                               " SELECT A.WORK_ORDER,"
                             + " A.SERIAL_NUMBER H07_MLB_SN, "
                             + " A.CUSTOMER_SN,"
                             + " A.PALLET_NO,"
                             + " A.CARTON_NO,"
                             + " I.LOT_NO,"
                             + " I.DATECODE,"
                             + " F.REEL_NO,"
                             + " G.VENDOR_NAME,"
                             + " C.PROCESS_NAME,"
                             + " D.PDLINE_NAME,"
                             + " A.OUT_PROCESS_TIME,"
                             + " B.ITEM_PART_SN H06_FATP_SN,"
                             + " B.ITEM_GROUP,"
                             + " E.PART_SN "
                       + " FROM SAJET.G_SN_STATUS A,"
                             + " SAJET.G_SN_KEYPARTS B,"
                             + " SAJET.G_SN_PARTSN_MULTIMAPPING E,"
                             + " SAJET.SYS_PROCESS C,"
                             + " SAJET.SYS_PDLINE D ,"
                             + " SAJET.G_SMT_KEYPARTS J,"
                             + " SAJET.G_MATERIAL F,"
                             + " SAJET.SYS_VENDOR G,"
                             + " SMT.G_SMT_SN_MAP  I"
                       + " WHERE     A.SERIAL_NUMBER = B.SERIAL_NUMBER"
                            + " AND B.ITEM_PART_SN = E.SERIAL_NUMBER"
                            + " AND E.PART_TYPE=3"
                            + " AND A.PROCESS_ID = C.PROCESS_ID"
                            + " AND A.PDLINE_ID = D.PDLINE_ID"
                            + " AND B.SERIAL_NUMBER = J.ITEM_PART_SN"
                            + " AND G.VENDOR_ID = F.VENDOR_ID"
                            + " AND I.SERIAL_NUMBER = J.SERIAL_NUMBER "
                            + " AND I.REEL_NO = F.REEL_NO";
        //+ " AND E.PART_SN LIKE '060006090001HB2%'"


        string endSqlStr1 = " ) g,  (SELECT NEXT_PROCESS_ID, SEQ, ROUTE_ID "
          + "  FROM (SELECT A1.NEXT_PROCESS_ID, A1.SEQ, A1.ROUTE_ID "
           + "         FROM SAJET.SYS_ROUTE_DETAIL A1  "
           + "        WHERE A1.SEQ = A1.STEP)   "
         + " UNION   "
          + "SELECT DISTINCT (A2.NEXT_PROCESS_ID), 0 AS SEQ, A2.ROUTE_ID  "
          + "  FROM SAJET.SYS_ROUTE_DETAIL A2  "
         + "  WHERE A.NEXT_PROCESS_ID = '100003') h WHERE     g.ROUTE_ID = h.route_id  AND g.process_id = h.NEXT_PROCESS_ID  ORDER BY h.seq ASC";
        #endregion

        private void fMain_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + " (" + SajetCommon.g_sFileVersion + ")";
        }
        

        private DataTable GetH06LikeData()
        {
            DataTable dt = new DataTable();
            object[][] Params = null;

            string sql = "";
            sql = startSqlStr3;
            sql = sql + " AND B.PART_SN LIKE '" + tbSN.Text.Trim() + "%' ORDER BY A.WORK_ORDER";

            DataSet ds = ClientUtils.ExecuteSQL(sql, Params);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }

            return dt;
        }

        private DataTable GetH07LikeData()
        {
            DataTable dt = new DataTable();
            object[][] Params = null;

            string sql = "";
            sql = startSqlStr4;
            sql = sql + " AND E.PART_SN LIKE '" + tbSN.Text.Trim() + "%' ORDER BY A.WORK_ORDER";
            //MessageBox.Show(sql);
            DataSet ds = ClientUtils.ExecuteSQL(sql, Params);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }

            return dt;
        }

        private DataTable GetH06SNData()
        {
            DataTable dt = new DataTable();
            object[][] Params = null;
            string SN = "";
            string ssn = "";

            foreach (string str in lstSN.Items)
            {
                ssn = "'" + str.Trim() + "'" + ",";
                SN = SN + ssn.Trim();
            }

            string sql = "";
            sql = startSqlStr1;
            sql = sql + " AND B.ITEM_PART_SN In (" + SN.TrimEnd(',') + ") ORDER BY A.WORK_ORDER ";

            DataSet ds = ClientUtils.ExecuteSQL(sql, Params);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }

            return dt;
        }

        private DataTable GetH07SNData()
        {
            DataTable dt = new DataTable();
            object[][] Params = null;
            string SN = "";
            string ssn = "";

            foreach (string str in lstSN.Items)
            {
                ssn = "'" + str.Trim() + "'" + ",";
                SN = SN + ssn.Trim();
            }

            string sql = "";
            sql = startSqlStr2;
            sql = sql + " AND E.ITEM_PART_SN In (" + SN.TrimEnd(',') + ") ORDER BY A.WORK_ORDER";

            DataSet ds = ClientUtils.ExecuteSQL(sql, Params);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }

            return dt;
        }


        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                btnQuery.Text = "正在查询";
                btnQuery.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                if ((rbHousing.Checked) || (rbCAP.Checked) || (rbDriver.Checked))
                {
                    if (tbSN.Text.Trim() != string.Empty)
                    {
                        dgvMainTable.DataSource = GetH06LikeData();
                        dgvMainTable2.DataSource = GetH07LikeData();
                    }
                    else
                    {
                        MessageBox.Show("请输入Housing或CAP或Driver编码");
                        tbSN.Focus();
                    }
                }
                
                if ((rbBattery.Checked) || (rbMLB.Checked) || (rbAntenna.Checked))
                {
                    dgvMainTable.DataSource = GetH06SNData();
                    dgvMainTable2.DataSource = GetH07SNData();
                }
            }
            finally
            {
                btnQuery.Enabled = true;
                btnQuery.Text = "查询";
                this.Cursor = Cursors.Default;
            }
            return;
        }

        private void 导出数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "Excel文件(*.csv)|*.csv|所有文件(*.*)|*.*";
            sfd.Filter = "Excel文件(*.xls)|*.xls|所有文件(*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.Title = "导出到Excel";
            sfd.InitialDirectory = Directory.GetCurrentDirectory();
            sfd.RestoreDirectory = false;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(sfd.FileName, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.Unicode);

                // 写标题
                string title = "";
                for (int i = 0; i < dgvMainTable.Columns.Count; i++)
                {
                    title = dgvMainTable.Columns[i].HeaderText;
                    sw.Write(title);
                    sw.Write("\t");
                }
                sw.WriteLine("");     //防止数据的第一行数据显示不出来
                // 写内容                    
                for (int i = 0; i < dgvMainTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvMainTable.Columns.Count; j++)
                    {
                        sw.Write(dgvMainTable.Rows[i].Cells[j].Value);
                        sw.Write("\t");
                    }
                    sw.WriteLine("");
                    sw.Flush();
                }

                fs.Close();

                this.Cursor = Cursors.Default;
                MessageBox.Show("导出数据成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }
        }

        private void 导出数据ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "Excel文件(*.csv)|*.csv|所有文件(*.*)|*.*";
            sfd.Filter = "Excel文件(*.xls)|*.xls|所有文件(*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.Title = "导出到Excel";
            sfd.InitialDirectory = Directory.GetCurrentDirectory();
            sfd.RestoreDirectory = false;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(sfd.FileName, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.Unicode);

                // 写标题
                string title = "";
                for (int i = 0; i < dgvMainTable2.Columns.Count; i++)
                {
                    title = dgvMainTable2.Columns[i].HeaderText;
                    sw.Write(title);
                    sw.Write("\t");
                }
                sw.WriteLine("");     //防止数据的第一行数据显示不出来
                // 写内容                    
                for (int i = 0; i < dgvMainTable2.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvMainTable2.Columns.Count; j++)
                    {
                        sw.Write(dgvMainTable2.Rows[i].Cells[j].Value);
                        sw.Write("\t");
                    }
                    sw.WriteLine("");
                    sw.Flush();
                }

                fs.Close();

                this.Cursor = Cursors.Default;
                MessageBox.Show("导出数据成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }
        }

        private void rbHousing_Click(object sender, EventArgs e)
        {
            //housing cap driver  模糊查询 H06 FATP SN状态
            tabItem1.Text = "Housing-H06 FATP SN 状态";
            tabItem2.Text = "Housing-H07 三合一后 SN 状态";
        }

        private void rbCAP_Click(object sender, EventArgs e)
        {
            tabItem1.Text = "CAP-H06 FATP SN 状态";
            tabItem2.Text = "CAP-H07 三合一后 SN 状态";
        }

        private void rbDriver_Click(object sender, EventArgs e)
        {
            tabItem1.Text = "Driver-H06 FATP SN 状态";
            tabItem2.Text = "Driver-H07 三合一后 SN 状态";
        }

        private void OpenExcel(string strFileName)
        {
            object missing = System.Reflection.Missing.Value;

            Excel.Application excel = new Excel.ApplicationClass();
            try
            {
                {
                    excel.Visible = false;
                    excel.UserControl = true;
                    // 以只读的形式打开EXCEL文件  
                    Excel.Workbook wb = excel.Application.Workbooks.Open(strFileName, missing, true, missing, missing, missing,
                     missing, missing, missing, true, missing, missing, missing, missing, missing);
                    //取得第一个工作薄  
                    Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets.get_Item(1);


                    //取得总记录行数   (包括标题列)  
                    int rowsint = ws.UsedRange.Cells.Rows.Count; //得到行数  


                    //取得数据范围区域 (不包括标题列)   
                    Excel.Range rng1 = ws.Cells.get_Range("A2", "A" + rowsint);   //item  

                    object[,] arryItem = (object[,])rng1.Value2;   //get range's value  

                    //将新值赋给一个数组  
                    string[,] arry = new string[rowsint - 1, 2];
                    lstSN.Items.Clear();
                    for (int i = 1; i <= rowsint - 1; i++)
                    {
                        //SN列  
                        lstSN.Items.Add(arryItem[i, 1].ToString());
                    }

                }
            }
            finally
            {
                excel.Quit();
                excel = null;

                Process[] procs = Process.GetProcessesByName("excel");

                foreach (Process pro in procs)
                {
                    pro.Kill();//没有更好的方法,只有杀掉进程  
                }
                GC.Collect();
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fd = new OpenFileDialog();  //打开文件对话框
            fd.Filter = @"Excel文件 (*.xls; *.xlsx)|*.xls; *.xlsx|All Files (*.*)|*.*";

            if (fd.ShowDialog() == DialogResult.OK)
            {
                string fileName = fd.FileName;
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    OpenExcel(fileName);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }

                lbCount.Text = "共" + lstSN.Items.Count.ToString() + "条";
            }
        }

        private void rbAntenna_Click(object sender, EventArgs e)
        {
            tabItem1.Text = "Antenna-H06 FATP SN 状态";
            tabItem2.Text = "Antenna-H07 三合一后 SN 状态";
        }

        private void rbMLB_Click(object sender, EventArgs e)
        {
            tabItem1.Text = "MLB-H06 FATP SN 状态";
            tabItem2.Text = "MLB-H07 三合一后 SN 状态";
        }

        private void rbBattery_Click(object sender, EventArgs e)
        {
            tabItem1.Text = "Battery-H06 FATP SN 状态";
            tabItem2.Text = "Battery-H07 三合一后 SN 状态";
        }


    }
}
