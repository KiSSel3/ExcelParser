using ExcelParser.Parser.Models;
using ExcelParser.Parser.Utils;
using OfficeOpenXml;

namespace ExcelParser.Parser;

public class Parser
{
    private readonly string _filePath;
    private readonly int _columnCount;
    private readonly int _firstRelevantLine;

    private string? _lastId;
    private string? _lastBankName;
    private string? _lastCreditProduct;

    private RateСalculator _rateСalculator;
    public Parser(string filePath, int columnCount, int firstRelevantLine)
    {
        _filePath = filePath;
        _columnCount = columnCount;
        _firstRelevantLine = firstRelevantLine;

        _lastId = null;
        _lastBankName = null;
        _lastCreditProduct = null;

        //TODO: заглушка
        _rateСalculator = new RateСalculator();
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
                TableRow? newTableRow = GetTableRow(row,worksheet);
                if (newTableRow == null)
                {
                    return tableRows;
                }
                    
                tableRows.Add(newTableRow);
            }
        }

        return tableRows;
    }

    private TableRow? GetTableRow(int row, ExcelWorksheet worksheet)
    {
        TableRow? newTableRow = new TableRow();

        newTableRow.Id = ParseId(row, worksheet);
        newTableRow.BankName = ParseBankName(row, worksheet);
        newTableRow.CreditProduct = ParseCreditProduct(row, worksheet);

        newTableRow.TermMin = ParseTermMin(row, worksheet);
        newTableRow.TermMax = ParseTermMax(row, worksheet);

        newTableRow.Period = ParsePeriod(row, worksheet);

        newTableRow.RateMin = ParseRateMin(row, worksheet);
        newTableRow.RateMax = ParseRateMax(row, worksheet);

        newTableRow.Note = ParseNote(row, worksheet);
        
        if (newTableRow.TermMin == null && newTableRow.TermMax == null && newTableRow.Period == null &&
            newTableRow.RateMin == null && newTableRow.RateMax == null && newTableRow.Note == null)
        {
            return null;
        }

        return newTableRow;
    }

    private string? ParseId(int row, ExcelWorksheet worksheet)
    {
        string? cellValue = worksheet.Cells[row, 1].Value?.ToString();
        if (cellValue is not null)
        {
            _lastId = cellValue;
        }

        return _lastId;
    }
    
    private string? ParseBankName(int row, ExcelWorksheet worksheet)
    {
        string? cellValue = worksheet.Cells[row, 2].Value?.ToString();
        if (cellValue is not null)
        {
            _lastBankName = cellValue;
        }

        return _lastBankName;
    }
    
    private string? ParseCreditProduct(int row, ExcelWorksheet worksheet)
    {
        string? cellValue = worksheet.Cells[row, 3].Value?.ToString();
        if (cellValue is not null)
        {
            _lastCreditProduct = cellValue;
        }

        return _lastBankName;
    }

    private int? ParseTermMin(int row, ExcelWorksheet worksheet)
    {
        string? cellValue = worksheet.Cells[row, 4].Value?.ToString();
        if (cellValue is null)
        {
            return null;
        }

        return Convert.ToInt32(cellValue);
    }
    
    private int? ParseTermMax(int row, ExcelWorksheet worksheet)
    {
        string? cellValue = worksheet.Cells[row, 5].Value?.ToString();
        if (cellValue is null)
        {
            return null;
        }

        return Convert.ToInt32(cellValue);
    }

    private string? ParsePeriod(int row, ExcelWorksheet worksheet)
    {
        string? cellValue = worksheet.Cells[row, 6].Value?.ToString();
        
        return cellValue;
    }

    private Rate? ParseRateMin(int row, ExcelWorksheet worksheet)
    {
        string? cellValue = worksheet.Cells[row, 7].Value?.ToString();
        if (cellValue is null)
        {
            return null;
        }
        
        return _rateСalculator.Сalculation(cellValue);
    }
    
    private Rate? ParseRateMax(int row, ExcelWorksheet worksheet)
    {
        string? cellValue = worksheet.Cells[row, 8].Value?.ToString();
        if (cellValue is null)
        {
            return null;
        }
        
        return _rateСalculator.Сalculation(cellValue);
    }
    
    private string? ParseNote(int row, ExcelWorksheet worksheet)
    {
        string? cellValue = worksheet.Cells[row, 9].Value?.ToString();
        
        return cellValue;
    }
}