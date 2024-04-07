using System.Diagnostics;
using System.Text;
using ExcelParser.Parser.Models;
using Microsoft.AspNetCore.Mvc;
using ExcelParser.WebApp.Models;

namespace ExcelParser.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly Dictionary<string, int> _rellevantLineInTables=new Dictionary<string, int>();

    public HomeController(ILogger<HomeController> logger)
    {
        _rellevantLineInTables["Строительство "]=4;
        _rellevantLineInTables["Потребительские кредиты "] = 4;
        _rellevantLineInTables["Платежные карты и Овердрафт "] = 5;
        _rellevantLineInTables["Автокредитование "] = 4;
    }

    private TableRowViewModel GetTableRowsByTableName(string tableName,string filePath,int firstRelevantLine)
    {
        Parser.Parser parser = new Parser.Parser(filePath, 9);
        var data = parser.GetTableRows(tableName, firstRelevantLine);
        return new TableRowViewModel(){TableRows = data,FilePath = filePath};
    }

    private IActionResult CheckForDataValid(TableRowViewModel dataModel)
    {
        if (dataModel.TableRows==null)
        {
            return View("Error", new ErrorViewModel() { RequestId = "Данных нет" });
        }
        return View("Table", dataModel);
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Home(string filePath)
    {
        return View("Home", filePath);
    }
    
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return Content("File not selected");
        var filePath = Path.GetTempFileName();
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        return View("Home",filePath);
    }

    public async Task<IActionResult> GetBuildingTable(string filePath)
    {
        var buildingData = GetTableRowsByTableName("Строительство ",filePath,4);
        ViewData["Title"] = "Строительство ";
        return CheckForDataValid(buildingData);
    }
    public async Task<IActionResult> GetConsumerLoansTable(string filePath)
    {
        var consumerLoansData = GetTableRowsByTableName("Потребительские кредиты ", filePath, 4);
        ViewData["Title"] = "Потребительские кредиты ";
        return CheckForDataValid(consumerLoansData);
    }

    public async Task<IActionResult> GetPaymentCardsAndOverdraftTable(string filePath)
    {
        var  paymentCardsAndOverdraftData = GetTableRowsByTableName("Платежные карты и Овердрафт ", filePath, 5);
        ViewData["Title"] = "Платежные карты и Овердрафт ";
        return CheckForDataValid(paymentCardsAndOverdraftData);
    }
    public async Task<IActionResult> GetCarLoansTable(string filePath)
    {
        var carLoansData = GetTableRowsByTableName("Автокредитование ", filePath, 4);
        ViewData["Title"] = "Автокредитование ";
        return CheckForDataValid(carLoansData);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateTable(string filePath, string tableName)
    {
        switch (tableName)
        {
            case "Автокредитование ":
            {
                var tableRows = GetTableRowsByTableName(tableName,filePath,_rellevantLineInTables[tableName]);
                return View("EditTable", tableRows);
            }
            case "Платежные карты и Овердрафт ":
            {
                var tableRows = GetTableRowsByTableName(tableName,filePath,_rellevantLineInTables[tableName]);
                return View("EditTable", tableRows);
            }
            case "Потребительские кредиты ":
            {
                var tableRows = GetTableRowsByTableName(tableName,filePath,_rellevantLineInTables[tableName]);
                return View("EditTable", tableRows);
            }
            case "Строительство ":
            {
                var tableRows = GetTableRowsByTableName(tableName,filePath,_rellevantLineInTables[tableName]);
                return View("EditTable", tableRows);
            }
        }
        return View("Home", filePath);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateTable(TableRowViewModel tableRowViewModel)
    {
        return View("Home", tableRowViewModel.FilePath);
    }
}