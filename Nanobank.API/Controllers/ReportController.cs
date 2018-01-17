using System.Threading.Tasks;
using System.Web.Http;
using Nanobank.API.DAL.Interface;
using Nanobank.API.DAL.Models;

namespace Nanobank.API.Controllers
{
  [RoutePrefix("report")]
  [Authorize(Roles = RoleTypes.Admin)]
  public class ReportController : ApiController
  {
    private readonly IComplainRepository _repo;

    public ReportController(IComplainRepository repo)
    {
      _repo = repo;
    }

    // GET report/complain/{complainId}
    [HttpGet]
    [Route("complain/{complainId}")]
    public async Task<IHttpActionResult> ComplainPdf(string complainId)
    {
      var report = await _repo.GetReport(complainId);
      if (report == null)
      {
        return BadRequest($"Complain {complainId} not found.");
      }

      return Ok(report);
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
