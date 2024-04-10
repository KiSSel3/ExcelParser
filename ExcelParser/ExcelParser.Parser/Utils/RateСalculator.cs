using System.Data;
using ExcelParser.Parser.Models;
using Newtonsoft.Json;

namespace ExcelParser.Parser.Utils;

public class RateСalculator
{
    private ApiService _api;

    public RateСalculator()
    {
        _api = new ApiService();
    }
    
    public Rate? Сalculation(string value, string date)
    {
        Rate rate = new Rate();
        
        rate.ValueString = value;
        bool inPercent = false;

        if(value.Contains("РВСР"))
        {
            Macroprudentialregulation macroprudentialregulation = _api.GetMacroprudentialregulation(date).Result;
            value = value.Replace("РВСР", macroprudentialregulation.CreditsIndividuals);
            
            inPercent = true;
        }
        else if (value.Contains("СР"))
        {
            RefinancingRate refinancingRate = _api.GetRefinancingRate(date).Result;
            value = value.Replace("СР", refinancingRate.Value.ToString());

            inPercent = true;
        }
        else if(value.Contains("СКО"))
        {
            double overnightLendingRate = _api.OvernightLendingRate();
            value = value.Replace("СКО", overnightLendingRate.ToString());
            
            inPercent = true;
        }
        else if(value.Contains("%"))
        {
            value = value.Replace("%", "");
            
            inPercent = true;
        }
        
        rate.ValueDouble = GetDoubleValue(value);

        if (!inPercent)
        {
            rate.ValueDouble *= 100;
        }
        
        return rate;
    }

    private double GetDoubleValue(string value)
    {
        value = value.Replace(',', '.');
        
        DataTable dt = new DataTable();
        var result = dt.Compute(value, "");

        return Convert.ToDouble(result);
    }
}