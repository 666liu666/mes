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
namespace ProcessWipDetialReport
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
        string startSqlStr1 = "SELECT g.* FROM (  SELECT E.MODEL_NAME,A.WORK_ORDER,A.SERIAL_NUMBER,c.pdline_name,D.PROCESS_NAME,A.WIP_QTY,B.PART_NO,"
                  +" A.OUT_PROCESS_TIME,"
                  +" D.PROCESS_ID, "
                  + " B.PART_ID,A.ROUTE_ID "
             +" FROM sajet.g_sn_status a, "
                  +" SAJET.SYS_PART b,"
                  +" SAJET.SYS_PDLINE c,"
                  +" SAJET.SYS_PROCESS d,"
                  +" SAJET.SYS_MODEL e "
           +"  WHERE     A.PART_ID = B.PART_ID  "
                  +" AND a.wip_process <> 0"
                  +" AND A.WIP_PROCESS = d.process_id "
                  + " AND A.PDLINE_ID = C.PDLINE_ID "
                  +" AND B.MODEL_ID = E.MODEL_ID";

        string endSqlStr1 = " ) g,  (SELECT NEXT_PROCESS_ID, SEQ, ROUTE_ID "
          + "  FROM (SELECT A1.NEXT_PROCESS_ID, A1.SEQ, A1.ROUTE_ID "
           + "         FROM SAJET.SYS_ROUTE_DETAIL A1  "
           + "        WHERE A1.SEQ = A1.STEP)   "
         + " UNION   "
          + "SELECT DISTINCT (A2.NEXT_PROCESS_ID), 0 AS SEQ, A2.ROUTE_ID  "
          + "  FROM SAJET.SYS_ROUTE_DETAIL A2  "
         + "  WHERE A2.NEXT_PROCESS_ID = '100003') h WHERE     g.ROUTE_ID = h.route_id  AND g.process_id = h.NEXT_PROCESS_ID  ORDER BY h.seq ASC";

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
            processData.Clear();
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
                sql = sql + " AND A.WORK_ORDER='"+p.Work_order+"' ";
            }

            if (!string.IsNullOrEmpty(p.Pdline_id) && p.Pdline_id != "0")
            {
                sql = sql + " AND C.PDLINE_ID='" + p.Pdline_id+ "'";
            }

            if (!string.IsNullOrEmpty(p.Process_id) && p.Process_id != "0")
            {
                sql = sql + " AND D.PROCESS_ID='" + p.Process_id + "'";
            }

             if (!string.IsNullOrEmpty(p.Startdatetime) && p.Startdatetime != "0" && !string.IsNullOrEmpty(p.Enddatetime)&&p.Enddatetime!="0")
            {
                sql = sql + " AND A.OUT_PROCESS_TIME>=to_date('" + p.Startdatetime + "','YYYYMMDDHH24') AND A.OUT_PROCESS_TIME<TO_DATE('" + p.Enddatetime + "','YYYYMMDDHH24') ";
            }

            if (!string.IsNullOrEmpty(p.Model_id) && p.Model_id != "0")
            {
                sql = sql + "  AND E.MODEL_ID = '"+p.Model_id+"'";
            }
            else
            {
                
            }
            sql = sql+endSqlStr1;
            DataSet ds = ClientUtils.ExecuteSQL(sql, Params);
            if (ds.Tables[0].Rows.Count > 0)
            {
                
                dt = ds.Tables[0];
                
                DateTime dtime = ClientUtils.GetSysDate();
               var dtdefectcount = dt.Rows.Cast<DataRow>().GroupBy(q => new { defectcode = q["MODEL_NAME"].ToString(), defectdesc = q["PDLINE_NAME"].ToString(), processname = q["PROCESS_NAME"] }).Select(g => new { defectItem = g.Key, defectcount = g.Count() });
              ////  tspan = dtime - Convert.ToDateTime(dt.Rows[0]["OUT_PROCESS_TIME"].ToString());

                dt.Columns.Add(new DataColumn("0<WIP<=0.5H"));
                dt.Columns.Add(new DataColumn("0.5H<WIP<=1H"));
                dt.Columns.Add(new DataColumn("1H<WIP<=2H"));
                dt.Columns.Add(new DataColumn("2H<WIP<=3H"));
                dt.Columns.Add(new DataColumn("3H<WIP<=4H"));
                dt.Columns.Add(new DataColumn("4H<WIP"));

                foreach (DataRow item in dt.Rows) 
                {
                    TimeSpan ts=new TimeSpan();
                    ts = dtime - Convert.ToDateTime(item["OUT_PROCESS_TIME"].ToString());
                    double timeMinutes = ts.TotalMinutes;
                    
                    if (timeMinutes > 0 && timeMinutes <= 30)
                    {
                        item["0<WIP<=0.5H"] = 1;
                        item["0.5H<WIP<=1H"] = 0;
                        item["1H<WIP<=2H"] = 0;
                        item["2H<WIP<=3H"] = 0;
                        item["3H<WIP<=4H"] = 0;
                        item["4H<WIP"] = 0;
                    }

                    if (timeMinutes > 30 && timeMinutes <= 60) 
                    {
                        item["0<WIP<=0.5H"] = 0;
                        item["0.5H<WIP<=1H"] = 1;
                        item["1H<WIP<=2H"] = 0;
                        item["2H<WIP<=3H"] = 0;
                        item["3H<WIP<=4H"] = 0;
                        item["4H<WIP"] = 0;
                    }
                    else if (timeMinutes > 60 && timeMinutes <= 120)
                    {
                        item["0<WIP<=0.5H"] = 0;
                        item["0.5H<WIP<=1H"] = 0;
                        item["1H<WIP<=2H"] = 1;
                        item["2H<WIP<=3H"] = 0;
                        item["3H<WIP<=4H"] = 0;
                        item["4H<WIP"] = 0;
                    }
                    else if (timeMinutes > 120 && timeMinutes <= 180) 
                    {
                        item["0<WIP<=0.5H"] = 0;
                        item["0.5H<WIP<=1H"] = 0;
                        item["1H<WIP<=2H"] = 0;
                        item["2H<WIP<=3H"] = 1;
                        item["3H<WIP<=4H"] = 0;
                        item["4H<WIP"] = 0;
                    }
                    else if (timeMinutes > 180 && timeMinutes <= 240)
                    {
                        item["0<WIP<=0.5H"] = 0;
                        item["0.5H<WIP<=1H"] = 0;
                        item["1H<WIP<=2H"] = 0;
                        item["2H<WIP<=3H"] = 0;
                        item["3H<WIP<=4H"] = 1;
                        item["4H<WIP"] = 0;
                    }
                    else if (timeMinutes >= 240)
                    {
                        item["0<WIP<=0.5H"] = 0;
                        item["0.5H<WIP<=1H"] = 0;
                        item["1H<WIP<=2H"] = 0;
                        item["2H<WIP<=3H"] = 0;
                        item["3H<WIP<=4H"] = 0;
                        item["4H<WIP"] = 1;
                    }
                }
                dtsum = dt;

                DataTable dtt = new DataTable();

                dtt.Columns.Add(new DataColumn("MODEL_NAME"));

                dtt.Columns.Add(new DataColumn("PDLINE_NAME"));

                dtt.Columns.Add(new DataColumn("PROCESS_NAME"));

                dtt.Columns.Add(new DataColumn("WIP"));

                dtt.Columns.Add(new DataColumn("0<WIP<=0.5H"));

                dtt.Columns.Add(new DataColumn("0.5H<WIP<=1H"));

                dtt.Columns.Add(new DataColumn("1H<WIP<=2H"));

                dtt.Columns.Add(new DataColumn("2H<WIP<=3H"));

                dtt.Columns.Add(new DataColumn("3H<WIP<=4H"));

                dtt.Columns.Add(new DataColumn("4H<WIP"));

                foreach (var item in dtdefectcount)
                {

                    DataRow dr = dtt.NewRow();

                    dr["MODEL_NAME"] = item.defectItem.defectcode;

                    dr["PDLINE_NAME"] = item.defectItem.defectdesc;

                    dr["PROCESS_NAME"] = item.defectItem.processname;

                    dr["WIP"] = item.defectcount;

                    dr["0<WIP<=0.5H"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["0<WIP<=0.5H"].ToString()));
                    dr["0.5H<WIP<=1H"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["0.5H<WIP<=1H"].ToString()));
                    dr["1H<WIP<=2H"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["1H<WIP<=2H"].ToString()));
                    dr["2H<WIP<=3H"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["2H<WIP<=3H"].ToString()));
                    dr["3H<WIP<=4H"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["3H<WIP<=4H"].ToString()));
                    dr["4H<WIP"] = dt.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == item.defectItem.defectcode && q["PDLINE_NAME"].ToString() == item.defectItem.defectdesc && q["PROCESS_NAME"].ToString() == item.defectItem.processname.ToString()).Sum(s => int.Parse(s["4H<WIP"].ToString()));
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
           string swip= dgvMainTable.Columns[j].HeaderText;

           //paramsdata = new ParamsData(cmbModle.SelectedValue.ToString(), cmbPdline.SelectedValue.ToString(), cmbProcess.SelectedValue.ToString(), txtWorkOrder.Text.Trim(), dtpStartDate.Value.Date.ToString("yyyyMMdd") + cmbStartTime.SelectedItem, dtpEndDate.Value.Date.ToString("yyyyMMdd") + cmbEndTime.SelectedItem);
           //mainDataTable = GetReportData(paramsdata,smodel,spdline,sprocess,swip,1);


           DataTable dtdetial = new DataTable();
           dtdetial.Columns.Add(new DataColumn("MODEL_NAME"));

           dtdetial.Columns.Add(new DataColumn("WORK_ORDER"));

           dtdetial.Columns.Add(new DataColumn("SERIAL_NUMBER"));

           dtdetial.Columns.Add(new DataColumn("PDLINE_NAME"));

           dtdetial.Columns.Add(new DataColumn("PROCESS_NAME"));

           dtdetial.Columns.Add(new DataColumn("WIP_QTY"));

           dtdetial.Columns.Add(new DataColumn("PART_NO"));

           dtdetial.Columns.Add(new DataColumn("OUT_PROCESS_TIME"));
                   switch (j)
                   {
                       case 3:
                           var itemdetial = dtsum.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == smodel && q["PDLINE_NAME"].ToString() == spdline && q["PROCESS_NAME"].ToString() == sprocess);
                           foreach (var item in itemdetial) 
                           {
                               DataRow dr = dtdetial.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               dr["WIP_QTY"] = item["WIP_QTY"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial.Rows.Add(dr);
                           }
                           break;
                       case 4:
                           var itemdetial1 = dtsum.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == smodel && q["PDLINE_NAME"].ToString() == spdline && q["PROCESS_NAME"].ToString() == sprocess && q["0<WIP<=0.5H"].ToString() == Convert.ToString(1));
                           foreach (var item in itemdetial1) 
                           {
                               DataRow dr = dtdetial.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               dr["WIP_QTY"] = item["WIP_QTY"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial.Rows.Add(dr);
                           }
                           break;
                       case 5:
                           var itemdetial2 = dtsum.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == smodel && q["PDLINE_NAME"].ToString() == spdline && q["PROCESS_NAME"].ToString() == sprocess && q["0.5H<WIP<=1H"].ToString() == Convert.ToString(1));
                           foreach (var item in itemdetial2) 
                           {
                               DataRow dr = dtdetial.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               dr["WIP_QTY"] = item["WIP_QTY"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial.Rows.Add(dr);
                           }
                           break;
                       case 6:
                           var itemdetial3 = dtsum.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == smodel && q["PDLINE_NAME"].ToString() == spdline && q["PROCESS_NAME"].ToString() == sprocess && q["1H<WIP<=2H"].ToString() ==Convert.ToString(1));
                           foreach (var item in itemdetial3) 
                           {
                               DataRow dr = dtdetial.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               dr["WIP_QTY"] = item["WIP_QTY"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial.Rows.Add(dr);
                           }
                           break;
                       case 7:
                           var itemdetial4 = dtsum.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == smodel && q["PDLINE_NAME"].ToString() == spdline && q["PROCESS_NAME"].ToString() == sprocess && q["2H<WIP<=3H"].ToString() == Convert.ToString(1));
                           foreach (var item in itemdetial4) 
                           {
                               DataRow dr = dtdetial.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               dr["WIP_QTY"] = item["WIP_QTY"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial.Rows.Add(dr);
                           }
                           break;
                       case 8:
                           var itemdetial5 = dtsum.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == smodel && q["PDLINE_NAME"].ToString() == spdline && q["PROCESS_NAME"].ToString() == sprocess && q["3H<WIP<=4H"].ToString() == Convert.ToString(1));
                           foreach (var item in itemdetial5) 
                           {
                               DataRow dr = dtdetial.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               dr["WIP_QTY"] = item["WIP_QTY"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial.Rows.Add(dr);
                           }
                           break;
                       case 9:
                           var itemdetial6 = dtsum.Rows.Cast<DataRow>().Where(q => q["MODEL_NAME"].ToString() == smodel && q["PDLINE_NAME"].ToString() == spdline && q["PROCESS_NAME"].ToString() == sprocess && q["4H<WIP"].ToString() == Convert.ToString(1));
                           foreach (var item in itemdetial6) 
                           {
                               DataRow dr = dtdetial.NewRow();
                               dr["MODEL_NAME"] = item["MODEL_NAME"].ToString();
                               dr["WORK_ORDER"] = item["WORK_ORDER"].ToString();
                               dr["SERIAL_NUMBER"] = item["SERIAL_NUMBER"].ToString();
                               dr["PDLINE_NAME"] = item["PDLINE_NAME"].ToString();
                               dr["PROCESS_NAME"] = item["PROCESS_NAME"].ToString();
                               dr["WIP_QTY"] = item["WIP_QTY"].ToString();
                               dr["PART_NO"] = item["PART_NO"].ToString();
                               dr["OUT_PROCESS_TIME"] = item["OUT_PROCESS_TIME"].ToString();
                               dtdetial.Rows.Add(dr);
                           }
                           break;
                       default: break;
                   }

                   dgvMainTable2.DataSource = dtdetial;
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
