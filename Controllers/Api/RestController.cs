using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Elpris.Controllers.Api
{
    public class RestApiController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "hello world" };
        }
        [HttpGet]
        [Route("api/restapi/echo/{message}")]
        public HttpResponseMessage Echo(string message)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new { Message = message });
        }

        // GET api/restapi/2024-06-19/longitude/latitude
        [HttpGet]
        public HttpResponseMessage GetByDateAndCoordinates(string date, string longitude, string latitude)
        {
            DateTime parsedDate;
            if (!DateTime.TryParse(date, out parsedDate))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid date format. Use yyyy-mm-dd format.");
            }
            double parsedLatitude;
            if (!double.TryParse(latitude.Replace(".",","), out parsedLatitude))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid latitude format.");
            }
            double parsedLongitude;
            if (!double.TryParse(longitude.Replace(".", ","), out parsedLongitude))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid longitude format.");
            }

            GeoPolygonen geoPolygonen = new GeoPolygonen();
            string priceclass_coordinates = geoPolygonen.FindAreaByCoordinates(parsedLongitude, parsedLatitude);
            if (string.IsNullOrEmpty(priceclass_coordinates))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot find price class from coordinates.");
            }
            HttpResponseMessage response = Common.GetElprisByDateAndPriceClass(priceclass_coordinates, parsedDate);

            return response;
        }

        // GET api/restapi/2024-06-19/SE1
        [HttpGet]
        [Route("api/restapi/{date}/{priceClass}")]
        public HttpResponseMessage GetByDateAndPriceClass(string date, string priceClass)
        {
            DateTime parsedDate;
            if (!DateTime.TryParse(date, out parsedDate))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid date format. Use yyyy-mm-dd format.");
            }
            if (priceClass != "SE1" && priceClass != "SE2" && priceClass != "SE3" && priceClass != "SE4")
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid price class.");
            }
            HttpResponseMessage response = Common.GetElprisByDateAndPriceClass(priceClass, parsedDate);

            return response;
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}