namespace ExcelParser.Parser.Models;

public class Macroprudentialregulation
{
    public Macroprudentialregulation()
    {
        OnPeriod = "0";
        RevocableDeposits = "0";
        IrrevocableDeposits_1_6 = "0";
        IrrevocableDeposits_6_1 = "0";
        IrrevocableDeposits_12 = "0";
        CreditsLegal = "0";
        CreditsIndividuals = "0";
        ResolutionBordDate = "0";
        ResolutionBordNumber = "0";
    }

    public string OnPeriod { get; set; }
    public string RevocableDeposits { get; set; }
    public string IrrevocableDeposits_1_6 { get; set; }
    public string IrrevocableDeposits_6_1 { get; set; }
    public string IrrevocableDeposits_12 { get; set; }
    public string CreditsLegal { get; set; }
    public string CreditsIndividuals { get; set; }
    public string ResolutionBordDate { get; set; }
    public string ResolutionBordNumber { get; set; }
}