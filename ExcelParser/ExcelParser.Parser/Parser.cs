using System.Threading.Channels;
using ExcelParser.Parser.Models;
using ExcelParser.Parser.Utils;
using OfficeOpenXml;

namespace ExcelParser.Parser;

public class Parser
{
    private readonly string _filePath;
    private readonly int _columnCount;

    private string? _lastId;
    private string? _lastBankName;
    private string? _lastCreditProduct;

    private bool _finishParsing;
    
    private RateСalculator _rateСalculator;
    public Parser(string filePath, int columnCount)
    {
        _filePath = filePath;
        _columnCount = columnCount;

        _lastId = null;
        _lastBankName = null;
        _lastCreditProduct = null;

        _finishParsing = false;
        
        _rateСalculator = new RateСalculator();
    }

    public List<TableRow> GetTableRows(string tableName, int firstRelevantLine)
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
            if (currentColumnCount < _columnCount || currentRowCount < firstRelevantLine)
            {
                throw new Exception("Incorrect table size");
            }

            for (int row = firstRelevantLine; row <= currentRowCount; ++row)
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
        _finishParsing = true;
        
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
        
        if (_finishParsing)
        {
            return null;
        }

        return newTableRow;
    }

    private string? ParseId(int row, ExcelWorksheet worksheet)
    {
        try
        {
            ExcelRange cell = worksheet.Cells[row, 1];

            string? cellValue = cell.Value?.ToString();
            if (string.IsNullOrEmpty(cellValue))
            {
                if (!cell.Merge)
                {
                    return null;
                }

                _finishParsing = false;
                return _lastId;
            }

            _lastId = cellValue;
            _finishParsing = false;
        
            return cellValue;
        }
        catch
        {
            return null;
        }
    }
    
    private string? ParseBankName(int row, ExcelWorksheet worksheet)
    {
        try
        {
            ExcelRange cell = worksheet.Cells[row, 2];
        
            string? cellValue = cell.Value?.ToString();
            if (string.IsNullOrEmpty(cellValue))
            {
                if (!cell.Merge)
                {
                    return null;
                }
            
                _finishParsing = false;
                return _lastBankName;
            }
                    
            _lastBankName = cellValue;
            _finishParsing = false;
        
            return _lastBankName;
        }
        catch
        {
            return null;
        }
    }
    
    private string? ParseCreditProduct(int row, ExcelWorksheet worksheet)
    {
        try
        {
            ExcelRange cell = worksheet.Cells[row, 3];
        
            string? cellValue = cell.Value?.ToString();
            if (string.IsNullOrEmpty(cellValue))
            {
                if (!cell.Merge)
                {
                    return null;
                }
            
                _finishParsing = false;
                return _lastCreditProduct;
            }
        
            _lastCreditProduct = cellValue;
            _finishParsing = false;
        
            return _lastCreditProduct;
        }
        catch
        {
            return null;
        }
    }

    private int? ParseTermMin(int row, ExcelWorksheet worksheet)
    {
        try
        {
            string? cellValue = worksheet.Cells[row, 4].Value?.ToString();
        
            bool successfully = int.TryParse(cellValue, out int result);
            if (!successfully)
            {
                return null;
            }
        
            _finishParsing = false;
        
            return result;
        }
        catch
        {
            return null;
        }
    }
    
    private int? ParseTermMax(int row, ExcelWorksheet worksheet)
    {
        try
        {
            string? cellValue = worksheet.Cells[row, 5].Value?.ToString();
        
            bool successfully = int.TryParse(cellValue, out int result);
            if (!successfully)
            {
                return null;
            }
        
            _finishParsing = false;
        
            return result;
        }
        catch
        {
            return null;
        }
    }

    private string? ParsePeriod(int row, ExcelWorksheet worksheet)
    {
        try
        {
            string? cellValue = worksheet.Cells[row, 6].Value?.ToString();
            if (string.IsNullOrEmpty(cellValue))
            {
                return null;
            }
        
            _finishParsing = false;
        
            return cellValue;
        }
        catch
        {
            return null;
        }
    }

    private Rate? ParseRateMin(int row, ExcelWorksheet worksheet)
    {
        try
        {
            string? cellValue = worksheet.Cells[row, 7].Value?.ToString();
            if (string.IsNullOrEmpty(cellValue))
            {
                return null;
            }
        
            _finishParsing = false;
        
            Rate rate = _rateСalculator.Сalculation(cellValue, GetDate(worksheet));
            return rate;
        }
        catch
        {
            return null;
        }
    }
    
    private Rate? ParseRateMax(int row, ExcelWorksheet worksheet)
    {
        try
        {
            string? cellValue = worksheet.Cells[row, 8].Value?.ToString();
            if (string.IsNullOrEmpty(cellValue))
            {
                return null;
            }
        
            _finishParsing = false;
        
            Rate rate = _rateСalculator.Сalculation(cellValue, GetDate(worksheet));
            return rate;
        }
        catch
        {
            return null;
        }
    }
    
    private string? ParseNote(int row, ExcelWorksheet worksheet)
    {
        try
        {
            string? cellValue = worksheet.Cells[row, 9].Value?.ToString();
            if (string.IsNullOrEmpty(cellValue))
            {
                return null;
            }
        
            _finishParsing = false;
        
            return cellValue;
        }
        catch
        {
            return null;
        }
    }

    private string? GetDate(ExcelWorksheet worksheet)
    {
        for (int col = 1; col <= 9; ++col)
        {
            string? cellValue = worksheet.Cells[1, col].Value?.ToString();
            if (!string.IsNullOrEmpty(cellValue))
            {
                return cellValue;
            }
        }
        
        return DateTime.Today.ToString();
    }

    public string? GetTableCreateDate(string tableName)
    {
        FileInfo fileInfo = new FileInfo(_filePath);
        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[tableName];
            if (worksheet == null)
            {
                throw new Exception("Table not found");
            }

            return GetDate(worksheet);
        }
    }

    public void SaveChanges(List<TableRow> list, string tableName, int firstRelevantLine)
    {
        FileInfo fileInfo = new FileInfo(_filePath);
        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[tableName];
            if (worksheet == null)
            {
                throw new Exception("Table not found");
            }

            int row = firstRelevantLine;
            foreach (var item in list)
            {
                worksheet.Cells[row, 1].Value = item.Id;
                worksheet.Cells[row, 2].Value = item.BankName;
                worksheet.Cells[row, 3].Value = item.CreditProduct;
                worksheet.Cells[row, 4].Value = item.TermMin;
                worksheet.Cells[row, 5].Value = item.TermMax;
                worksheet.Cells[row, 6].Value = item.Period;
                worksheet.Cells[row, 7].Value = item.RateMin?.ValueString;
                worksheet.Cells[row, 8].Value = item.RateMax?.ValueString;
                worksheet.Cells[row, 9].Value = item.Note;
            
                row++;
            }

            package.Save();
        }
    }

}