using System.Diagnostics;
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

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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

        Parser.Parser parser = new Parser.Parser(filePath, 9);
        var buildingData =  parser.GetTableRows("Строительство ", 4);
        var consumerLoansData =  parser.GetTableRows("Потребительские кредиты ", 4);
        var paymentCardsAndOverdraftData =  parser.GetTableRows("Платежные карты и Овердрафт ", 5);
        var carLoansData =  parser.GetTableRows("Автокредитование ", 4);
        
        // Обработка загруженного файла

        return View("Word",filePath);
    }
}