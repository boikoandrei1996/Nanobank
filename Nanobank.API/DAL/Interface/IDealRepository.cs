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
    Task<IdentityResult> CreateDeal(DealRequestViewModel dealModel, string ownerUsername);
    Task<IdentityResult> UpdateDeal(string dealId, DealRequestViewModel dealModel, string ownerUsername);
    Task<IdentityResult> RespondOnDeal(string dealId, string creditorUsername);
    Task<IdentityResult> CloseDeal(string dealId, string creditorUsername);
    Task<IdentityResult> SetRating(string dealId, RatingRequestViewModel ratingModel, string creditorUsername);
    Task<IdentityResult> DeleteDealByAdmin(string dealId);
    Task<IdentityResult> DeleteDealByUser(string dealId, string currentUsername);
  }
}
