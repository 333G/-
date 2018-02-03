using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Data;

using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Web;
using NPOI.SS.Util;
using NPOI.HSSF.Util;
using NFine.Code.ViewModel;

namespace NFine.Code
{
    /// <summary>
    /// Excel操作类
    /// </summary>
    public class ExcelHelperPro
    {
        #region 导出excel
        private const Int32 MaxRowPerSheet = 65535;
        private Int32 _rowPerSheet = 1000;
        /// <summary>
        /// 单sheet最大行数
        /// </summary>
        public Int32 RowPerSheet
        {
            get { return _rowPerSheet; }
            set
            {
                if (value < 0 || value > MaxRowPerSheet)
                {
                    value = MaxRowPerSheet;
                }
                else
                {
                    _rowPerSheet = value;
                }
            }
        }
        /// <summary>
        /// 表头文本
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// 填充导出数据并返回Iworkbook
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="records"></param>
        /// <param name="headers"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public void Export<T>(IList<T> records, IList<DataExport.ExportColumn> headers, string fileName = null)
        {
            if (records == null)
                throw new ArgumentNullException("records");
            if (headers == null || headers.Count == 0)
                throw new ArgumentNullException("headers");

            var props = new PropertyInfo[headers.Count];
            for (var i = 0; i < headers.Count; i++)
            {
                props[i] = typeof(T).GetProperty(headers[i].dataIndex); //注意属性数组仍然可以有元素为null
            }

            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = null;

            var titleRowIndex = 0;
            for (var r = 0; r < records.Count; r++)
            {
                IRow row = null;
                if ((r % RowPerSheet) == 0)
                {
                    var sheetIndex = (Int32)((Double)r / RowPerSheet) + 1;
                    sheet = workbook.CreateSheet("Sheet" + sheetIndex);

                    if (!string.IsNullOrEmpty(HeaderText))
                    {
                        titleRowIndex = 1;
                        var headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(HeaderText);
                        headerRow.GetCell(0).CellStyle = GetCellStyle(workbook, ExcelStyle.Header);

                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, headers.Count - 1));
                    }

                    row = sheet.CreateRow(titleRowIndex);
                    for (var i = 0; i < headers.Count; i++)
                    {
                        row.CreateCell(i).SetCellValue(headers[i].text);
                        row.GetCell(i).CellStyle = GetCellStyle(workbook, ExcelStyle.Title);
                    }
                }


                //注意CreateRow(Int32 rownum)中参数rownum虽然从第0行开始，但因为表头存在，每次得往下一行
                if (sheet != null) row = sheet.CreateRow(r % RowPerSheet + (titleRowIndex + 1));
                for (var i = 0; i < props.Length; i++)
                {
                    if (props[i] == null) continue;
                    var value = props[i].GetValue(records[r], null);
                    if (value == null) continue;
                    var drValue = value.ToString();
                    if (row != null)
                    {
                        var newCell = row.CreateCell(i);
                        switch (value.GetType().FullName)
                        {
                            case "System.String"://字符串类型
                                newCell.SetCellValue(drValue);
                                newCell.CellStyle = GetCellStyle(workbook, ExcelStyle.Default);
                                break;
                            case "System.DateTime"://日期类型
                                DateTime dateV;
                                DateTime.TryParse(drValue, out dateV);
                                newCell.SetCellValue(dateV);
                                newCell.CellStyle = GetCellStyle(workbook, ExcelStyle.Date); ;//格式化显示
                                break;
                            case "System.Boolean"://布尔型
                                bool boolV = false;
                                bool.TryParse(drValue, out boolV);
                                newCell.SetCellValue(boolV);
                                newCell.CellStyle = GetCellStyle(workbook, ExcelStyle.Default); ;//格式化显示
                                break;
                            case "System.Int16"://整型
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                int intV = 0;
                                int.TryParse(drValue, out intV);
                                newCell.SetCellValue(intV);
                                newCell.CellStyle = GetCellStyle(workbook, ExcelStyle.Number); ;//格式化显示
                                break;
                            case "System.Decimal"://浮点型
                            case "System.Double":
                                double doubV = 0;
                                double.TryParse(drValue, out doubV);
                                newCell.SetCellValue(doubV);
                                newCell.CellStyle = GetCellStyle(workbook, ExcelStyle.Decimal); ;//格式化显示
                                break;
                            case "System.DBNull"://空值处理
                                newCell.SetCellValue("");
                                newCell.CellStyle = GetCellStyle(workbook, ExcelStyle.Default); ;//格式化显示
                                break;
                            default:
                                newCell.SetCellValue("");
                                newCell.CellStyle = GetCellStyle(workbook, ExcelStyle.Default); ;//格式化显示
                                break;
                        }
                    }
                }
            }

