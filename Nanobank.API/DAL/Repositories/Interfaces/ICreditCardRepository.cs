using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL.Models.RequestViewModels;
using Nanobank.API.DAL.Models.ResponseViewModels;

namespace Nanobank.API.DAL.Repositories.Interfaces
{
  public interface ICreditCardRepository : IDisposable
  {
    Task<List<CreditCardResponseViewModel>> GetCreditCards();
    Task<IdentityResult> Transit(string username, CreditCardTransitRequestViewModel transitModel);
  }
}
