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
using DevComponents.DotNetBar.Metro;
using System.Xml.Linq;
using System.IO;
namespace QueryReport
{
    public partial class fMain : MetroForm
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
        //良率表 不按途程排序
        //string startSqlStr1 = "SELECT G.MODEL_NAME, B.PDLINE_NAME, C.PROCESS_NAME, SUM (A.PASS_QTY) PASS_QTY, SUM (A.FAIL_QTY) FAIL_QTY, SUM (A.OUTPUT_QTY) - SUM (A.PASS_QTY) REPASS_QTY, DECODE (SIGN (SUM (A.PASS_QTY) + SUM (A.FAIL_QTY) - SUM (A.OUTPUT_QTY)),-1, (SUM (A.OUTPUT_QTY) - SUM (A.PASS_QTY) - SUM (A.FAIL_QTY)),(SUM (A.PASS_QTY) + SUM (A.FAIL_QTY) - SUM (A.OUTPUT_QTY))) REFAIL_QTY, SUM (A.OUTPUT_QTY) OUTPUT_QTY, "
        //                    + "TRUNC ( (SUM (A.PASS_QTY)) / DECODE ( NVL (SUM (A.PASS_QTY), 0) + NVL (SUM (A.FAIL_QTY), 0), 0, NULL, NVL (SUM (A.PASS_QTY), 0) + NVL (SUM (A.FAIL_QTY), 0)) * 100, 2) || '%' FPY, "
        //                    + "TRUNC ( (SUM (OUTPUT_QTY)) / DECODE ( NVL (SUM (A.PASS_QTY), 0) + NVL (SUM (A.FAIL_QTY), 0), 0, NULL, NVL (SUM (A.PASS_QTY), 0) + NVL (SUM (A.FAIL_QTY), 0)) * 100, 2) || '%' PASS_RATE "
        //                    + "FROM SAJET.G_SN_COUNT A, SAJET.SYS_PDLINE B, SAJET.SYS_PROCESS C, SAJET.SYS_PART F, SAJET.SYS_MODEL G "
        //                    + "WHERE A.PDLINE_ID = B.PDLINE_ID AND B.ENABLED = 'Y' AND A.PROCESS_ID = C.PROCESS_ID AND C.ENABLED = 'Y' AND A.PART_ID = F.PART_ID AND F.MODEL_ID = G.MODEL_ID AND ( A.PASS_QTY + A.REPASS_QTY) > 0";

        //string endSqlStr1 = " GROUP BY G.MODEL_NAME, B.PDLINE_NAME , C.PROCESS_NAME ORDER BY  C.PROCESS_NAME, B.PDLINE_NAME ";

        #region 良率表SQL(按途程排序) 当实际选择机种时使用
        string startSortSqlStr1 = "  SELECT DISTINCT H.MODEL_NAME,H.PDLINE_NAME,H.PROCESS_NAME,H.PASS_QTY,H.FAIL_QTY,H.REPASS_QTY,H.REFAIL_QTY,H.OUTPUT_QTY,H.FPY,H.PASS_RATE ,MAX (I.STEP) AS STEP " +
            " FROM (SELECT G.MODEL_NAME,B.PDLINE_NAME,C.PROCESS_NAME,A.PROCESS_ID," +
            " SUM (A.PASS_QTY) PASS_QTY,SUM (A.FAIL_QTY) FAIL_QTY,SUM (A.OUTPUT_QTY) - SUM (A.PASS_QTY) REPASS_QTY," +
            " DECODE (SIGN (SUM (A.PASS_QTY) + SUM (A.FAIL_QTY) - SUM (A.OUTPUT_QTY)),-1, (SUM (A.OUTPUT_QTY) - SUM (A.PASS_QTY) - SUM (A.FAIL_QTY)),(SUM (A.PASS_QTY) + SUM (A.FAIL_QTY) - SUM (A.OUTPUT_QTY))) REFAIL_QTY, SUM (A.OUTPUT_QTY) OUTPUT_QTY, " +
            "TRUNC ( (SUM (A.PASS_QTY)) / DECODE ( NVL (SUM (A.PASS_QTY), 0) + NVL (SUM (A.FAIL_QTY), 0), 0, NULL, NVL (SUM (A.PASS_QTY), 0) + NVL (SUM (A.FAIL_QTY), 0)) * 100, 2) || '%' FPY, " +
            "TRUNC ( (SUM (OUTPUT_QTY)) / DECODE ( NVL (SUM (A.PASS_QTY), 0) + NVL (SUM (A.FAIL_QTY), 0), 0, NULL, NVL (SUM (A.PASS_QTY), 0) + NVL (SUM (A.FAIL_QTY), 0)) * 100, 2) || '%' PASS_RATE " +
            " FROM SAJET.G_SN_COUNT A,SAJET.SYS_PDLINE B,SAJET.SYS_PROCESS C,SAJET.SYS_PART F,SAJET.SYS_MODEL G " +
            " WHERE A.PDLINE_ID = B.PDLINE_ID AND B.ENABLED = 'Y' AND A.PROCESS_ID = C.PROCESS_ID AND C.ENABLED = 'Y' AND A.PART_ID = F.PART_ID AND F.MODEL_ID = G.MODEL_ID AND (A.PASS_QTY + A.REPASS_QTY + A.FAIL_QTY + A.REFAIL_QTY) >0";

