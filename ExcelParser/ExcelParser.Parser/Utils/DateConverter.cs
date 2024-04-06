namespace ExcelParser.Parser.Utils;

public static class DateConverter
{
    public static string FormatDateWithDay(string inputDate)
    {
        DateTime date = DateTime.ParseExact(inputDate, "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        
        string formattedDate = date.ToString("yyyy-MM-dd");

        return formattedDate;
    }
    
    public static string FormatDateWithoutDay(string inputDate)
    {
        DateTime date = DateTime.ParseExact(inputDate, "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        
        string formattedDate = date.ToString("MM-yyyy");
        return formattedDate;
    }
}