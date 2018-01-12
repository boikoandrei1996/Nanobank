using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL.Interface;
using Nanobank.API.DAL.Models;
using Nanobank.API.Models;

namespace Nanobank.API.Controllers
{
  [RoutePrefix("api/complain")]
  [Authorize]
  public class ComplainController : ApiController
  {
    private readonly IComplainRepository _repo;

    public ComplainController(IComplainRepository repo)
    {
      _repo = repo;
    }

    // GET api/complain/all
    [HttpGet]
    [Route("all")]
    [Authorize(Roles = RoleTypes.Admin)]
    public async Task<IHttpActionResult> All()
    {
      IList<ComplainResponseViewModel> complains = await _repo.GetComplains();

      return Ok(complains);
    }

    // GET api/complain/{complainId}
    [HttpGet]
    [Route("{complainId}")]
    [Authorize(Roles = RoleTypes.Admin)]
    public async Task<IHttpActionResult> Get(string complainId)
    {
      ComplainResponseViewModel complain = await _repo.GetComplain(complainId);
      
      return Ok(complain);
    }

    // POST api/complain/add
    [HttpPost]
    [Route("add")]
    public async Task<IHttpActionResult> Create(ComplainRequestViewModel complainModel)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      string username = HttpContext.Current.User.Identity.Name;
      IdentityResult result = await _repo.CreateComplain(username, complainModel);

      IHttpActionResult errorResult = GetErrorResult(result);

      return errorResult == null ? Ok() : errorResult;
    }

    // DELETE api/complain/{complainId}
    [HttpDelete]
    [Route("{complainId}")]
    [Authorize(Roles = RoleTypes.Admin)]
    public async Task<IHttpActionResult> Delete(string complainId)
    {
      IdentityResult result = await _repo.DeleteComplain(complainId);

      IHttpActionResult errorResult = GetErrorResult(result);

      return errorResult == null ? Ok() : errorResult;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        _repo.Dispose();
      }

      base.Dispose(disposing);
    }

    private IHttpActionResult GetErrorResult(IdentityResult result)
    {
      if (result == null)
      {
        return InternalServerError();
      }

      if (result.Succeeded)
      {
        return null;
      }

      if (result.Errors != null)
      {
        foreach (string error in result.Errors)
        {
          ModelState.AddModelError("", error);
        }
      }

      if (ModelState.IsValid)
      {
        return BadRequest();
      }

      return BadRequest(ModelState);
    }
  }
}
