using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Nanobank.API.DAL.Interface;
using Nanobank.API.DAL.Models;
using Nanobank.API.Models;

namespace Nanobank.API.Controllers
{

  [RoutePrefix("api/user")]
  [Authorize]
  public class UserController : ApiController
  {
    private readonly IAuthRepository _repo;

    public UserController(IAuthRepository repo)
    {
      _repo = repo;
    }

    // GET api/user/all
    [HttpGet]
    [Route("all")]
    [Authorize(Roles = RoleTypes.Admin)]
    public async Task<IHttpActionResult> All()
    {
      IList<UserResponseViewModel> users = await _repo.GetUsers();

      return Ok(users);
    }

    // GET api/user/all/unapproved
    [HttpGet]
    [Route("all/unapproved")]
    [Authorize(Roles = RoleTypes.Admin)]
    public async Task<IHttpActionResult> AllUnapproved()
    {
      IList<UserResponseViewModel> users = await _repo.GetUsers(user => !user.IsApproved);

      return Ok(users);
    }

    // GET api/user/{userName}
    [HttpGet]
    [Route("{userName}")]
    public async Task<IHttpActionResult> Get(string userName)
    {
      UserResponseViewModel user = await _repo.GetUser(userName);
      if (user == null)
      {
        return BadRequest($"Can not find user by username: '{userName}'");
      }

      return Ok(user);
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
