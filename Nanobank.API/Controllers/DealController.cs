using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL.Interface;
using Nanobank.API.Models;

namespace Nanobank.API.Controllers
{
  [RoutePrefix("api/deal")]
  public class DealController : ApiController
  {
    private readonly IDealRepository _repo;

    public DealController(IDealRepository repo)
    {
      _repo = repo;
    }

    // GET api/deal/all
    [HttpGet]
    [Route("all")]
    public async Task<IHttpActionResult> All()
    {
      IList<DealResponseViewModel> deals = await _repo.GetDeals();

      return Ok(deals);
    }

    // GET api/deal/{dealId}
    [HttpGet]
    [Route("{dealId}")]
    public async Task<IHttpActionResult> Get(string dealId)
    {
      DealResponseViewModel deal = await _repo.GetDeal(dealId);
      if (deal == null)
      {
        return BadRequest($"Can not find deal by id: '{dealId}'");
      }

      return Ok(deal);
    }

    // POST api/deal/register
    [HttpPost]
    [Route("register")]
    public async Task<IHttpActionResult> Register(DealRequestViewModel dealModel)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      IdentityResult result = await _repo.CreateDeal(dealModel);

      IHttpActionResult errorResult = GetErrorResult(result);

      return errorResult != null ? errorResult : Ok();
    }

    // PUT api/deal/{dealId}
    [HttpPut]
    [Route("{dealId}")]
    public async Task<IHttpActionResult> Update(string dealId, [FromBody]DealRequestViewModel dealModel)
    {
      return BadRequest("NotImplemented");
    }

    // PUT api/deal/close/{dealId}
    [HttpPut]
    [Route("close/{dealId}")]
    public async Task<IHttpActionResult> Close(string dealId)
    {
      return BadRequest("NotImplemented");
    }

    // PUT api/deal/rating/positive
    [HttpPut]
    [Route("rating/positive")]
    public async Task<IHttpActionResult> SetPositiveRating(decimal? ratingValue)
    {
      return BadRequest("NotImplemented");
    }

    // PUT api/deal/rating/positive
    [HttpPut]
    [Route("rating/negative")]
    public async Task<IHttpActionResult> SetNegativeRating(decimal? ratingValue)
    {
      return BadRequest("NotImplemented");
    }

    // DELETE api/deal/{dealId}
    [HttpDelete]
    [Route("{dealId}")]
    public async Task<IHttpActionResult> Delete(string dealId)
    {
      return BadRequest("NotImplemented");
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