        string endSortSqlStr1 = " GROUP BY G.MODEL_NAME, B.PDLINE_NAME, C.PROCESS_NAME, A.PROCESS_ID ORDER BY B.PDLINE_NAME, A.PROCESS_ID) H," +
            " (SELECT NEXT_PROCESS_ID, STEP FROM SAJET.SYS_ROUTE_DETAIL WHERE STEP = SEQ AND ROUTE_ID IN (SELECT ROUTE_ID FROM SAJET.SYS_PART WHERE MODEL_ID = '@MODEL_ID')) I " +
            " WHERE H.PROCESS_ID = I.NEXT_PROCESS_ID GROUP BY H.MODEL_NAME,H.PDLINE_NAME,H.PROCESS_NAME,H.PASS_QTY,H.FAIL_QTY,H.REPASS_QTY,H.REFAIL_QTY,H.OUTPUT_QTY,H.FPY,H.PASS_RATE ORDER BY STEP";
        #endregion

        //不良数量表
        string startSqlStr2 = "SELECT DISTINCT G.MODEL_NAME,B.PDLINE_NAME,C.PROCESS_NAME,D.TERMINAL_NAME,E.DEFECT_CODE,E.DEFECT_DESC,COUNT (*) QTY " +
            " FROM (SELECT WORK_ORDER, SERIAL_NUMBER, PDLINE_ID, PROCESS_ID, TERMINAL_ID, DEFECT_ID, PART_ID,REC_TIME FROM SAJET.G_SN_DEFECT UNION ALL SELECT WORK_ORDER, SERIAL_NUMBER, PDLINE_ID, PROCESS_ID, TERMINAL_ID, DEFECT_ID, PART_ID,REC_TIME FROM SAJET.G_SN_DEFECT_BACKUP) A,SAJET.SYS_PDLINE B,SAJET.SYS_PROCESS C,SAJET.SYS_TERMINAL D,SAJET.SYS_DEFECT E,SAJET.SYS_PART F,SAJET.SYS_MODEL G " +
            " WHERE A.PDLINE_ID = B.PDLINE_ID AND B.ENABLED = 'Y' AND A.PROCESS_ID = C.PROCESS_ID AND C.ENABLED = 'Y' AND A.TERMINAL_ID = D.TERMINAL_ID AND D.ENABLED = 'Y' AND A.DEFECT_ID = E.DEFECT_ID  AND A.PART_ID = F.PART_ID  AND F.MODEL_ID = G.MODEL_ID";
        string endSqlStr2 = "GROUP BY G.MODEL_NAME, B.PDLINE_NAME, C.PROCESS_NAME, D.TERMINAL_NAME, E.DEFECT_CODE, E.DEFECT_DESC ORDER BY  B.PDLINE_NAME, C.PROCESS_NAME, D.TERMINAL_NAME ";


        //详细数据  (SELECT WORK_ORDER, SERIAL_NUMBER, PDLINE_ID, PROCESS_ID, TERMINAL_ID, DEFECT_ID, PART_ID,REC_TIME FROM SAJET.G_SN_DEFECT UNION ALL SELECT WORK_ORDER, SERIAL_NUMBER, PDLINE_ID, PROCESS_ID, TERMINAL_ID, DEFECT_ID, PART_ID,REC_TIME FROM SAJET.G_SN_DEFECT_BACKUP)
        string startSqlStr3 = "SELECT DISTINCT G.MODEL_NAME, B.PDLINE_NAME, C.PROCESS_NAME, D.TERMINAL_NAME, A.WORK_ORDER, A.SERIAL_NUMBER,  E.DEFECT_CODE ,E.DEFECT_DESC , DECODE(H.CURRENT_STATUS,'1','NG','9','Retest','0','OK') AS STATUS,H.OUT_PROCESS_TIME " +
            " FROM  (SELECT WORK_ORDER, SERIAL_NUMBER, PDLINE_ID, PROCESS_ID, TERMINAL_ID, DEFECT_ID, PART_ID,REC_TIME FROM SAJET.G_SN_DEFECT UNION ALL SELECT WORK_ORDER, SERIAL_NUMBER, PDLINE_ID, PROCESS_ID, TERMINAL_ID, DEFECT_ID, PART_ID,REC_TIME FROM SAJET.G_SN_DEFECT_BACKUP) A, SAJET.SYS_PDLINE B, SAJET.SYS_PROCESS C, SAJET.SYS_TERMINAL D, SAJET.SYS_DEFECT E, SAJET.SYS_PART F, SAJET.SYS_MODEL G ,SAJET.V_SN_TRAVEL_RETEST  H" +
            " WHERE A.PDLINE_ID = B.PDLINE_ID AND B.ENABLED = 'Y' AND A.PROCESS_ID = C.PROCESS_ID AND C.ENABLED = 'Y' AND A.TERMINAL_ID = D.TERMINAL_ID AND D.ENABLED = 'Y' AND A.DEFECT_ID = E.DEFECT_ID AND A.PART_ID = F.PART_ID AND F.MODEL_ID = G.MODEL_ID AND A.SERIAL_NUMBER=H.SERIAL_NUMBER AND A.PROCESS_ID=H.PROCESS_ID  ";
        string endSqlStr3 = " ORDER BY A.SERIAL_NUMBER, OUT_PROCESS_TIME,DEFECT_CODE,DEFECT_DESC,C.PROCESS_NAME";

