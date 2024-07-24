using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SajetClass;
using System.Data.OracleClient;
//using Excel;
using System.Collections;

namespace REELQUERY
{
    public partial class fMain : Form
    {

        public fMain()
        {
            InitializeComponent();
        }
        //public ArrayList ParamsValue;
        public int g_iPrivilege = 0;
        public static String g_sUserID;
        public String g_sProgram, g_sFunction;
        StringCollection scErrorMsg;
        DataSet sDataSet1;
        DataSet sDataSet;
        bool isclick = false;

        private void btn_query_Click(object sender, EventArgs e)
        {
            isclick = true;
            if (richBox.Text == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("请输入SN！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("请选择SN区段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            proBar.Value = 0;
            ShowData();
            label2.Text = this.gridView1.RowCount.ToString();


        }

        private void fMain_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            g_sUserID = ClientUtils.UserPara1;
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;
            this.Text = this.Text + " (" + SajetCommon.g_sFileVersion + ")";
            this.WindowState = FormWindowState.Maximized;
            panel1.BackColor = Color.Azure;
            simpleButton2.BackColor = Color.SkyBlue;
            label2.Text = this.gridView1.RowCount.ToString();
            richBox.Enabled = false;
            simpleButton2.Enabled = false;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

            richBox.Clear();
            this.gvData.DataSource=null;
            label2.Text = this.gridView1.RowCount.ToString();
            //輸入SN時增加由excel匯入大量資料的輸入方式    2011/10/19 by Sharon
            OpenFileDialog ofdImport = new OpenFileDialog();
            ofdImport.InitialDirectory = System.Windows.Forms.Application.StartupPath + @"\..\..\";
            DialogResult DResult = ofdImport.ShowDialog();
            scErrorMsg = new StringCollection();
            if (DResult == DialogResult.OK)
            {
                String sFilePath = ofdImport.FileName;                    //sFilePath: 檔案的完整路徑+名稱
                String[] sFile = sFilePath.Split('\\');
                String[] sFileName = sFile[sFile.Length - 1].Split('.');  //sFileName: 檔案名稱+附檔名
                String sFileExten = sFileName[sFileName.Length - 1];

                //擷取所選取的路徑的檔案的附檔名確認是否為txt檔
                if (!(sFileExten.Equals("xls") || sFileExten.Equals("xlsx")))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select Excel File"), 0);
                    return;
                }

                //讀取出Excel檔案內容


                ExportOfficeExcel.ExcelEdit ExcelData = new ExportOfficeExcel.ExcelEdit();
                try
                {
                    ExcelData.Open(sFilePath);
                    int iPageCount = ExcelData.GetSheetCount();
                    for (int k = 1; k <= iPageCount; k++)
                    {
                        Excel.Worksheet ws = ExcelData.GetSheet(k);

                        //Excel.Worksheet ws = ExcelData.GetSheet("Sheet 1" );
                        int iRowCount = ws.UsedRange.Rows.Count;
                        proBar.Minimum = 0;
                        proBar.Maximum = iRowCount;
                        proBar.Value = 0;
                        int i = 1;
                        scErrorMsg = new StringCollection();
                        for (; i <= iRowCount; i++)
                        {
                            System.Windows.Forms.Application.DoEvents();
                            string sSN = ExcelData.ReadCellValue(ws, 1, i).ToString();
                            if (string.IsNullOrEmpty(sSN.Trim()))
                                continue;
                            //if (CheckPieceSN(ref sSN))
                            richBox.AppendText("" + sSN + "\r\n");
                            i++;
                            proBar.Value++;
                        }

                        proBar.Value = iRowCount;
                    }
                }
                finally
                {
                    ExcelData.Close();
                }
            }
        } 

