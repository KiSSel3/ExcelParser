//Для тестирования библиотеки

using ExcelParser.Parser;
using ExcelParser.Parser.Models;
using System;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using ExcelParser.Service.Implementations;
using Newtonsoft.Json;

internal class Program
{
    public static async Task Main(string[] args)
    {
        #region BaseTest

        /*string filePath = @"C:\Users\Kissel\OneDrive\Рабочий стол\KREDITNIEPRODUKTI — копия.xlsx";

        // Проверяем существует ли файл
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Файл не найден.");
            return;
        }

        Parser parser = new Parser(filePath, 9);

        Console.WriteLine("Строительство: ");
        var buildingData =  parser.GetTableRows("Строительство ", 4);
        foreach (var item in buildingData)
        {
            /*Console.WriteLine("\n\n");
            Console.WriteLine($"{item.Id} | {item.BankName} | {item.CreditProduct} | {item.TermMin} | {item.TermMax} | {item.Period} | {item.RateMin?.ValueString} | {item.RateMax?.ValueString} | {item.Note}");
            Console.WriteLine("\n\n");

            Console.WriteLine($"{item.RateMax?.ValueString} = {item.RateMax?.ValueDouble}");
        }


        Console.WriteLine("Потребительские кредиты: ");
        var consumerLoansData =  parser.GetTableRows("Потребительские кредиты ", 4);
        foreach (var item in consumerLoansData)
        {
            Console.WriteLine("\n\n");
            Console.WriteLine($"{item.Id} | {item.BankName} | {item.CreditProduct} | {item.TermMin} | {item.TermMax} | {item.Period} | {item.RateMin?.ValueString} | {item.RateMax?.ValueString} | {item.Note}");
            Console.WriteLine("\n\n");
        }

        Console.WriteLine("Платежные карты и Овердрафт: ");
        var paymentCardsAndOverdraftData =  parser.GetTableRows("Платежные карты и Овердрафт ", 5);
        foreach (var item in paymentCardsAndOverdraftData)
        {
            Console.WriteLine("\n\n");
            Console.WriteLine($"{item.Id} | {item.BankName} | {item.CreditProduct} | {item.TermMin} | {item.TermMax} | {item.Period} | {item.RateMin?.ValueString} | {item.RateMax?.ValueString} | {item.Note}");
            Console.WriteLine("\n\n");
            //Console.WriteLine(item.RateMax?.ValueString);
        }

        Console.WriteLine("Автокредитование: ");
        var carLoansData =  parser.GetTableRows("Автокредитование ", 4);
        foreach (var item in carLoansData)
        {
            Console.WriteLine("\n\n");
            Console.WriteLine($"{item.Id} | {item.BankName} | {item.CreditProduct} | {item.TermMin} | {item.TermMax} | {item.Period} | {item.RateMin?.ValueString} | {item.RateMax?.ValueString} | {item.Note}");
            Console.WriteLine("\n\n");
        }*/

        #endregion

        #region CompareTest

        /*string firstFilePath = @"C:\Users\Kissel\OneDrive\Рабочий стол\KREDITNIEPRODUKTI.xlsx";
        if (!File.Exists(firstFilePath))
        {
            Console.WriteLine("Файл не найден.");
            return;
        }

        Parser firstParser = new Parser(firstFilePath, 9);
        var firstBuildingData = firstParser.GetTableRows("Строительство ", 4);

        string secondFilePath = @"C:\Users\Kissel\OneDrive\Рабочий стол\КРЕДИТНЫЕ ПРОДУКТЫ.xlsx";
        if (!File.Exists(secondFilePath))
        {
            Console.WriteLine("Файл не найден.");
            return;
        }

        Parser secondParser = new Parser(secondFilePath, 9);
        var secondBuildingData = secondParser.GetTableRows("Строительство ", 4);

        TableComparisonService service = new TableComparisonService();

        var comparisonTable = service.GetComparisonTable(firstBuildingData, secondBuildingData);

        foreach (var row in comparisonTable)
        {
            Console.WriteLine(row.Item2);
        }*/

        #endregion

        #region StatisticTest

        /*string firstFilePath = @"C:\Users\Kissel\OneDrive\Рабочий стол\KREDITNIEPRODUKTI.xlsx";
        if (!File.Exists(firstFilePath))
        {
            Console.WriteLine("Файл не найден.");
            return;
        }

        Parser firstParser = new Parser(firstFilePath, 9);
        var firstBuildingData = firstParser.GetTableRows("Строительство ", 4);
        Console.WriteLine("Список получен");
        TableStatisticsService service = new TableStatisticsService();

        Console.WriteLine(service.GetMaxRate(firstBuildingData));

        Console.WriteLine(service.GetCountOfBanksWithMaxRateBelowRVSR(firstBuildingData, firstParser.GetTableCreateDate("Строительство ")));
        Console.WriteLine(service.GetCountOfCreditProductsWithMaxRateBelowRVSR(firstBuildingData, firstParser.GetTableCreateDate("Строительство ")));

        Console.WriteLine(service.GetCountOfBanksWithRateAboveRVSRBelow20(firstBuildingData, firstParser.GetTableCreateDate("Строительство ")));
        Console.WriteLine(service.GetCountOfCreditProductsWithRateAboveRVSRBelow20(firstBuildingData, firstParser.GetTableCreateDate("Строительство ")));*/

        #endregion

        #region SaveTest

        string filePath = @"C:\Users\Kissel\OneDrive\Рабочий стол\KREDITNIEPRODUKTI (2).xlsx";
        
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Файл не найден.");
            return;
        }

        Parser parser = new Parser(filePath, 9);

        Console.WriteLine("Строительство: ");
        var buildingData = parser.GetTableRows("Строительство ", 4);
        buildingData[2].BankName = "testVal";
        buildingData[0].Note = "Yes";
        
        parser.SaveChanges(buildingData, "Строительство ", 4);

        #endregion
    }
}