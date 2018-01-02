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
      if (complain == null)
      {
        return BadRequest($"Can not find complain by id: '{complainId}'.");
      }

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

      IdentityResult result = await _repo.CreateComplain(complainModel, HttpContext.Current.User.Identity.Name);

      IHttpActionResult errorResult = GetErrorResult(result);

      return errorResult != null ? errorResult : Ok();
    }

    // DELETE api/complain/{complainId}
    [HttpDelete]
    [Route("{complainId}")]
    [Authorize(Roles = RoleTypes.Admin)]
    public async Task<IHttpActionResult> Delete(string complainId)
    {
      IdentityResult result = await _repo.DeleteComplain(complainId);

      IHttpActionResult errorResult = GetErrorResult(result);

      return errorResult != null ? errorResult : Ok();
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

      if (!result.Succeeded)
      {
        if (result.Errors != null)
        {
          foreach (string error in result.Errors)
          {
            ModelState.AddModelError("", error);
          }
        }

        if (ModelState.IsValid)
        {
          // No ModelState errors are available to send, so just return an empty BadRequest.
          return BadRequest();
        }

        return BadRequest(ModelState);
      }

      return null;
    }
  }
}
