using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace Elpris
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
        name: "DefaultApi",
        routeTemplate: "api/{controller}/{id}",
        defaults: new { id = RouteParameter.Optional }
    );

            config.Routes.MapHttpRoute(
        name: "GetByCoordinates",
        routeTemplate: "api/{controller}/{date}/{longitude}/{latitude}",
        defaults: new { id = RouteParameter.Optional },
        constraints: new { id = @"[\w\.]*" } // Tillåt punkter i koordinater
    );
        }
    }
}