        //    string startSqlStr3 = "SELECT DISTINCT G.MODEL_NAME, B.PDLINE_NAME, C.PROCESS_NAME, D.TERMINAL_NAME, A.WORK_ORDER, A.SERIAL_NUMBER,  E.DEFECT_CODE ,E.DEFECT_DESC , A.REC_TIME " +
        //" FROM SAJET.G_SN_DEFECT A, SAJET.SYS_PDLINE B, SAJET.SYS_PROCESS C, SAJET.SYS_TERMINAL D, SAJET.SYS_DEFECT E, SAJET.SYS_PART F, SAJET.SYS_MODEL G " +
        //" WHERE A.PDLINE_ID = B.PDLINE_ID AND B.ENABLED = 'Y' AND A.PROCESS_ID = C.PROCESS_ID AND C.ENABLED = 'Y' AND A.TERMINAL_ID = D.TERMINAL_ID AND D.ENABLED = 'Y' AND A.DEFECT_ID = E.DEFECT_ID AND A.PART_ID = F.PART_ID AND F.MODEL_ID = G.MODEL_ID   ";
        //    string endSqlStr3 = " ORDER BY A.SERIAL_NUMBER, A.REC_TIME,DEFECT_CODE,DEFECT_DESC,C.PROCESS_NAME";



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
                listparams.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "MODEL_ID", param.Model_id });
                //querySqlStrBuilder.Append(" AND A.PART_ID IN(SELECT PART_ID FROM SAJET.SYS_PART WHERE MODEL_ID=:model_id )");
                querySqlStrBuilder.Append(" AND g.MODEL_ID=:MODEL_ID ");

            }
            else
            {
                //querySqlStrBuilder.Append(" AND A.PART_ID IN(SELECT part_id FROM SAJET.SYS_part where model_id in(select MODEL_ID from sajet.sys_group_model WHERE GROUP_ID in(select GROUP_ID from sajet.sys_group_emp where emp_id='" + ClientUtils.UserPara1 + "')))");
                querySqlStrBuilder.Append(" AND G.MODEL_ID IN(SELECT MODEL_ID FROM SAJET.SYS_GROUP_MODEL WHERE GROUP_ID IN(SELECT GROUP_ID FROM SAJET.SYS_GROUP_EMP WHERE EMP_ID='" + ClientUtils.UserPara1 + "'))");
            }
            if (!string.IsNullOrEmpty(param.Pdline_id) && param.Pdline_id != "0")
            {
                //listparams.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "pdline_id", param.Pdline_id });
                querySqlStrBuilder.Append(" AND A.PDLINE_ID IN (" + param.Pdline_id + ") ");
            }
            if (!string.IsNullOrEmpty(param.Process_id) && param.Process_id != "0")
            {
                //listparams.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "process_id", param.Process_id });
                //querySqlStrBuilder.Append(" AND A.PROCESS_ID=:PROCESS_ID");
                querySqlStrBuilder.Append(" AND A.PROCESS_ID IN (" + param.Process_id + ") ");
            }
            if (!string.IsNullOrEmpty(param.Work_order))
            {
                listparams.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "work_order", param.Work_order });
                querySqlStrBuilder.Append(" AND A.WORK_ORDER=:WORK_ORDER");
            }
            if (queryType == 1)
            {
                querySqlStrBuilder.Append(" AND  A.WORK_DATE|| LPAD(A.WORK_TIME,2,'0') >= :STARTDATETIME AND A.WORK_DATE|| LPAD(A.WORK_TIME,2,'0')<:ENDDATETIME ");
            }
            else if (queryType == 2 || queryType == 3)
            {
                querySqlStrBuilder.Append(" AND A.REC_TIME >= TO_DATE (:STARTDATETIME||'0000','YYYYMMDDHH24MISS') AND A.REC_TIME <= TO_DATE (:ENDDATETIME||'5959', 'YYYYMMDDHH24MISS') ");

            }
            if (!string.IsNullOrEmpty(param.Terminal_id))
            {
                listparams.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "TERMINAL_ID", param.Terminal_id });
                querySqlStrBuilder.Append(" AND A.TERMINAL_ID=:TERMINAL_ID");
            }
            if (!string.IsNullOrEmpty(param.Defect_id))
            {

                listparams.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_ID", param.Defect_id });
                querySqlStrBuilder.Append(" AND A.DEFECT_ID=:DEFECT_ID");
            }
            listparams.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "STARTDATETIME", param.Startdatetime });
            listparams.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "ENDDATETIME", param.Enddatetime });

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
            string strSql = "SELECT MODEL_ID,MODEL_NAME FROM SAJET.SYS_MODEL WHERE MODEL_ID IN(SELECT MODEL_ID FROM SAJET.SYS_GROUP_MODEL WHERE GROUP_ID IN (SELECT GROUP_ID FROM SAJET.SYS_GROUP_EMP WHERE EMP_ID='" + ClientUtils.UserPara1 + "')) ORDER BY MODEL_NAME";
            DataSet ds = ClientUtils.ExecuteSQL(strSql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                DataRow dr = dt.NewRow();

            }

            return dt;
        }


        private DataTable GetRoute()
        {
            DataTable dt = null;
            string strSql = "SELECT ROUTE_ID,ROUTE_NAME FROM SAJET.SYS_ROUTE WHERE enabled='Y' ORDER BY ROUTE_NAME ";
            DataSet ds = ClientUtils.ExecuteSQL(strSql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                DataRow dr = dt.NewRow();

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
            }
            return dt;
        }



        private DataTable GetProcessDataByRouteId(string routeid)
        {
            DataTable dt = null;
            string sqlstr = "SELECT A.ROUTE_ID,B.PROCESS_ID, B.PROCESS_NAME,C.PROCESS_ID NEXT_PROCESS_ID, C.PROCESS_NAME NEXT_PROCESS_NAME"
                           + " FROM sajet.sys_route_detail A, SAJET.SYS_PROCESS B, SAJET.SYS_PROCESS C "
                           + " WHERE A.ROUTE_ID = :ROUTE_ID AND A.PROCESS_ID = B.PROCESS_ID AND A.NEXT_PROCESS_ID = C.PROCESS_ID AND A.STEP=A.SEQ ORDER BY A.STEP,A.SEQ ";

            object[][] sqlparams = new object[][] { new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", routeid } };
            try
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("PROCESS_ID"));
                dt.Columns.Add(new DataColumn("PROCESS_NAME"));

                DataRow firstdr = dt.NewRow();
                firstdr["PROCESS_ID"] = "0";
                firstdr["PROCESS_NAME"] = "--所有制程--";
                dt.Rows.Add(firstdr);
                if (string.IsNullOrEmpty(routeid) || routeid == "0")
                {
                    return dt;
                }

                DataTable dttemp = ClientUtils.ExecuteSQL(sqlstr, sqlparams).Tables[0];
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["PROCESS_ID"] = dttemp.Rows[i]["PROCESS_ID"];
                    dr["PROCESS_NAME"] = dttemp.Rows[i]["PROCESS_NAME"];

                    dt.Rows.Add(dr);
                    if (i == dttemp.Rows.Count - 1)
                    {
                        DataRow lastdr = dt.NewRow();
                        lastdr["PROCESS_ID"] = dttemp.Rows[i]["NEXT_PROCESS_ID"];
                        lastdr["PROCESS_NAME"] = dttemp.Rows[i]["NEXT_PROCESS_NAME"];
                        dt.Rows.Add(lastdr);
                    }

                }

                return dt;
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
                return dt;
            }
        }

        //临时存放机种数据，方便后续使用
        Dictionary<string, string> modelData = new Dictionary<string, string>();
        //临时存放线别数据，方便后续使用
        Dictionary<string, string> pdlineData = new Dictionary<string, string>();
        //临时存放制程数据，方便后续使用
        Dictionary<string, string> processData = new Dictionary<string, string>();


        Dictionary<string, string> routeData = new Dictionary<string, string>();


        //存放良率报表数据，方便后续使用
        DataTable mainDataTable = new DataTable();
        DataTable mainDataTable1 = new DataTable();

        //存放不良数量报表数据，方便后续使用
        DataTable defectStatisticsDataTable = new DataTable();

        //存放详细报表数据，方便后续使用
        DataTable detailDataTable = new DataTable();
        XElement xdoc = null;
        private void fMain_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + " (" + SajetCommon.g_sFileVersion + ")";
            dtpStartDate.Value = DateTime.Today;
            dtpEndDate.Value = DateTime.Today;
            cmbStartTime.SelectedIndex = 0;
            cmbEndTime.SelectedIndex = 23;
            CommData.dtModel = GetModelData();

            btnQueryConfig.Enabled = false;
            btnQueryConfig.Enabled = (ClientUtils.GetPrivilege(ClientUtils.UserPara1, ClientUtils.fFunctionName, ClientUtils.fProgramName) > 1);

            if (!File.Exists(Application.StartupPath + @"\Query\ModelData.xml"))
            {
                File.AppendAllText(Application.StartupPath + @"\Query\ModelData.xml", @"<?xml version=""1.0"" encoding=""utf-8"" ?>" + Environment.NewLine + @"<Models name=""机种"">" + Environment.NewLine + @"</Models>");
            }
            if (CommData.dtModel == null || CommData.dtModel.Rows.Count==0) 
            {
                MessageBox.Show("该工号没有设定机种权限","消息提示:",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            foreach (DataRow itemrow in CommData.dtModel.Rows)
            {
                modelData.Add(itemrow["MODEL_NAME"].ToString(), itemrow["MODEL_ID"].ToString());
            }


            CommData.dtPdline = GetPdlineData();

            foreach (DataRow itemrow in CommData.dtPdline.Rows)
            {
                pdlineData.Add(itemrow["PDLINE_NAME"].ToString(), itemrow["PDLINE_ID"].ToString());

            }

            CommData.dtProcess = GetProcessData();

            foreach (DataRow itemrow in CommData.dtProcess.Rows)
            {
                processData.Add(itemrow["PROCESS_NAME"].ToString(), itemrow["PROCESS_ID"].ToString());

            }

            CommData.dtRoute = GetRoute();
            xdoc = XElement.Load(Application.StartupPath + @"\Query\ModelData.xml");

            var modelNodes = xdoc.Elements("Model");

            Dictionary<string, string> currModelData = new Dictionary<string, string>();
            foreach (var item in modelNodes)
            {
                currModelData.Add(item.Attribute("name").Value.ToString(), item.Attribute("id").Value.ToString());
            }
            BindingSource bs = new BindingSource();
            bs.DataSource = currModelData;
            cmbModle.DisplayMember = "Key";
            cmbModle.ValueMember = "Value";
            cmbModle.DataSource = bs;

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
            string sql = string.Empty;
            //if (!string.IsNullOrEmpty(p.Model_id) && p.Model_id != "0" && ckbSortByRoute.Checked) 
            //{
            string endSortSqlStr1temp = endSortSqlStr1.Replace("@MODEL_ID", p.Model_id);
            sql = GetFullSqlAndParams(startSortSqlStr1, endSortSqlStr1temp, p, out Params, queryType);

            //}
            //else
            //{
            // sql = GetFullSqlAndParams(startSqlStr1, endSqlStr1, p, out Params, queryType);
            //}

            DataSet ds = ClientUtils.ExecuteSQL(sql, Params);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                //double countpassrate = 1;
                //foreach (DataRow dr in dt.Rows)
                //{
                //    if (int.Parse(dr[5].ToString()) > int.Parse(dr[4].ToString()))
                //    {
                //        dr[9] = (int.Parse(dr[7].ToString()) / (int.Parse(dr[3].ToString()) + int.Parse(dr[4].ToString()) + int.Parse(dr[6].ToString()))).ToString("P2");
                //    }

                //    string data = dr["PASS_RATE"].ToString().Remove(dr["PASS_RATE"].ToString().Length - 1, 1);
                //    if (string.IsNullOrEmpty(data)) continue;
                //    countpassrate *= double.Parse(data) / 100;
                //}

                //DataRow drr = dt.NewRow();
                //drr["PASS_RATE"] = countpassrate.ToString("P2");
                //dt.Rows.Add(drr);

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
            try
            {
                dt = ClientUtils.ExecuteSQL(sql, Params).Tables[0];
                if (dt.Rows.Count > 0)
                {

                    string datestartstr = dtpStartDate.Value.ToString("yyyy/MM/dd") + " " + cmbStartTime.SelectedItem.ToString() + ":00:00";
                    string dateendstr = dtpEndDate.Value.ToString("yyyy/MM/dd") + " " + cmbEndTime.SelectedItem.ToString() + ":00:00";
                    DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();

                    dtFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
                    DateTime datestart = Convert.ToDateTime(datestartstr, dtFormat);
                    DateTime dateend = Convert.ToDateTime(dateendstr, dtFormat);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        DateTime datatemp = Convert.ToDateTime(dr["OUT_PROCESS_TIME"].ToString());
                        if (datestart > datatemp || dateend < datatemp)
                        {
                            dt.Rows.Remove(dr);
                            i--;
                        }
                    }
                    List<string> snlist = new List<string>();
                    snlist = dt.Rows.Cast<DataRow>().Select(q => q["SERIAL_NUMBER"].ToString()).Distinct().ToList<string>();

                    DataTable dttemp = new DataTable();

                    foreach (DataColumn dc in dt.Columns)
                    {
                        dttemp.Columns.Add(new DataColumn(dc.ColumnName));
                    }

                    foreach (string item in snlist)
                    {

                        DataRow iteminfo = dt.Rows.Cast<DataRow>().Where(q => q["SERIAL_NUMBER"].ToString() == item).OrderBy(q => q["OUT_PROCESS_TIME"]).Last(q => q["SERIAL_NUMBER"].ToString() == item);

                        if (iteminfo["STATUS"].ToString() == "NG" || iteminfo["STATUS"].ToString().ToUpper() == "RETEST")
                        {
                            DataRow dr = dttemp.NewRow();
                            for (int i = 0; i < dttemp.Columns.Count; i++)
                            {
                                dr[i] = iteminfo[i];
                            }
                            dttemp.Rows.Add(dr);
                        }


                    }
                    var dtdefectcount = dttemp.Rows.Cast<DataRow>().GroupBy(q => new { defectcode = q["DEFECT_CODE"].ToString(), defectdesc = q["DEFECT_DESC"].ToString(), processname = q["PROCESS_NAME"] }).Select(g => new { defectItem = g.Key, defectcount = g.Count() });



                    DataTable dtt = new DataTable();

                    dtt.Columns.Add(new DataColumn("DEFECT_CODE"));
                    dtt.Columns.Add(new DataColumn("DEFECT_DESC"));
                    dtt.Columns.Add(new DataColumn("PROCESS_NAME"));
                    dtt.Columns.Add(new DataColumn("COUNT"));

                    foreach (var item in dtdefectcount)
                    {
                        DataRow dr = dtt.NewRow();
                        dr["DEFECT_CODE"] = item.defectItem.defectcode;
                        dr["DEFECT_DESC"] = item.defectItem.defectdesc;
                        dr["PROCESS_NAME"] = item.defectItem.processname;
                        dr["COUNT"] = item.defectcount;
                        dtt.Rows.Add(dr);
                    }

                    dtDefectFinalcount = dtt;
                    dtDefectFinalDetial = dttemp;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dt;
        }
        DataTable dtDefectFinalcount = null;
        DataTable dtDefectFinalDetial = null;


        private DataTable GetFinalMainReportData(DataTable maindt,DataTable finalfaildt)
        {

            //foreach (DataRow itemrow in maindt.Rows)
            //{
            //    itemrow["FAIL_QTY"] = 0;
            //}

            if (finalfaildt != null && finalfaildt.Rows.Count > 0)
            {
                var finalfaildata = finalfaildt.Rows.Cast<DataRow>().GroupBy(q => new { model = q["MODEL_NAME"].ToString(), pdlinename = q["PDLINE_NAME"].ToString(), processname = q["PROCESS_NAME"].ToString() }).Select(g => new { defectItem = g.Key, defectcount = g.Count() });
                if (finalfaildata.Count() > 0)
                {
                    foreach (var item in finalfaildata)
                    {
                        if (maindt.Rows.Cast<DataRow>().Count(q => q["MODEL_NAME"].ToString() == item.defectItem.model && q["PDLINE_NAME"].ToString() == item.defectItem.pdlinename && q["PROCESS_NAME"].ToString() == item.defectItem.processname)==0) continue;
                        DataRow dr = maindt.Rows.Cast<DataRow>().Single(q => q["MODEL_NAME"].ToString() == item.defectItem.model && q["PDLINE_NAME"].ToString() == item.defectItem.pdlinename && q["PROCESS_NAME"].ToString() == item.defectItem.processname);
                      
                        dr["FAIL_QTY"] = item.defectcount;
                    }
                }
            }
            float countpassrate = 1;
            if (maindt != null && maindt.Rows.Count > 0)
            {
                foreach (DataRow dr in maindt.Rows)
                {
                    float failqty = float.Parse(dr["FAIL_QTY"].ToString());
                    float passqty = float.Parse(dr["PASS_QTY"].ToString());
                    float outqty = float.Parse(dr["OUTPUT_QTY"].ToString());
                    dr["FPY"] = (passqty / (outqty + failqty)).ToString("P2");
                    dr["PASS_RATE"] = (outqty / (outqty + failqty)).ToString("P2");

                    string data = dr["PASS_RATE"].ToString().Remove(dr["PASS_RATE"].ToString().Length - 1, 1);
                    if (string.IsNullOrEmpty(data)) continue;
                    countpassrate *= float.Parse(data) / 100;
                }
            }
            DataRow drr = maindt.NewRow();
            drr["PASS_RATE"] = countpassrate.ToString("P2");
            maindt.Rows.Add(drr);


            return maindt;
        }

        //保存用户实际输入数据
        ParamsData paramsdata = null;

        private void btnQuery_Click(object sender, EventArgs e)
        {
            btnQuery.Text = "正在查询";
            btnQuery.Enabled = false;
            try
            {
                if (cmbModle.Items.Count == 0)
                {
                    MessageBox.Show("机种不能为空！", "消息提示:", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnQuery.Enabled = true;
                    btnQuery.Text = "查询";
                    return;
                }

                StringBuilder pdlineids = new StringBuilder();
                for (int i = 0; i < cmbPdline.Items.Count; i++)
                {
                    if (i == 0 && cmbPdline.GetItemsChecks(i))
                    {
                        pdlineids.Append("0");
                        break;
                    }
                    if (i > 0 && cmbPdline.GetItemsChecks(i))
                    {
                        pdlineids.Append("'" + pdlineData[cmbPdline.Items[i].ToString()] + "',");
                    }

                }
                if (pdlineids.Length > 1)
                    pdlineids.Remove(pdlineids.Length - 1, 1);
                if (pdlineids.Length == 0) { pdlineids.Append("0"); }


                StringBuilder processids = new StringBuilder();
                for (int i = 0; i < cmbProcess.Items.Count; i++)
                {
                    if (i == 0 && cmbProcess.GetItemsChecks(i))
                    {
                        processids.Append("0");
                        break;
                    }
                    if (i > 0 && cmbProcess.GetItemsChecks(i))
                    {
                        processids.Append("'" + processData[cmbProcess.Items[i].ToString()] + "',");
                    }

                }
                if (processids.Length > 1)
                    processids.Remove(processids.Length - 1, 1);
                if (processids.Length == 0) { processids.Append("0"); }


                paramsdata = new ParamsData(cmbModle.SelectedValue.ToString(), pdlineids.ToString(), processids.ToString(), txtWorkOrder.Text.Trim(), dtpStartDate.Value.Date.ToString("yyyyMMdd") + cmbStartTime.SelectedItem, dtpEndDate.Value.Date.ToString("yyyyMMdd") + cmbEndTime.SelectedItem);


                mainDataTable = GetMainReportData(paramsdata, 1);

                defectStatisticsDataTable = GetDefectStatisticsData(paramsdata, 2);
                detailDataTable = GetDetialData(paramsdata, 3);


                if (mainDataTable != null)
                {
                    dgvMainTable.DataSource = GetFinalMainReportData(mainDataTable, dtDefectFinalDetial);
                    dgvDefectStatistics.DataSource = defectStatisticsDataTable;
                    dgvDetial.DataSource = detailDataTable;
                    dgvSNdefectCount.DataSource = dtDefectFinalcount;
                }


                btnQuery.Enabled = true;
                btnQuery.Text = "查询";

            }
            catch (Exception ex) 
            {

                SajetCommon.Show_Message(ex.Message, 0);
                btnQuery.Enabled = true;
                btnQuery.Text = "查询";
            }

        }

        private void dgwMainTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!(e.RowIndex > -1))
            {
                return;
            }
            dgvDefectStatistics.DataSource = null;
            dgvDetial.DataSource = null;
            dgvSNdefectCount.DataSource = null;
            int defectcount = Convert.ToInt32(dgvMainTable.Rows[e.RowIndex].Cells["FAIL_QTY"].Value.ToString());
            if (defectcount == 0)
            {
                return;
            }
            string modelid = modelData[dgvMainTable.Rows[e.RowIndex].Cells["MODEL_NAME"].Value.ToString()];
            string pdlineid = pdlineData[dgvMainTable.Rows[e.RowIndex].Cells["PDLINE_NAME"].Value.ToString()];
            string processid = processData[dgvMainTable.Rows[e.RowIndex].Cells["PROCESS_NAME"].Value.ToString()];

            ParamsData pd = new ParamsData(modelid, pdlineid, processid, paramsdata.Work_order, paramsdata.Startdatetime, paramsdata.Enddatetime);

            defectStatisticsDataTable = GetDefectStatisticsData(pd, 2);
            dgvDefectStatistics.DataSource = defectStatisticsDataTable;

            detailDataTable = GetDetialData(pd, 3);
            dgvDetial.DataSource = detailDataTable;
            dgvSNdefectCount.DataSource = dtDefectFinalcount;

        }

        private void dgvDefectStatistics_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!(e.RowIndex > -1))
            {
                return;
            }
            dgvDetial.DataSource = null;
            dgvSNdefectCount.DataSource = null;
            string modelid = modelData[dgvDefectStatistics.Rows[e.RowIndex].Cells["MODEL_NAME"].Value.ToString()];
            string pdlineid = pdlineData[dgvDefectStatistics.Rows[e.RowIndex].Cells["PDLINE_NAME"].Value.ToString()];
            string processid = processData[dgvDefectStatistics.Rows[e.RowIndex].Cells["PROCESS_NAME"].Value.ToString()];

            ParamsData pd = new ParamsData(modelid, pdlineid, processid, paramsdata.Work_order, paramsdata.Startdatetime, paramsdata.Enddatetime);
            string terminal_name = dgvDefectStatistics.Rows[e.RowIndex].Cells["TERMINAL_NAME"].Value.ToString();
            pd.Terminal_id = ClientUtils.ExecuteSQL("select terminal_id from sajet.sys_terminal where terminal_name='" + terminal_name + "' and pdline_id='" + pdlineid + "'").Tables[0].Rows[0][0].ToString();

            pd.Defect_id = ClientUtils.ExecuteSQL("select defect_id from sajet.sys_defect where defect_code='" + dgvDefectStatistics.Rows[e.RowIndex].Cells["DEFECT_CODE"].Value.ToString() + "'").Tables[0].Rows[0][0].ToString();

            detailDataTable = GetDetialData(pd, 3);
            dgvDetial.DataSource = detailDataTable;
            dgvSNdefectCount.DataSource = dtDefectFinalcount;

        }

        //用于导出Excel
        DataGridView gvDataTable = null;

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
                gvDataTable = dgvMainTable;
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void dgvDefectStatistics_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right && e.RowIndex > -1)
            {
                gvDataTable = dgvDefectStatistics;
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void dgvDetial_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right && e.RowIndex > -1)
            {
                gvDataTable = dgvDetial;
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

        private void dgvSNdefectCount_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dgvSNdefectCount.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dgvSNdefectCount.RowHeadersDefaultCellStyle.Font, rectangle, dgvSNdefectCount.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            if ((e.RowIndex + 1) % 2 == 0)
                dgvSNdefectCount.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCyan;
            else
            {
                dgvSNdefectCount.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.AliceBlue;
            }
        }

        private void ckbCount_CheckedChanged(object sender, EventArgs e)
        {

        }
        Dictionary<string, string> dicpdline = new Dictionary<string, string>();
        private void cmbModle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (xdoc != null && cmbModle.SelectedValue != null)
            {

                var lines = xdoc.Descendants("Pdline").Where(q => q.Parent.Parent.Attribute("id").Value.ToString() == cmbModle.SelectedValue.ToString());
                dicpdline.Clear();
                foreach (var item in lines)
                {
                    dicpdline.Add(item.Attribute("name").Value.ToString(), item.Attribute("id").Value.ToString());
                }

                BindingSource bspdline = new BindingSource();
                bspdline.DataSource = dicpdline.Keys;
                cmbPdline.DataSource = bspdline;


                var routes = xdoc.Descendants("Route").Where(q => q.Parent.Parent.Attribute("id").Value.ToString() == cmbModle.SelectedValue.ToString());
                routeData.Clear();
                foreach (var item in routes)
                {
                    routeData.Add(item.Attribute("name").Value.ToString(), item.Attribute("id").Value.ToString());
                }

                BindingSource bsroute = new BindingSource();
                bsroute.DataSource = routeData;
                cmbRoute.DisplayMember = "Key";
                cmbRoute.ValueMember = "Value";
                cmbRoute.DataSource = bsroute;



                //var processs = xdoc.Descendants("Process").Where(q => q.Parent.Parent.Attribute("id").Value.ToString() == cmbModle.SelectedValue.ToString());
                //Dictionary<string, string> dicprocess = new Dictionary<string, string>();
                //foreach (var item in processs)
                //{
                //    dicprocess.Add(item.Attribute("name").Value.ToString(), item.Attribute("id").Value.ToString());
                //}

                //BindingSource bsprocess = new BindingSource();
                //bsprocess.DataSource = dicprocess;
                //cmbProcess.DisplayMember = "Key";
                //cmbProcess.ValueMember = "Value";
                //cmbProcess.DataSource = bsprocess;

            }
        }

        private void btnQueryConfig_Click(object sender, EventArgs e)
        {
            fDataConfig fdataConfig = new fDataConfig();
            fdataConfig.EnableCustomStyle = false;
            fdataConfig.ShowDialog();
        }

        private void dgvSNdefectCount_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex > -1)
            {
                gvDataTable = dgvSNdefectCount;
                contextMenuStrip2.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void ToolStripMenuItemFinalDefectDetial_Click(object sender, EventArgs e)
        {
            dgvSNdefectCount.DataSource = dtDefectFinalDetial;
            dgvSNdefectCount.Refresh();
        }

        private void ToolStripMenuItemFinalDefectCount_Click(object sender, EventArgs e)
        {
            dgvSNdefectCount.DataSource = dtDefectFinalcount;
            dgvSNdefectCount.Refresh();
        }
        Dictionary<string, string> dicprocess = new Dictionary<string, string>();
        private void cmbRoute_SelectedIndexChanged(object sender, EventArgs e)
        {


             DataTable dt = GetProcessDataByRouteId(cmbRoute.SelectedValue.ToString());
             dicprocess.Clear();
             foreach (DataRow item in dt.Rows)
             {
                 dicprocess.Add(item["PROCESS_NAME"].ToString(),item["PROCESS_ID"].ToString());
             }

             BindingSource bsprocess = new BindingSource();
             bsprocess.DataSource = dicprocess.Keys;
             cmbProcess.DataSource = bsprocess;

        }

        private void btnDownLoad_Click(object sender, EventArgs e)
        {
            string program = "Query";
            string path = Application.StartupPath + Path.DirectorySeparatorChar + program + Path.DirectorySeparatorChar;
            string destFile = path + "ModelData.xml";
            string sDir = Path.GetDirectoryName(destFile);
            if (!Directory.Exists(sDir))
                Directory.CreateDirectory(sDir);
            if (File.Exists(destFile))
                File.Delete(destFile);
            byte[] data = ClientUtils.remoteObject.DownloadFileByte(@"LoadClient\"+program, "ModelData.xml");
            FileStream localFS = new FileStream(destFile, FileMode.Create, FileAccess.Write);
            localFS.Write(data, 0, data.Length);
            localFS.Close();
        }


    }
}
