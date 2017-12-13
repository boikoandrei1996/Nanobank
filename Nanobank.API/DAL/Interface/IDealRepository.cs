using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL.Models;
using Nanobank.API.Models;

namespace Nanobank.API.DAL.Interface
{
  public interface IDealRepository : IDisposable
  {
    Task<IList<DealResponseViewModel>> GetDeals(Func<Deal, bool> predicate = null);
    /*Task<UserResponseViewModel> GetDeal(string dealId);
    Task<IdentityResult> Create();
    Task<IdentityResult> Update();
    Task<IdentityResult> Delete(string dealId);*/
  }
}
