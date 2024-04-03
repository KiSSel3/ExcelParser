using ExcelParser.Parser.Models;
using OfficeOpenXml;

namespace ExcelParser.Parser;

public class Parser
{
    private readonly string _filePath;
    private readonly int _columnCount;
    private readonly int _firstRelevantLine;

    public Parser(string filePath, int columnCount, int firstRelevantLine)
    {
        _filePath = filePath;
        _columnCount = columnCount;
        _firstRelevantLine = firstRelevantLine;
    }

    public List<TableRow> GetTableRows(string tableName)
    {
        List<TableRow> tableRows = new List<TableRow>();
        
        FileInfo fileInfo = new FileInfo(_filePath);
        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[tableName];
            if (worksheet == null)
            {
                throw new Exception("Table not found");
            }
            
            int currentRowCount = worksheet.Dimension.Rows;
            int currentColumnCount = worksheet.Dimension.Columns;
            if (currentColumnCount < _columnCount || currentRowCount < _firstRelevantLine)
            {
                throw new Exception("Incorrect table size");
            }

            for (int row = _firstRelevantLine; row <= currentRowCount; ++row)
            {
                for (int col = 1; col <= _columnCount; ++col)
                {
                    TableRow newTableRow = GetTableRow(row, col, worksheet);
                    if (newTableRow == null)
                    {
                        return tableRows;
                    }
                    
                    tableRows.Add(newTableRow);
                }
            }
        }

        return tableRows;
    }

    private TableRow GetTableRow(int row, int col, ExcelWorksheet worksheet)
    {
        throw new NotImplementedException();
    }
}