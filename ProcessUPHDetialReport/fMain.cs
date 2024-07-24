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
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Schedule;
using System.IO;
namespace ProcessUPHDetialReport
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

        #region OUTPUT_QTY
        string startSqlStr1 = "SELECT m.* FROM (SELECT h.model_name,h.pdline_name,h.process_name,h.work_date,h.work_time, "
                            + " sum(h.output_qty) as output_qty,h.route_id,h.process_id  FROM (SELECT E.MODEL_NAME, B.PDLINE_NAME,"
                            + "  C.PROCESS_NAME,  A.WORK_DATE || LPAD (A.WORK_TIME, 2, '0') AS WORK_DATE,  A.WORK_TIME,"
                            + " A.OUTPUT_QTY AS OUTPUT_QTY,d.route_id,c.process_id FROM SAJET.G_SN_COUNT A,SAJET.SYS_PDLINE B,SAJET.SYS_PROCESS C,"
                            + " SAJET.SYS_PART D, SAJET.SYS_MODEL E  WHERE     A.PART_ID = D.PART_ID   AND A.PDLINE_ID = B.PDLINE_ID "
                            + " AND A.PROCESS_ID = C.PROCESS_ID AND D.MODEL_ID = E.MODEL_ID";

        string endSqlStr1 = " ) H GROUP BY    MODEL_NAME,  PDLINE_NAME, PROCESS_NAME, WORK_DATE,  WORK_TIME, ROUTE_ID,process_id) M, "
                          + "  (SELECT NEXT_PROCESS_ID,SEQ,ROUTE_ID FROM( "
                          + "     SELECT A1.NEXT_PROCESS_ID,A1.SEQ,A1.ROUTE_ID FROM SAJET.SYS_ROUTE_DETAIL A1 WHERE  A1.SEQ=A1.STEP) UNION "
                          + "     SELECT DISTINCT(A2.NEXT_PROCESS_ID),0 AS SEQ,A2.ROUTE_ID FROM SAJET.SYS_ROUTE_DETAIL A2 WHERE  "
                          + "     A2.NEXT_PROCESS_ID='100003') N  "
                          + "      WHERE M.ROUTE_ID = N.ROUTE_ID AND M.PROCESS_ID=N.NEXT_PROCESS_ID  "
                          + "       ORDER BY N.SEQ ASC";

                          
        #endregion

        string startSqlStr2 = "SELECT  m3.model_name, "
      +"  m2.part_no, "
            +"   m1.work_order, "
            +"   m1.serial_number,  "
            +"   m5.pdline_name, "
            +"   m4.process_name,  "
            +"   m1.pallet_no, "
             + "  m1.carton_no, "
             +"  m1.out_process_time, "
             +"  TO_CHAR (m1.out_process_time, 'HH24') AS TIME "
