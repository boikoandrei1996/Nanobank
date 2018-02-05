using System.IO;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using Nanobank.API.Models.ResponseViewModels;

namespace Nanobank.API.Infrastructure.ReportGenerators
{
  public class HtmlGenerator
  {
    public static string CreateHtml(ReportResponseViewModel report, string xsltTemplatePath)
    {
      string xmlPath = HostingEnvironment.MapPath($"\\App_Data\\temp-{report.ComplainId}.xml");
      string htmlPath = HostingEnvironment.MapPath($"\\App_Data\\temp-{report.ComplainId}.html");

      var formatter = new XmlSerializer(typeof(ReportResponseViewModel));
      using (var fs = new FileStream(xmlPath, FileMode.Create))
      {
        formatter.Serialize(fs, report);
      }

      var xmlDoc = new XPathDocument(xmlPath);
      var xslTransform = new XslCompiledTransform();
      xslTransform.Load(xsltTemplatePath);
      xslTransform.Transform(xmlPath, htmlPath);

      return File.ReadAllText(htmlPath);
    }
  }
}