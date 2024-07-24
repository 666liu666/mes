using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using Srvtools;
using System.Diagnostics;
using System.Data.OleDb;
//using Excel;
using System.Reflection;
using System.Runtime.InteropServices;
using SajetClass;
using System.IO;
using System.Collections;

namespace CTravelCard
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }
        public fMain(int nParam, string strSN)
        {
            InitializeComponent();
            m_nParam = nParam;
            txtSN.Text = strSN;
        }

        public string m_strTravelSql;
        public string m_strRPID;
        public int m_nParam;
        public bool bMultiLang = false;
        public struct TControlData
        {
            public DataGridView gvTravel;
            public System.Windows.Forms.CheckBox chkTravel;
        }
        public TControlData[] m_tGridView;

        //public void show_travel(string sn)
        //{
        //    fTravelCard f = new fTravelCard();
        //    f.txtSN.Text = sn;

        //    f.ShowDialog();
        //    f.Dispose();
        //}
        private void Initial_Form()
        {
            SajetCommon.SetLanguageControl(this);
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }
        private void fMain_Load(object sender, EventArgs e)
        {
            // multiLanguage1.SetLanguage(true);
            string sSQL = "SELECT SQL_VALUE " +
                            "FROM SAJET.SYS_REPORT_SQL " +
                            "WHERE (FUN_TYPE = 'SN')";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            m_strTravelSql = dsTemp.Tables[0].Rows[0]["SQL_VALUE"].ToString();
            //m_strRPID = GetRPID(); 
            AddCmbParamItem(cmbParam);
            cmbParam.SelectedIndex = m_nParam;

            //最下層的Tabpage
            DspReportSQL();



            KeyPressEventArgs key = new KeyPressEventArgs((char)Keys.Enter);
            txtSN_KeyPress(txtSN, key);
            //Initial_Form();
        }
        private void AddCmbParamItem(ComboBox cmb)
        {
            cmb.Items.Clear();
            string sSQL = "select fun_type from sajet.sys_report_sql where rp_id = 'FIELD' order by fun_idx";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
            {
                cmb.Items.Add(dsTemp.Tables[0].Rows[i]["fun_type"].ToString());
            }
        }
        private void txtSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return )
                return;
            //if (bMultiLang == false)
            //{
            //    Initial_Form();
            //    bMultiLang = true;

            AddCmbParamItem(cmbParamTmp);
            cmbParamTmp.SelectedIndex = cmbParam.SelectedIndex;
            //}
            if (e.KeyChar.ToString() == "\r")  
            {
                //cmbParamTmp.SelectedIndex = cmbParam.SelectedIndex;
                string strParam = cmbParamTmp.Items.Count == 0 ? cmbParam.SelectedItem.ToString() : cmbParamTmp.SelectedItem.ToString();
                string sSQL = "SELECT SQL_VALUE " +
                                "FROM SAJET.SYS_REPORT_SQL " +
                                "WHERE (FUN_TYPE = '" + strParam + "') AND (ROWNUM = 1)";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows[0]["SQL_VALUE"].ToString() != "")//轉換欄位資料取得SN
                {
                    sSQL = dsTemp.Tables[0].Rows[0]["SQL_VALUE"].ToString();
                    sSQL = sSQL.Replace(":SN", "'" + txtSN.Text + "'");
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    if (dsTemp.Tables[0].Rows.Count != 0)
                    {
                        labSN.Text = dsTemp.Tables[0].Rows[0][0].ToString();
                        //m_strTravelSql = dsTemp.Tables[0].Rows[0]["SQL_VALUE"].ToString();
                    }
                    else
                        labSN.Text = "";
                }
                else
                {
                    labSN.Text = txtSN.Text;
                }
                //sSQL = "select * from all_tab_columns " +
                //    "where owner='SAJET' and table_name = 'G_SN_STATUS' and column_name ='BOX_NO'";
                //dsTemp = ClientUtils.ExecuteSQL(sSQL);
                //if (dsTemp.Tables[0].Rows.Count == 0)
                //{
                //    txtBox.Visible = true;
                //    labBoxNO.Visible = true;
                //}
                ShowSNData();
            }
            Initial_Form();
        }
        private void ShowSNData()
        {
            if (DspWOData())//顯示目前工單資料
            {
                DspShipData();//Ship
                DspReportData();//Travel Card資料
            }
            else
                ClearData();
        }
        private void ClearData()//無該筆SN時清空資料
        {
            txtCustomer.Text = "";
            txtWarranty.Text = "";
            txtShipto.Text = "";
            txtVehicle.Text = "";
            txtContainer.Text = "";
            txtShipTime.Text = "";
            labShipNO.Text = "";

            for (int i = 0; i < m_tGridView.Length; i++)
            {
                if (m_tGridView[i].gvTravel.DataSource != null)
                {
                    ((DataSet)m_tGridView[i].gvTravel.DataSource).Clear();
                }
            }
        }

        private bool DspWOData()
        {
            string sSQL = m_strTravelSql;
            sSQL = sSQL.Replace(":SN", "'" + labSN.Text + "'");
            DataSet dsTemp;
            try
            {
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }
            catch
            {
                return false;
            }
            if (dsTemp.Tables[0].Rows.Count == 0)
            {//SN錯誤時清空
                labWO.Text = "";
                labMasterWO.Text = "";
                labPN.Text = "";
                labSpec.Text = "";
                labVersion.Text = "";
                labRoute.Text = "";
                labPDLine.Text = "";
                labWOType.Text = "";
                labNextProcess.Text = "";
                labCustomer.Text = "";
                labStatus.Text = "";
                txtBox.Text = "";
                txtCarton.Text = "";
                txtPallet.Text = "";
                labCustSN.Text = "";
                return false;
            }
            else
            {
                labWO.Text = dsTemp.Tables[0].Rows[0]["WORK_ORDER"].ToString();
                labMasterWO.Text = dsTemp.Tables[0].Rows[0]["MASTER_WO"].ToString();
                labPN.Text = dsTemp.Tables[0].Rows[0]["PART_NO"].ToString();
                labSpec.Text = dsTemp.Tables[0].Rows[0]["Spec1"].ToString();
                labVersion.Text = dsTemp.Tables[0].Rows[0]["VERSION"].ToString();
                labRoute.Text = dsTemp.Tables[0].Rows[0]["ROUTE_NAME"].ToString();
                labPDLine.Text = dsTemp.Tables[0].Rows[0]["PDLINE_NAME"].ToString();
                labWOType.Text = dsTemp.Tables[0].Rows[0]["WO_TYPE"].ToString();
                labNextProcess.Text = dsTemp.Tables[0].Rows[0]["Process_Name"].ToString();
                labCustomer.Text = dsTemp.Tables[0].Rows[0]["Customer_Code"].ToString();
                labModelName.Text = dsTemp.Tables[0].Rows[0]["MODEL_NAME"].ToString();

                if (dsTemp.Tables[0].Rows[0]["work_flag"].ToString() == "1")
                {
                    labStatus.Text = "Scrap";
                    labStatus.BackColor = Color.Yellow;
                    labStatus.ForeColor = Color.Red;
                }
                else if (dsTemp.Tables[0].Rows[0]["work_flag"].ToString() == "")
                {
                    labSN.Text = "";
                    labStatus.Text = "";
                }
                else
                {
                    labStatus.Text = dsTemp.Tables[0].Rows[0]["current_result"].ToString();
                    if (dsTemp.Tables[0].Rows[0]["current_status"].ToString() == "0")
                    {
                        labStatus.ForeColor = Color.Blue;
                    }
                    else
                    {
                        labStatus.ForeColor = Color.Red;
                    }
                }
                if (txtBox.Visible == true)
                {
                    txtBox.Text = dsTemp.Tables[0].Rows[0]["Box_NO"].ToString();
                }
                txtCarton.Text = dsTemp.Tables[0].Rows[0]["CARTON_NO"].ToString();
                txtPallet.Text = dsTemp.Tables[0].Rows[0]["PALLET_NO"].ToString();
                labCustSN.Text = dsTemp.Tables[0].Rows[0]["Customer_SN"].ToString();
            }
            return true;
        }
        private void DspShipData()
        {
            string sSQL = "Select A.CONTAINER,A.VEHICLE_NO,TO_CHAR(A.WARRANTY,'YYYY/MM/DD HH24:MI') WARRANTY "
                        + "      ,TO_CHAR(A.UPDATE_TIME,'YYYY/MM/DD HH24:MI') TIME "
                        + "      ,B.DN_NO SHIPPING_NO,B.SHIP_TO ,C.CUSTOMER_CODE,C.CUSTOMER_NAME "
                        + "From SAJET.G_SHIPPING_SN A "
                        + "    ,SAJET.G_DN_BASE B "
                        + "    ,SAJET.SYS_CUSTOMER C "
                        + "Where A.SERIAL_NUMBER = '" + labSN.Text + "' "
                        + "and A.SHIPPING_ID = B.DN_ID "
                        + "and B.CUSTOMER_ID = C.CUSTOMER_ID(+) ";

            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count != 0)
            {
                labShipNO.Text = dsTemp.Tables[0].Rows[0]["SHIPPING_NO"].ToString();
                txtCustomer.Text = dsTemp.Tables[0].Rows[0]["CUSTOMER_CODE"].ToString() + "-" + dsTemp.Tables[0].Rows[0]["CUSTOMER_NAME"].ToString();
                txtWarranty.Text = dsTemp.Tables[0].Rows[0]["WARRANTY"].ToString();
                txtShipto.Text = dsTemp.Tables[0].Rows[0]["SHIP_TO"].ToString();
                txtVehicle.Text = dsTemp.Tables[0].Rows[0]["VEHICLE_NO"].ToString();
                txtContainer.Text = dsTemp.Tables[0].Rows[0]["CONTAINER"].ToString();
                txtShipTime.Text = dsTemp.Tables[0].Rows[0]["TIME"].ToString();
            }
        }


        // 2011.08.03 宇睿新增功能：針對cell按右鍵選取後，獲得額外的詳細資料。
        #region 2011.08.03 宇睿新增功能：針對cell按右鍵選取後，獲得額外的詳細資料。
        ContextMenuStrip strip;
        ToolStripMenuItem tsDLLFunction;
        ToolStripMenuItem tsDetailFunction;
        private DataGridViewCellEventArgs mouseLocation; //滑鼠目前所在欄位
        private DataGridView mouseAtDgv;　//滑鼠目前所在的DataGridView
        private string g_strDLL_FILENAME = string.Empty; //用來裝載客製DLL名稱
        private string g_strSQL_VALUE = string.Empty;//用來裝載明細SQL的內容
        private Hashtable hsDGVColumnAndValue = new Hashtable();//裝載滑鼠所在位置欄位以及值

        public string g_strDllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);//實際執行路徑


        // 呼叫客製DLL
        private void tsDLLFunction_Click(object sender, EventArgs args)
        {
            //if (mouseAtDgv != null)
            //{
          //  if (sender is DataGridView)
          //      return;
            string sSQLTemp = " SELECT * FROM SAJET.SYS_REPORT_SQL_DETAIL  "
                           + "WHERE FUN_TYPE='" + mouseAtDgv.Parent.Name + "'"
                           + "AND  DLL_FILENAME IS NOT NULL";
            DataSet dsSQL_DETIAL = ClientUtils.ExecuteSQL(sSQLTemp);
            if (dsSQL_DETIAL.Tables[0].Rows.Count > 0)
            {
                string sDetailDesc = SajetCommon.SetLanguage(dsSQL_DETIAL.Tables[0].Rows[0]["DETAIL_DESC"].ToString());
                string DllName = dsSQL_DETIAL.Tables[0].Rows[0]["DLL_FILENAME"].ToString();
                //MessageBox.Show("Now Start \"" + DllName + ".dll\" Function");
                Assembly assembly = null;
                object obj = null;
                Type type = null;
                try
                {
                    assembly = Assembly.LoadFrom(g_strDllPath + "\\" + DllName + ".dll");
                    type = assembly.GetType((DllName + ".fMain"));
                    //準備 DataSet 資料
                    DataSet dsTemp = new DataSet();
                    dsTemp.Tables.Add();
                    dsTemp.Tables[0].Rows.Add();
                    for (int i = 0; i < mouseAtDgv.ColumnCount; i++)
                    {
                        dsTemp.Tables[0].Columns.Add(mouseAtDgv.Columns[i].Name);
                        dsTemp.Tables[0].Rows[0][i] = mouseAtDgv.Rows[mouseLocation.RowIndex].Cells[i].Value;
                    }
                    //傳送 DataSet 與按鈕上的名稱作為顯示視窗的名稱
                    obj = assembly.CreateInstance(type.FullName, true, BindingFlags.CreateInstance, null, new object[] { dsTemp, sDetailDesc }, null, null);
                }
                catch(Exception ex)
                {
                    SajetMessageBox.Show(ex.Message);
                }
                finally
                {
                    if (obj != null)
                    {
                        //調整大小
                        ((Form)obj).Size = new Size(((Form)obj).Width * 2, ((Form)obj).Height);
                        ((Form)obj).Show();
                    }
                }
            }

        }
        //顯示固定DETIAL
        private void tsDetailFunction_Click(object sender, EventArgs args)
        {
            string sSQLTemp = " SELECT * FROM SAJET.SYS_REPORT_SQL_DETAIL  "
                                           + "WHERE FUN_TYPE='" + mouseAtDgv.Parent.Name + "'"
                                           + "AND  SQL_VALUE IS NOT NULL";
            DataSet dsSQL_DETIAL = ClientUtils.ExecuteSQL(sSQLTemp);
            if (dsSQL_DETIAL.Tables[0].Rows.Count > 0)
            {
                g_strSQL_VALUE = dsSQL_DETIAL.Tables[0].Rows[0]["SQL_VALUE"].ToString();
                hsDGVColumnAndValue = new Hashtable();
                for (int i = 0; i < mouseAtDgv.ColumnCount; i++)
                {
                    hsDGVColumnAndValue.Add(mouseAtDgv.Columns[i].Name, mouseAtDgv.Rows[mouseLocation.RowIndex].Cells[i].Value.ToString());
                }
                string[] sSQL = g_strSQL_VALUE.Split(new char[] { '[', ']' });
                for (int i = 0; i < sSQL.Length - 1; i++)
                {
                    if (hsDGVColumnAndValue.Contains(sSQL[i]))
                        g_strSQL_VALUE = g_strSQL_VALUE.Replace("[" + sSQL[i] + "]", " '" + hsDGVColumnAndValue[sSQL[i]].ToString() + "' ");
                }

                DataSet dsDetailSql = ClientUtils.ExecuteSQL(g_strSQL_VALUE);
                Form fSQLDetail = new Form();
                DataGridView gvTemp = new DataGridView();

                gvTemp.AllowUserToAddRows = false;
                gvTemp.AllowUserToDeleteRows = false;
                gvTemp.BackgroundColor = Color.White;
                gvTemp.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;
                gvTemp.ReadOnly = true;
                gvTemp.Dock = DockStyle.Fill;
                gvTemp.DataSource = dsDetailSql;
                gvTemp.DataMember = dsDetailSql.Tables[0].ToString();
                fSQLDetail.Controls.Add(gvTemp);
                fSQLDetail.StartPosition = FormStartPosition.CenterScreen;
                fSQLDetail.Text = SajetCommon.SetLanguage(dsSQL_DETIAL.Tables[0].Rows[0]["DETAIL_DESC"].ToString());
                fSQLDetail.Size = new Size(fSQLDetail.Width * 2, fSQLDetail.Height);
                fSQLDetail.Load += new EventHandler(fSQLDetail_Load);
                fSQLDetail.Show();
            }
        }

        void fSQLDetail_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl((Control)sender);
        }

        /// <summary>
        /// 得知使用者滑鼠目前所在位置
        /// </summary>
        /// <param name="l_mouseAtDgv"></param>
        /// <param name="location"></param>
        private void dataGridView_CellMouseEnter(object l_mouseAtDgv, DataGridViewCellEventArgs location)
        {
            mouseLocation = location; //老鼠所在欄位
            mouseAtDgv = (DataGridView)l_mouseAtDgv; //老鼠所在資料表
        }

        private void DspReportData()
        {//顯示Travel Card資料
            string sSQL = "select * from sajet.sys_report_sql " +
                "where rp_id = 'QUERY' order by fun_idx";
            DataSet dsReportSql = ClientUtils.ExecuteSQL(sSQL);
            //int nCnt = ;
            for (int i = 0; i < dsReportSql.Tables[0].Rows.Count; i++)
            {
                sSQL = dsReportSql.Tables[0].Rows[i]["SQL_VALUE"].ToString();
                sSQL = sSQL.Replace(":SN", "'" + labSN.Text + "'");
                DataSet dsTravel = ClientUtils.ExecuteSQL(sSQL);
                m_tGridView[i].gvTravel.DataSource = dsTravel;
                m_tGridView[i].gvTravel.DataMember = dsTravel.Tables[0].ToString();
                if (m_tGridView[i].gvTravel.Rows.Count == 0)
                    continue;

                //new
                sSQL = " SELECT * FROM SAJET.SYS_REPORT_SQL_DETAIL  "
                         + "WHERE FUN_TYPE='" + dsReportSql.Tables[0].Rows[i]["FUN_TYPE"].ToString() + "'";
                DataSet dsSQL_DETIAL = ClientUtils.ExecuteSQL(sSQL);
                if (dsSQL_DETIAL.Tables[0].Rows.Count > 0)
                {
                    int intFunctionCount = 0;//計算共有幾個功能加入選單
                    strip = new ContextMenuStrip();
                    tsDLLFunction = new ToolStripMenuItem();
                    tsDetailFunction = new ToolStripMenuItem();
                    foreach (DataGridViewColumn column in m_tGridView[i].gvTravel.Columns)
                    {
                        intFunctionCount = 0;
                        column.ContextMenuStrip = strip;
                        for (int j = 0; j < dsSQL_DETIAL.Tables[0].Rows.Count; j++)
                        {
                            if (!dsSQL_DETIAL.Tables[0].Rows[j]["DLL_FILENAME"].ToString().Equals(string.Empty))
                            {
                                //讓使用者能開啟客製的DLL檔案
                                tsDLLFunction.Text = SajetCommon.SetLanguage((dsSQL_DETIAL.Tables[0].Rows[j]["DETAIL_DESC"].ToString()));
                                if (!string.IsNullOrEmpty(tsDLLFunction.Text.Trim()))
                                {
                                    column.ContextMenuStrip.Items.Add(tsDLLFunction);
                                    intFunctionCount++;
                                }
                            }
                            if (dsSQL_DETIAL.Tables[0].Rows[j]["DLL_FILENAME"].ToString().Equals(string.Empty))
                            {
                                //展示固定的DETIAL
                                if (!dsSQL_DETIAL.Tables[0].Rows[j]["SQL_VALUE"].ToString().Equals(string.Empty))
                                {
                                    tsDetailFunction.Text = SajetCommon.SetLanguage(dsSQL_DETIAL.Tables[0].Rows[j]["DETAIL_DESC"].ToString());
                                    if (!string.IsNullOrEmpty(tsDetailFunction.Text.Trim()))
                                    {
                                        column.ContextMenuStrip.Items.Add(tsDetailFunction);
                                        intFunctionCount++;
                                    }
                                }
                            }
                        }
                        m_tGridView[i].gvTravel.CellMouseEnter += new DataGridViewCellEventHandler(dataGridView_CellMouseEnter);
                    }
                    tsDLLFunction.Click += new EventHandler(tsDLLFunction_Click);
                    tsDetailFunction.Click += new EventHandler(tsDetailFunction_Click);
                    //當選項僅展示固定的DETIAL時，才支援雙按看詳細資料
                    if (intFunctionCount==1)
                    {
                        if (strip.Items.Contains(tsDLLFunction))
                            m_tGridView[i].gvTravel.MouseDoubleClick += new MouseEventHandler(tsDLLFunction_Click);
                        else if(strip.Items.Contains(tsDetailFunction))
                        m_tGridView[i].gvTravel.MouseDoubleClick += new MouseEventHandler(tsDetailFunction_Click);
                    }
                }
            }
        }
        #endregion

        private void DspReportSQL()
        {//填TabPage與GridView
            string sSQL = "select * from sajet.sys_report_sql " +
                                 "where rp_id = 'QUERY' order by fun_idx";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            m_tGridView = new TControlData[dsTemp.Tables[0].Rows.Count];
            int nTop = 5, nLeft = 5;
            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
            {
                TabPage tbTemp = new TabPage(dsTemp.Tables[0].Rows[i]["FUN_TYPE"].ToString());
                tbTemp.Name = dsTemp.Tables[0].Rows[i]["FUN_TYPE"].ToString();
                tabControl1.TabPages.Add(tbTemp);
                DataGridView gvTemp = new DataGridView();
                gvTemp.AllowUserToAddRows = false;
                gvTemp.AllowUserToDeleteRows = false;
                gvTemp.BackgroundColor = Color.White;
                gvTemp.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;
                gvTemp.ReadOnly = true;

                tabControl1.TabPages[i].Controls.Add(gvTemp);
                m_tGridView[i].gvTravel = gvTemp;
                gvTemp.Dock = DockStyle.Fill;

                System.Windows.Forms.CheckBox chkTmp = new System.Windows.Forms.CheckBox();
                chkTmp.Top = nTop;
                chkTmp.Left = nLeft;
                chkTmp.Text = dsTemp.Tables[0].Rows[i]["FUN_TYPE"].ToString();
                panel3.Controls.Add(chkTmp);
                m_tGridView[i].chkTravel = chkTmp;
                nTop = nTop + chkTmp.Height;
            }
        }
        private string GetRPID()
        {
            string sSQL = "select RP_ID from sajet.sys_report_name " +
                            "where upper(DLL_FILENAME) = 'REPORTTRAVELDLL.DLL' and rownum = 1";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            return dsTemp.Tables[0].Rows[0]["RP_ID"].ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = "xls";
            saveFileDialog1.Filter = "All Files(*.xls)|*.xls";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            string sFileName = saveFileDialog1.FileName;
            ExportExcel.CreateExcel Export = new ExportExcel.CreateExcel(sFileName);
            DataSet dsExport = new DataSet();
            int j = 0;
            for (int i = 0; i < m_tGridView.Length; i++)
            {
                if (m_tGridView[i].chkTravel.Checked == true)
                {
                    dsExport.Tables.Add(((DataSet)m_tGridView[i].gvTravel.DataSource).Tables[0].Copy());
                    dsExport.Tables[j].TableName = m_tGridView[i].chkTravel.Text;
                    for (int k = 0; k < m_tGridView[i].gvTravel.Columns.Count; k++)
                    {
                        dsExport.Tables[j].Columns[k].ColumnName = m_tGridView[i].gvTravel.Columns[k].HeaderText;
                    }
                    j++;
                }
            }
            if (dsExport.Tables.Count == 0)
            {
                dsExport.Tables.Add("Sheet1");
            }
            Export.ExportToExcel(dsExport);
            ////string strPath = null;// = OpenFile();
            ////int nSheetCnt = 1;
            //SaveFileDialog saveFileDialog = new SaveFileDialog();

            //saveFileDialog.Filter = "Execl files (*.xls)|*.xls";

            //saveFileDialog.FilterIndex = 0;

            //saveFileDialog.RestoreDirectory = true;

            //saveFileDialog.CreatePrompt = true;

            //saveFileDialog.Title = "Export Excel File To";


            //saveFileDialog.ShowDialog();

            //string strName = saveFileDialog.FileName;


            //Excel.Application app = new Excel.Application();
            //Excel.Workbooks workbooks = app.Workbooks;
            //Excel._Workbook workbook = workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            //Excel.Sheets sheets = workbook.Worksheets;


            //string currentSheet = "";
            //int nExcelSheet = 1;
            //for (int i = 0; i < m_tGridView.Length; i++)
            //{
            //    if (m_tGridView[i].chkTravel.Checked == true)
            //    {
            //        //currentSheet = m_tGridView[i].chkTravel.Text;
            //        try
            //        {
            //            if (app == null)
            //            {
            //                //return false;
            //            }


            //            //Excel.Workbooks workbooks = app.Workbooks;
            //            //Excel._Workbook workbook = workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            //            //Excel.Sheets sheets = workbook.Worksheets;
            //            Excel._Worksheet worksheet;

            //            if (nExcelSheet == 1)
            //            {
            //                worksheet = (Excel._Worksheet)sheets.get_Item(1);
            //                nExcelSheet++;
            //            }
            //            else
            //            {
            //                Excel.Worksheet excelWorksheet = (Excel.Worksheet)sheets.get_Item(currentSheet);
            //                worksheet = (Excel.Worksheet)sheets.Add(Type.Missing, excelWorksheet, Type.Missing, Type.Missing);
            //            }
            //            worksheet.Name = m_tGridView[i].chkTravel.Text;
            //            currentSheet = m_tGridView[i].chkTravel.Text;
            //            //
            //            if (worksheet == null)
            //            {
            //                //return false;
            //            }
            //            string sLen = "";
            //            //
            //            char H = (char)(64 + m_tGridView[i].gvTravel.ColumnCount / 26);
            //            char L = (char)(64 + m_tGridView[i].gvTravel.ColumnCount % 26);
            //            if (m_tGridView[i].gvTravel.ColumnCount < 26)
            //            {
            //                sLen = L.ToString();
            //            }
            //            else
            //            {
            //                sLen = H.ToString() + L.ToString();
            //            }


            //            //
            //            string sTmp = sLen + "1";
            //            Excel.Range ranCaption = worksheet.get_Range(sTmp, "A1");
            //            string[] asCaption = new string[m_tGridView[i].gvTravel.ColumnCount];
            //            for (int j = 0; j < m_tGridView[i].gvTravel.ColumnCount; j++)
            //            {
            //                asCaption[j] = m_tGridView[i].gvTravel.Columns[j].HeaderText;
            //            }
            //            ranCaption.Value2 = asCaption;

            //            //
            //            object[] obj = new object[m_tGridView[i].gvTravel.Columns.Count];
            //            for (int r = 0; r < m_tGridView[i].gvTravel.RowCount; r++)
            //            {
            //                for (int l = 0; l < m_tGridView[i].gvTravel.Columns.Count; l++)
            //                {
            //                    if (m_tGridView[i].gvTravel[l, r].ValueType == typeof(DateTime))
            //                    {
            //                        obj[l] = m_tGridView[i].gvTravel[l, r].Value.ToString();
            //                    }
            //                    else
            //                    {
            //                        obj[l] = m_tGridView[i].gvTravel[l, r].Value;
            //                    }
            //                }
            //                string cell1 = sLen + ((int)(r + 2)).ToString();
            //                string cell2 = "A" + ((int)(r + 2)).ToString();
            //                Excel.Range ran = worksheet.get_Range(cell1, cell2);
            //                ran.Value2 = obj;
            //            }
            //            //worksheet.Cells.EntireRow.AutoFit();

            //        }
            //        catch (Exception exp)
            //        {
            //        }
            //    }
            //}
            //workbook.SaveCopyAs(strName);
            //workbook.Saved = true;
            //app.Visible = false;
            //app.UserControl = false;
            //app.Quit();
            //app = null;
            //GC.Collect();

            //return true;


        }

        //public Excel.Application excel;
        //public Excel.Workbooks books;
        //public Excel.Workbook book;
        //public Excel.Worksheet sheet;
        //public Excel.Sheets sheets;

        //private string OpenFile()
        //{
        //    SaveFileDialog SaveFileDialog = new SaveFileDialog();
        //    SaveFileDialog.Filter = "Excel Files (*.xls)|*.xls";
        //    SaveFileDialog.InitialDirectory = "c:\\";
        //    DialogResult result = SaveFileDialog.ShowDialog();
        //    if (result == DialogResult.OK)
        //    {
        //        //bool bExport;
        //    }
        //    return null;
        //}
    }
}

