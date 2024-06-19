using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Services;

namespace Elpris.Controllers.Api
{
    /// <summary>
    /// Summary description for Soap
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Soap : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string GetByDateAndPriceClass(string date, string priceClass)
        {
            DateTime parsedDate;
            if (!DateTime.TryParse(date, out parsedDate))
                return "Invalid date format. Use yyyy-mm-dd format.";
            if (priceClass != "SE1" && priceClass != "SE2" && priceClass != "SE3" && priceClass != "SE4")
                return "Invalid price class.";

            HttpResponseMessage response = Common.GetElprisByDateAndPriceClass(priceClass, parsedDate);
            return response.IsSuccessStatusCode ? response.Content.ReadAsStringAsync().Result : "";
        }

        [WebMethod]
        public string GetByDateAndCoordinates(string date, string longitude, string latitude)
        {
            DateTime parsedDate;
            if (!DateTime.TryParse(date, out parsedDate))
            {
                return "Invalid date format. Use yyyy-mm-dd format.";
            }
            double parsedLatitude;
            if (!double.TryParse(latitude.Replace(".", ","), out parsedLatitude))
            {
                return "Invalid latitude format.";
            }
            double parsedLongitude;
            if (!double.TryParse(longitude.Replace(".", ","), out parsedLongitude))
            {
                return "Invalid longitude format.";
            }

            GeoPolygonen geoPolygonen = new GeoPolygonen();
            string priceclass_coordinates = geoPolygonen.FindAreaByCoordinates(parsedLongitude, parsedLatitude);
            if (string.IsNullOrEmpty(priceclass_coordinates))
            {
                return "Cannot find price class from coordinates.";
            }
            HttpResponseMessage response = Common.GetElprisByDateAndPriceClass(priceclass_coordinates, parsedDate);

            return response.IsSuccessStatusCode ? response.Content.ReadAsStringAsync().Result : "";
        }
    }
}
