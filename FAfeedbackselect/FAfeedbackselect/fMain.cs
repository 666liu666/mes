using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Windows.Forms;
using SajetClass;


namespace FAfeedbackselect
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }

         
        #region //定义变量
        private string g_sUserID;
        private string g_sProgram;
        private string g_sFunction;
        #endregion
        #region //窗体设计
        private void btexport_Click(object sender, EventArgs e)
        {
            string fileName = "";

            string saveFileName = "";

            SaveFileDialog saveDialog = new SaveFileDialog();

            saveDialog.DefaultExt = "xls";

            saveDialog.Filter = "Excel文件|*.xls";

            saveDialog.FileName = fileName;

            saveDialog.ShowDialog();

            saveFileName = saveDialog.FileName;

            if (saveFileName.IndexOf(":") < 0) return; //被点了取消

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {

                MessageBox.Show("无法创建Excel对象，您的电脑可能未安装Excel");

                return;

            }

            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;

            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);

            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1 

            //写入标题             

            for (int i = 0; i < m_tGridView.ColumnCount; i++)

            { worksheet.Cells[1, i + 1] = m_tGridView.Columns[i].HeaderText; }

            //写入数值

            for (int r = 0; r < m_tGridView.Rows.Count; r++)
            {
                for (int i = 0; i < m_tGridView.ColumnCount; i++)
                {

                    worksheet.Cells[r + 2, i + 1] = m_tGridView.Rows[r].Cells[i].Value;

                }

                System.Windows.Forms.Application.DoEvents();

            }

            worksheet.Columns.EntireColumn.AutoFit();//列宽自适应

            MessageBox.Show(fileName + "资料导入成功", "提示", MessageBoxButtons.OK);

            if (saveFileName != "")
            {

                try
                {
                    workbook.Saved = true;

                    workbook.SaveCopyAs(saveFileName);  //fileSaved = true;                 

                }

                catch (Exception ex)
                {//fileSaved = false;                      

                    MessageBox.Show("导出文件时出错,文件可能正被打开！\n" + ex.Message);

                }

            }

            xlApp.Quit();

            GC.Collect();//强行销毁           }

        }

        private void btclear_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtsn.Text) || checkSN.Checked == true)
            {
                txtsn.Text = "";
                checkSN.Checked = false;
                m_tGridView.DataSource = null;
            }
            else
            {
                m_tGridView.DataSource = null;
            }
           
        }

        private void btselect_Click(object sender, EventArgs e)
        {
            
                if (checkSN.Checked == true )
                { 
                    

                    string sqlstrtemp = "SELECT SERIAL_NUMBER,DEFECT_CODE,DEFECT_DESC,UPDATEUSER,UPDATE_TIME FROM SAJET.G_FAfeedbackinfo WHERE SERIAL_NUMBER=:SERIAL_NUMBER ";
                    object[][] sqlparamstemp = new object[][] { new object[] { ParameterDirection.Input, OracleType.VarChar, "serial_number", txtsn.Text } };
                    //DataTable dt = ClientUtils.ExecuteSQL(sqlstrtemp, sqlparamstemp).Tables[0];
                    DataSet ds3 = ClientUtils.ExecuteSQL(sqlstrtemp, sqlparamstemp);
                    if (ds3.Tables[0].Rows.Count > 0)
                    {
                        m_tGridView.DataSource = ds3.Tables[0];
                        lbmessage1.Text = "查询成功！";

                    }
                    else
                    {
                        m_tGridView.DataSource = null;
                        lbmessage1.Text = "无数据！";

                    }
                }
                else
                {
                    string sql4 = "SELECT SERIAL_NUMBER,DEFECT_CODE,DEFECT_DESC,UPDATEUSER,UPDATE_TIME FROM SAJET.G_FAFEEDBACKINFO WHERE UPDATE_TIME BETWEEN :STARTTIME AND :ENDTIME ORDER BY UPDATE_TIME";

                    object[][] Param1 = new object[2][];
                    Param1[0] = new object[] { ParameterDirection.Input, OracleType.DateTime, "STARTTIME", dt_start.Value };
                    Param1[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "ENDTIME", dt_end.Value };


                    DataSet ds4 = ClientUtils.ExecuteSQL(sql4, Param1);
                    if (ds4.Tables[0].Rows.Count > 0)
                    {
                        m_tGridView.DataSource = ds4.Tables[0];
                        lbmessage1.Text = "查询成功！";

                    }
                    else
                    {
                        m_tGridView.DataSource = null;
                        lbmessage1.Text = "无数据！";

                    }


                }

            }
        #endregion

        private void fMain_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            this.Text = this.Text + "( " + SajetCommon.g_sFileVersion + " )";
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            lbmessage1.Text = "Message";
            g_sUserID = ClientUtils.UserPara1;
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;
        }

        }


    }



