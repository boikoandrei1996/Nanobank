using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL.Interface;
using Nanobank.API.DAL.Models;
using Nanobank.API.Models.RequestViewModels;

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
    public async Task<IHttpActionResult> Register([FromBody]UserRequestViewModel userModel)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      IdentityResult result = await _repo.RegisterUser(userModel);

      IHttpActionResult errorResult = GetErrorResult(result);

      return errorResult == null ? Ok() : errorResult;
    }

    // PUT api/account/approve/{username}
    [HttpPut]
    [Route("approve/{username}")]
    [Authorize(Roles = RoleTypes.Admin)]
    public async Task<IHttpActionResult> Approve(string username)
    {
      IdentityResult result = await _repo.ApproveUser(username);

      IHttpActionResult errorResult = GetErrorResult(result);

      return errorResult == null ? Ok() : errorResult;
    }

    // PUT api/account/{username}/add/role
    [HttpPut]
    [Route("{username}/add/role")]
    [Authorize(Roles = RoleTypes.Admin)]
    public async Task<IHttpActionResult> AddRole(string username, [FromBody]string roleName)
    {
      IdentityResult result = await _repo.AddRoleToUser(username, roleName);

      IHttpActionResult errorResult = GetErrorResult(result);

      return errorResult == null ? Ok() : errorResult;
    }

    // PUT api/account/{username}/update/card
    [HttpPut]
    [Route("{username}/update/card")]
    public async Task<IHttpActionResult> UpdateCard(string username, [FromBody]UserCardRequestViewModel userCardModel)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      IdentityResult result = await _repo.UpdateUserCard(username, userCardModel);

      IHttpActionResult errorResult = GetErrorResult(result);

      return errorResult == null ? Ok() : errorResult;
    }

    // DELETE api/account/{username}
    [HttpDelete]
    [Route("{username}")]
    [Authorize(Roles = RoleTypes.Admin)]
    public async Task<IHttpActionResult> Delete(string username)
    {
      IdentityResult result = await _repo.DeleteUser(username);

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
