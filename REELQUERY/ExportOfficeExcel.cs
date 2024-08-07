using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
//using System.Windows.Forms;
using Excel;
using System.Runtime.InteropServices;

namespace ExportOfficeExcel
{
    public class CreateOfficeExcel
    {
        // 存檔路徑
        private string _SaveFilePath;
        public string SaveFilePath
        {
            get { return _SaveFilePath; }
            set { _SaveFilePath = value; }
        }
        //範本檔路徑
        private string _SampleFilePath;
        public string SampleFilePath
        {
            get { return _SampleFilePath; }
            set { _SampleFilePath = value; }
        }
        //SheetName
        private string _SheetName;
        public string SheetName
        {
            get { return _SheetName; }
            set { _SheetName = value; }
        }
        //是否須存檔
        private bool _Save;
        public bool Save
        {
            get { return _Save; }
            set { _Save = value; }
        }
        //巨集名稱
        private string _Macro;
        public string Macro
        {
            get { return _Macro; }
            set { _Macro = value; }
        }
        
        //是否須列印
        private bool _Print;
        public bool Print
        {
            get { return _Print; }
            set { _Print = value; }
        }
        //列印張數
        private int _PrintQty;
        public int PrintQty
        {
            get { return _PrintQty; }
            set { _PrintQty = value; }
        }

        public CreateOfficeExcel() : this(string.Empty,string.Empty) { }
        public CreateOfficeExcel(string SaveFilePath) : this(SaveFilePath, string.Empty) { }
        public CreateOfficeExcel(string SaveFilePath, string SampleFilePath)
        {
            this.SaveFilePath = SaveFilePath;
            this.SampleFilePath = SampleFilePath;

            _Save = true;
            _Print = false;
            _SheetName = "Sheet1";
            _PrintQty = 1;
            _Macro = string.Empty;            
        }

        /*
        public bool ExportToOfficeExcel(DataGridView GridView)
        {
            DataSet dsGrid = new DataSet();
            dsGrid.Tables.Add();
            for (int i = 0; i <= GridView.Columns.Count - 1; i++)
            {
                dsGrid.Tables[0].Columns.Add(GridView.Columns[i].Name);
            }

            for (int i = 0; i <= GridView.Rows.Count - 1; i++)
            {
                dsGrid.Tables[0].Rows.Add();
                for (int j = 0; j <= GridView.Columns.Count - 1; j++)
                {
                    dsGrid.Tables[0].Rows[i][j] = GridView.Rows[i].Cells[j].Value;
                }
            }
            return ExportToOfficeExcel(dsGrid);
        }
         */ 
        public bool ExportToOfficeExcel(DataSet ds)
        {
            return ExportToOfficeExcel(ds.Tables[0]);
        }
        public bool ExportToOfficeExcel(System.Data.DataTable dt)
        {
            Excel.Application IExcelApp = null;
            Excel.Workbook IExcelWorkBook = null;
            Excel.Worksheet IExcelWorkSheet = null;
            Excel.Range IExcelRange = null;

            try
            {                
                IExcelApp = new Excel.Application();                
                IExcelApp.Visible = false;

                //產生新的 Excel Work Book
                if (string.IsNullOrEmpty(SampleFilePath))
                {
                    IExcelWorkBook = (Excel.Workbook)(IExcelApp.Workbooks.Add(Type.Missing));
                    IExcelWorkSheet = (Excel.Worksheet)IExcelWorkBook.ActiveSheet;
                    IExcelWorkSheet.Name = SheetName;
                }
                else //開啟範本檔
                {
                    IExcelWorkBook = (Excel.Workbook)(IExcelApp.Workbooks.Add(SampleFilePath));
                    IExcelWorkSheet = (Excel.Worksheet)IExcelWorkBook.Worksheets.get_Item(SheetName);
                }
                 

                //製作表頭
                DataColumn dc;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dc = dt.Columns[i];
                    IExcelWorkSheet.Cells[1, i + 1] = dc.ColumnName;
                }

                //內容
                int int_Row = 2;
                DataRow dr;
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    dr = dt.Rows[j];

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dc = dt.Columns[i];
                        IExcelWorkSheet.Cells[int_Row, i + 1] = dr[i].ToString();
                    }
                    int_Row = int_Row + 1;
                }

