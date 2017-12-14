using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Nanobank.API.DAL.Interface;
using Nanobank.API.DAL.Models;
using Nanobank.API.Infrastructure.Identity;
using Nanobank.API.Models;

namespace Nanobank.API.DAL
{
  public class DealRepository : IDealRepository
  {
    private readonly ApplicationContext _context;
    private readonly ApplicationUserManager _userManager;

    public DealRepository(
      ApplicationContext context,
      ApplicationUserManager userManager)
    {
      _context = context;
      _userManager = userManager;
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

    public void Dispose()
    {
      _context.Dispose();
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
        CreditorUserName = deal.UserCreditor.UserName,
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
        UserOwner = await _userManager.FindByNameAsync(deal.OwnerUserName),
        UserCreditor = await _userManager.FindByNameAsync(deal.CreditorUserName),
        DealStartDate = DateTime.Today.Date,
        RatingPositive = null,
        RatingNegative = null,
        DealClosedDate = null,
        IsClosed = false
      };
    }
  }
}