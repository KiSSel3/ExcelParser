using ExcelParser.Parser.Models;
using Newtonsoft.Json;

namespace ExcelParser.Parser.Utils;

public class ApiService
{
    
    public async Task<RefinancingRate> GetRefinancingRate(string date)
    {
        date = DateConverter.FormatDateWithDay(date);
        string apiUrl = $"https://api.nbrb.by/RefinancingRate?ondate={date}";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new RefinancingRate();
                }
                
                string json = await response.Content.ReadAsStringAsync();
                List<RefinancingRate> refinancingRates = JsonConvert.DeserializeObject<List<RefinancingRate>>(json);

                return refinancingRates[0];
            }
            catch (Exception ex)
            {
                return new RefinancingRate();
            }
        }
    }
    
    public async Task<Macroprudentialregulation> GetMacroprudentialregulation(string date)
    {
        date = DateConverter.FormatDateWithoutDay(date);
        
        string apiUrl = $"https://api.nbrb.by/Macroprudentialregulation/{date}";
        
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new Macroprudentialregulation();
                }
                
                string json = await response.Content.ReadAsStringAsync();
                Macroprudentialregulation macroprudentialregulation = JsonConvert.DeserializeObject<Macroprudentialregulation>(json);

                return macroprudentialregulation;
            }
            catch (Exception ex)
            {
                return new Macroprudentialregulation();
            }
        }
    }

    //TODO:Добавить api
    public double OvernightLendingRate()
    {
        return 10.75;
    }
}