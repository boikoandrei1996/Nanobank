﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL.Interface;
using Nanobank.API.Models;

namespace Nanobank.API.DAL
{
  public class CreditCardRepository : ICreditCardRepository
  {
    private readonly ApplicationContext _context;

    public CreditCardRepository(ApplicationContext context)
    {
      _context = context;
    }

    public Task<List<CreditCardResponseViewModel>> GetCreditCards()
    {
      var resultList = new List<CreditCardResponseViewModel>();

      foreach (var card in _context.Cards.ToList())
      {
        resultList.Add(new CreditCardResponseViewModel
        {
          CardNumber = card.CardNumber,
          Balance = card.Balance,
          Users =
            _context.Users
            .Where(u => u.UserInfo.CardNumber == card.CardNumber)
            .Select(u => u.UserName)
            .ToList()
        });
      }

      return Task.Run(() => resultList);
    }

    public async Task<IdentityResult> Transit(CreditCardTransitRequestViewModel transitModel)
    {
      var deal = await _context.Deals.FirstOrDefaultAsync(d => d.Id == transitModel.DealId);
      if (deal == null)
      {
        return IdentityResult.Failed($"Deal '{transitModel.DealId}' not found.");
      }

      var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == transitModel.Username);
      if (user == null)
      {
        return IdentityResult.Failed($"User '{transitModel.Username}' not found.");
      }

      if (deal.UserOwner.Id != user.Id)
      {
        return IdentityResult.Failed($"Can not transit money via other account.");
      }

      if (user.UserInfo.Card.Balance < transitModel.Amount)
      {
        return IdentityResult.Failed("Not enough money.");
      }

      deal.UserOwner.UserInfo.Card.Balance -= transitModel.Amount;
      deal.UserCreditor.UserInfo.Card.Balance += transitModel.Amount;
      deal.ReturnedAmount += transitModel.Amount;

      try
      {
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
      }
      catch(DbEntityValidationException ex)
      {
        return IdentityResult.Failed(GetValidationErrors(ex));
      }
      catch(Exception ex)
      {
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

      foreach (var error in ex.EntityValidationErrors)
      {
        validationErrors.AddRange(error.ValidationErrors.Select(err => $"[{err.PropertyName}]: '{err.ErrorMessage}'"));
      }

      return validationErrors.ToArray();
    }
  }
}