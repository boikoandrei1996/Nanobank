using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL.Models;
using Nanobank.API.Models;

namespace Nanobank.API.DAL.Interface
{
  public interface IDealRepository : IDisposable
  {
    Task<IList<DealResponseViewModel>> GetDeals(Func<Deal, bool> predicate = null);
    Task<DealResponseViewModel> GetDeal(string dealId);
    Task<IdentityResult> CreateDeal(string currentUsername, DealRequestViewModel dealModel);
    Task<IdentityResult> UpdateDeal(string currentUsername, string dealId, DealRequestViewModel dealModel);
    Task<IdentityResult> RespondOnDeal(string currentUsername, string dealId);
    Task<IdentityResult> CloseDeal(string currentUsername, string dealId);
    Task<IdentityResult> SetRating(string currentUsername, string dealId, RatingRequestViewModel ratingModel);
    Task<IdentityResult> DeleteDealByAdmin(string dealId);
    Task<IdentityResult> DeleteDealByUser(string currentUsername, string dealId);
  }
}
