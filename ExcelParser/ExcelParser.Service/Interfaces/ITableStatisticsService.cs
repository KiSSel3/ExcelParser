using ExcelParser.Parser.Models;

namespace ExcelParser.Service.Interfaces;

public interface ITableStatisticsService
{
    public double GetMaxRate(List<TableRow> table);
    
    public int GetCountOfBanksWithMaxRateBelowRVSR(List<TableRow> tableRows, string date);
    public int GetCountOfBanksWithRateAboveRVSRBelow20(List<TableRow> tableRows, string date);
    
    public int GetCountOfCreditProductsWithRateAboveRVSRBelow20(List<TableRow> tableRows, string date);
    public int GetCountOfCreditProductsWithMaxRateBelowRVSR(List<TableRow> tableRows, string date);
}