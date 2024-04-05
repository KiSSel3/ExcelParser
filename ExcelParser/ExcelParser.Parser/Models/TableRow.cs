namespace ExcelParser.Parser.Models;

public class TableRow
{
    public string? Id { get; set; }
    public string? BankName { get; set; }
    public string? CreditProduct { get; set; }
    
    public int? TermMin { get; set; }
    public int? TermMax { get; set; }
    
    public string? Period { get; set; }
    
    public Rate? RateMin { get; set; }
    public Rate? RateMax { get; set; }
    
    public string? Note { get; set; }
}