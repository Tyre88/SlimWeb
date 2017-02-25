using Excel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SW.Core.Helpers
{
    public class ExcelHelper
    {
        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static string CreateExcelDocument(DataTable dt, string workSheetName, bool printHeaders, string moduleName)
        {
            string path = string.Format(@"{0}Downloads\{1}\", AppDomain.CurrentDomain.BaseDirectory, moduleName);

            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = Guid.NewGuid().ToString() + ".xlsx";
            FileInfo newFile = new FileInfo(path + fileName);
            using (ExcelPackage pck = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(workSheetName);
                ws.Cells["A1"].LoadFromDataTable(dt, printHeaders);
                pck.Save();
                return fileName;
            }
        }

        public static DataTable GetDataTableFromXls(Stream stream)
        {
            IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            DataTable dt = new DataTable();
            bool firstRow = true;
            DataRow row = null;
            while (excelReader.Read())
            {
                row = dt.NewRow();
                for (int i = 0; i < excelReader.FieldCount; i++)
                {
                    if(firstRow)
                    {
                        if (dt.Columns.Contains(excelReader.GetString(i)))
                            dt.Columns.Add(string.Format("{0}-{1}", excelReader.GetString(i), i));
                        else
                            dt.Columns.Add(excelReader.GetString(i));

                    }
                    else
                    {
                        row[i] = excelReader.GetString(i);
                    }

                }

                if (!firstRow)
                    dt.Rows.Add(row);

                firstRow = false;
            }

            row = dt.NewRow();
            for (int i = 0; i < excelReader.FieldCount; i++)
            {
                row[i] = excelReader.GetString(i);
            }

            dt.Rows.Add(row);

            return dt;
        }

        public static DataTable GetDataTableFromExcel(Stream stream, bool hasHeader = true)
        {
            using (var pck = new ExcelPackage(stream))
            {
                //pck.Load(stream);
                
                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                return tbl;
            }
        }
    }
}
