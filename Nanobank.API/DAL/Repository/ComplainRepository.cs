using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL.Interface;
using Nanobank.API.DAL.Models;
using Nanobank.API.DAL.Extensions;
using Nanobank.API.Models.RequestViewModels;
using Nanobank.API.Models.ResponseViewModels;

namespace Nanobank.API.DAL.Repository
{
  public class ComplainRepository : IComplainRepository
  {
    private readonly ApplicationContext _context;

    public ComplainRepository(ApplicationContext context)
    {
      _context = context;
    }

    public async Task<IList<ComplainResponseViewModel>> GetComplains(Func<Complain, bool> predicate = null)
    {
      var resultComplains = new List<ComplainResponseViewModel>();

      var complains = await _context.Complains.ToListAsync();
      if (predicate != null)
      {
        complains = complains.Where(predicate).ToList();
      }

      foreach (var complain in complains)
      {
        resultComplains.Add(MapComplain(complain));
      }

      return resultComplains;
    }

    public async Task<ComplainResponseViewModel> GetComplain(string complainId)
    {
      var complain = await _context.Complains.FirstOrDefaultAsync(c => c.Id == complainId);
      if (complainId == null)
      {
        return null;
      }

      return MapComplain(complain);
    }

    public async Task<IdentityResult> CreateComplain(string currentUsername, ComplainRequestViewModel complainModel)
    {
      var deal = await _context.Deals.FirstOrDefaultAsync(d => d.Id == complainModel.DealId);
      if (deal == null)
      {
        return IdentityResult.Failed($"Deal {complainModel.DealId} not found.");
      }

      if (deal.UserCreditor == null || deal.UserCreditor.UserName != currentUsername)
      {
        return IdentityResult.Failed($"User '{currentUsername}' can not create complain for this deal.");
      }

      if (deal.IsClosed)
      {
        return IdentityResult.Failed($"Can not create complain for the closed deal.");
      }

      _context.Complains.Add(MapComplain(complainModel));

      try
      {
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
      }
      catch (DbEntityValidationException ex)
      {
        return IdentityResult.Failed(ex.GetValidationErrors());
      }
      catch (Exception ex)
      {
        // TODO: setting Logger
        // Logger.Error($"Can not add deal: exception {ex.GetType()} with message: {ex.Message}");
        return null;
      }
    }

    public async Task<IdentityResult> DeleteComplain(string complainId)
    {
      Complain complain = await _context.Complains.FirstOrDefaultAsync(c => c.Id == complainId);
      if (complainId == null)
      {
        return IdentityResult.Failed($"Complain with id: '{complainId}' not found.");
      }

      // TODO: The hook should be deleted.
      // The hook for load lazy property UserInfo.
      complain.Deal.ToString();

      _context.Complains.Remove(complain);

      try
      {
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
      }
      catch (DbEntityValidationException ex)
      {
        return IdentityResult.Failed(ex.GetValidationErrors());
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

    private ComplainResponseViewModel MapComplain(Complain complain)
    {
      return new ComplainResponseViewModel
      {
        ComplainId = complain.Id,
        ComplainText = complain.Text,
        DealId = complain.DealId,
        DateOfCreating = complain.DateOfCreating
      };
    }

    private Complain MapComplain(ComplainRequestViewModel complain)
    {
      return new Complain
      {
        Text = complain.ComplainText,
        DealId = complain.DealId,
        DateOfCreating = DateTime.Today.Date
      };
    }
  }
}