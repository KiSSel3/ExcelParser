using ExcelParser.Parser.Models;
using ExcelParser.Service.Interfaces;
using ExcelParser.WebApp.Enums;
using Microsoft.AspNetCore.Mvc;
using ExcelParser.WebApp.Models;
using OfficeOpenXml.Style.Table;

namespace ExcelParser.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly Dictionary<string, int> _rellevantLineInTables = new();
    private readonly ITableComparisonService _comparisonService;
    private readonly ITableStatisticsService _statisticsService;
    private readonly IRowFilteringService _rowFilteringService;

    public HomeController(ITableComparisonService comparisonService, ITableStatisticsService statisticsService, IRowFilteringService rowFilteringService)
    {
        _comparisonService = comparisonService;
        _statisticsService = statisticsService;
        _rowFilteringService = rowFilteringService;
        _rellevantLineInTables["Строительство "] = 4;
        _rellevantLineInTables["Потребительские кредиты "] = 4;
        _rellevantLineInTables["Платежные карты и Овердрафт "] = 5;
        _rellevantLineInTables["Автокредитование "] = 4;
    }

    private TableStatisticsViewModel GetTableStatistic(string filePath, string tableName)
    {
        var date = GetTableCreateDate(tableName, filePath);
        var table = GetTableRowsByTableName(tableName, filePath, _rellevantLineInTables[tableName]).TableRows;
        var tableStatistics = new StatisticsViewModel();
        tableStatistics.MaxRate = _statisticsService.GetMaxRate(table);
        tableStatistics.CountOfBanksWithMaxRateBelowRVSR =
            _statisticsService.GetCountOfBanksWithMaxRateBelowRVSR(table, date);
        tableStatistics.CountOfBanksWithRateAboveRVSRBelow20 =
            _statisticsService.GetCountOfBanksWithRateAboveRVSRBelow20(table, date);
        tableStatistics.CountOfCreditProductsWithMaxRateBelowRVSR =
            _statisticsService.GetCountOfCreditProductsWithMaxRateBelowRVSR(table, date);
        tableStatistics.CountOfCreditProductsWithRateAboveRVSRBelow20 =
            _statisticsService.GetCountOfCreditProductsWithRateAboveRVSRBelow20(table, date);
        return new TableStatisticsViewModel(){StatisticsViewModel = tableStatistics,TableName = tableName,Date = date};
    }
    private string GetTableCreateDate(string tableName, string filePath)
    {
        Parser.Parser parser = new Parser.Parser(filePath, 9);
        return parser.GetTableCreateDate(tableName);
    }
    
    private TableStatisticsViewModel GetStatistics(string filePath,string tableName)
    {
        return GetTableStatistic(filePath,tableName);
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
    public IActionResult DownloadExcelFile(string filePath)
    {
        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
        string fileName = Path.GetFileName(filePath);
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }

    private TableRowViewModel GetTableRowsByTableName(string tableName, string filePath, int firstRelevantLine)
    {
        Parser.Parser parser = new Parser.Parser(filePath, 9);
        var data = parser.GetTableRows(tableName, firstRelevantLine);
        return new TableRowViewModel() { TableRows = data, FilePath = filePath };
    }

    private IActionResult CheckForDataValid(TableRowViewModel dataModel)
    {
        if (dataModel.TableRows == null)
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
        return View("Home", new TableRowViewModel() { FilePath = filePath});
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        try
        {
            string filePath = await UploadFile(file);
            return View("Home", new TableRowViewModel() { FilePath = filePath });
        }
        catch (Exception exception)
        {
            return View("Error", new ErrorViewModel() { RequestId = exception.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> TableFiltering(TableRowViewModel tableRowViewModel, string tableName)
    {
        var data = GetTableRowsByTableName(tableName, tableRowViewModel.FilePath, _rellevantLineInTables[tableName]).TableRows;
        
        if (tableRowViewModel.TermMinFiltering != BaseFilteringParameters.None)
        {
            if (tableRowViewModel.TermMinFiltering == BaseFilteringParameters.IsNull)
            {
                data = _rowFilteringService.TermMinIsNull(data).ToList();
            }
            else
            {
                data = _rowFilteringService.TermMinIsNotNull(data).ToList();
            }
        }

        if (tableRowViewModel.TermMinMinimumValue != null || tableRowViewModel.TermMinMaximumValue != null)
        {
            data = _rowFilteringService.TermMinByInterval(tableRowViewModel.TermMinMinimumValue,
                tableRowViewModel.TermMinMaximumValue, data).ToList();
        }
        
        if (tableRowViewModel.TermMaxFiltering != BaseFilteringParameters.None)
        {
            if (tableRowViewModel.TermMaxFiltering == BaseFilteringParameters.IsNull)
            {
                data = _rowFilteringService.TermMaxIsNull(data).ToList();
            }
            else
            {
                data = _rowFilteringService.TermMaxIsNotNull(data).ToList();
            }
        }
        
        if (tableRowViewModel.TermMaxMinimumValue != null || tableRowViewModel.TermMaxMaximumValue != null)
        {
            data = _rowFilteringService.TermMaxByInterval(tableRowViewModel.TermMaxMinimumValue,
                tableRowViewModel.TermMaxMaximumValue, data).ToList();
        }
        
        if (tableRowViewModel.RateMinFiltering != BaseFilteringParameters.None)
        {
            if (tableRowViewModel.RateMinFiltering == BaseFilteringParameters.IsNull)
            {
                data = _rowFilteringService.RateMinIsNull(data).ToList();
            }
            else
            {
                data = _rowFilteringService.RateMinIsNotNull(data).ToList();
            }
        }
        
        if (tableRowViewModel.RateMinMinimumValue != null || tableRowViewModel.RateMinMaximumValue != null)
        {
            data = _rowFilteringService.RateMinByInterval(tableRowViewModel.RateMinMinimumValue,
                tableRowViewModel.RateMinMaximumValue, data).ToList();
        }
        
        if (tableRowViewModel.RateMaxFiltering != BaseFilteringParameters.None)
        {
            if (tableRowViewModel.RateMaxFiltering == BaseFilteringParameters.IsNull)
            {
                data = _rowFilteringService.RateMaxIsNull(data).ToList();
            }
            else
            {
                data = _rowFilteringService.RateMaxIsNotNull(data).ToList();
            }
        }
        
        if (tableRowViewModel.RateMaxMinimumValue != null || tableRowViewModel.RateMaxMaximumValue != null)
        {
            data = _rowFilteringService.TermMaxByInterval(tableRowViewModel.RateMaxMinimumValue,
                tableRowViewModel.RateMaxMaximumValue, data).ToList();
        }
        
        if (tableRowViewModel.NoteFiltering != BaseFilteringParameters.None)
        {
            if (tableRowViewModel.NoteFiltering == BaseFilteringParameters.IsNull)
            {
                data = _rowFilteringService.NoteIsNull(data).ToList();
            }
            else
            {
                data = _rowFilteringService.NoteIsNotNull(data).ToList();
            }
        }

        if (tableRowViewModel.PeriodFiltering != PeriodFilteringParameters.None)
        {
            if (tableRowViewModel.PeriodFiltering == PeriodFilteringParameters.Month)
            {
                data = _rowFilteringService.Period("мес", data).ToList();
            }
            else
            {
                data = _rowFilteringService.Period("год", data).ToList();
            }
        }
        
        if (!string.IsNullOrEmpty(tableRowViewModel.KeywordSearch))
        {
            data = _rowFilteringService.KeywordSearch(tableRowViewModel.KeywordSearch, data).ToList();
        }

        tableRowViewModel.TableRows = data;
        
        ViewData["Title"] = tableName;
        return CheckForDataValid(tableRowViewModel);
    }
    
    public async Task<IActionResult> GetBuildingTable(string filePath)
    {
        var tableName = "Строительство ";
        var buildingData = GetTableRowsByTableName(tableName, filePath, _rellevantLineInTables[tableName]);
        ViewData["Title"] = tableName;
        return CheckForDataValid(buildingData);
    }

    public async Task<IActionResult> GetConsumerLoansTable(string filePath)
    {
        var tableName = "Потребительские кредиты ";
        var consumerLoansData = GetTableRowsByTableName(tableName, filePath, _rellevantLineInTables[tableName]);
        ViewData["Title"] = tableName;
        return CheckForDataValid(consumerLoansData);
    }

    public async Task<IActionResult> GetPaymentCardsAndOverdraftTable(string filePath)
    {
        var tableName = "Платежные карты и Овердрафт ";
        var paymentCardsAndOverdraftData =
            GetTableRowsByTableName(tableName, filePath, _rellevantLineInTables[tableName]);
        ViewData["Title"] = tableName;
        return CheckForDataValid(paymentCardsAndOverdraftData);
    }

    public async Task<IActionResult> GetCarLoansTable(string filePath)
    {
        var tableName = "Автокредитование ";
        var carLoansData = GetTableRowsByTableName(tableName, filePath, _rellevantLineInTables[tableName]);
        ViewData["Title"] = tableName;
        return CheckForDataValid(carLoansData);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateTable(string filePath, string tableName)
    {
        var tableRows = GetTableRowsByTableName(tableName, filePath, _rellevantLineInTables[tableName]);
        ViewData["Title"] = tableName;
    
        HttpContext.Session.SetString("FilePath", filePath);
        HttpContext.Session.SetString("TableName", tableName);
        
        foreach (TableRow row in tableRows.TableRows)
        {
            row.CreditProduct ??= "";
            row.TermMin ??= 0;
            row.TermMax ??= 0;
            row.Period ??= "";
            row.RateMin ??= new Rate();
            row.RateMax ??= new Rate();
            row.Note ??= "";
        }
        
        return View("EditTable", tableRows);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTable(List<TableRow> TableRows)
    {
        Console.WriteLine(TableRows.Count);
        string filePath = HttpContext.Session.GetString("FilePath");
        Console.WriteLine(filePath);
        string tableName = HttpContext.Session.GetString("TableName");
        Console.WriteLine(tableName);
        HttpContext.Session.Remove("FilePath");
        HttpContext.Session.Remove("TableName");
        
        Parser.Parser parser = new Parser.Parser(filePath, 9);
        parser.SaveChanges(TableRows,tableName,_rellevantLineInTables[tableName]);
        return View("Download", filePath);
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
    public async Task<IActionResult> CompareTables(string firstFilePath, string secondFilePath, string tableName)
    {
        Task<TableRowViewModel> getRowsFromFirstTable =
            Task.Run(() => GetTableRowsByTableName(tableName, firstFilePath, _rellevantLineInTables[tableName]));
        Task<TableRowViewModel> getRowsFromSecondTable =
            Task.Run(() => GetTableRowsByTableName(tableName, secondFilePath, _rellevantLineInTables[tableName]));
        await Task.WhenAll(getRowsFromFirstTable, getRowsFromSecondTable);
        var fistTableRows = await getRowsFromFirstTable;
        var secondTableRows = await getRowsFromSecondTable;
        var compareResult = _comparisonService.GetComparisonTable(fistTableRows.TableRows, secondTableRows.TableRows);
        return View("Compare",
            new CompareFileViewModel()
                { CompareResult = compareResult, FirstFilePath = firstFilePath, SecondFilePath = secondFilePath });
    }

    [HttpGet]
    public async Task<IActionResult> Statistics(string filePath,string tableName,bool isForAll)
    {
        if (isForAll)
        {
            List<TableStatisticsViewModel> statisticsViewModels = new();
            foreach (var name in _rellevantLineInTables.Keys)
            {
                statisticsViewModels.Add(GetStatistics(filePath,name));
            }
            return View("Statistics", new TableRowViewModel() { Statistics =statisticsViewModels, FilePath = filePath });
        }
        var statistics = GetStatistics(filePath,tableName);
        return View("Statistics", new TableRowViewModel() { Statistics = new List<TableStatisticsViewModel>(){statistics}, FilePath = filePath });
    }
}