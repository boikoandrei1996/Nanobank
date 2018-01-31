using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL.Models;
using Nanobank.API.Models.RequestViewModels;
using Nanobank.API.Models.ResponseViewModels;

namespace Nanobank.API.DAL.Interface
{
  public interface IDealRepository : IDisposable
  {
    Task<IList<DealResponseViewModel>> GetDeals(Func<Deal, bool> predicate = null);
    Task<DealResponseViewModel> GetDeal(string dealId);
    Task<IdentityResult> CreateDeal(string username, DealRequestViewModel dealModel);
    Task<IdentityResult> UpdateDeal(string username, string dealId, DealRequestViewModel dealModel);
    Task<IdentityResult> RespondOnDeal(string username, string dealId);
    Task<IdentityResult> CloseDeal(string username, string dealId);
    Task<IdentityResult> SetRating(string username, string dealId, RatingRequestViewModel ratingModel);
    Task<IdentityResult> DeleteDealByAdmin(string dealId);
    Task<IdentityResult> DeleteDealByUser(string username, string dealId);
  }
}
