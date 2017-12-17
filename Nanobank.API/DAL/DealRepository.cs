using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL.Interface;
using Nanobank.API.DAL.Models;
using Nanobank.API.Infrastructure.Identity;
using Nanobank.API.Models;

namespace Nanobank.API.DAL
{
  public class DealRepository : IDealRepository
  {
    private readonly ApplicationContext _context;

    public DealRepository(ApplicationContext context)
    {
      _context = context;
    }

    public async Task<IList<DealResponseViewModel>> GetDeals(Func<Deal, bool> predicate = null)
    {
      var resultDeals = new List<DealResponseViewModel>();

      var deals = await _context.Deals.ToListAsync();
      if (predicate != null)
      {
        deals = deals.Where(predicate).ToList();
      }

      foreach (var deal in deals)
      {
        resultDeals.Add(MapDeal(deal));
      }

      return resultDeals;
    }

    public async Task<DealResponseViewModel> GetDeal(string dealId)
    {
      var deal = await _context.Deals.FirstOrDefaultAsync(d => d.Id == dealId);
      if (deal == null)
      {
        return null;
      }

      return MapDeal(deal);
    }

    public async Task<IdentityResult> CreateDeal(DealRequestViewModel dealModel)
    {
      try
      {
        _context.Deals.Add(await MapDealAsync(dealModel));
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
      }
      catch(DbEntityValidationException ex)
      {
        return IdentityResult.Failed(GetValidationErrors(ex));
      }
      catch(Exception ex)
      {
        // TODO: setting Logger
        // Logger.Error($"Can not add deal: exception {ex.GetType()} with message: {ex.Message}");
        return null;
      }
    }

    public async Task<IdentityResult> UpdateDeal(string dealId, DealRequestViewModel dealModel)
    {
      Deal oldDeal = await _context.Deals.FirstOrDefaultAsync(d => d.Id == dealId);
      if (oldDeal == null)
      {
        return IdentityResult.Failed($"Deal with id '{dealId}' not found.");
      }

      if (oldDeal.IsClosed)
      {
        return IdentityResult.Failed("Deal is closed.");
      }

      if (oldDeal.UserCreditor != null)
      {
        return IdentityResult.Failed("Can not change условие deal after заключения договора.");
      }

      ApplicationUser creditorUser = await _context.Users.FirstOrDefaultAsync(d => d.UserName == dealModel.CreditorUserName);
      if (!string.IsNullOrWhiteSpace(dealModel.CreditorUserName) && creditorUser == null)
      {
        return IdentityResult.Failed($"User with username '{dealModel.CreditorUserName}' not found");
      }

      oldDeal.Title = dealModel.Title;
      oldDeal.StartAmount = dealModel.StartAmount;
      oldDeal.DealDurationInMonth = dealModel.DealDurationInMonth;
      oldDeal.PercentRate = dealModel.PercentRate;
      oldDeal.UserCreditor = creditorUser;

      try
      {
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
      }
      catch (DbEntityValidationException ex)
      {
        return IdentityResult.Failed(GetValidationErrors(ex));
      }
      catch (Exception ex)
      {
        // TODO: setting Logger
        // Logger.Error($"Can not update deal: exception {ex.GetType()} with message: {ex.Message}");
        return null;
      }
    }

    public async Task<IdentityResult> RespondOnDeal(string dealId, string creditorUsername)
    {
      Deal deal = await _context.Deals.FirstOrDefaultAsync(d => d.Id == dealId);
      if (deal == null)
      {
        return IdentityResult.Failed($"Deal with id '{dealId}' not found.");
      }

      if (deal.IsClosed)
      {
        return IdentityResult.Failed("Deal is closed.");
      }

      if (deal.UserCreditor != null)
      {
        return IdentityResult.Failed($"Deal with id '{dealId}' is busy.");
      }

      ApplicationUser userCreditor = await _context.Users.FirstOrDefaultAsync(u => u.UserName == creditorUsername);
      if (userCreditor == null)
      {
        return IdentityResult.Failed($"User '{creditorUsername}' not found.");
      }

      if (userCreditor.UserInfo.Card.Balance < deal.StartAmount)
      {
        return IdentityResult.Failed($"User '{creditorUsername}' can not respond on deal because of the lack of balance.");
      }

      userCreditor.UserInfo.Card.Balance -= deal.StartAmount;
      deal.UserOwner.UserInfo.Card.Balance += deal.StartAmount;

      deal.UserCreditor = userCreditor;
      deal.DealStartDate = DateTime.Today.Date;

      try
      {
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
      }
      catch (DbEntityValidationException ex)
      {
        return IdentityResult.Failed(GetValidationErrors(ex));
      }
      catch (Exception ex)
      {
        // TODO: setting Logger
        // Logger.Error($"Can not remove deal: exception {ex.GetType()} with message: {ex.Message}");
        return null;
      }
    }

    public async Task<IdentityResult> CloseDeal(string dealId, string creditorUsername)
    {
      Deal deal = await _context.Deals.FirstOrDefaultAsync(d => d.Id == dealId);
      if (deal == null)
      {
        return IdentityResult.Failed($"Deal with id '{dealId}' not found.");
      }

      if (deal.IsClosed)
      {
        return IdentityResult.Failed("Deal already is closed.");
      }
      
      if (deal.UserCreditor == null)
      {
        return IdentityResult.Failed("Deal has not creditor still.");
      }

      if (deal.UserCreditor.UserName != creditorUsername)
      {
        return IdentityResult.Failed($"Deal creditor username are not equal param creditor username. '{deal.UserCreditor.UserName}' are not equal '{creditorUsername}'.");
      }

      deal.DealClosedDate = DateTime.Today.Date;
      deal.IsClosed = true;

      try
      {
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
      }
      catch (DbEntityValidationException ex)
      {
        return IdentityResult.Failed(GetValidationErrors(ex));
      }
      catch (Exception ex)
      {
        // TODO: setting Logger
        // Logger.Error($"Can not update deal: exception {ex.GetType()} with message: {ex.Message}");
        return null;
      }
    }