            for (var i = 0; i < workbook.NumberOfSheets; i++)
            {
                sheet = workbook.GetSheetAt(i);
                for (var h = 0; h < headers.Count; h++)
                {
                    if (headers[h].width > 0)
                        sheet.SetColumnWidth(h, headers[h].width * 40);
                    else
                        sheet.AutoSizeColumn(h); //每列宽度自适应
                }
            }
            this.Export(workbook, fileName);
        }

        /// <summary>
        /// web导出
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="fileName"></param>
        private void Export(IWorkbook workbook, string fileName)
        {
            if (fileName == null) fileName = HeaderText;
            using (var ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;

                HttpContext content = HttpContext.Current;

                // 设置编码和附件格式
                content.Response.ContentType = "application/vnd.ms-excel";
                content.Response.ContentEncoding = Encoding.UTF8;
                content.Response.Charset = "";
                content.Response.AppendHeader("Content-Disposition",
                    "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8) + ".xls");

                content.Response.BinaryWrite(ms.GetBuffer());
                content.Response.End();
            }
        }
        /// <summary>
        /// excel样式
        /// </summary>
        public enum ExcelStyle
        {
            /// <summary>
            /// 头
            /// </summary>
            Header,
            /// <summary>
            /// 标题
            /// </summary>
            Title,
            /// <summary>
            /// 链接
            /// </summary>
            Url,
            /// <summary>
            /// 时间
            /// </summary>
            Date,
            /// <summary>
            /// 数字
            /// </summary>
            Number,
            /// <summary>
            /// 货币
            /// </summary>
            Decimal,
            /// <summary>
            /// 百分比
            /// </summary>
            Percentage,
            /// <summary>
            /// 科学计数
            /// </summary>
            ScientificNotation,
            /// <summary>
            /// 默认
            /// </summary>
            Default
        }

        /// <summary>
        /// 单元格样式
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="sty"></param>
        /// <returns></returns>
        static ICellStyle GetCellStyle(IWorkbook wb, ExcelStyle sty)
        {
            var cellStyle = wb.CreateCellStyle();

            //定义几种字体  
            var font = wb.CreateFont();
            font.FontName = "微软雅黑";

            var fontcolorblue = wb.CreateFont();
            fontcolorblue.Color = HSSFColor.Blue.Index;
            fontcolorblue.IsItalic = true;//下划线  
            fontcolorblue.FontName = "微软雅黑";

            //边框  
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            //边框颜色  
            cellStyle.BottomBorderColor = HSSFColor.Black.Index;
            cellStyle.TopBorderColor = HSSFColor.Black.Index;

            //水平对齐  
            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;

            //垂直对齐  
            cellStyle.VerticalAlignment = VerticalAlignment.Center;

            //自动换行  
            //cellStyle.WrapText = true;

            //缩进;当设置为1时，前面留的空白太大了。希旺官网改进。或者是我设置的不对  
            cellStyle.Indention = 0;

            //上面基本都是设共公的设置  
            //下面列出了常用的字段类型  
            switch (sty)
            {
                case ExcelStyle.Header:
                    cellStyle.Alignment = HorizontalAlignment.Center;
                    var headerFont = wb.CreateFont();
                    headerFont.FontHeightInPoints = 20;
                    headerFont.Boldweight = 700;
                    cellStyle.SetFont(headerFont);
                    break;
                case ExcelStyle.Title:
                    cellStyle.Alignment = HorizontalAlignment.Center;
                    var titleFont = wb.CreateFont();
                    titleFont.FontHeightInPoints = 10;
                    titleFont.Boldweight = 700;
                    cellStyle.SetFont(titleFont);
                    break;
                case ExcelStyle.Date:
                    IDataFormat datastyle = wb.CreateDataFormat();

                    cellStyle.DataFormat = datastyle.GetFormat("yyyy/MM/dd HH:mm");
                    cellStyle.SetFont(font);
                    break;
                case ExcelStyle.Number:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                    cellStyle.SetFont(font);
                    break;
                case ExcelStyle.Decimal:
                    var format = wb.CreateDataFormat();
                    cellStyle.DataFormat = format.GetFormat("￥#,##0");
                    cellStyle.SetFont(font);
                    break;
                case ExcelStyle.Url:
                    //fontcolorblue.Underline = 1;
                    cellStyle.SetFont(fontcolorblue);
                    break;
                case ExcelStyle.Percentage:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00%");
                    cellStyle.SetFont(font);
                    break;
                case ExcelStyle.ScientificNotation:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00E+00");
                    cellStyle.SetFont(font);
                    break;
                case ExcelStyle.Default:
                    cellStyle.SetFont(font);
                    break;
            }
            return cellStyle;
        }
        #endregion

        #region 读取excel
        /// <summary>
        /// 从excel读取为dataset
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="HeaderRowIndex"></param>
        /// <returns></returns>
        public static DataSet RenderDataSetFromExcel(string FilePath, int HeaderRowIndex = 0)
        {
            HSSFWorkbook workbook;
            using (var excelFileStream = File.Open(FilePath, FileMode.Open, FileAccess.Read))
            {
                workbook = new HSSFWorkbook(excelFileStream);
            }

            var ds = new DataSet();
            for (int sheetIndex = 0; sheetIndex < workbook.NumberOfSheets; sheetIndex++)
            {
                var sheet = workbook.GetSheetAt(sheetIndex);
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

                var dt = new DataTable(sheet.SheetName);

                var headerRow = sheet.GetRow(HeaderRowIndex);
                int cellCount = headerRow.LastCellNum;

                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    var column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    dt.Columns.Add(column);
                }

                var rowCount = sheet.LastRowNum;

                for (var j = (sheet.FirstRowNum + 1); j <= sheet.LastRowNum; j++)
                {
                    var row = sheet.GetRow(j);
                    var dr = dt.NewRow();
                    for (var i = 0; i < row.LastCellNum; i++)
                    {
                        var cell = row.GetCell(i);

                        if (cell == null)
                        {
                            dr[i] = null;
                        }
                        else
                        {
                            dr[i] = cell.ToString();
                        }
                    }
                    dt.Rows.Add(dr);
                }

                ds.Tables.Add(dt);
            }


            return ds;
        }
        /// <summary>
        /// 从excel读取为datatable
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sheetIndex"></param>
        /// <param name="headerRowIndex"></param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(string filePath, int sheetIndex = 0, int headerRowIndex = 0)
        {
            HSSFWorkbook workbook;
            using (var excelFileStream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                workbook = new HSSFWorkbook(excelFileStream);
            }

            var sheet = workbook.GetSheetAt(sheetIndex);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            var dt = new DataTable(sheet.SheetName);

            var headerRow = sheet.GetRow(headerRowIndex);
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                var column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                dt.Columns.Add(column);
            }

            var rowCount = sheet.LastRowNum;

            for (int j = (sheet.FirstRowNum + 1); j < sheet.LastRowNum; j++)
            {
                var row = sheet.GetRow(j);
                var dr = dt.NewRow();
                for (var i = 0; i < row.LastCellNum; i++)
                {
                    var cell = row.GetCell(i);

                    if (cell == null)
                    {
                        dr[i] = null;
                    }
                    else
                    {
                        dr[i] = cell.ToString();
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion
    }
}
