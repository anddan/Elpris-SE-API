using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
public class ElprisJson
{
    public double SEK_per_kWh { get; set; }
    public double EUR_per_kWh { get; set; }
    public double EXR { get; set; }
    public DateTime time_start { get; set; }
    public DateTime time_end { get; set; }

    private static readonly HttpClient client = new HttpClient();
    public async Task<List<ElprisJson>> GetElprisAsync(string url)
    {
        List<ElprisJson> elprisList = null;
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode(); // Kastar exception om statuskoden inte är lyckad

            string responseBody = await response.Content.ReadAsStringAsync();
            elprisList = JsonConvert.DeserializeObject<List<ElprisJson>>(responseBody);
        }
        catch (HttpRequestException e)
        {
            throw new Exception($"Request error: {e.Message}");
        }
        catch (JsonException e)
        {
            throw new Exception($"JSON deserialization error: {e.Message}");
        }

        return elprisList;
    }
}

