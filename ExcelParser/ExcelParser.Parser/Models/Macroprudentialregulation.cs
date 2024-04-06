namespace ExcelParser.Parser.Models;

public class Macroprudentialregulation
{
    public Macroprudentialregulation()
    {
        OnPeriod = "";
        RevocableDeposits = "";
        IrrevocableDeposits_1_6 = "";
        IrrevocableDeposits_6_1 = "";
        IrrevocableDeposits_12 = "";
        CreditsLegal = "";
        CreditsIndividuals = "";
        ResolutionBordDate = "";
        ResolutionBordNumber = "";
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