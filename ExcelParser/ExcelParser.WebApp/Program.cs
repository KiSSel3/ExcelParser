using ExcelParser.Service.Implementations;
using ExcelParser.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ITableComparisonService, TableComparisonService>();
builder.Services.AddScoped<ITableStatisticsService, TableStatisticsService>();
builder.Services.AddScoped<IRowFilteringService, RowFilteringService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(new ExceptionHandlerOptions
    {
    });
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();