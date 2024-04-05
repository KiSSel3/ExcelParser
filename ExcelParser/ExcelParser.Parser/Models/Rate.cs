namespace ExcelParser.Parser.Models;

public class Rate
{
    public Rate(string valueString, int valueInt)
    {
        ValueString = valueString;
        ValueInt = valueInt;
    }

    public string ValueString { get; set; }
    public int ValueInt { get; set; }
}