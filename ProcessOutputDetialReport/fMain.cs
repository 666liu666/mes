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

namespace ProcessOutputDetialReport
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




        #region 查询字符串
        //良率表
        string startSqlStr1 = "SELECT g.model_name ,b.pdline_name ,c.process_name ,SUM (A.PASS_QTY) PASS_QTY,SUM(A.FAIL_QTY) FAIL_QTY,SUM(REPASS_QTY) REPASS_QTY,SUM(REFAIL_QTY) REFAIL_QTY,sum(a.pass_qty)+SUM(a.repass_qty) pass_total_qty,sum(a.fail_qty)+SUM(a.refail_qty) fail_total_qty ," +
           " DECODE (NVL (SUM (A.PASS_QTY), 0)+ NVL (SUM (A.FAIL_QTY), 0)+ NVL (SUM (A.REPASS_QTY), 0)+ NVL (SUM (A.REFAIL_QTY), 0),0, NULL,NVL (SUM (A.PASS_QTY), 0)+ NVL (SUM (A.FAIL_QTY), 0)+ +NVL (SUM (A.REPASS_QTY), 0)+ NVL (SUM (A.REFAIL_QTY), 0)) total_qty ," +
           " TRUNC ((SUM (A.PASS_QTY))/ DECODE (NVL (SUM (A.PASS_QTY), 0)+ NVL (SUM (A.FAIL_QTY), 0)+ NVL (SUM (A.REPASS_QTY), 0)+ NVL (SUM (A.REFAIL_QTY), 0),0, NULL,NVL (SUM (A.PASS_QTY), 0)+ NVL (SUM (A.FAIL_QTY), 0)+ +NVL (SUM (A.REPASS_QTY), 0)+ NVL (SUM (A.REFAIL_QTY), 0))* 100,2)||'%' FPY,"+
           " TRUNC ((SUM (A.PASS_QTY+A.REPASS_QTY))/ DECODE (NVL (SUM (A.PASS_QTY), 0)+ NVL (SUM (A.FAIL_QTY), 0)+ NVL (SUM (A.REPASS_QTY), 0)+ NVL (SUM (A.REFAIL_QTY), 0),0, NULL,NVL (SUM (A.PASS_QTY), 0)+ NVL (SUM (A.FAIL_QTY), 0)+ +NVL (SUM (A.REPASS_QTY), 0)+ NVL (SUM (A.REFAIL_QTY), 0))* 100,2)||'%' PASS_RATE "+           
            " FROM sajet.g_sn_count A, SAJET.SYS_PDLINE b, sajet.sys_process c, SAJET.SYs_part f, sajet.sys_model g " +
            " WHERE a.pdline_id = b.pdline_id AND B.ENABLED = 'Y' AND a.process_id = c.process_id AND C.ENABLED = 'Y' AND a.part_id = f.part_id AND f.model_id = g.model_id AND (A.PASS_QTY + A.REPASS_QTY + A.FAIL_QTY + A.REFAIL_QTY) > 0";
        string endSqlStr1 = " GROUP BY g.model_name, b.pdline_name, c.process_name ORDER BY b.pdline_name, C.PROCESS_NAME ";

        #region 良率表SQL(按途程排序) 当实际选择机种时使用
        string startSortSqlStr1 = "  SELECT DISTINCT H.model_name,h.pdline_name,h.process_name,h.pass_qty,h.fail_qty,h.repass_qty,h.refail_qty,h.pass_total_qty,h.fail_total_qty,h.total_qty,h.fpy,h.pass_rate ,MAX (i.step) AS step " +
            " FROM (SELECT g.model_name,b.pdline_name,c.process_name,a.process_id,"+
            " SUM (A.PASS_QTY) PASS_QTY,SUM (A.FAIL_QTY) FAIL_QTY,SUM (REPASS_QTY) REPASS_QTY,"+
            " SUM (REFAIL_QTY) REFAIL_QTY,SUM (a.pass_qty) + SUM (a.repass_qty) pass_total_qty,"+
            " SUM (a.fail_qty) + SUM (a.refail_qty) fail_total_qty,"+
            " DECODE (NVL (SUM (A.PASS_QTY), 0)+ NVL (SUM (A.FAIL_QTY), 0)+ NVL (SUM (A.REPASS_QTY), 0)+ NVL (SUM (A.REFAIL_QTY), 0),0, NULL,NVL (SUM (A.PASS_QTY), 0)+ NVL (SUM (A.FAIL_QTY), 0)+ +NVL (SUM (A.REPASS_QTY), 0)+ NVL (SUM (A.REFAIL_QTY), 0))total_qty,"+
            " TRUNC ((SUM (A.PASS_QTY))/ DECODE (NVL (SUM (A.PASS_QTY), 0)+ NVL (SUM (A.FAIL_QTY), 0)+ NVL (SUM (A.REPASS_QTY), 0)+ NVL (SUM (A.REFAIL_QTY), 0),0, NULL,NVL (SUM (A.PASS_QTY), 0)+ NVL (SUM (A.FAIL_QTY), 0)+ +NVL (SUM (A.REPASS_QTY), 0)+ NVL (SUM (A.REFAIL_QTY), 0))* 100,2)|| '%' FPY,"+
            " TRUNC ((SUM (A.PASS_QTY + A.REPASS_QTY))/ DECODE (NVL (SUM (A.PASS_QTY), 0)+ NVL (SUM (A.FAIL_QTY), 0)+ NVL (SUM (A.REPASS_QTY), 0)+ NVL (SUM (A.REFAIL_QTY), 0),0, NULL,NVL (SUM (A.PASS_QTY), 0)+ NVL (SUM (A.FAIL_QTY), 0)+ +NVL (SUM (A.REPASS_QTY), 0)+ NVL (SUM (A.REFAIL_QTY), 0))* 100,2)|| '%' PASS_RATE "+
            " FROM sajet.g_sn_count A,SAJET.SYS_PDLINE b,sajet.sys_process c,SAJET.SYs_part f,sajet.sys_model g "+
            " WHERE a.pdline_id = b.pdline_id AND B.ENABLED = 'Y' AND a.process_id = c.process_id AND C.ENABLED = 'Y' AND a.part_id = f.part_id AND f.model_id = g.model_id AND (A.PASS_QTY + A.REPASS_QTY + A.FAIL_QTY + A.REFAIL_QTY) >0";
        
        string endSortSqlStr1 = " GROUP BY g.model_name, b.pdline_name, c.process_name, a.process_id ORDER BY b.pdline_name, a.process_id) h,"+
            " (SELECT next_process_id, step FROM SAJET.SYS_ROUTE_DETAIL WHERE step = seq AND route_id IN (SELECT route_id FROM sajet.sys_part WHERE model_id = '@model_id')) i "+
            " WHERE h.process_id = i.next_process_id GROUP BY h.model_name,h.pdline_name,h.process_name,h.pass_qty,h.fail_qty,h.repass_qty,h.refail_qty,h.pass_total_qty,h.fail_total_qty,h.total_qty,h.fpy,h.pass_rate ORDER BY step";
        #endregion

        //不良数量表
        string startSqlStr2 = "SELECT g.model_name,b.pdline_name,c.process_name,d.terminal_name,e.defect_code,e.defect_desc,COUNT (*) qty " +
            " FROM sajet.g_sn_defect a,SAJET.SYS_PDLINE b,sajet.sys_process c,sajet.sys_terminal d,sajet.sys_defect e,SAJET.SYs_part f,sajet.sys_model g " +
            " WHERE a.pdline_id = b.pdline_id AND B.ENABLED = 'Y' AND a.process_id = c.process_id AND C.ENABLED = 'Y' AND a.terminal_id = d.terminal_id AND D.ENABLED = 'Y' AND a.defect_id = e.defect_id  AND a.part_id = f.part_id  AND f.model_id = g.model_id";
        string endSqlStr2 = "GROUP BY g.model_name, b.pdline_name, c.process_name, d.terminal_name, e.defect_code, e.defect_desc ORDER BY  b.pdline_name, C.PROCESS_NAME, D.TERMINAL_NAME ";


        //详细数据
        string startSqlStr3 = "SELECT G.MODEL_NAME, b.pdline_name, c.process_name, d.terminal_name, a.work_order, a.serial_number, e.defect_code || '(' || e.defect_desc || ')' defect_code_desc, a.rec_time " +
            " FROM sajet.g_sn_defect a, sajet.sys_pdline b, sajet.sys_process c, sajet.sys_terminal d, SAJET.SYS_DEFECT e, SAJET.SYs_part f, sajet.sys_model g " +
            " WHERE A.PDLINE_ID = B.PDLINE_ID AND B.ENABLED = 'Y' AND A.PROCESS_ID = C.PROCESS_ID AND C.ENABLED = 'Y' AND A.TERMINAL_ID = D.TERMINAL_ID AND D.ENABLED = 'Y' AND A.DEFECT_ID = E.DEFECT_ID AND a.part_id = f.part_id AND f.model_id = g.model_id ";
        string endSqlStr3 = " ORDER BY B.PDLINE_NAME,C.PROCESS_name,d.terminal_name";

        #endregion

        /// <summary>
        /// 生成完整的SQL及查询参数
        /// </summary>
        /// <param name="startsqlstr">sql前面固定部分</param>
        /// <param name="endsqlstr">sql后面固定部分</param>
        /// <param name="p">用户输入的数据</param>
        /// <param name="queryParams">查询参数</param>
        /// <param name="queryType">查询类型：1：良率报表 2：不良数量表 3：详细报表</param>
        /// <returns></returns>
        private string GetFullSqlAndParams(string startsqlstr, string endsqlstr, ParamsData p, out object[][] queryParams, int queryType)
        {
            StringBuilder sqlStrBuilder = new StringBuilder();
            sqlStrBuilder.Append(startsqlstr);

            string sqltemp;
            sqltemp = GetQueryParameters(p, out queryParams, queryType);
            sqlStrBuilder.Append(sqltemp);
            sqlStrBuilder.Append(endsqlstr);
            return sqlStrBuilder.ToString();
        }


        
        /// <summary>
        /// 根据用户输入生成WHERE sql及参数
        /// </summary>
        /// <param name="param">用户数据</param>
        /// <param name="Params">查询参数</param>
        /// <param name="queryType">查询类型</param>
        /// <returns>根据用户的输入生成Where 后的SQL</returns>
        private string GetQueryParameters(ParamsData param, out object[][] Params, int queryType)
        {

            List<object[]> listparams = new List<object[]>();
            StringBuilder querySqlStrBuilder = new StringBuilder();



            if (!string.IsNullOrEmpty(param.Model_id) && param.Model_id != "0")
            {
                listparams.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "model_id", param.Model_id });
                //querySqlStrBuilder.Append(" AND A.PART_ID IN(SELECT PART_ID FROM SAJET.SYS_PART WHERE MODEL_ID=:model_id )");
                querySqlStrBuilder.Append(" AND g.MODEL_ID=:model_id ");

            }
            else
            {
                //querySqlStrBuilder.Append(" AND A.PART_ID IN(SELECT part_id FROM SAJET.SYS_part where model_id in(select MODEL_ID from sajet.sys_group_model WHERE GROUP_ID in(select GROUP_ID from sajet.sys_group_emp where emp_id='" + ClientUtils.UserPara1 + "')))");
                querySqlStrBuilder.Append(" AND g.model_id in(select MODEL_ID from sajet.sys_group_model WHERE GROUP_ID in(select GROUP_ID from sajet.sys_group_emp where emp_id='" + ClientUtils.UserPara1 + "'))");
            }
            if (!string.IsNullOrEmpty(param.Pdline_id) && param.Pdline_id != "0")
            {
                listparams.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "pdline_id", param.Pdline_id });
                querySqlStrBuilder.Append(" and a.pdline_id=:pdline_id");
            }
            if (!string.IsNullOrEmpty(param.Process_id) && param.Process_id != "0")
            {
                listparams.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "process_id", param.Process_id });
                querySqlStrBuilder.Append(" and a.process_id=:process_id");
            }
            if (!string.IsNullOrEmpty(param.Work_order))
            {
                listparams.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "work_order", param.Work_order });
                querySqlStrBuilder.Append(" and a.work_order=:work_order");
            }
            if (queryType == 1)
            {
                querySqlStrBuilder.Append(" AND  A.WORK_DATE|| lpad(a.work_time,2,'0') >= :startdatetime AND A.WORK_DATE|| lpad(a.work_time,2,'0')<=:enddatetime ");
            }
            else if (queryType == 2 || queryType == 3)
            {
                querySqlStrBuilder.Append(" and a.rec_time >= TO_DATE (:startdatetime||'0000','yyyymmddhh24miss') AND a.rec_time <= TO_DATE (:enddatetime||'5959', 'yyyymmddhh24miss') ");
            }
            if (!string.IsNullOrEmpty(param.Terminal_id))
            {
                listparams.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "terminal_id", param.Terminal_id });
                querySqlStrBuilder.Append(" and a.terminal_id=:terminal_id");
            }

            if (!string.IsNullOrEmpty(param.Defect_id))
            {

                listparams.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "defect_id", param.Defect_id });
                querySqlStrBuilder.Append(" and a.defect_id=:defect_id");
            }
            listparams.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "startdatetime", param.Startdatetime });
            listparams.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "enddatetime", param.Enddatetime });

            int arrayLength = listparams.Count;
            Params = new object[arrayLength][];

            for (int i = 0; i < arrayLength; i++)
            {
                Params[i] = listparams[i];
            }

            return querySqlStrBuilder.ToString();
        }

        /// <summary>
        /// 获取机种数据
        /// </summary>
        /// <returns>机种ID+机种名称</returns>
        private DataTable GetModelData()
        {
            DataTable dt = null;
            string strSql = "select model_id,model_name from SAJET.SYS_MODEL where model_id in(select MODEL_ID from sajet.sys_group_model WHERE GROUP_ID in (select GROUP_ID from sajet.sys_group_emp where emp_id='" + ClientUtils.UserPara1 + "')) ORDER BY MODEL_NAME";
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
            string strSql = "select pdline_id,pdline_name from sajet.sys_pdline WHERE ENABLED='Y' ORDER BY PDLINE_NAME ";
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
            string strSql = "select process_id,process_name from sajet.sys_process WHERE ENABLED='Y' ORDER BY PROCESS_NAME";
            DataSet ds = ClientUtils.ExecuteSQL(strSql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                DataRow dr = dt.NewRow();
                dr["process_id"] = "0";
                dr["process_name"] = "--请选择--";
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
        
        //存放良率报表数据，方便后续使用
        DataTable mainDataTable = new DataTable();

        //存放不良数量报表数据，方便后续使用
        DataTable defectStatisticsDataTable = new DataTable();

        //存放详细报表数据，方便后续使用
        DataTable detailDataTable = new DataTable();

        private void fMain_Load(object sender, EventArgs e)
        {
            cmbStartTime.SelectedIndex = 0;
            cmbEndTime.SelectedIndex = 23;

            DataTable dtModel = GetModelData();
            if (dtModel != null)
            {
                cmbModle.DataSource = dtModel;
                cmbModle.DisplayMember = "model_name";
                cmbModle.ValueMember = "model_id";

                foreach (DataRow dr in dtModel.Rows)
                {
                    modelData.Add(dr[1].ToString(), dr[0].ToString());
                }


            }


            DataTable dtPdline = GetPdlineData();
            if (dtPdline != null)
            {
                cmbPdline.DataSource = dtPdline;
                cmbPdline.DisplayMember = "pdline_name";
                cmbPdline.ValueMember = "pdline_id";
                foreach (DataRow dr in dtPdline.Rows)
                {
                    pdlineData.Add(dr[1].ToString(), dr[0].ToString());
                }
            }

            DataTable dtProcess = GetProcessData();
            if (dtProcess != null)
            {
                cmbProcess.DataSource = dtProcess;
                cmbProcess.DisplayMember = "process_name";
                cmbProcess.ValueMember = "process_id";
                foreach (DataRow dr in dtProcess.Rows)
                {
                    processData.Add(dr[1].ToString(), dr[0].ToString());
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
            DataTable dt = null;
            object[][] Params = null;
            string sql = "";
            if (!string.IsNullOrEmpty(p.Model_id) && p.Model_id != "0" && ckbSortByRoute.Checked) 
            {
                string endSortSqlStr1temp = endSortSqlStr1.Replace("@model_id",p.Model_id);
                sql = GetFullSqlAndParams(startSortSqlStr1, endSortSqlStr1temp, p, out Params, queryType);

            }
            else
            {
             sql = GetFullSqlAndParams(startSqlStr1, endSqlStr1, p, out Params, queryType);
            }

            DataSet ds = ClientUtils.ExecuteSQL(sql, Params);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                dt.Columns["model_name"].ColumnName = "机种";
                dt.Columns["pdline_name"].ColumnName = "线别";
                dt.Columns["process_name"].ColumnName = "制程";
                dt.Columns["PASS_QTY"].ColumnName = "良品数";
                dt.Columns["Fail_qty"].ColumnName = "不良品数";
                dt.Columns["REPASS_QTY"].ColumnName = "复测良品数";
                dt.Columns["refail_qty"].ColumnName = "复测不良品数";
                dt.Columns["pass_total_qty"].ColumnName = "良品总数";
                dt.Columns["fail_total_qty"].ColumnName = "不良品总数";
                dt.Columns["TOTAL_QTY"].ColumnName = "总数";
                dt.Columns["FPY"].ColumnName = "直通率";
                dt.Columns["PASS_RATE"].ColumnName = "良率";

            }

            return dt;

        }

        /// <summary>
        /// 获取不良数量报表数据
        /// </summary>
        /// <param name="p">用户输入数据</param>
        /// <param name="queryType">查询类型 2</param>
        /// <returns>不良数量数据</returns>
        private DataTable GetDefectStatisticsData(ParamsData p, int queryType)
        {

            DataTable dt = null;
            object[][] Params = null;
            string sql = GetFullSqlAndParams(startSqlStr2, endSqlStr2, p, out Params, queryType);
            DataSet ds = ClientUtils.ExecuteSQL(sql, Params);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                dt.Columns["model_name"].ColumnName = "机种";
                dt.Columns["pdline_name"].ColumnName = "线别";
                dt.Columns["process_name"].ColumnName = "制程";
                dt.Columns["terminal_name"].ColumnName = "站点";
                dt.Columns["defect_code"].ColumnName = "不良代码";
                dt.Columns["defect_desc"].ColumnName = "不良现象";
                dt.Columns["qty"].ColumnName = "不良数量";
            }
            return dt;
        }

        /// <summary>
        /// 获取详细报表数据
        /// </summary>
        /// <param name="p">用户输入数据</param>
        /// <param name="queryType">查询类型 3</param>
        /// <returns>详细报表数据</returns>
        private DataTable GetDetialData(ParamsData p, int queryType)
        {
            DataTable dt = null;
            object[][] Params = null;
            string sql = GetFullSqlAndParams(startSqlStr3, endSqlStr3, p, out Params, queryType);
            DataSet ds = ClientUtils.ExecuteSQL(sql, Params);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                dt.Columns["model_name"].ColumnName = "机种";
                dt.Columns["pdline_name"].ColumnName = "线别";
                dt.Columns["process_name"].ColumnName = "制程";
                dt.Columns["terminal_name"].ColumnName = "站点";
                dt.Columns["work_order"].ColumnName = "工单";
                dt.Columns["serial_number"].ColumnName = "SN";
                dt.Columns["defect_code_desc"].ColumnName = "不良现象";
                dt.Columns["rec_time"].ColumnName = "不良时间";

            }
            return dt;
        }


        //保存用户实际输入数据
        ParamsData paramsdata = null;

        private void btnQuery_Click(object sender, EventArgs e)
        {


            btnQuery.Text = "正在查询";
            btnQuery.Enabled = false;
            if (cmbModle.Items.Count == 0)
            {
                MessageBox.Show("员工机种没有设定！", "消息提示:", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnQuery.Enabled = true;
                btnQuery.Text = "查询";
                return;
            }
            paramsdata = new ParamsData(cmbModle.SelectedValue.ToString(), cmbPdline.SelectedValue.ToString(), cmbProcess.SelectedValue.ToString(), txtWorkOrder.Text.Trim(), dtpStartDate.Value.Date.ToString("yyyyMMdd") + cmbStartTime.SelectedItem, dtpEndDate.Value.Date.ToString("yyyyMMdd") + cmbEndTime.SelectedItem);


            mainDataTable = GetMainReportData(paramsdata, 1);

            defectStatisticsDataTable = GetDefectStatisticsData(paramsdata, 2);
            detailDataTable = GetDetialData(paramsdata, 3);

            dgvMainTable.DataSource = mainDataTable;
            dgvDefectStatistics.DataSource = defectStatisticsDataTable;
            dgvDetial.DataSource = detailDataTable;

            btnQuery.Enabled = true;
            btnQuery.Text = "查询";

           

        }

        private void dgwMainTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!(e.RowIndex > -1))
            {
                return;
            }
            dgvDefectStatistics.DataSource = null;
            dgvDetial.DataSource = null;
            int defectcount = Convert.ToInt32(dgvMainTable.Rows[e.RowIndex].Cells["不良品数"].Value.ToString());
            if (defectcount == 0)
            {
                return;
            }
            string modelid = modelData[dgvMainTable.Rows[e.RowIndex].Cells["机种"].Value.ToString()];
            string pdlineid = pdlineData[dgvMainTable.Rows[e.RowIndex].Cells["线别"].Value.ToString()];
            string processid = processData[dgvMainTable.Rows[e.RowIndex].Cells["制程"].Value.ToString()];

            ParamsData pd = new ParamsData(modelid, pdlineid, processid, paramsdata.Work_order, paramsdata.Startdatetime, paramsdata.Enddatetime);
            
            defectStatisticsDataTable = GetDefectStatisticsData(pd, 2);
            dgvDefectStatistics.DataSource = defectStatisticsDataTable;

            detailDataTable = GetDetialData(pd, 3);
            dgvDetial.DataSource = detailDataTable;


        }

        private void dgvDefectStatistics_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!(e.RowIndex > -1))
            {
                return;
            }
            dgvDetial.DataSource = null;
            string modelid = modelData[dgvDefectStatistics.Rows[e.RowIndex].Cells["机种"].Value.ToString()];
            string pdlineid = pdlineData[dgvDefectStatistics.Rows[e.RowIndex].Cells["线别"].Value.ToString()];
            string processid = processData[dgvDefectStatistics.Rows[e.RowIndex].Cells["制程"].Value.ToString()];

            ParamsData pd = new ParamsData(modelid, pdlineid, processid, paramsdata.Work_order, paramsdata.Startdatetime, paramsdata.Enddatetime);
            string terminal_name = dgvDefectStatistics.Rows[e.RowIndex].Cells["站点"].Value.ToString();
            pd.Terminal_id = ClientUtils.ExecuteSQL("select terminal_id from sajet.sys_terminal where terminal_name='" + terminal_name + "' and pdline_id='" + pdlineid + "'").Tables[0].Rows[0][0].ToString();

            pd.Defect_id = ClientUtils.ExecuteSQL("select defect_id from sajet.sys_defect where defect_code='" + dgvDefectStatistics.Rows[e.RowIndex].Cells["不良代码"].Value.ToString() + "'").Tables[0].Rows[0][0].ToString();

            detailDataTable = GetDetialData(pd, 3);
            dgvDetial.DataSource = detailDataTable;



        }


        DataTable gvDataTable = null;

        private void ExportExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = "xls";
            saveFileDialog1.Filter = "All Files(*.xls)|*.xls";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            string sFileName = saveFileDialog1.FileName;

            ExportExcel.CreateExcel Export = new ExportExcel.CreateExcel(sFileName);
            Export.ExportToExcel(gvDataTable);
        }

        private void dgvMainTable_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right && e.RowIndex > -1)
            {
                gvDataTable =mainDataTable ;
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void dgvDefectStatistics_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right && e.RowIndex > -1)
            {
                gvDataTable = defectStatisticsDataTable;
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void dgvDetial_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right && e.RowIndex > -1)
            {
                gvDataTable = detailDataTable;
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void dgvMainTable_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dgvMainTable.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dgvMainTable.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dgvMainTable.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            if ((e.RowIndex + 1) % 2 == 0)
                dgvMainTable.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCyan;
            else
            {
                dgvMainTable.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.AliceBlue;
            }
        }

        private void dgvDefectStatistics_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dgvDefectStatistics.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dgvDefectStatistics.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dgvDefectStatistics.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            if ((e.RowIndex + 1) % 2 == 0)
                dgvDefectStatistics.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCyan;
            else
            {
                dgvDefectStatistics.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.AliceBlue;
            }
        }

        private void dgvDetial_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dgvDetial.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dgvDetial.RowHeadersDefaultCellStyle.Font, rectangle, dgvDetial.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            if ((e.RowIndex + 1) % 2 == 0)
                dgvDetial.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCyan;
            else
            {
                dgvDetial.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.AliceBlue;
            }
        }

    }
}
