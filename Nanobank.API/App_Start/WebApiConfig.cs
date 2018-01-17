using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Nanobank.API.Infrastructure.Formatters;
using Nanobank.API.Models.ResponseViewModels;
using Newtonsoft.Json.Serialization;

namespace Nanobank.API
{
  public static class WebApiConfig
  {
    public static void Register(HttpConfiguration config)
    {
      // Web API routes
      config.MapHttpAttributeRoutes();

      config.Routes.MapHttpRoute(
        name: "DefaultApi",
        routeTemplate: "api/{controller}/{id}",
        defaults: new { id = RouteParameter.Optional }
      );

      // Web API configuration and services
      var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
      jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
      //config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));

      config.Formatters.Add(new PdfMediaTypeFormatter<ReportResponseViewModel>());
    }
  }
}
