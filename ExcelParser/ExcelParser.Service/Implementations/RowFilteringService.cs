using ExcelParser.Parser.Models;
using ExcelParser.Service.Interfaces;

namespace ExcelParser.Service.Implementations;

public class RowFilteringService : IRowFilteringService
{
    public IEnumerable<TableRow> TermMinIsNull(IEnumerable<TableRow> tableRows)
    {
        return tableRows.Where(item => item.TermMin is null);
    }

    public IEnumerable<TableRow> TermMinIsNotNull(IEnumerable<TableRow> tableRows)
    {
        return tableRows.Where(item => item.TermMin is not null);
    }

    public IEnumerable<TableRow> TermMinByInterval(double? min, double? max, IEnumerable<TableRow> tableRows)
    {
        return tableRows.Where(item => item.TermMin is not null && (min is null || item.TermMin >= min) && (max is null || item.TermMin <= max));
    }

    public IEnumerable<TableRow> TermMaxIsNull(IEnumerable<TableRow> tableRows)
    {
        return tableRows.Where(item => item.TermMax is null);
    }

    public IEnumerable<TableRow> TermMaxIsNotNull(IEnumerable<TableRow> tableRows)
    {
        return tableRows.Where(item => item.TermMax is not null);
    }

    public IEnumerable<TableRow> TermMaxByInterval(double? min, double? max, IEnumerable<TableRow> tableRows)
    {
        return tableRows.Where(item => item.TermMax is not null && (min is null || item.TermMax >= min) && (max is null || item.TermMax <= max));
    }

    public IEnumerable<TableRow> Period(string period, IEnumerable<TableRow> tableRows)
    {
        return tableRows.Where(item => item.Period is not null && item.Period.Equals(period));
    }

    public IEnumerable<TableRow> RateMinIsNull(IEnumerable<TableRow> tableRows)
    {
        return tableRows.Where(item => item.RateMin is null);
    }

    public IEnumerable<TableRow> RateMinIsNotNull(IEnumerable<TableRow> tableRows)
    {
        return tableRows.Where(item => item.RateMin is not null);
    }

    public IEnumerable<TableRow> RateMinByInterval(double? min, double? max, IEnumerable<TableRow> tableRows)
    {
        return tableRows.Where(item => item.RateMin is not null && (min is null || item.RateMin.ValueDouble >= min) && (max is null || item.RateMin.ValueDouble <= max));
    }

    public IEnumerable<TableRow> RateMaxIsNull(IEnumerable<TableRow> tableRows)
    {
        return tableRows.Where(item => item.RateMax is null);
    }

    public IEnumerable<TableRow> RateMaxIsNotNull(IEnumerable<TableRow> tableRows)
    {
        return tableRows.Where(item => item.RateMax is not null);
    }

    public IEnumerable<TableRow> RateMaxByInterval(double? min, double? max, IEnumerable<TableRow> tableRows)
    {
        return tableRows.Where(item => item.RateMax is not null && (min is null || item.RateMax.ValueDouble >= min) && (max is null || item.RateMax.ValueDouble <= max));
    }

    public IEnumerable<TableRow> NoteIsNull(IEnumerable<TableRow> tableRows)
    {
        return tableRows.Where(item => string.IsNullOrWhiteSpace(item.Note));
    }

    public IEnumerable<TableRow> NoteIsNotNull(IEnumerable<TableRow> tableRows)
    {
        return tableRows.Where(item => !string.IsNullOrWhiteSpace(item.Note));
    }

    public IEnumerable<TableRow> KeywordSearch(string keyword, IEnumerable<TableRow> tableRows)
    {
        return tableRows.Where(item => ((item.Id is not null && item.Id.Contains(keyword)) ||
                                                 (item.BankName is not null && item.BankName.Contains(keyword)) ||
                                                 (item.CreditProduct is not null && item.CreditProduct.Contains(keyword)) ||
                                                 (item.TermMin is not null && item.TermMin.ToString().Contains(keyword)) ||
                                                 (item.TermMax is not null && item.TermMax.ToString().Contains(keyword)) ||
                                                 (item.Period is not null && item.Period.Contains(keyword)) ||
                                                 (item.RateMin is not null && item.RateMin.ValueString.Contains(keyword)) ||
                                                 (item.RateMax is not null && item.RateMax.ValueString.Contains(keyword)) ||
                                                  item.Note is not null && item.Note.Contains(keyword)));
    }
}