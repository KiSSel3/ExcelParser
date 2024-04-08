using ExcelParser.Parser.Models;
using ExcelParser.Service.Interfaces;

namespace ExcelParser.Service.Implementations;

public class TableComparisonService : ITableComparisonService
{
    public List<Tuple<TableRow, string>> GetComparisonTable(List<TableRow> newTable, List<TableRow> previousTable)
    {
        List<Tuple<TableRow, string>> comparisonTable = new List<Tuple<TableRow, string>>();

        foreach (var newRow in newTable)
        {
            var existingRow = previousTable.Find(item => item.Equals(newRow));
            if (existingRow is not null)
            {
                comparisonTable.Add(new Tuple<TableRow, string>(newRow, "default"));
                previousTable.Remove(existingRow);
            }
            else
            {
                comparisonTable.Add(new Tuple<TableRow, string>(newRow, "green"));
            }
        }

        foreach (var previousRow in previousTable)
        {
            int index = comparisonTable.FindLastIndex(item => item.Item1.Id == previousRow.Id);

            if (index != -1)
            {
                comparisonTable.Insert(index + 1, new Tuple<TableRow, string>(previousRow, "red"));
            }
            else
            {
                comparisonTable.Add(new Tuple<TableRow, string>(previousRow, "red"));
            }
        }

        return comparisonTable;
    }
}