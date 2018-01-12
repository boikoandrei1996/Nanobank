using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL.Interface;
using Nanobank.API.DAL.Models;
using Nanobank.API.Models;

namespace Nanobank.API.Controllers
{
  [RoutePrefix("api/account")]
  [Authorize]
  public class AccountController : ApiController
  {
    private readonly IUserRepository _repo;

    public AccountController(IUserRepository repo)
    {
      _repo = repo;
    }

    // POST api/account/register
    [HttpPost]
    [Route("register")]
    [AllowAnonymous]
    public async Task<IHttpActionResult> Register(UserRequestViewModel userModel)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      IdentityResult result = await _repo.RegisterUser(userModel);

      IHttpActionResult errorResult = GetErrorResult(result);

      return errorResult != null ? errorResult : Ok();
    }

    // PUT api/account/approve/{username}
    [HttpPut]
    [Route("approve/{username}")]
    [Authorize(Roles = RoleTypes.Admin)]
    public async Task<IHttpActionResult> Approve(string username)
    {
      IdentityResult result = await _repo.ApproveUser(username);

      IHttpActionResult errorResult = GetErrorResult(result);

      return errorResult != null ? errorResult : Ok();
    }

    // PUT api/account/{username}/add/role
    [HttpPut]
    [Route("{username}/add/role")]
    [Authorize(Roles = RoleTypes.Admin)]
    public async Task<IHttpActionResult> AddRole(string username, [FromBody]string roleName)
    {
      IdentityResult result = await _repo.AddRoleToUser(username, roleName);

      IHttpActionResult errorResult = GetErrorResult(result);

      return errorResult != null ? errorResult : Ok();
    }

    // DELETE api/account/{username}
    [HttpDelete]
    [Route("{username}")]
    [Authorize(Roles = RoleTypes.Admin)]
    public async Task<IHttpActionResult> Delete(string username)
    {
      IdentityResult result = await _repo.DeleteUser(username);

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
