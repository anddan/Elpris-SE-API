using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Web;
public class Common
{
    public Common()
    { }
    public static string RestAPIUrl
    {
        get
        {
            return "/api/restapi/";
        }
    }
    public static string ElPrisetJustNuUrl
    {
        get
        {
            return "https://www.elprisetjustnu.se/api/v1/prices/";
        }
    }

    public static HttpResponseMessage GetElprisByDateAndPriceClass(string priceClass, DateTime parsedDate)
    {
        try
        {
            var service = new ElprisJson();
            string url = string.Format(@"{0}{1}/{2}-{3}_{4}.json", Common.ElPrisetJustNuUrl, parsedDate.Year, parsedDate.Month.ToString("00"), parsedDate.Day.ToString("00"), priceClass);
            var elpris_list = new List<ElprisJson>();
            Task.Run(async () =>
            {
                elpris_list = await service.GetElprisAsync(url);
            }).Wait(); // Vänta på att uppgiften ska slutföras

            string json = JsonConvert.SerializeObject(elpris_list, Formatting.Indented);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return response;
        }
        catch (Exception ex)
        {
            // Om något går fel, hantera och returnera lämpligt svar
            var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.Content = new StringContent($"Error: {ex.Message}");
            return errorResponse;
        }
    }
}

