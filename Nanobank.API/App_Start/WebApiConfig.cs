using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Nanobank.API.DAL.Models.ResponseViewModels;
using Nanobank.API.Formatters;
using Newtonsoft.Json.Serialization;

namespace Nanobank.API
{
  public static class WebApiConfig
  {
    public static void Register(HttpConfiguration config)
    {
      // enable routing via attributes
      config.MapHttpAttributeRoutes();

      // default map route
      config.Routes.MapHttpRoute(
        name: "DefaultApi",
        routeTemplate: "api/{controller}/{id}",
        defaults: new { id = RouteParameter.Optional }
      );

      // JSON formatter
      var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
      jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
      
      // PDF formatter
      config.Formatters.Add(new PdfMediaTypeFormatter<ReportResponseViewModel>());
    }
  }
}