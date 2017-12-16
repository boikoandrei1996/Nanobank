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

      if (oldDeal.UserCreditor != null)
      {
        return IdentityResult.Failed("Can not change условие deal after заключения договора");
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
        DealStartDate = DateTime.Today.Date,
        RatingPositive = null,
        RatingNegative = null,
        DealClosedDate = null,
        IsClosed = false
      };
    }
  }
}