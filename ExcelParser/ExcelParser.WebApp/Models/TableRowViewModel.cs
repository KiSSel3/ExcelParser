using ExcelParser.Parser.Models;

namespace ExcelParser.WebApp.Models;

public class TableRowViewModel
{
    public string FilePath { get; set; }
    public List<TableRow> TableRows { get; set; }
    public List<TableStatisticsViewModel> Statistics { get; set; }
}