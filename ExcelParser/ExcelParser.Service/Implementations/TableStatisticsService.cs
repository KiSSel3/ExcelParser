using System.Globalization;
using ExcelParser.Parser.Models;
using ExcelParser.Parser.Utils;
using ExcelParser.Service.Interfaces;

namespace ExcelParser.Service.Implementations;

public class TableStatisticsService : ITableStatisticsService
{
    public double GetMaxRate(List<TableRow> table)
    {
        double maxRate = table.Max(item => item.RateMax?.ValueDouble ?? 0.0);

        return maxRate;
    }

    public int GetCountOfBanksWithMaxRateBelowRVSR(List<TableRow> tableRows, string date)
    {
        ApiService api = new ApiService();
        Macroprudentialregulation macroprudentialregulation = api.GetMacroprudentialregulation(date).Result;
        double rvsr = double.Parse(macroprudentialregulation.CreditsIndividuals, NumberStyles.Any, CultureInfo.InvariantCulture);

        int count = 0;

        var banks = tableRows.GroupBy(item => item.BankName);
        foreach (var bank in banks)
        {
            double maxRvsrInBank = bank.Max(item => item.RateMax?.ValueDouble ?? 0.0);
            if (maxRvsrInBank <= rvsr)
            {
                ++count;
            }
        }

        return count;
    }
    
    public int GetCountOfCreditProductsWithMaxRateBelowRVSR(List<TableRow> tableRows, string date)
    {
        ApiService api = new ApiService();
        Macroprudentialregulation macroprudentialregulation = api.GetMacroprudentialregulation(date).Result;
        double rvsr = double.Parse(macroprudentialregulation.CreditsIndividuals, NumberStyles.Any, CultureInfo.InvariantCulture);

        int count = 0;

        var creditProducts = tableRows.GroupBy(item => new { item.CreditProduct, item.BankName }); 
        foreach (var creditProduct in creditProducts)
        {
            double maxRvsrInCreditProduct = creditProduct.Max(item => item.RateMax?.ValueDouble ?? 0.0);
            if (maxRvsrInCreditProduct <= rvsr)
            {
                ++count;
            }
        }

        return count;
    }
    
    public int GetCountOfBanksWithRateAboveRVSRBelow20(List<TableRow> tableRows, string date)
    {
        ApiService api = new ApiService();
        Macroprudentialregulation macroprudentialregulation = api.GetMacroprudentialregulation(date).Result;
        double rvsr = double.Parse(macroprudentialregulation.CreditsIndividuals, NumberStyles.Any, CultureInfo.InvariantCulture);

        int count = 0;

        var banks = tableRows.GroupBy(item => item.BankName);
        foreach (var bank in banks)
        {
            double maxRvsrInBank = bank.Max(item => item.RateMax?.ValueDouble ?? 0.0);
            if (maxRvsrInBank > rvsr)
            {
                ++count;
            }
        }

        return count;
    }

    public int GetCountOfCreditProductsWithRateAboveRVSRBelow20(List<TableRow> tableRows, string date)
    {
        ApiService api = new ApiService();
        Macroprudentialregulation macroprudentialregulation = api.GetMacroprudentialregulation(date).Result;
        double rvsr = double.Parse(macroprudentialregulation.CreditsIndividuals, NumberStyles.Any, CultureInfo.InvariantCulture);

        int count = 0;

        var creditProducts = tableRows.GroupBy(item => new { item.CreditProduct, item.BankName });
        foreach (var creditProduct in creditProducts)
        {
            double maxRvsrInCreditProduct = creditProduct.Max(item => item.RateMax?.ValueDouble ?? 0.0);
            if (maxRvsrInCreditProduct > rvsr)
            {
                ++count;
            }
        }

        return count;
    }
}