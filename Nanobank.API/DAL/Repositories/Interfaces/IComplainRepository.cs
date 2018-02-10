using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL.Models.EFModels;
using Nanobank.API.DAL.Models.RequestViewModels;
using Nanobank.API.DAL.Models.ResponseViewModels;

namespace Nanobank.API.DAL.Repositories.Interfaces
{
  public interface IComplainRepository : IDisposable
  {
    Task<IList<ComplainResponseViewModel>> GetComplains(Func<Complain, bool> predicate = null);
    Task<ComplainResponseViewModel> GetComplain(string complainId);
    Task<IdentityResult> CreateComplain(string username, ComplainRequestViewModel complainModel);
    Task<IdentityResult> DeleteComplain(string complainId);
  }
}