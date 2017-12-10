using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Nanobank.API.DAL.Interface;
using Nanobank.API.Models.ViewModels;

namespace Nanobank.API.Controllers
{

  [RoutePrefix("api/User")]
  public class UserController : ApiController
  {
    private readonly IAuthRepository _repo;

    public UserController(IAuthRepository repo)
    {
      _repo = repo;
    }

    // GET api/User/All
    [HttpGet]
    [Route("All")]
    public async Task<IHttpActionResult> All()
    {
      IList<UserViewModel> users = await _repo.GetUsers();

      return Ok(users);
    }
  }
}
