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

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        TableRow other = (TableRow)obj;
        
        return this.Id == other.Id &&
               this.BankName == other.BankName &&
               this.CreditProduct == other.CreditProduct &&
               this.TermMin == other.TermMin &&
               this.TermMax == other.TermMax &&
               this.Period == other.Period &&
               this.RateMin?.ValueString == other.RateMin?.ValueString &&
               this.RateMax?.ValueString == other.RateMax?.ValueString &&
               this.Note == other.Note; 
    }
    
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 3;

            hash = hash * 4 + (Id?.GetHashCode() ?? 0);
            hash = hash * 4 + (BankName?.GetHashCode() ?? 0);
            hash = hash * 4 + (CreditProduct?.GetHashCode() ?? 0);
            hash = hash * 4 + (TermMin?.GetHashCode() ?? 0);
            hash = hash * 4 + (TermMax?.GetHashCode() ?? 0);
            hash = hash * 4 + (Period?.GetHashCode() ?? 0);
            hash = hash * 4 + (RateMin?.ValueString?.GetHashCode() ?? 0);
            hash = hash * 4 + (RateMax?.ValueString?.GetHashCode() ?? 0);
            hash = hash * 4 + (Note?.GetHashCode() ?? 0);

            return hash;
        }
    }

}