    public async Task<IdentityResult> SetRating(string dealId, RatingRequestViewModel ratingModel)
    {
      Deal deal = await _context.Deals.FirstOrDefaultAsync(d => d.Id == dealId);
      if (deal == null)
      {
        return IdentityResult.Failed($"Deal with id '{dealId}' not found.");
      }

      if (deal.IsClosed)
      {
        return IdentityResult.Failed("Deal is closed.");
      }

      if (deal.UserCreditor == null)
      {
        return IdentityResult.Failed("Deal has not creditor still.");
      }

      if (deal.UserCreditor.UserName != ratingModel.CreditorUsername)
      {
        return IdentityResult.Failed($"Deal creditor username are not equal param creditor username. '{deal.UserCreditor.UserName}' are not equal '{ratingModel.CreditorUsername}'.");
      }

      // logic with update rating for deal
      short oldDealRatingPositive = 0;
      if (deal.RatingPositive.HasValue)
      {
        oldDealRatingPositive = deal.RatingPositive.Value;
      }
      deal.RatingPositive = ratingModel.Positive;

      short oldDealRatingNegative = 0;
      if (deal.RatingNegative.HasValue)
      {
        oldDealRatingNegative = deal.RatingNegative.Value;
      }
      deal.RatingNegative = ratingModel.Negative;

      // logic with update rating for user owner
      long koef = (long)Math.Floor(deal.StartAmount / 100);

      long userPositiveRating = (ratingModel.Positive - oldDealRatingPositive) * koef;
      if (deal.UserOwner.UserInfo.RatingPositive.HasValue)
      {
        deal.UserOwner.UserInfo.RatingPositive += userPositiveRating;
      }
      else
      {
        deal.UserOwner.UserInfo.RatingPositive = userPositiveRating;
      }

      long userNegativeRating = (ratingModel.Negative - oldDealRatingNegative) * koef;
      if (deal.UserOwner.UserInfo.RatingNegative.HasValue)
      {
        deal.UserOwner.UserInfo.RatingNegative += userNegativeRating;
      }
      else
      {
        deal.UserOwner.UserInfo.RatingNegative = userNegativeRating;
      }

      // save changes
      try
      {
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
      }
      catch (DbEntityValidationException ex)
      {
        return IdentityResult.Failed(GetValidationErrors(ex));
      }
      catch (Exception ex)
      {
        // TODO: setting Logger
        // Logger.Error($"Can not update deal: exception {ex.GetType()} with message: {ex.Message}");
        return null;
      }
    }

    public async Task<IdentityResult> DeleteDeal(string dealId)
    {
      Deal deal = await _context.Deals.FirstOrDefaultAsync(d => d.Id == dealId);
      if (deal == null)
      {
        return IdentityResult.Failed($"Deal with id '{dealId}' not found.");
      }

      try
      {
        _context.Deals.Remove(deal);
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
      }
      catch (DbEntityValidationException ex)
      {
        return IdentityResult.Failed(GetValidationErrors(ex));
      }
      catch (Exception ex)
      {
        // TODO: setting Logger
        // Logger.Error($"Can not remove deal: exception {ex.GetType()} with message: {ex.Message}");
        return null;
      }
    }

    public void Dispose()
    {
      _context.Dispose();
    }

    private string[] GetValidationErrors(DbEntityValidationException ex)
    {
      var validationErrors = new List<string>();

      foreach(var error in ex.EntityValidationErrors)
      {
        validationErrors.AddRange(error.ValidationErrors.Select(err => $"[{err.PropertyName}]: '{err.ErrorMessage}'"));
      }

      return validationErrors.ToArray();
    }

    private DealResponseViewModel MapDeal(Deal deal)
    {
      return new DealResponseViewModel
      {
        Id = deal.Id,
        Title = deal.Title,
        StartAmount = deal.StartAmount,
        DealDurationInMonth = deal.DealDurationInMonth,
        PercentRate = deal.PercentRate,
        OwnerUserName = deal.UserOwner.UserName,
        CreditorUserName = deal.UserCreditor != null ? deal.UserCreditor.UserName : null,
        DealStartDate = deal.DealStartDate,
        DealClosedDate = deal.DealClosedDate,
        IsClosed = deal.IsClosed,
        RatingPositive = deal.RatingPositive,
        RatingNegative = deal.RatingNegative
      };
    }

    private async Task<Deal> MapDealAsync(DealRequestViewModel deal)
    {
      return new Deal
      {
        Title = deal.Title,
        StartAmount = deal.StartAmount,
        DealDurationInMonth = deal.DealDurationInMonth,
        PercentRate = deal.PercentRate,
        UserOwner = await _context.Users.FirstOrDefaultAsync(u => u.UserName == deal.OwnerUserName),
        UserCreditor = await _context.Users.FirstOrDefaultAsync(u => u.UserName == deal.CreditorUserName),
        DealStartDate = null,
        RatingPositive = null,
        RatingNegative = null,
        DealClosedDate = null,
        IsClosed = false
      };
    }
  }
}