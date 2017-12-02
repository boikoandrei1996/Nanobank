﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL.Interface;
using Nanobank.API.Models;
using Nanobank.API.Models.ViewModels;

namespace Nanobank.API.Controllers
{
  [RoutePrefix("api/Account")]
  public class AccountController : ApiController
  {
    private readonly IAuthRepository _repo;

    public AccountController(IAuthRepository repo)
    {
      _repo = repo;
    }

    // GET api/Account/All
    //[Authorize]
    [HttpGet]
    [Route("All")]
    public async Task<IHttpActionResult> Index()
    {
      IList<UserViewModel> users = await _repo.GetUsers();

      return Ok(users);
    }

    // POST api/Account/Register
    [AllowAnonymous]
    [HttpPost]
    [Route("Register")]
    public async Task<IHttpActionResult> Register(UserModel userModel)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      IdentityResult result = await _repo.RegisterUser(userModel);

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
