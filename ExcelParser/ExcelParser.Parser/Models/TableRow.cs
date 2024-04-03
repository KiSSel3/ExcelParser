namespace ExcelParser.Parser.Models;

public class TableRow
{
    public string? Id { get; set; }
    public string? BankName { get; set; }
    public string? CreditProduct { get; set; }
    public string? TermMin { get; set; }
    public string? TermMax { get; set; }
    public string? Period { get; set; }
    public string? RateMin { get; set; }
    public string? RateMax { get; set; }
    public string? Note { get; set; }
}