                //Run巨集
                if (!string.IsNullOrEmpty(Macro))
                {
                    RunMacro(IExcelApp, new object[] { Macro });
                }

                //存入 
                if (Save)
                    IExcelWorkBook.SaveAs(SaveFilePath, Excel.XlFileFormat.xlWorkbookNormal, null, null, false, false, Excel.XlSaveAsAccessMode.xlExclusive, false, false, null, null, null);
                //列印
                if (Print)
                {                
                    IExcelWorkBook.PrintOut(1, Type.Missing, PrintQty, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }                
                return true;
            }
            catch (Exception e)
            {
                
                return false;
            }
            finally
            {
                //釋放所有物件及資源
                if (IExcelWorkBook != null)
                    IExcelWorkBook.Close(false, null, null);
                if (IExcelApp != null)
                {
                    IExcelApp.Workbooks.Close();
                    IExcelApp.Quit();
                }

                NAR(IExcelRange);
                NAR(IExcelWorkSheet);
                NAR(IExcelWorkBook);
                NAR(IExcelApp);
               
                IExcelWorkSheet = null;
                IExcelWorkBook = null;
                IExcelApp = null;
                GC.Collect();
            }
        }

        private void NAR(object o)
        {
            //為了解決記憶體無法釋放
            try
            {
                if (o != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
            }
            finally
            {
                o = null;
            }
        }

        private void RunMacro(object oApp, object[] oRunArgs)
        {
            oApp.GetType().InvokeMember("Run", System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.InvokeMethod, null, oApp, oRunArgs);
        }
    }

    public class ExcelEdit
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out   int ID); 
        public string mFilename;
        public Excel.Application app;
        public Excel.Workbooks wbs;
        public Excel.Workbook wb;
        public Excel.Worksheets wss;
        public Excel.Worksheet ws;
        public ExcelEdit()
        {

        }
        public void RunMacro(string sMacorName)
        {
            app.GetType().InvokeMember("Run", System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.InvokeMethod, null, app, new object[] { sMacorName });
        }
        public void Create()//建立Excel
        {
            app = new Excel.Application();
            wbs = app.Workbooks;
            wb = wbs.Add(true);
        }
        public void Open(string FileName)//打開Excel範本
        {
            app = new Excel.Application();
            wbs = app.Workbooks;
            wb = wbs.Add(FileName);
            mFilename = FileName;
        }
        public int GetSheetCount()
        {
            return wb.Sheets.Count;
        }
        public Excel.Worksheet GetSheet(int i)
        //獲取一個工作表
        {
            Excel.Worksheet s = (Excel.Worksheet)wb.Worksheets[i];
            return s;
        }
        public Excel.Worksheet GetSheet(string SheetName)
        //獲取一個工作表
        {
            Excel.Worksheet s = (Excel.Worksheet)wb.Worksheets[SheetName];
            return s;
        }

        public Excel.Worksheet AddSheet(string SheetName)
        //加入一個工作表
        {
            Excel.Worksheet s = (Excel.Worksheet)wb.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            s.Name = SheetName;
            return s;
        }

        public void DelSheet(string SheetName)
        //刪除一個工作表
        {
            ((Excel.Worksheet)wb.Worksheets[SheetName]).Delete();
        }

        public Excel.Worksheet ReNameSheet(string OldSheetName, string NewSheetName)
        //重命名一個工作表一
        {
            Excel.Worksheet s = (Excel.Worksheet)wb.Worksheets[OldSheetName];
            s.Name = NewSheetName;
            return s;
        }
        public Excel.Worksheet ReNameSheet(Excel.Worksheet Sheet, string NewSheetName)
        //重命名一個工作表二
        {
            Sheet.Name = NewSheetName;
            return Sheet;
        }

        //ws：要設值的工作表  X行Y列 value值 
        public void SetCellValue(Excel.Worksheet ws, int x, int y, object value)
        {
            ws.Cells[y, x] = value;
        }
        //ws：要讀值的工作表  X行Y列 value值 
        public string ReadCellValue(Excel.Worksheet ws, int x, int y)
        {
            Excel.Range r = (Excel.Range)ws.Cells[y, x];
            return r.Text.ToString();
        }
        public void SetCellValue(string SheetName, int x, int y, object value)
        {
            SetCellValue(GetSheet(SheetName), x, y, value);
        }
        public string GetCellValue(Excel.Worksheet ws, int x, int y)
        {
            //return  ws.Columns[y, x];
            Excel.Range r = (Excel.Range)ws.Cells[y, x];
            return  r.Text.ToString();
            //return ws.Cells[y, x].;
        }
        public string GetCellValue(string SheetName, int x, int y)
        {
            return GetCellValue(GetSheet(SheetName), x, y);
        }
        //設置一個單元格的字型(字體,大小,粗體)
        public void SetCellFont(Excel.Worksheet ws, int Startx, int Starty, int Endx, int Endy, int FontSize, string FontName, bool FontBold)
        {
            //name = "新細明體";
            //size = 12; 
            if (!string.IsNullOrEmpty(FontName))
                ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).Font.Name = FontName;     //字型
            ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).Font.Size = FontSize;         //大小
            ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).Font.Bold = FontBold;         //粗體    
        }
        public void SetCellFont(string SheetName, int Startx, int Starty, int Endx, int Endy, int FontSize, string FontName, bool FontBold)
        {
            Excel.Worksheet ws = GetSheet(SheetName);
            SetCellFont(ws, Startx, Starty, Endx, Endy, FontSize, FontName, FontBold);
        }

        //設置單元格的屬性(對齊方式)
        public void SetCellAlignment(Excel.Worksheet ws, int Startx, int Starty, int Endx, int Endy, string Vertical, string Horizontal)
        {
            //垂直對齊
            if (!string.IsNullOrEmpty(Vertical))
            {
                if (Vertical == "T")
                    ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
                else if (Vertical == "C")
                    ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                else if (Vertical == "B")
                    ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).VerticalAlignment = Excel.XlVAlign.xlVAlignBottom;
            }
            //水平對齊 
            if (!string.IsNullOrEmpty(Horizontal))
            {
                if (Horizontal == "L")
                    ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                else if (Horizontal == "C")
                    ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                else if (Horizontal == "R")
                    ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            }
        }
        public void SetCellAlignment(string SheetName, int Startx, int Starty, int Endx, int Endy, string Vertical, string Horizontal)
        {
            Excel.Worksheet ws = GetSheet(SheetName);
            SetCellAlignment(ws, Startx, Starty, Endx, Endy, Vertical, Horizontal);
        }

        //設置單元格的屬性(自動調整列高)
        public void SetCellAutoFit(Excel.Worksheet ws, int Startx, int Starty, int Endx, int Endy, bool EntireRow, bool EntireColumn)
        {
            //自動調整列高 
            if (EntireRow)
                ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).EntireRow.AutoFit();

            //自動調整欄寬
            if (EntireColumn)
                ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).EntireColumn.AutoFit();
        }
        public void SetCellAutoFit(string SheetName, int Startx, int Starty, int Endx, int Endy, bool EntireRow, bool EntireColumn)
        {
            Excel.Worksheet ws = GetSheet(SheetName);
            SetCellAutoFit(ws, Startx, Starty, Endx, Endy, EntireRow, EntireColumn);
        }


        //設置單元格的字型或儲存格顏色
        public void SetCellColor(Excel.Worksheet ws, int Startx, int Starty, int Endx, int Endy, object FontColor, object CellColor)
        {
            //FontColor=Color.Red;
            if (FontColor != null)
                ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).Font.Color = System.Drawing.ColorTranslator.ToOle((System.Drawing.Color)FontColor);
            if (CellColor != null)
                ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).Interior.Color = System.Drawing.ColorTranslator.ToOle((System.Drawing.Color)CellColor);
        }
        public void SetCellColor(string SheetName, int Startx, int Starty, int Endx, int Endy, object FontColor, object CellColor)
        {
            Excel.Worksheet ws = GetSheet(SheetName);
            SetCellColor(ws, Startx, Starty, Endx, Endy, FontColor, CellColor);
        }

        //調整儲存格格式(設為文字格式)
        public void SetCellNumberFormat(Excel.Worksheet ws, int Startx, int Starty, int Endx, int Endy)
        {
            ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).NumberFormatLocal = "@";
        }
        public void SetCellNumberFormat(string SheetName, int Startx, int Starty, int Endx, int Endy)
        {
            Excel.Worksheet ws = GetSheet(SheetName);
            SetCellNumberFormat(ws, Startx, Starty, Endx, Endy);
        }

        //合倂單元格
        public void MergeCells(Excel.Worksheet ws, int x1, int y1, int x2, int y2)
        {
            ws.get_Range(ws.Cells[y1, x1], ws.Cells[y2, x2]).Merge(Type.Missing);
        }
        public void MergeCells(string SheetName, int x1, int y1, int x2, int y2)
        {
            MergeCells(GetSheet(SheetName), x1, y1, x2, y2);
        }

        //將DataTable加到Excel指定工作表的指定位置
        public void AddTable(System.Data.DataTable dt, string SheetName, int startX, int startY)
        {
            AddTable(dt, GetSheet(SheetName), startX, startY);
        }
        public void AddTable(System.Data.DataTable dt, Excel.Worksheet ws, int startX, int startY)
        {
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {
                    ws.Cells[j + startY, i + startX] = dt.Rows[i][j];
                }
            }
        }

        public void SetCellLineStyle(Excel.Worksheet ws, int Startx, int Starty, int Endx, int Endy, int LineStyle,int iDirecttion)
        {
            if (iDirecttion==1)
              ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).Borders[XlBordersIndex.xlEdgeTop].LineStyle = LineStyle;
            if (iDirecttion==2)
              ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).Borders[XlBordersIndex.xlEdgeRight].LineStyle = LineStyle;
            if (iDirecttion==3)
              ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).Borders[XlBordersIndex.xlEdgeBottom].LineStyle = LineStyle;
            if (iDirecttion==4)
              ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).Borders[XlBordersIndex.xlEdgeLeft].LineStyle = LineStyle;

        }
        //儲存格框線
        public void SetCellLineStyle(Excel.Worksheet ws, int Startx, int Starty, int Endx, int Endy, int LineStyle)
        {
            //LineStyle = 1;
            
            ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).Borders.LineStyle = LineStyle;
        }
        public void SetCellLineStyle(string SheetName, int Startx, int Starty, int Endx, int Endy, int LineStyle)
        {
            Excel.Worksheet ws = GetSheet(SheetName);
            SetCellLineStyle(ws, Startx, Starty, Endx, Endy, LineStyle);
        }
        public void SetCellLineStyle(string SheetName, int Startx, int Starty, int Endx, int Endy, int LineStyle,int iDirection)
        {
            Excel.Worksheet ws = GetSheet(SheetName);
            SetCellLineStyle(ws, Startx, Starty, Endx, Endy, LineStyle,iDirection);
        }

        public void InsertActiveChart(Excel.XlChartType ChartType, string SheetName, int DataSourcesX1, int DataSourcesY1, int DataSourcesX2, int DataSourcesY2, Excel.XlRowCol ChartDataType)
        //插入圖片操作
        {
            ChartDataType = Excel.XlRowCol.xlColumns;
            wb.Charts.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            {
                wb.ActiveChart.ChartType = ChartType;
                wb.ActiveChart.SetSourceData(GetSheet(SheetName).get_Range(GetSheet(SheetName).Cells[DataSourcesY1, DataSourcesX1], GetSheet(SheetName).Cells[DataSourcesX2, DataSourcesY2]), ChartDataType);
                wb.ActiveChart.Location(Excel.XlChartLocation.xlLocationAsObject, SheetName);
            }
        }

        public bool Save()
        //儲存文件
        {
            if (mFilename == "")
            {
                return false;
            }

            try
            {
                wb.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool SaveAs(object FileName)
        //文件另存為
        {
            if (mFilename == "")
            {
                return false;
            }

            try
            {
                wb.SaveAs(FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Close()
        //關閉Excel,釋放Excel
        {
            wb.Close(false, Type.Missing, Type.Missing);
            wbs.Close();
            app.Quit();
            try 
            {
                IntPtr t = new IntPtr(app.Hwnd);
                int k = 0;
                GetWindowThreadProcessId(t, out   k);
                System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);
                p.Kill();
            }
            catch(Exception ex)
            {
                
            }

            NAR(wbs);
            NAR(wb);
            NAR(app);

            wb = null;
            wbs = null;
            app = null;
            GC.Collect();
        }
        private void NAR(object o)
        {
            //為了解決記憶體無法釋放
            try
            {
                if (o != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
            }
            finally
            {
                o = null;
            }
        }
    }
}
