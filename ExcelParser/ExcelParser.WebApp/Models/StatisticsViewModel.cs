namespace ExcelParser.WebApp.Models;

public class StatisticsViewModel
{
    public int CountOfBanksWithRateAboveRVSRBelow20 { get; set; }
    public int CountOfCreditProductsWithMaxRateBelowRVSR { get; set; }
    public int CountOfBanksWithMaxRateBelowRVSR { get; set; }
    public double MaxRate { get; set; }
    public int CountOfCreditProductsWithRateAboveRVSRBelow20 { get; set; }
}