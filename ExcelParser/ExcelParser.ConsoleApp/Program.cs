//Для тестирования библиотеки

using ExcelParser.Parser;
using ExcelParser.Parser.Models;
using System;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

internal class Program
{
    public static async Task Main(string[] args)
    {
        string filePath = @"C:\Users\Kissel\OneDrive\Рабочий стол\KREDITNIEPRODUKTI.xlsx";
        
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
            Console.WriteLine("\n\n");
            Console.WriteLine($"{item.Id} | {item.BankName} | {item.CreditProduct} | {item.TermMin} | {item.TermMax} | {item.Period} | {item.RateMin?.ValueString} | {item.RateMax?.ValueString} | {item.Note}");
            Console.WriteLine("\n\n");

            //Console.WriteLine($"{item.RateMax?.ValueString} = {item.RateMax?.ValueDouble}");
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
        }
    }
}