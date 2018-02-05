using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Nanobank.API.DAL.Models;
using Nanobank.API.DAL.Repositories.Interfaces;
using Nanobank.API.Models.ResponseViewModels;

namespace Nanobank.API.Controllers
{
  [RoutePrefix("api/user")]
  [Authorize]
  public class UserController : ApiController
  {
    private readonly IUserRepository _userRepo;
    private readonly IPhotoRepository _photoRepo;

    public UserController(
      IUserRepository userRepo,
      IPhotoRepository photoRepo)
    {
      _userRepo = userRepo;
      _photoRepo = photoRepo;
    }

    // GET api/user/all
    [HttpGet]
    [Route("all")]
    [Authorize(Roles = RoleTypes.Admin)]
    public async Task<IHttpActionResult> All()
    {
      IList<UserResponseViewModel> users = await _userRepo.GetUsers();

      return Ok(users);
    }

    // GET api/user/all/unapproved
    [HttpGet]
    [Route("all/unapproved")]
    [Authorize(Roles = RoleTypes.Admin)]
    public async Task<IHttpActionResult> AllUnapproved()
    {
      IList<UserResponseViewModel> users = await _userRepo.GetUsers(user => !user.IsApproved);

      return Ok(users);
    }

    // GET api/user/{userName}
    [HttpGet]
    [Route("{userName}")]
    public async Task<IHttpActionResult> Get(string userName)
    {
      UserResponseViewModel user = await _userRepo.GetUser(userName);

      return Ok(user);
    }

    // GET api/user/{userName}/passport/photo
    [HttpGet]
    [Route("{userName}/passport/photo")]
    [Authorize(Roles = RoleTypes.Admin)]
    public async Task<IHttpActionResult> GetPassportPhoto(string userName)
    {
      PhotoResponseViewModel photo = await _photoRepo.GetPhoto(userName);

      return Ok(photo);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        _userRepo.Dispose();
        _photoRepo.Dispose();
      }

      base.Dispose(disposing);
    }
  }
}
