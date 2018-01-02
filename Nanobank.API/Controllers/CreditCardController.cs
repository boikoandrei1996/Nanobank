﻿using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL.Interface;
using Nanobank.API.DAL.Models;
using Nanobank.API.Models;

namespace Nanobank.API.Controllers
{
  [RoutePrefix("api/creditcard")]
  [Authorize]
  public class CreditCardController : ApiController
  {
    private readonly ICreditCardRepository _repo;

    public CreditCardController(ICreditCardRepository repo)
    {
      _repo = repo;
    }

    // GET api/creditcard/all
    [HttpGet]
    [Route("all")]
    [Authorize(Roles = RoleTypes.Admin)]
    public IHttpActionResult All()
    {
      var cards = _repo.GetCreditCards();

      return Ok(cards);
    }

    // PUT api/creditcard/transit
    [HttpPut]
    [Route("transit")]
    public async Task<IHttpActionResult> Transit([FromBody] CreditCardTransitRequestViewModel transitModel)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      IdentityResult result = await _repo.Transit(transitModel, HttpContext.Current.User.Identity.Name);

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
