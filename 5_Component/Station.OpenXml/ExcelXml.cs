using System.Data;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Station.OpenXml
{
    public static class ExcelXml
    {
        public static void ExcelTest(string excelOutput,DataTable dataTable)
        {
            using var spreadsheetDocument = SpreadsheetDocument.Create(excelOutput, SpreadsheetDocumentType.Workbook);
            var workbookPart = spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());
            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());
            Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };
            sheets.Append(sheet);
            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
            #region 此处换成datatable
            for (int i = 0; i < 10; i++)
            {
                Row row = new Row();
                for (int j = 0; j < 10; j++)
                {
                    Cell dataCell = new Cell();
                    dataCell.CellValue = new CellValue($"{i + 1}行{j + 1}列");
                    dataCell.DataType = new EnumValue<CellValues>(CellValues.String);
                    row.AppendChild(dataCell);
                }
                sheetData.Append(row);
            }
            #endregion
            workbookPart.Workbook.Save();
            spreadsheetDocument.Close();
        }
    }
}