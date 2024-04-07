using System.Diagnostics;
using System.Text;
using ExcelParser.Parser.Models;
using Microsoft.AspNetCore.Mvc;
using ExcelParser.WebApp.Models;

namespace ExcelParser.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    private List<TableRow> GetTableRowsByTableName(string tableName,string filePath,int firstRelevantLine)
    {
        Parser.Parser parser = new Parser.Parser(filePath, 9);
        var data = parser.GetTableRows(tableName, firstRelevantLine);
        return data;
    }

    private IActionResult CheckForDataValid(List<TableRow> data)
    {
        if (data==null)
        {
            return View("Error", new ErrorViewModel() { RequestId = "Данных нет" });
        }
        return View("Table", data);
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
        ViewData["filePath"] = filePath;
        return CheckForDataValid(buildingData);
    }
    public async Task<IActionResult> GetConsumerLoansTable(string filePath)
    {
        Parser.Parser parser = new Parser.Parser(filePath, 9);
        var consumerLoansData =  parser.GetTableRows("Потребительские кредиты ", 4);
        ViewData["filePath"] = filePath;
        return CheckForDataValid(consumerLoansData);
    }

    public async Task<IActionResult> GetPaymentCardsAndOverdraftTable(string filePath)
    {
        var  paymentCardsAndOverdraftData = GetTableRowsByTableName("Платежные карты и Овердрафт ", filePath, 5);
        ViewData["filePath"] = filePath;
        return CheckForDataValid(paymentCardsAndOverdraftData);
    }
    public async Task<IActionResult> GetCarLoansTable(string filePath)
    {
        var carLoansData = GetTableRowsByTableName("Автокредитование ", filePath, 4);
        ViewData["filePath"] = filePath;
        return CheckForDataValid(carLoansData);
    }
}