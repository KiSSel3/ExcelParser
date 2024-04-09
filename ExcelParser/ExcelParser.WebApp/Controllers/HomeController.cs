using ExcelParser.Parser.Models;
using ExcelParser.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ExcelParser.WebApp.Models;

namespace ExcelParser.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly Dictionary<string, int> _rellevantLineInTables=new();
    private readonly ITableComparisonService _comparisonService;

    public HomeController(ITableComparisonService comparisonService)
    {
        _comparisonService = comparisonService;
        _rellevantLineInTables["Строительство "] = 4;
        _rellevantLineInTables["Потребительские кредиты "] = 4;
        _rellevantLineInTables["Платежные карты и Овердрафт "] = 5;
        _rellevantLineInTables["Автокредитование "] = 4;
    }

    private async Task<string> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new Exception("File is empty");
        var filePath = Path.GetTempFileName();
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return filePath;
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
        try
        {
            string filePath = await UploadFile(file);
            return View("Home", filePath);
        }
        catch (Exception exception)
        {
            return View("Error", new ErrorViewModel() { RequestId = exception.Message });
        }
        
    }

    public async Task<IActionResult> GetBuildingTable(string filePath)
    {
        var tableName = "Строительство ";
        var buildingData = GetTableRowsByTableName(tableName,filePath,_rellevantLineInTables[tableName]);
        ViewData["Title"] = tableName;
        return CheckForDataValid(buildingData);
    }
    public async Task<IActionResult> GetConsumerLoansTable(string filePath)
    {
        var tableName = "Потребительские кредиты ";
        var consumerLoansData = GetTableRowsByTableName(tableName,filePath,_rellevantLineInTables[tableName]);
        ViewData["Title"] = tableName;
        return CheckForDataValid(consumerLoansData);
    }

    public async Task<IActionResult> GetPaymentCardsAndOverdraftTable(string filePath)
    {
        var tableName = "Платежные карты и Овердрафт ";
        var  paymentCardsAndOverdraftData = GetTableRowsByTableName(tableName,filePath,_rellevantLineInTables[tableName]);
        ViewData["Title"] = tableName;
        return CheckForDataValid(paymentCardsAndOverdraftData);
    }
    public async Task<IActionResult> GetCarLoansTable(string filePath)
    {
        var tableName = "Автокредитование ";
        var carLoansData = GetTableRowsByTableName(tableName,filePath,_rellevantLineInTables[tableName]);
        ViewData["Title"] = tableName;
        return CheckForDataValid(carLoansData);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateTable(string filePath, string tableName)
    {
        var tableRows = GetTableRowsByTableName(tableName,filePath,_rellevantLineInTables[tableName]);
        return View("EditTable", tableRows);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateTable(TableRowViewModel tableRowViewModel)
    {
        return View("Home", tableRowViewModel.FilePath);
    }

    [HttpGet]
    public async Task<IActionResult> UploadComparingTables()
    {
        return View("CompareIndex");
    }
    [HttpPost]
    public async Task<IActionResult> UploadComparingTables(IFormFile firstFile, IFormFile secondFile)
    {
        try
        {
            string firstFilePath = await UploadFile(firstFile);
            string secondFilePath = await UploadFile(secondFile);
            return View("CompareHome",
                new CompareFileViewModel() { FirstFilePath = firstFilePath, SecondFilePath = secondFilePath });
        }
        catch (Exception exception)
        {
            return View("Error", new ErrorViewModel() { RequestId = exception.Message });
        }
        
    }

    [HttpGet]
    public async Task<IActionResult> CompareHome(string firstFilePath, string secondFilePath)
    {
        return View("CompareHome",
            new CompareFileViewModel() { FirstFilePath = firstFilePath, SecondFilePath = secondFilePath });
    }

    [HttpGet]
    public async Task<IActionResult> CompareTables(string firstFilePath,string secondFilePath, string tableName)
    {
        Task<TableRowViewModel> getRowsFromFirstTable =
            Task.Run(() => GetTableRowsByTableName(tableName, firstFilePath, _rellevantLineInTables[tableName]));
        Task<TableRowViewModel> getRowsFromSecondTable =
            Task.Run(() => GetTableRowsByTableName(tableName, secondFilePath, _rellevantLineInTables[tableName]));
        await Task.WhenAll(getRowsFromFirstTable,getRowsFromSecondTable);
        var fistTableRows = await getRowsFromFirstTable;
        var secondTableRows = await getRowsFromSecondTable;
        var compareResult=_comparisonService.GetComparisonTable(fistTableRows.TableRows, secondTableRows.TableRows);
        return View("Compare", new CompareFileViewModel(){CompareResult = compareResult,FirstFilePath = firstFilePath,SecondFilePath = secondFilePath});
    }
}