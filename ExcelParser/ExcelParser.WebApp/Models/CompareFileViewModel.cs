using ExcelParser.Parser.Models;

namespace ExcelParser.WebApp.Models;

public class CompareFileViewModel
{
    public string FirstFilePath { get; set; }
    public string SecondFilePath { get; set; }
    public List<Tuple<TableRow, string>> CompareResult { get; set; } = new ();
}