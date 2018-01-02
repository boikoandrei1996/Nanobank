using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Nanobank.API.Models;

namespace Nanobank.API.DAL.Interface
{
  public interface ICreditCardRepository : IDisposable
  {
    Task<List<CreditCardResponseViewModel>> GetCreditCards();
    Task<IdentityResult> Transit(string currentUsername, CreditCardTransitRequestViewModel transitModel);
  }
}
