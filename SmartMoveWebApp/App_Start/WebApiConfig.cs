using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace SmartMoveWebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //For setting camel case of JSON field names
            var settings = config.Formatters.JsonFormatter.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Formatting = Formatting.Indented;

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
