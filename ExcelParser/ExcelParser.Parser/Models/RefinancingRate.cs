using System.ComponentModel.DataAnnotations;

namespace ExcelParser.Parser.Models;

public class RefinancingRate
{
    public RefinancingRate()
    {
        Date = DateTime.Now;
        Value = -1;
    }
    
    [Key]
    public DateTime Date { get; set; }
    public double Value { get; set; }
}