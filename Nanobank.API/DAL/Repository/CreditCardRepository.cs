using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL.Interface;
using Nanobank.API.Infrastructure.Extensions;
using Nanobank.API.Models;

namespace Nanobank.API.DAL.Repository
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

    public async Task<IdentityResult> Transit(string currentUsername, CreditCardTransitRequestViewModel transitModel)
    {
      var deal = await _context.Deals.FirstOrDefaultAsync(d => d.Id == transitModel.DealId);
      if (deal == null)
      {
        return IdentityResult.Failed($"Deal '{transitModel.DealId}' not found.");
      }

      var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == currentUsername);
      if (user == null)
      {
        return IdentityResult.Failed($"User '{currentUsername}' not found.");
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
        return IdentityResult.Failed(ex.GetValidationErrors());
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
  }
}