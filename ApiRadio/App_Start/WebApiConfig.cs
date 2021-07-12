using Api.Filters;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace ApiRadio
{
	public static class WebApiConfig
	{
        public static void Register(HttpConfiguration config)
        {
            ConfigureRoutes(config);
            ConfigureFormatters(config);
            ConfigureFilters(config);
        }

        private static void ConfigureRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void ConfigureFormatters(HttpConfiguration config)
        {
            config.Formatters.Clear();
            JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter();
            formatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.Add(formatter);
        }

        private static void ConfigureFilters(HttpConfiguration config)
        {
            config.Filters.Add(new AuthenticationFilter());
        }
    }
}