+"  FROM (SELECT t1.part_id,t1.work_order, "
                             +" t1.serial_number, "
                             +" t1.pallet_no, "
                             +" t1.carton_no,"
                             +"  t1.pdline_id, "
              + "  t1.process_id, "
                             +" t1.out_process_time,"
                             +" TO_CHAR( t1.out_process_time,'HH24') AS TIME "
                             +" FROM SAJET.G_SN_travel t1 "
                             +" RIGHT JOIN "
                             + "( SELECT serial_number, MAX (out_process_time) ct "
                 +"   FROM SAJET.G_SN_travel t2,SAJET.SYS_PART t3,sajet.sys_model t4,SAJET.SYS_PDLINE t5,SAJET.SYS_PROCESS t6  "
                 +"  WHERE  T2.PART_ID=T3.PART_ID and T3.MODEL_ID=T4.MODEL_ID AND T2.PDLINE_ID=T5.PDLINE_ID AND T2.PROCESS_ID=T6.PROCESS_ID AND" ;
                           
  string endSqlStr2 =  "   GROUP BY serial_number) t2 "
                +"  ON     t2.serial_number = t1.serial_number   "
                +"     AND t2.ct = t1.out_process_time) m1,sajet.sys_part m2,sajet.sys_model m3,sajet.sys_process m4,sajet.sys_pdline m5 "
                +"     where m1.part_id=m2.part_id and m2.model_id=m3.model_id and m1.process_id=m4.process_id and m1.pdline_id=m5.pdline_id";

    


        /// <summary>
     
        /// <summary>
        /// 获取机种数据
        /// </summary>
        /// <returns>机种ID+机种名称</returns>
        private DataTable GetModelData()
        {
            DataTable dt = null;
            string strSql = "SELECT MODEL_ID,MODEL_NAME FROM SAJET.SYS_MODEL WHERE MODEL_ID IN(SELECT MODEL_ID FROM SAJET.SYS_GROUP_MODEL WHERE GROUP_ID IN (SELECT GROUP_ID FROM SAJET.SYS_GROUP_EMP WHERE EMP_ID='" +ClientUtils.UserPara1 + "')) ORDER BY MODEL_NAME";
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
        private DataTable GetStageData()
        {
            DataTable dt = null;
            string strSql = "SELECT DISTINCT(STAGE) FROM SAJET.SYS_PDLINE WHERE ENABLED='Y' AND STAGE IS NOT NULL ORDER BY STAGE ";
            DataSet ds = ClientUtils.ExecuteSQL(strSql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                DataRow dr = dt.NewRow();
               // dr["pdline_id"] = "0";
                dr["STAGE"] = "--请选择--";
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
            pdlineData.Clear();
            DataTable dt = null;
            string strSql = "SELECT PDLINE_ID,PDLINE_NAME FROM SAJET.SYS_PDLINE WHERE ENABLED='Y' AND STAGE='"+comStage.Text+"' ORDER BY PDLINE_NAME ";
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
            processData.Clear();
            DataTable dt = null;
            string strSql = "SELECT DISTINCT(A.PROCESS_ID),A.PROCESS_NAME FROM SAJET.SYS_PROCESS A,SAJET.SYS_TERMINAL B,SAJET.SYS_PDLINE C "
                            + "WHERE A.ENABLED='Y' AND A.PROCESS_ID=B.PROCESS_ID AND B.PDLINE_ID=C.PDLINE_ID AND C.PDLINE_NAME='" + cmbPdline.Text+ "' ORDER BY PROCESS_NAME";
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
            comStage.Items.Clear();
            DataTable dtStage = GetStageData();
            if (dtStage != null)
            {
                //comStage.DataSource = dtStage;
              //  comStage.DisplayMember = "STAGE";
                //cmbPdline.ValueMember = "PDLINE_ID";
               // comStage.SelectedIndex = 0;
                foreach (DataRow dr in dtStage.Rows)
                {
                    comStage.Items.Add(dr[0].ToString());
                }
                comStage.SelectedIndex = 0;
            }

        }

        DataTable dtsum = null;
     
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
                sql = sql + " AND C.PROCESS_ID='" + p.Process_id + "'";
            }

            if (!string.IsNullOrEmpty(p.Startdatetime) && p.Startdatetime != "0" && !string.IsNullOrEmpty(p.Enddatetime) && p.Enddatetime != "0")
            {
                sql = sql + " AND A.WORK_DATE || LPAD (A.WORK_TIME, 2, '0')>=('" + p.Startdatetime + "') AND A.WORK_DATE || LPAD (A.WORK_TIME, 2, '0')<('" + p.Enddatetime + "') ";
            }

            if (!string.IsNullOrEmpty(p.Model_id) && p.Model_id != "0")
            {
                sql = sql + "  AND E.MODEL_ID = '" + p.Model_id + "'";
            }
            else
            {

            }
            sql = sql + endSqlStr1;
            DataSet ds = ClientUtils.ExecuteSQL(sql, Params);
            if (ds.Tables[0].Rows.Count > 0)
            {

                dt = ds.Tables[0];

                DateTime dtime = ClientUtils.GetSysDate();
                var dtdefectcount = dt.Rows.Cast<DataRow>().GroupBy(q => new { defectcode = q["MODEL_NAME"].ToString(), defectdesc = q["PDLINE_NAME"].ToString(), processname = q["PROCESS_NAME"] }).Select(g => new { defectItem = g.Key, defectcount = g.Count() });
                ////  tspan = dtime - Convert.ToDateTime(dt.Rows[0]["OUT_PROCESS_TIME"].ToString());

                dt.Columns.Add(new DataColumn("08:00-09:00"));
                dt.Columns.Add(new DataColumn("09:00-10:00"));
                dt.Columns.Add(new DataColumn("10:00-11:00"));
                dt.Columns.Add(new DataColumn("11:00-12:00"));
                dt.Columns.Add(new DataColumn("12:00-13:00"));
                dt.Columns.Add(new DataColumn("13:00-14:00"));
                dt.Columns.Add(new DataColumn("14:00-15:00"));
                dt.Columns.Add(new DataColumn("15:00-16:00"));
                dt.Columns.Add(new DataColumn("16:00-17:00"));
                dt.Columns.Add(new DataColumn("17:00-18:00"));
                dt.Columns.Add(new DataColumn("18:00-19:00"));
                dt.Columns.Add(new DataColumn("19:00-20:00"));
                dt.Columns.Add(new DataColumn("20:00-21:00"));
                dt.Columns.Add(new DataColumn("21:00-22:00"));
                dt.Columns.Add(new DataColumn("22:00-23:00"));
                dt.Columns.Add(new DataColumn("23:00-00:00"));
                dt.Columns.Add(new DataColumn("00:00-01:00"));
                dt.Columns.Add(new DataColumn("01:00-02:00"));
                dt.Columns.Add(new DataColumn("02:00-03:00"));
                dt.Columns.Add(new DataColumn("03:00-04:00"));
                dt.Columns.Add(new DataColumn("04:00-05:00"));
                dt.Columns.Add(new DataColumn("05:00-06:00"));
                dt.Columns.Add(new DataColumn("06:00-07:00"));
                dt.Columns.Add(new DataColumn("07:00-08:00"));


                foreach (DataRow item in dt.Rows)
                {
                    if (item[4].ToString() == "8")
                    {
                        item["08:00-09:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["08:00-09:00"] = 0;
                    }

                    if (item[4].ToString() == "9")
                    {
                        item["09:00-10:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["09:00-10:00"] = 0;
                    }

                    if (item[4].ToString() == "10")
                    {
                        item["10:00-11:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["10:00-11:00"] = 0;
                    }

                    if (item[4].ToString() == "11")
                    {
                        item["11:00-12:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["11:00-12:00"] = 0;
                    }

                    if (item[4].ToString() == "12")
                    {
                        item["12:00-13:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["12:00-13:00"] = 0;
                    }

                    if (item[4].ToString() == "13")
                    {
                        item["13:00-14:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["13:00-14:00"] = 0;
                    }

                    if (item[4].ToString() == "14")
                    {
                        item["14:00-15:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["14:00-15:00"] = 0;
                    }

                    if (item[4].ToString() == "15")
                    {
                        item["15:00-16:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["15:00-16:00"] = 0;
                    }

                    if (item[4].ToString() == "16")
                    {
                        item["16:00-17:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["16:00-17:00"] = 0;
                    }

                    if (item[4].ToString() == "17")
                    {
                        item["17:00-18:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["17:00-18:00"] = 0;
                    }

                    if (item[4].ToString() == "18")
                    {
                        item["18:00-19:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["18:00-19:00"] = 0;
                    }

                    if (item[4].ToString() == "19")
                    {
                        item["19:00-20:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["19:00-20:00"] = 0;
                    }

                    if (item[4].ToString() == "20")
                    {
                        item["20:00-21:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["20:00-21:00"] = 0;
                    }

                    if (item[4].ToString() == "21")
                    {
                        item["21:00-22:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["21:00-22:00"] = 0;
                    }

                    if (item[4].ToString() == "22")
                    {
                        item["22:00-23:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["22:00-23:00"] = 0;
                    }

                    if (item[4].ToString() == "23")
                    {
                        item["23:00-00:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["23:00-00:00"] = 0;
                    }

                    if (item[4].ToString() == "0")
                    {
                        item["00:00-01:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["00:00-01:00"] = 0;
                    }

                    if (item[4].ToString() == "1")
                    {
                        item["01:00-02:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["01:00-02:00"] = 0;
                    }

                    if (item[4].ToString() == "2")
                    {
                        item["02:00-03:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["02:00-03:00"] = 0;
                    }

                    if (item[4].ToString() == "3")
                    {
                        item["03:00-04:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["03:00-04:00"] = 0;
                    }

                    if (item[4].ToString() == "4")
                    {
                        item["04:00-05:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["04:00-05:00"] = 0;
                    }

                    if (item[4].ToString() == "5")
                    {
                        item["05:00-06:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["05:00-06:00"] = 0;
                    }

                    if (item[4].ToString() == "6")
                    {
                        item["06:00-07:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["06:00-07:00"] = 0;
                    }

                    if (item[4].ToString() == "7")
                    {
                        item["07:00-08:00"] = item[5].ToString();
                    }
                    else
                    {
                        item["07:00-08:00"] = 0;
                    }

                }
                dtsum = dt;
                DataTable dtt = new DataTable();
                dtt.Columns.Add(new DataColumn("MODEL_NAME"));

                dtt.Columns.Add(new DataColumn("PDLINE_NAME"));

                dtt.Columns.Add(new DataColumn("PROCESS_NAME"));

                dtt.Columns.Add(new DataColumn("08:00-09:00"));

                dtt.Columns.Add(new DataColumn("09:00-10:00"));

                dtt.Columns.Add(new DataColumn("10:00-11:00"));

                dtt.Columns.Add(new DataColumn("11:00-12:00"));

                dtt.Columns.Add(new DataColumn("12:00-13:00"));

                dtt.Columns.Add(new DataColumn("13:00-14:00"));

                dtt.Columns.Add(new DataColumn("14:00-15:00"));

                dtt.Columns.Add(new DataColumn("15:00-16:00"));

                dtt.Columns.Add(new DataColumn("16:00-17:00"));

                dtt.Columns.Add(new DataColumn("17:00-18:00"));

                dtt.Columns.Add(new DataColumn("18:00-19:00"));

                dtt.Columns.Add(new DataColumn("19:00-20:00"));

                dtt.Columns.Add(new DataColumn("20:00-21:00"));

                dtt.Columns.Add(new DataColumn("21:00-22:00"));

                dtt.Columns.Add(new DataColumn("22:00-23:00"));

                dtt.Columns.Add(new DataColumn("23:00-00:00"));

                dtt.Columns.Add(new DataColumn("00:00-01:00"));

                dtt.Columns.Add(new DataColumn("01:00-02:00"));

                dtt.Columns.Add(new DataColumn("02:00-03:00"));

                dtt.Columns.Add(new DataColumn("03:00-04:00"));

                dtt.Columns.Add(new DataColumn("04:00-05:00"));

                dtt.Columns.Add(new DataColumn("05:00-06:00"));

                dtt.Columns.Add(new DataColumn("06:00-07:00"));

                dtt.Columns.Add(new DataColumn("07:00-08:00"));

                dtt.Columns.Add(new DataColumn("Daily CUM Qutput"));

               dtt.Columns.Add(new DataColumn("DS Avg UFH"));

               dtt.Columns.Add(new DataColumn("NS Avg UFH"));

               dtt.Columns.Add(new DataColumn("Avg UPH")); 

               dtt.Columns.Add(new DataColumn("UPH Target"));

               dtt.Columns.Add(new DataColumn("CUM WIP"));

               dtt.Columns.Add(new DataColumn("WIP Alert"));

                foreach (var item in dtdefectcount)
                {
                    DataRow dr = dtt.NewRow();

                    dr["MODEL_NAME"] = item.defectItem.defectcode;

                    dr["PDLINE_NAME"] = item.defectItem.defectdesc;

                    dr["PROCESS_NAME"] = item.defectItem.processname;

                    dr["08:00-09:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["08:00-09:00"].ToString()));

                    dr["09:00-10:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["09:00-10:00"].ToString()));

                    dr["10:00-11:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["10:00-11:00"].ToString()));

                    dr["11:00-12:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["11:00-12:00"].ToString()));

                    dr["12:00-13:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["12:00-13:00"].ToString()));

                    dr["13:00-14:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["13:00-14:00"].ToString()));

                    dr["14:00-15:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["14:00-15:00"].ToString()));

                    dr["15:00-16:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["15:00-16:00"].ToString()));

                    dr["16:00-17:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["16:00-17:00"].ToString()));

                    dr["17:00-18:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["17:00-18:00"].ToString()));

                    dr["18:00-19:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["18:00-19:00"].ToString()));

                    dr["19:00-20:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["19:00-20:00"].ToString()));

                    dr["20:00-21:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["20:00-21:00"].ToString()));

                    dr["21:00-22:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["21:00-22:00"].ToString()));

                    dr["22:00-23:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["22:00-23:00"].ToString()));

                    dr["23:00-00:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["23:00-00:00"].ToString()));

                    dr["00:00-01:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["00:00-01:00"].ToString()));

                    dr["01:00-02:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["01:00-02:00"].ToString()));

                    dr["02:00-03:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["02:00-03:00"].ToString()));

                    dr["03:00-04:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["03:00-04:00"].ToString()));

                    dr["04:00-05:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["04:00-05:00"].ToString()));

                    dr["05:00-06:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["05:00-06:00"].ToString()));

                    dr["06:00-07:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["06:00-07:00"].ToString()));

                    dr["07:00-08:00"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["07:00-08:00"].ToString()));

                    dr["Daily CUM Qutput"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["OUTPUT_QTY"].ToString()));

                    dr["DS Avg UFH"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString() && Convert.ToInt32(q["WORK_TIME"].ToString()) >= 8 && Convert.ToInt32(q["WORK_TIME"].ToString()) < 20).Sum(s =>Double.Parse(s["OUTPUT_QTY"].ToString()))/10;

                    dr["NS Avg UFH"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString() && (Convert.ToInt32(q["WORK_TIME"].ToString()) >= 20 || Convert.ToInt32(q["WORK_TIME"].ToString()) < 8)).Sum(s => Double.Parse(s["OUTPUT_QTY"].ToString())) / 10;

                    dr["Avg UPH"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => Double.Parse(s["OUTPUT_QTY"].ToString())) / 20;

                    dr["UPH Target"] = "";

                    dr["CUM WIP"] = "";

                    dr["WIP Alert"] = "";

                    dtt.Rows.Add(dr);
                  

                }

                dt = dtt;
            }
            return dt;
        }

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

            if (comStage.Text == "--请选择--")
            {
                MessageBox.Show("区域不能为空！", "消息提示:", MessageBoxButtons.OK, MessageBoxIcon.Information);
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



            paramsdata = new ParamsData(cmbModle.SelectedValue.ToString(), cmbPdline.SelectedValue == null ? "0" : cmbPdline.SelectedValue.ToString(), cmbProcess.SelectedValue == null ? "0" : cmbProcess.SelectedValue.ToString(), txtWorkOrder.Text.Trim(), dtpStartDate.Value.Date.ToString("yyyyMMdd") + cmbStartTime.SelectedItem, dtpEndDate.Value.Date.ToString("yyyyMMdd") + cmbEndTime.SelectedItem);
            mainDataTable = GetMainReportData(paramsdata, 1);
            dgvMainTable.DataSource = mainDataTable;
            btnQuery.Enabled = true;
            btnQuery.Text = "查询";
            return;

        }

        private void dgvMainTable_DoubleClick(object sender, EventArgs e)
        {    
            int a = dgvMainTable.RowCount;
            int b = dgvMainTable.ColumnCount;
       //     dgvMainTable2.DataSource = null;
    
           // dgvMainTable.Rows.Count;    //获取总行数：
            int i = this.dgvMainTable.CurrentRow.Index;             //获取当前选中行索引：
            int j = this.dgvMainTable.CurrentCell.ColumnIndex;           //获取当前选中列索引：
           string smodel=dgvMainTable.Rows[i].Cells[0].Value.ToString();
           string spdline = dgvMainTable.Rows[i].Cells[1].Value.ToString();
           string sprocess = dgvMainTable.Rows[i].Cells[2].Value.ToString();
           string sdate= dgvMainTable.Columns[j].HeaderText;
    


           DataTable dtOutput = null;
           string sql2 = "";
           object[][] Params2 = null;
           sql2 = startSqlStr2 + " t2.OUT_PROCESS_TIME>=TO_DATE('" + dtpStartDate.Value.Date.ToString("yyyyMMdd") + cmbStartTime.SelectedItem + "','YYYYMMDDHH24') " +" AND t2.OUT_PROCESS_TIME<TO_DATE('"+dtpEndDate.Value.Date.ToString("yyyyMMdd") + cmbEndTime.SelectedItem+"','YYYYMMDDHH24')" + " AND  t4.model_name ='" + smodel + "' AND t5.PDLINE_name = '" + spdline + "' AND t6.process_name='" + sprocess + "'";
           if (!string.IsNullOrEmpty(txtWorkOrder.Text) && txtWorkOrder.Text != "0")
           {
               sql2 = sql2 + " AND T2.WORK_ORDER='" + txtWorkOrder.Text + "' ";
           }
         
           sql2 = sql2 + endSqlStr2;
           DataSet ds2 = ClientUtils.ExecuteSQL(sql2, Params2);
           if (ds2.Tables[0].Rows.Count > 0)
           {
               dtOutput = ds2.Tables[0];
           }

           DataTable dtdetial2 = new DataTable();
           dtdetial2.Columns.Add(new DataColumn("MODEL_NAME"));

           dtdetial2.Columns.Add(new DataColumn("PART_NO"));

           dtdetial2.Columns.Add(new DataColumn("WORK_ORDER"));

          

           dtdetial2.Columns.Add(new DataColumn("SERIAL_NUMBER"));

           dtdetial2.Columns.Add(new DataColumn("PDLINE_NAME"));

           dtdetial2.Columns.Add(new DataColumn("PROCESS_NAME"));

           dtdetial2.Columns.Add(new DataColumn("PALLET_NO"));

           dtdetial2.Columns.Add(new DataColumn("CARTON_NO"));

           dtdetial2.Columns.Add(new DataColumn("OUT_PROCESS_TIME"));

                   switch (j)
                   {
                       case 3:
                           //var itemdetial3 = dtOutput.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == smodel && q["PDLINE_NAME"].ToString() == spdline && q["PROCESS_NAME"].ToString() == sprocess && q["time"].ToString() == "08");
                           var itemdetial3 = dtOutput.Rows.Cast<DataRow>().Where(q => q["time"].ToString() == "08");
                           foreach (var item in itemdetial3) 
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();
                               
                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                              // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                             //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 4:
                           var itemdetial4 = dtOutput.Rows.Cast<DataRow>().Where(q => q["time"].ToString() == "09");
                           foreach (var item in itemdetial4)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 5:
                           var itemdetial5 = dtOutput.Rows.Cast<DataRow>().Where(q => q["time"].ToString() == "10");
                           foreach (var item in itemdetial5)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 6:
                           var itemdetial6 = dtOutput.Rows.Cast<DataRow>().Where(q =>  q["time"].ToString() == "11");
                           foreach (var item in itemdetial6)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 7:
                           var itemdetial7 = dtOutput.Rows.Cast<DataRow>().Where(q =>  q["time"].ToString() == "12");
                           foreach (var item in itemdetial7)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 8:
                           var itemdetial8 = dtOutput.Rows.Cast<DataRow>().Where(q =>  q["time"].ToString() == "13");
                           foreach (var item in itemdetial8)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 9:
                           var itemdetial9 = dtOutput.Rows.Cast<DataRow>().Where(q =>  q["time"].ToString() == "14");
                           foreach (var item in itemdetial9)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 10:
                           var itemdetial10 = dtOutput.Rows.Cast<DataRow>().Where(q => q["time"].ToString() == "15");
                           foreach (var item in itemdetial10)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 11:
                           var itemdetial11 = dtOutput.Rows.Cast<DataRow>().Where(q =>  q["time"].ToString() == "16");
                           foreach (var item in itemdetial11)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 12:
                           var itemdetial12 = dtOutput.Rows.Cast<DataRow>().Where(q =>  q["time"].ToString() == "17");
                           foreach (var item in itemdetial12)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 13:
                           var itemdetial13 = dtOutput.Rows.Cast<DataRow>().Where(q =>  q["time"].ToString() == "18");
                           foreach (var item in itemdetial13)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 14:
                           var itemdetial14 = dtOutput.Rows.Cast<DataRow>().Where(q =>q["time"].ToString() == "19");
                           foreach (var item in itemdetial14)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 15:
                           var itemdetial15 = dtOutput.Rows.Cast<DataRow>().Where(q =>  q["time"].ToString() == "20");
                           foreach (var item in itemdetial15)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 16:
                           var itemdetial16 = dtOutput.Rows.Cast<DataRow>().Where(q =>  q["time"].ToString() == "21");
                           foreach (var item in itemdetial16)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 17:
                           var itemdetial17 = dtOutput.Rows.Cast<DataRow>().Where(q => q["time"].ToString() == "22");
                           foreach (var item in itemdetial17)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 18:
                           var itemdetial18 = dtOutput.Rows.Cast<DataRow>().Where(q => q["time"].ToString() == "23");
                           foreach (var item in itemdetial18)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 19:
                           var itemdetial19 = dtOutput.Rows.Cast<DataRow>().Where(q =>  q["time"].ToString() == "00");
                           foreach (var item in itemdetial19)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 20:
                           var itemdetial20 = dtOutput.Rows.Cast<DataRow>().Where(q =>  q["time"].ToString() == "01");
                           foreach (var item in itemdetial20)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 21:
                           var itemdetial21 = dtOutput.Rows.Cast<DataRow>().Where(q => q["time"].ToString() == "02");
                           foreach (var item in itemdetial21)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 22:
                           var itemdetial22 = dtOutput.Rows.Cast<DataRow>().Where(q => q["time"].ToString() == "03");
                           foreach (var item in itemdetial22)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 23:
                           var itemdetial23 = dtOutput.Rows.Cast<DataRow>().Where(q =>  q["time"].ToString() == "04");
                           foreach (var item in itemdetial23)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 24:
                           var itemdetial24 = dtOutput.Rows.Cast<DataRow>().Where(q =>  q["time"].ToString() == "05");
                           foreach (var item in itemdetial24)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 25:
                           var itemdetial25 = dtOutput.Rows.Cast<DataRow>().Where(q => q["time"].ToString() == "06");
                           foreach (var item in itemdetial25)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                       case 26:
                           var itemdetial26 = dtOutput.Rows.Cast<DataRow>().Where(q => q["time"].ToString() == "07");
                           foreach (var item in itemdetial26)
                           {
                               DataRow dr = dtdetial2.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();

                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               // dr["CURRENT_STATUS"] = item["CURRENT_STATUS"].ToString();
                               //  dr["WORK_FLAG"] = item["WORK_FLAG"].ToString();
                               dr["PALLET_NO"] = item["PALLET_NO"].ToString();
                               dr["CARTON_NO"] = item["CARTON_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial2.Rows.Add(dr);
                           }
                           break;
                    
                       default: break;
                   }

                   dgvMainTable2.DataSource = dtdetial2;
           btnQuery.Enabled = true;
           btnQuery.Text = "查询";
           return;


        }

        private void buttonX1_Click(object sender, EventArgs e)
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

            if (comStage.Text == "--请选择--")
            {
                MessageBox.Show("区域不能为空！", "消息提示:", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

          

            paramsdata = new ParamsData(cmbModle.SelectedValue.ToString(), cmbPdline.SelectedValue == null ? "0" : cmbPdline.SelectedValue.ToString(), cmbProcess.SelectedValue == null ? "0" : cmbProcess.SelectedValue.ToString(), txtWorkOrder.Text.Trim(), dtpStartDate.Value.Date.ToString("yyyyMMdd") + cmbStartTime.SelectedItem, dtpEndDate.Value.Date.ToString("yyyyMMdd") + cmbEndTime.SelectedItem);
            mainDataTable = GetMainReportData(paramsdata, 1);
            dgvMainTable.DataSource = mainDataTable;
            btnQuery.Enabled = true;
            btnQuery.Text = "查询";
            return;
        }

        private void buttonX1_Click_1(object sender, EventArgs e)
        {
           
        }

        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void 导出ToolStripMenuItem1_Click(object sender, EventArgs e)
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
                MessageBox.Show("導出數據成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }
        }

        private void comStage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comStage.Text != "--请选择--")
            {
         
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
                //cmbProcess.Items.Add("--请选择--");
               // cmbProcess.SelectedIndex = 0;
            }
        }

        private void cmbPdline_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPdline.Text != "--请选择--")
            {
                DataTable dtProcess = GetProcessData();
                if (dtProcess != null)
                {
                    cmbProcess.DataSource = dtProcess;
                    cmbProcess.DisplayMember = "PROCESS_NAME";
                    cmbProcess.ValueMember = "PROCESS_ID";
                    foreach (DataRow dr in dtProcess.Rows)
                    {
                        processData.Add(dr[1].ToString(), dr[0].ToString());
                    }
                }
            }
        }

  

    }
}
