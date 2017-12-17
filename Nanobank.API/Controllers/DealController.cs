﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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

    // GET api/deal/{username}/all
    [HttpGet]
    [Route("{username}/all")]
    public async Task<IHttpActionResult> All(string username)
    {
      IList<DealResponseViewModel> dealsAsOwner = await _repo.GetDeals(deal => deal.UserOwner.UserName == username);
      IList<DealResponseViewModel> dealsAsCreditor = await _repo.GetDeals(deal => deal.UserCreditor?.UserName == username);

      var responseDict = new Dictionary<string, IList<DealResponseViewModel>>();
      responseDict["asOwner"] = dealsAsOwner;
      responseDict["asCreditor"] = dealsAsCreditor;

      return Ok(responseDict);
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
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      IdentityResult result = await _repo.UpdateDeal(dealId, dealModel);

      IHttpActionResult errorResult = GetErrorResult(result);

      return errorResult != null ? errorResult : Ok();
    }

    // PUT api/deal/respond/{dealId}
    [HttpPut]
    [Route("respond/{dealId}")]
    public async Task<IHttpActionResult> RespondOn(string dealId, [FromBody]string creditorUsername)
    {
      IdentityResult result = await _repo.RespondOnDeal(dealId, creditorUsername);

      IHttpActionResult errorResult = GetErrorResult(result);

      return errorResult != null ? errorResult : Ok();
    }

    // PUT api/deal/close/{dealId}
    [HttpPut]
    [Route("close/{dealId}")]
    public async Task<IHttpActionResult> Close(string dealId, [FromBody]string creditorUsername)
    {
      IdentityResult result = await _repo.CloseDeal(dealId, creditorUsername);

      IHttpActionResult errorResult = GetErrorResult(result);

      return errorResult != null ? errorResult : Ok();
    }

    // PUT api/deal/{dealID}/set/rating
    [HttpPut]
    [Route("{dealId}/set/rating")]
    public async Task<IHttpActionResult> SetRating(string dealId, [FromBody]RatingRequestViewModel ratingModel)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      IdentityResult result = await _repo.SetRating(dealId, ratingModel);

      IHttpActionResult errorResult = GetErrorResult(result);

      return errorResult != null ? errorResult : Ok();
    }

    // DELETE api/deal/{dealId}
    [HttpDelete]
    [Route("{dealId}")]
    public async Task<IHttpActionResult> Delete(string dealId)
    {
      IdentityResult result = await _repo.DeleteDeal(dealId);

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
