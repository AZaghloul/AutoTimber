using Bim.Domain.Ifc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.BOQ
{
    public class GeometryCollection
    {
        public string ExcelContentType { get; set; }
        public DataTable BOQTable { get; set; }
        public List<IfElement> ElementCollection { get; set; }
        public List<int> NumberOfElements { get; set; }

        public GeometryCollection()
        {
            ElementCollection = new List<IfElement>();
            NumberOfElements = new List<int>();
            BOQTable = new DataTable("BOQ");
            BOQTable.Columns.Add("Elements Collection", typeof(string));
            BOQTable.Columns.Add("Number", typeof(int));
            ExcelContentType = @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        }

        public IfElement CheckElement(IfElement ifElement)
        {
            foreach (var E in ElementCollection)
            {
                if (E.IfDimension == ifElement.IfDimension)
                    return E;
            }
            return null;
        }

        public void AddToCollection(IEnumerable<IfElement> ifElements)
        {
            foreach (var item in ifElements)
            {
                int index;
                IfElement E = CheckElement(item);
                if (E != null)
                {
                    index = ElementCollection.IndexOf(E);
                    DataRow DR = BOQTable.Rows[index];
                    DR.SetField<int>("Number", DR.Field<int>("Number") + 1);
                    NumberOfElements[index]++;
                }
                else
                {
                    BOQTable.Rows.Add(item.ToString(), 1);
                    ElementCollection.Add(item);
                    NumberOfElements.Add(1);
                }
            }
        }

        public byte[] ToExcel(DataTable dataTable, string heading = "", bool showSrNo = false, params string[] columnsToTake)
        {
            byte[] result = null;

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", heading));
                int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 3;

                if (showSrNo)
                {
                    DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
                    dataColumn.SetOrdinal(0);
                    int index = 1;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item[0] = index;
                        index++;
                    }
                }


                // add the content into the Excel file  
                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

                // autofit width of cells with small content  
                int columnIndex = 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                    int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                    if (maxLength < 150)
                    {
                        workSheet.Column(columnIndex).AutoFit();
                    }


                    columnIndex++;
                }

                // format header - bold, yellow on black  
                using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1fb5ad"));
                }

                // format cells - add borders  
                using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }

                // removed ignored columns  
                //for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                //{
                //    if (i == 0 && showSrNo)
                //    {
                //        continue;
                //    }
                //    if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))
                //    {
                //        workSheet.DeleteColumn(i + 1);
                //    }
                //}

                if (!String.IsNullOrEmpty(heading))
                {
                    workSheet.Cells["A1"].Value = heading;
                    workSheet.Cells["A1"].Style.Font.Size = 20;

                    workSheet.InsertColumn(1, 1);
                    workSheet.InsertRow(1, 1);
                    workSheet.Column(1).Width = 5;
                }

                result = package.GetAsByteArray();
            }


            //
            return result;
        }
    }
}