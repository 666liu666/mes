using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Data.OracleClient;
using System.Threading;
using System.Runtime.CompilerServices;
using ExportExcel;
using System.Globalization;
using System.Linq;
using System.IO;

namespace MOPassQtyQuery
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
            public ParamsData(string modelId, string pdlineId, string processId, string workOrder, string startDate, string startTime, string endDate, string endTime, string routeid)
            {
                this.Model_id = modelId;
                this.pdline_id = pdlineId;
                this.Process_id = processId;
                this.Work_order = workOrder;
                this.Startdate = startDate;
                this.Enddate = endDate;
                this.Start_time = startTime;
                this.End_time = endTime;
                this.Route_id = routeid;
            }

            private string startTime;
            public string Start_time
            {
                get { return startTime; }
                set { startTime = value; }
            }

            private string endTime;
            public string End_time
            {
                get { return endTime; }
                set { endTime = value; }
            }


            private string route_id;
            public string Route_id
            {
                get { return route_id; }
                set { route_id = value; }
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
            public string Work_order
            {
                get { return work_order; }
                set { work_order = value; }
            }

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

            private string startdate;
            public string Startdate
            {
                get { return startdate; }
                set { startdate = value; }
            }

            private string enddate;
            public string Enddate
            {
                get { return enddate; }
                set { enddate = value; }
            }

        };



        #region WIP
        string startSqlStr0 = "SELECT g.* FROM (  SELECT E.MODEL_NAME,A.WORK_ORDER,A.SERIAL_NUMBER,c.pdline_name,D.PROCESS_NAME,A.WIP_QTY,B.PART_NO,"
                  + " A.OUT_PROCESS_TIME,"
                  + " D.PROCESS_ID, "
                  + " B.PART_ID,A.ROUTE_ID "
             + " FROM sajet.g_sn_status a, "
                  + " SAJET.SYS_PART b,"
                  + " SAJET.SYS_PDLINE c,"
                  + " SAJET.SYS_PROCESS d,"
                  + " SAJET.SYS_MODEL e "
           + "  WHERE     A.PART_ID = B.PART_ID  "
                  + " AND a.wip_process <> 0"
                  + " AND A.WIP_PROCESS = d.process_id "
                  + " AND A.PDLINE_ID = C.PDLINE_ID "
                  + " AND B.MODEL_ID = E.MODEL_ID";

        /*string startSqlStr1 =
                     " SELECT B.PDLINE_NAME,"
                     + "C.PROCESS_NAME,"
                     + "A.WORK_ORDER MO, "
                     + "D.TARGET_QTY MO_QTY,"
                     + "SUM (PASS_CNT) PASS_QTY "
                + "FROM SAJET.G_SN_STATUS A,"
                     + "SAJET.SYS_PDLINE B,"
                     + "SAJET.SYS_PROCESS C,"
                     + "SAJET.G_WO_BASE D,"
                     + "SAJET.SYS_PART E,"
                     + "SAJET.SYS_MODEL F, "
                     + "SAJET.SYS_ROUTE G "
               + "WHERE     A.PDLINE_ID = B.PDLINE_ID "
                     + "AND A.PROCESS_ID = C.PROCESS_ID "
                     + "AND A.WORK_ORDER = D.WORK_ORDER "
                     + "AND A.PART_ID = E.PART_ID "
                     + "AND F.MODEL_ID = E.MODEL_ID "
                     + "AND G.ROUTE_ID = D.ROUTE_ID";

        string endSqlStr1 = " GROUP BY "
                               + "A.WORK_ORDER,"
                               + "B.PDLINE_NAME,"
                               + "C.PROCESS_NAME,"
                               + "D.TARGET_QTY";*/

        string startSqlStr1 =
                 " SELECT B.PDLINE_NAME,"
                + " C.PROCESS_NAME,"
                + " A.WORK_ORDER MO,"
                + " D.TARGET_QTY MO_QTY,"
                + " SUM (A.PASS_QTY) PASS_QTY"
           + " FROM SAJET.G_SN_COUNT A,"
                + " SAJET.SYS_PDLINE B,"
                + " SAJET.SYS_PROCESS C,"
                + " SAJET.G_WO_BASE D,"
                + " SAJET.SYS_PART E,"
                + " SAJET.SYS_MODEL F"
           + " WHERE  A.PDLINE_ID = B.PDLINE_ID "
                + " AND A.PROCESS_ID = C.PROCESS_ID "
                + " AND A.WORK_ORDER = D.WORK_ORDER"
                + " AND E.PART_ID = D.PART_ID"
                + " AND E.MODEL_ID = F.MODEL_ID ";

        string endSqlStr1 = "GROUP BY B.PDLINE_NAME,"
               + "  C.PROCESS_NAME,"
               + "  A.WORK_ORDER,"
               + "  D.TARGET_QTY ORDER BY A.WORK_ORDER";

        // AND Y.WORK_ORDER='FL104-170600377' "
        //+" AND Y.PDLINE_NAME='A8-3F-FA-L02' AND Y.OUT_PROCESS_TIME>=TO_DATE('20170630','YYYYMMDD') "
        //+" AND Y.ROUTE_ID IN (SELECT C1.ROUTE_ID FROM SAJET.SYS_MODEL A1, SAJET.SYS_PART B1, SAJET.G_WO_BASE C1 "
        //+" WHERE A1.MODEL_ID = B1.MODEL_ID AND B1.PART_ID = C1.PART_ID AND A1.MODEL_NAME = 'H06 ASSY')"
        #endregion




        /// <summary>

        /// <summary>
        /// 获取机种数据
        /// </summary>
        /// <returns>机种ID+机种名称</returns>
        private DataTable GetModelData()
        {
            DataTable dt = null;
            string strSql = "SELECT MODEL_ID,MODEL_NAME FROM SAJET.SYS_MODEL WHERE MODEL_ID IN(SELECT MODEL_ID FROM SAJET.SYS_GROUP_MODEL WHERE GROUP_ID IN (SELECT GROUP_ID FROM SAJET.SYS_GROUP_EMP WHERE EMP_ID='" + ClientUtils.UserPara1 + "')) ORDER BY MODEL_NAME";
            DataSet ds = ClientUtils.ExecuteSQL(strSql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                DataRow dr = dt.NewRow();
                dr["model_id"] = "0";
                dr["model_name"] = "--请选择--";
                dt.Rows.InsertAt(dr, 0);
            }
            return dt;
        }

        /// <summary>
        /// 获取线别数据
        /// </summary>
        /// <returns>线别ID+线别名称</returns>
        private DataTable GetPdlineData()
        {
            DataTable dt = null;
            string strSql = "SELECT PDLINE_ID,PDLINE_NAME FROM SAJET.SYS_PDLINE WHERE ENABLED='Y' ORDER BY PDLINE_NAME ";
            DataSet ds = ClientUtils.ExecuteSQL(strSql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                DataRow dr = dt.NewRow();
                dr["pdline_id"] = "0";
                dr["pdline_name"] = "--请选择--";
                dt.Rows.InsertAt(dr, 0);
            }
            return dt;
        }

        /// <summary>
        /// 获取制程数据
        /// </summary>
        /// <returns>制程ID+制程名称</returns>
        private DataTable GetProcessData()
        {
            DataTable dt = null;
            string strSql = "SELECT PROCESS_ID,PROCESS_NAME FROM SAJET.SYS_PROCESS WHERE ENABLED='Y' ORDER BY PROCESS_NAME";
            DataSet ds = ClientUtils.ExecuteSQL(strSql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                /*DataRow dr = dt.NewRow();
                dr["process_id"] = "0";
                dr["process_name"] = "--请选择--";
                dt.Rows.InsertAt(dr, 0);*/
            }
            return dt;
        }

        /// <summary>
        /// 获取流程数据
        /// </summary>
        /// <returns>流程ID+流程名称</returns>
        private DataTable GetRouteData()
        {
            DataTable dt = null;
            string strSql = "SELECT ROUTE_ID,ROUTE_NAME FROM SAJET.SYS_ROUTE WHERE ENABLED='Y' ORDER BY ROUTE_NAME";
            DataSet ds = ClientUtils.ExecuteSQL(strSql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                DataRow dr = dt.NewRow();
                dr["ROUTE_ID"] = "0";
                dr["ROUTE_NAME"] = "--请选择--";
                dt.Rows.InsertAt(dr, 0);
            }
            return dt;
        }

        //临时存放机种数据，方便后续使用
        Dictionary<string, string> modelData = new Dictionary<string, string>();
        //临时存放线别数据，方便后续使用
        Dictionary<string, string> pdlineData = new Dictionary<string, string>();
        //临时存放制程数据，方便后续使用
        Dictionary<string, string> processData = new Dictionary<string, string>();
        //临时存放流程数据，方便后续使用
        Dictionary<string, string> routeData = new Dictionary<string, string>();

        //存放良率报表数据，方便后续使用
        DataTable mainDataTable = new DataTable();

        DataTable tDataTable = new DataTable();

        //存放不良数量报表数据，方便后续使用
        DataTable defectStatisticsDataTable = new DataTable();

        //存放详细报表数据，方便后续使用
        DataTable detailDataTable = new DataTable();

        private void fMain_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + " (" + SajetCommon.g_sFileVersion + ")";
            cmbStartTime.SelectedIndex = 0;
            cmbEndTime.SelectedIndex = 23;
            DataTable dtModel = GetModelData();
            if (dtModel != null)
            {
                cmbModle.DataSource = dtModel;
                cmbModle.DisplayMember = "MODEL_NAME";
                cmbModle.ValueMember = "MODEL_ID";

                foreach (DataRow dr in dtModel.Rows)
                {
                    modelData.Add(dr[1].ToString(), dr[0].ToString());
                }


            }


            DataTable dtPdline = GetPdlineData();
            if (dtPdline != null)
            {
                cmbPdline.DataSource = dtPdline;
                cmbPdline.DisplayMember = "PDLINE_NAME";
                cmbPdline.ValueMember = "PDLINE_ID";
                foreach (DataRow dr in dtPdline.Rows)
                {
                    pdlineData.Add(dr[1].ToString(), dr[0].ToString());
                }
            }

            DataTable dtProcess = GetProcessData();
            if (dtProcess != null)
            {
                mProcessID.DataSource = dtProcess;
                mProcessID.DisplayMember = "PROCESS_NAME";
                mProcessID.ValueMember = "PROCESS_ID";

                foreach (DataRow dr in dtProcess.Rows)
                {
                    processData.Add(dr[1].ToString(), dr[0].ToString());
                }
            }

            DataTable dtRoute = GetRouteData();
            if (dtRoute != null)
            {
                cmbRoute.DataSource = dtRoute;
                cmbRoute.DisplayMember = "ROUTE_NAME";
                cmbRoute.ValueMember = "ROUTE_ID";
                foreach (DataRow dr in dtRoute.Rows)
                {
                    routeData.Add(dr[1].ToString(), dr[0].ToString());
                }
            }

        }


        /// <summary>
        /// 获取良率报表数据
        /// </summary>
        /// <param name="p">用户输入数据</param>
        /// <param name="queryType">查询类型 1</param>
        /// <returns>良率数据</returns>
        private DataTable GetMainReportData(ParamsData p, int queryType)
        {
            DataTable dt = new DataTable();
            object[][] Params = null;
            string sql = "";
            sql = startSqlStr1;

            if (!string.IsNullOrEmpty(p.Work_order) && p.Work_order != "0")
            {
                sql = sql + " AND A.WORK_ORDER='" + p.Work_order + "' ";
            }

            if (!string.IsNullOrEmpty(p.Pdline_id) && p.Pdline_id != "0")
            {
                sql = sql + " AND B.PDLINE_ID='" + p.Pdline_id + "'";
            }

            if (!string.IsNullOrEmpty(p.Process_id) && p.Process_id != "0")
            {
                sql = sql + " AND A.PROCESS_ID IN (" + p.Process_id + ")";
            }

            if (!string.IsNullOrEmpty(p.Startdate) && p.Startdate != "0" && !string.IsNullOrEmpty(p.Enddate) && p.Enddate != "0")
            {
                sql = sql + " AND  A.WORK_DATE|| LPAD(A.WORK_TIME,2,'0') >='" + p.Startdate + p.Start_time + "' AND  A.WORK_DATE|| LPAD(A.WORK_TIME,2,'0') <'" + p.Enddate + p.End_time + "' ";
                //sql = sql + " AND A.WORK_TIME>='" + p.Start_time + "' AND A.WORK_TIME<'" + p.End_time + "' ";
            }

            if (!string.IsNullOrEmpty(p.Route_id) && p.Route_id != "0")
            {
                sql = sql + " AND E.ROUTE_ID='" + p.Route_id + "'";
            }

            if (!string.IsNullOrEmpty(p.Model_id) && p.Model_id != "0")
            {
                sql = sql + "  AND F.MODEL_ID = '" + p.Model_id + "'";
            }
            else
            {

            }
            sql = sql + endSqlStr1;
            //MessageBox.Show(sql);
            DataSet ds = ClientUtils.ExecuteSQL(sql, Params);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }

            return dt;
        }




        /// <summary>
        /// 获取良率报表数据
        /// </summary>
        /// <param name="p">用户输入数据</param>
        /// <param name="queryType">查询类型 1</param>
        /// <returns>良率数据</returns>
        /*private DataTable GetReportData(ParamsData p, string tmodel, string tpdline, string tprocess, string twip, int queryType)
        {

            DataTable dt = null;

            object[][] Params = null;
            string sql = "";
            sql = startSqlStr1;

            if (!string.IsNullOrEmpty(p.Work_order) && p.Work_order != "0")
            {
                sql = sql + "AND A.WORK_ORDER='" + p.Work_order + "' ";
            }

            if (!string.IsNullOrEmpty(p.Pdline_id) && p.Pdline_id != "0")
            {
                sql = sql + " AND C.PDLINE_ID='" + p.Pdline_id + "'";
            }

            if (!string.IsNullOrEmpty(p.Process_id) && p.Process_id != "0")
            {
                sql = sql + " AND D.PROCESS_ID='" + p.Process_id + "'";
            }

            if (!string.IsNullOrEmpty(p.Startdatetime) && p.Startdatetime != "0" && !string.IsNullOrEmpty(p.Enddatetime) && p.Enddatetime != "0")
            {
                sql = sql + " AND A.OUT_PROCESS_TIME>=to_date('" + p.Startdatetime + "','YYYYMMDDHH24') AND A.OUT_PROCESS_TIME<TO_DATE('" + p.Enddatetime + "','YYYYMMDDHH24') ";
            }

            if (!string.IsNullOrEmpty(p.Model_id) && p.Model_id != "0")
            {
                sql = sql + "  AND E.MODEL_ID = '" + p.Model_id + "'";
            }


            if (!string.IsNullOrEmpty(tpdline) && tpdline != "0")
            {
                sql = sql + " AND C.PDLINE_NAME='" + tpdline + "'";
            }
            if (!string.IsNullOrEmpty(tprocess) && tprocess != "0")
            {
                sql = sql + " AND D.PROCESS_NAME='" + tprocess + "'";
            }
            else
            {

            }
            sql = sql + endSqlStr1;
            DataSet ds = ClientUtils.ExecuteSQL(sql, Params);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }

            return dt;
        }
        */


        /// <summary>


        //保存用户实际输入数据
        ParamsData paramsdata = null;

        private void btnQuery_Click(object sender, EventArgs e)
        {
            btnQuery.Text = "正在查询";
            btnQuery.Enabled = false;
            if (cmbModle.Text == "--请选择--")
            {
                MessageBox.Show("机种不能为空！", "消息提示:", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnQuery.Enabled = true;
                btnQuery.Text = "查询";
                return;
            }


            if (cmbModle.Items.Count == 0)
            {
                MessageBox.Show("员工机种没有设定！", "消息提示:", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnQuery.Enabled = true;
                btnQuery.Text = "查询";
                return;
            }

            int CheckCount = 0;
            for (int i = 0; i < mProcessID.Items.Count; i++)
            {
                if (mProcessID.GetItemChecked(i))
                {
                    CheckCount += 1;
                }

            }
            if (CheckCount <= 0)
            {
                MessageBox.Show("制程不能为空！", "消息提示:", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnQuery.Enabled = true;
                btnQuery.Text = "查询";
                return;
            }


            if (mProcessID.Items.Count == 0)
            {
                MessageBox.Show("员工制程没有设定！", "消息提示:", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnQuery.Enabled = true;
                btnQuery.Text = "查询";
                return;
            }

            string Str = string.Empty;
            for (int i = 0; i < mProcessID.Items.Count; i++)
            {
                if (mProcessID.GetItemChecked(i))
                {
                    mProcessID.SelectedIndex = i;
                    //利用SelectedValue取得Value值时，只能取得当前焦点项的值。所以要对整个CheckedListBox中的所有勾选项,让其都做一次焦点项才能取得所有勾选的项的值。
                    Str += "'" + mProcessID.SelectedValue + "',";
                }
            }

            //MessageBox.Show(Str.TrimEnd(','));
            paramsdata = new ParamsData(cmbModle.SelectedValue.ToString(), cmbPdline.SelectedValue.ToString(), Str.TrimEnd(','), txtWorkOrder.Text.Trim(), dtpStartDate.Value.Date.ToString("yyyyMMdd"), cmbStartTime.Text, dtpEndDate.Value.Date.ToString("yyyyMMdd"), cmbEndTime.Text, cmbRoute.SelectedValue.ToString());
            mainDataTable = GetMainReportData(paramsdata, 1);
            dgvMainTable.DataSource = mainDataTable;
            btnQuery.Enabled = true;
            btnQuery.Text = "查询";
            return;

        }



        private void 导出数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "Excel文件(*.csv)|*.csv|所有文件(*.*)|*.*";
            sfd.Filter = "Excel文件(*.xls)|*.xls|所有文件(*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.Title = "導出到Excel";
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
                MessageBox.Show("導出數據成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }
        }

    }
}