        private void Export_Click(object sender, EventArgs e)
        {
            if (this.gridView1.RowCount==0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("无数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "导出Excel";
            fileDialog.Filter = "Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = fileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                //int iRowCount1 = 1;
                //iRowCount1 = this.gridView1.RowCount;
                //proBar.Minimum = 1;
                //proBar.Maximum = iRowCount1;
                //proBar.Value = 1;
                //int j = 1;
                //for (; j <= iRowCount1; j++)
                //{
                    DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                    gvData.ExportToXls(fileDialog.FileName);
                    DevExpress.XtraEditors.XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ShowData()
        {
            string sn = richBox.Text;
            string[] Str = sn.Trim().Split('\n');
            string Str_SN = string.Empty;
            for (int i = 0; i < Str.Length; i++)
            {
                Str_SN = Str_SN + "'" + Str[i].Replace("\t","") + "',";
            }
            Str_SN = Str_SN.Substring(0, Str_SN.Length - 1);

            if (comboBox1.Text== "FATP_SN")
            {

                //根据FATP条码查询
                string sSQL = " SELECT distinct B.WORK_ORDER, A.SERIAL_NUMBER,G.PANEL_NO,A.ITEM_PART_SN,d.REEL_NO,D.DATECODE,D.LOT, "
              + "  D.EXP_DATECODE,E.PART_NO,E.SPEC1,F.VENDOR_NAME "
              + "   FROM SAJET.G_SN_KEYPARTS A, SAJET.G_SMT_KEYPARTS B, "
              + "   smt.g_smt_sn_map c, SAJET.G_MATERIAL d, SAJET.SYS_PART e, SAJET.SYS_VENDOR f,SAJET.G_SN_STATUS G "
               + " WHERE A.ITEM_PART_SN = B.ITEM_PART_SN "
               + " AND A.ITEM_PART_SN =G.SERIAL_NUMBER "
              + "  and B.SERIAL_NUMBER = C.SERIAL_NUMBER "
              + "  and C.REEL_NO = D.REEL_NO "
              + "  and D.PART_ID = E.PART_ID "
              + "  and D.VENDOR_ID = F.VENDOR_ID "
              + "  AND A.SERIAL_NUMBER in (" + Str_SN + ") "
             + "   union "
              + "  select distinct  B.WORK_ORDER, A.SERIAL_NUMBER,B.PANEL_NO,A.ITEM_PART_SN,e.reel_no,E.DATECODE,E.LOT,E.EXP_DATECODE,F.PART_NO,F.SPEC1,G.VENDOR_NAME "
              + "  from SAJET.G_SN_KEYPARTS a, SAJET.G_SN_STATUS b, SAJET.G_SMT_KEYPARTS c， "
              + "  MESFJ.G_FJ_TRACEABILITY_PANEL_A188 @mesfj d ,SAJET.G_MATERIAL e, SAJET.SYS_PART f, SAJET.SYS_VENDOR g "
             + "   where A.ITEM_PART_SN = B.SERIAL_NUMBER "
             + "   and A.ITEM_PART_SN = C.ITEM_PART_SN "
             + "   and B.PANEL_NO = D.SERIAL_NUMBER "
              + "  and d.reel_no = E.REEL_NO "
              + "  and E.PART_ID = F.PART_ID "
              + "  and E.VENDOR_ID = G.VENDOR_ID "
              + "  and A.SERIAL_NUMBER in (" + Str_SN + ") ";

                DataSet sDataSet = ClientUtils.ExecuteSQL(sSQL);

                gvData.DataSource = sDataSet.Tables[0];

                //if (sDataSet.Tables[0].Rows.Count== 0)
                //{
                //    DevExpress.XtraEditors.XtraMessageBox.Show("无数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
            }
            else
            {
                //根据SMT条码查询

                string sSQL1 = "  select A.WORK_ORDER,D.SERIAL_NUMBER ITEM_PART_SN,D.PANEL_NO,G.SERIAL_NUMBER,C.REEL_NO,C.DATECODE,C.LOT,C.EXP_DATECODE, "
          + "   E.PART_NO,E.SPEC1,F.VENDOR_NAME  "
          + "    from SAJET.G_SMT_KEYPARTS a, SMT.G_SMT_SN_MAP b, SAJET.G_MATERIAL c, "
          + "   sajet.g_sn_status d, SAJET.SYS_PART e, SAJET.SYS_VENDOR f ,SAJET.G_SN_KEYPARTS g "
          + "   where a.item_part_sn in (" + Str_SN + ") "
          + "   and A.SERIAL_NUMBER = B.SERIAL_NUMBER "
          + "   and B.REEL_NO = C.REEL_no "
          + "   and A.ITEM_PART_SN = D.SERIAL_NUMBER "
          + "  and D.SERIAL_NUMBER=G.ITEM_PART_SN  (+) "
          + "   and C.PART_ID = E.PART_ID "
           + "  and C.VENDOR_ID = F.VENDOR_ID "
           + "  union "
          + "   select B.WORK_ORDER,B.SERIAL_NUMBER ITEM_PART_SN,B.PANEL_NO,G.SERIAL_NUMBER,c.REEL_NO, "
           + "  D.DATECODE,D.LOT,D.EXP_DATECODE,E.PART_NO,E.SPEC1,F.VENDOR_NAME "
           + "   from SAJET.G_SMT_KEYPARTS a, SAJET.G_SN_STATUS b, "
           + "   MESFJ.G_FJ_TRACEABILITY_PANEL_A188 @mesfj c,SAJET.G_MATERIAL d, "
           + "   SAJET.SYS_PART e, SAJET.SYS_VENDOR f ,SAJET.G_SN_KEYPARTS g "
           + "  where A.ITEM_PART_SN = B.SERIAL_NUMBER "
           + "   and B.PANEL_NO = c.serial_number "
           + "   and B.SERIAL_NUMBER=G.ITEM_PART_SN (+) "
           + "  and c.reel_no = D.REEL_NO "
            + " and D.PART_ID = E.PART_ID "
           + "  and D.VENDOR_ID = F.VENDOR_ID "
           + "  and A.ITEM_PART_SN in (" + Str_SN + ") ";

               

                DataSet sDataSet1 = ClientUtils.ExecuteSQL(sSQL1);

                gvData.DataSource = sDataSet1.Tables[0];
                //if (sDataSet1.Tables[0].Rows.Count == 0)
                //{
                //    DevExpress.XtraEditors.XtraMessageBox.Show("无数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
            }
        }

        private void gridView1_ColumnFilterChanged(object sender, EventArgs e)
        {
            label2.Text = this.gridView1.RowCount.ToString();
        }

        private void gridView1_CustomDrawEmptyForeground(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            if (isclick == true)
            {
                if (comboBox1.Text == "FATP_SN")
                {
                    this.judge_ds(sDataSet);
                    string str = "没有查询到你所想要的数据!";
                    Font f = new Font("宋体", 20, FontStyle.Bold);
                    Rectangle r = new Rectangle(e.Bounds.Top + 125, e.Bounds.Left + 100, e.Bounds.Right - 100, e.Bounds.Height - 125);
                    e.Graphics.DrawString(str, f, Brushes.Black, r);
                }
                if (comboBox1.Text == "SMT_SN")
                {
                    this.judge_ds(sDataSet1);
                    string str = "没有查询到你所想要的数据!";
                    Font f = new Font("宋体", 20, FontStyle.Bold);
                    Rectangle r = new Rectangle(e.Bounds.Top + 125, e.Bounds.Left + 100, e.Bounds.Right - 100, e.Bounds.Height - 125);
                    e.Graphics.DrawString(str, f, Brushes.Black, r);
                }
            }
           
        }
        #region 判断ds是否为空
        private bool judge_ds(DataSet ds)
        {
            bool flag = false;
            if (ds == null || ds.Tables.Count == 0 || (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0))
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            richBox.Clear();
            richBox.Enabled = true;
            simpleButton2.Enabled = true;
            this.gvData.DataSource = null;
            isclick = false;
            label2.Text = "0";
        }

        private void richBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                IDataObject dataObj = Clipboard.GetDataObject();
                if (dataObj.GetDataPresent(DataFormats.StringFormat))
                {
                    e.Handled = true; //去掉格式文本的格式   
                    var txt = (string)Clipboard.GetData(DataFormats.StringFormat);
                    Clipboard.Clear();
                    Clipboard.SetData(DataFormats.StringFormat, txt);
                    richBox.Paste();
                }
            }
        }


    }
}
