using System.Data;
using OfficeOpenXml;

namespace DemoMVC.Models.Process;

public class ExcelProcess
{
  public DataTable ExcelToDataTable(string filePath)
  {
    DataTable dataTable = new DataTable();

    using (var package = new ExcelPackage(new FileInfo(filePath)))
    {
      ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

      if (worksheet == null) throw new Exception("Worksheet not found.");

      int startRow = worksheet.Dimension.Start.Row;
      int endRow = worksheet.Dimension.End.Row;
      int startCol = worksheet.Dimension.Start.Column;
      int endCol = worksheet.Dimension.End.Column;
      Console.WriteLine($"StartRow: {startRow}, EndRow: {endRow}, StartCol: {startCol}, EndCol: {endCol}");

      for (int col = startCol; col <= endCol; col++)
      {
        string columnName = worksheet.Cells[startRow, col].Text;
        dataTable.Columns.Add(columnName);
      }

      for (int row = startRow + 1; row <= endRow; row++)
      {
        DataRow dataRow = dataTable.NewRow();

        for (int col = startCol; col <= endCol; col++)
        {
          dataRow[col - startCol] = worksheet.Cells[row, col].Text;
        }

        dataTable.Rows.Add(dataRow);
      }
    }

    return dataTable;
  }
}