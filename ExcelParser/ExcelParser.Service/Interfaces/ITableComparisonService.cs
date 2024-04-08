using ExcelParser.Parser.Models;

namespace ExcelParser.Service.Interfaces;

public interface ITableComparisonService
{
    public List<Tuple<TableRow, string>> GetComparisonTable(List<TableRow> newTable, List<TableRow> previousTable);
}