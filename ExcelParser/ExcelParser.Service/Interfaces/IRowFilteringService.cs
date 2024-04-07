using ExcelParser.Parser.Models;

namespace ExcelParser.Service.Interfaces;

public interface IRowFilteringService
{
    public IEnumerable<TableRow> TermMinIsNull(IEnumerable<TableRow> tableRows);
    public IEnumerable<TableRow> TermMinIsNotNull(IEnumerable<TableRow> tableRows);
    public IEnumerable<TableRow> TermMinByInterval(double? min, double? max, IEnumerable<TableRow> tableRows);
    
    public IEnumerable<TableRow> TermMaxIsNull(IEnumerable<TableRow> tableRows);
    public IEnumerable<TableRow> TermMaxIsNotNull(IEnumerable<TableRow> tableRows);
    public IEnumerable<TableRow> TermMaxByInterval(double? min, double? max, IEnumerable<TableRow> tableRows);
    
    public IEnumerable<TableRow> Period(string period, IEnumerable<TableRow> tableRows);
    
    public IEnumerable<TableRow> RateMinIsNull(IEnumerable<TableRow> tableRows);
    public IEnumerable<TableRow> RateMinIsNotNull(IEnumerable<TableRow> tableRows);
    public IEnumerable<TableRow> RateMinByInterval(double? min, double? max, IEnumerable<TableRow> tableRows);
    
    public IEnumerable<TableRow> RateMaxIsNull(IEnumerable<TableRow> tableRows);
    public IEnumerable<TableRow> RateMaxIsNotNull(IEnumerable<TableRow> tableRows);
    public IEnumerable<TableRow> RateMaxByInterval(double? min, double? max, IEnumerable<TableRow> tableRows);
    
    public IEnumerable<TableRow> NoteIsNull(IEnumerable<TableRow> tableRows);
    public IEnumerable<TableRow> NoteIsNotNull(IEnumerable<TableRow> tableRows);

    public IEnumerable<TableRow> KeywordSearch(string keyword, IEnumerable<TableRow> tableRows);
}