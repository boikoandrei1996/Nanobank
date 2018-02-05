using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using Nanobank.API.DAL.Models;
using Nanobank.API.DAL.Repositories.Interfaces;
using Nanobank.API.Infrastructure.ReportGenerators;
using Nanobank.API.Models.ResponseViewModels;

namespace Nanobank.API.Controllers
{
  [RoutePrefix("report")]
  [Authorize(Roles = RoleTypes.Admin)]
  public class ReportController : ApiController
  {
    private readonly IReportRepository _repo;

    public ReportController(IReportRepository repo)
    {
      _repo = repo;
    }

    // GET report/pdf/complain/{complainId}
    [HttpGet]
    [Route("pdf/complain/{complainId}")]
    public async Task<IHttpActionResult> ComplainPdf(string complainId)
    {
      ReportResponseViewModel report = await _repo.GetReport(complainId);

      if (report == null)
      {
        return BadRequest($"Complain {complainId} not found.");
      }

      return Ok(report);
    }

    // GET report/html/complain/{complainId}
    [HttpGet]
    [Route("html/complain/{complainId}")]
    public async Task<HttpResponseMessage> ComplainHtml(string complainId)
    {
      var response = new HttpResponseMessage();

      var report = await _repo.GetReport(complainId);
      if (report == null)
      {
        response.StatusCode = HttpStatusCode.BadRequest;
        response.Content = new StringContent($"{{ \"message\": \"Complain {complainId} not found.\" }}");
        return response;
      }

      var xsltTemplatePath = HostingEnvironment.MapPath("\\XSLT\\htmlReport.xslt");
      var htmlContent = HtmlGenerator.CreateHtml(report, xsltTemplatePath);

      response.StatusCode = HttpStatusCode.OK;
      response.Content = new StringContent(htmlContent);
      response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

      return response;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        _repo.Dispose();
      }

      base.Dispose(disposing);
    }
  }
}
