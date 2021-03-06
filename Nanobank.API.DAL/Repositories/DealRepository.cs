﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL.Extensions;
using Nanobank.API.DAL.Loggers;
using Nanobank.API.DAL.Models.EFModels;
using Nanobank.API.DAL.Models.RequestViewModels;
using Nanobank.API.DAL.Models.ResponseViewModels;
using Nanobank.API.DAL.Repositories.Interfaces;

namespace Nanobank.API.DAL.Repositories
{
  public class DealRepository : IDealRepository
  {
    private readonly ApplicationContext _context;
    private readonly ILogger _logger;

    public DealRepository(
      ApplicationContext context,
      ILogger logger)
    {
      _context = context;
      _logger = logger;
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

    public async Task<IdentityResult> CreateDeal(string username, DealRequestViewModel dealModel)
    {
      _context.Deals.Add(await MapDealAsync(dealModel, username));

      try
      {
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
      }
      catch(DbEntityValidationException ex)
      {
        _logger.Error("DealRepository.CreateDeal", ex);
        return IdentityResult.Failed(ex.GetValidationErrors());
      }
      catch(Exception ex)
      {
        _logger.Error("DealRepository.CreateDeal", ex);
        return null;
      }
    }

    public async Task<IdentityResult> UpdateDeal(string username, string dealId, DealRequestViewModel dealModel)
    {
      Deal oldDeal = await _context.Deals.FirstOrDefaultAsync(d => d.Id == dealId);
      if (oldDeal == null)
      {
        return IdentityResult.Failed($"Deal with id '{dealId}' not found.");
      }

      if (oldDeal.UserOwner.UserName != username)
      {
        return IdentityResult.Failed("Can not update deal by not owner user.");
      }

      if (oldDeal.IsClosed)
      {
        return IdentityResult.Failed("Deal is closed.");
      }

      if (oldDeal.UserCreditor != null)
      {
        return IdentityResult.Failed("Can not change condition deal after deal conclusion.");
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
        _logger.Error("DealRepository.UpdateDeal", ex);
        return IdentityResult.Failed(ex.GetValidationErrors());
      }
      catch (Exception ex)
      {
        _logger.Error("DealRepository.UpdateDeal", ex);
        return null;
      }
    }

    public async Task<IdentityResult> RespondOnDeal(string username, string dealId)
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

      ApplicationUser userCreditor = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
      if (userCreditor == null)
      {
        return IdentityResult.Failed($"User '{username}' not found.");
      }

      if (userCreditor.UserInfo.Card.Balance < deal.StartAmount)
      {
        return IdentityResult.Failed($"User '{username}' can not respond on deal because of the lack of balance.");
      }

      if (userCreditor.Id == deal.UserOwnerId)
      {
        return IdentityResult.Failed($"User can not respond on own deal.");
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
        _logger.Error("DealRepository.RespondOnDeal", ex);
        return IdentityResult.Failed(ex.GetValidationErrors());
      }
      catch (Exception ex)
      {
        _logger.Error("DealRepository.RespondOnDeal", ex);
        return null;
      }
    }

    public async Task<IdentityResult> CloseDeal(string username, string dealId)
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

      if (deal.UserCreditor.UserName != username)
      {
        return IdentityResult.Failed($"Deal creditor username are not equal param creditor username. '{deal.UserCreditor.UserName}' are not equal '{username}'.");
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
        _logger.Error("DealRepository.CloseDeal", ex);
        return IdentityResult.Failed(ex.GetValidationErrors());
      }
      catch (Exception ex)
      {
        _logger.Error("DealRepository.CloseDeal", ex);
        return null;
      }
    }

    public async Task<IdentityResult> SetRating(string username, string dealId, RatingRequestViewModel ratingModel)
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

      if (deal.UserCreditor.UserName != username)
      {
        return IdentityResult.Failed($"Deal creditor username are not equal param creditor username. '{deal.UserCreditor.UserName}' are not equal '{username}'.");
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
      long koef = (long)Math.Ceiling(deal.StartAmount / 100);

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
        _logger.Error("DealRepository.SetRating", ex);
        return IdentityResult.Failed(ex.GetValidationErrors());
      }
      catch (Exception ex)
      {
        _logger.Error("DealRepository.SetRating", ex);
        return null;
      }
    }

    public async Task<IdentityResult> DeleteDealByAdmin(string dealId)
    {
      Deal deal = await _context.Deals.FirstOrDefaultAsync(d => d.Id == dealId);
      if (deal == null)
      {
        return IdentityResult.Failed($"Deal with id '{dealId}' not found.");
      }

      return await PrivateDeleteDealAsync(deal);
    }

    public async Task<IdentityResult> DeleteDealByUser(string username, string dealId)
    {
      Deal deal = await _context.Deals.FirstOrDefaultAsync(d => d.Id == dealId);
      if (deal == null)
      {
        return IdentityResult.Failed($"Deal with id '{dealId}' not found.");
      }

      if (deal.UserOwner.UserName != username)
      {
        return IdentityResult.Failed($"You can not delete deal because of '{deal.UserOwner.UserName}' not equal '{username}'.");
      }

      return await PrivateDeleteDealAsync(deal);
    }

    public void Dispose()
    {
      _context.Dispose();
      _logger.Dispose();
    }

    private DealResponseViewModel MapDeal(Deal deal)
    {
      return new DealResponseViewModel
      {
        Id = deal.Id,
        Title = deal.Title,
        StartAmount = deal.StartAmount,
        ReturnedAmount = deal.ReturnedAmount,
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

    private async Task<Deal> MapDealAsync(DealRequestViewModel deal, string ownerUsername)
    {
      return new Deal
      {
        Title = deal.Title,
        StartAmount = deal.StartAmount,
        ReturnedAmount = 0m,
        DealDurationInMonth = deal.DealDurationInMonth,
        PercentRate = deal.PercentRate,
        UserOwner = await _context.Users.FirstOrDefaultAsync(u => u.UserName == ownerUsername),
        UserCreditor = await _context.Users.FirstOrDefaultAsync(u => u.UserName == deal.CreditorUserName),
        DealStartDate = null,
        RatingPositive = null,
        RatingNegative = null,
        DealClosedDate = null,
        IsClosed = false
      };
    }

    private async Task<IdentityResult> PrivateDeleteDealAsync(Deal deal)
    {
      _context.Deals.Remove(deal);

      try
      {
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
      }
      catch (DbEntityValidationException ex)
      {
        _logger.Error("DealRepository.DeleteDeal", ex);
        return IdentityResult.Failed(ex.GetValidationErrors());
      }
      catch (Exception ex)
      {
        _logger.Error("DealRepository.DeleteDeal", ex);
        return null;
      }
    }
  }
}