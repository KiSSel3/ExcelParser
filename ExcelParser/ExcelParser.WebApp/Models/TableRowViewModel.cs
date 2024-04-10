using ExcelParser.Parser.Models;
using ExcelParser.WebApp.Enums;

namespace ExcelParser.WebApp.Models;

public class TableRowViewModel
{
    public string FilePath { get; set; }
    public List<TableRow> TableRows { get; set; }
    public List<TableStatisticsViewModel> Statistics { get; set; }

    public BaseFilteringParameters TermMinFiltering { get; set; } = BaseFilteringParameters.None;
    public double? TermMinMinimumValue { get; set; } = null;
    public double? TermMinMaximumValue { get; set; } = null;
    
    public BaseFilteringParameters TermMaxFiltering { get; set; } = BaseFilteringParameters.None;
    public double? TermMaxMinimumValue { get; set; } = null;
    public double? TermMaxMaximumValue { get; set; } = null;
    
    public BaseFilteringParameters RateMinFiltering { get; set; } = BaseFilteringParameters.None;
    public double? RateMinMinimumValue { get; set; } = null;
    public double? RateMinMaximumValue { get; set; } = null;
    
    public BaseFilteringParameters RateMaxFiltering { get; set; } = BaseFilteringParameters.None;
    public double? RateMaxMinimumValue { get; set; } = null;
    public double? RateMaxMaximumValue { get; set; } = null;
    
    public BaseFilteringParameters NoteFiltering { get; set; } = BaseFilteringParameters.None;

    public PeriodFilteringParameters PeriodFiltering { get; set; } = PeriodFilteringParameters.None;
    
    public string? KeywordSearch { get; set; } = null;
}