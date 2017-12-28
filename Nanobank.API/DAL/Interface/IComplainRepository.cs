﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL.Models;
using Nanobank.API.Models;

namespace Nanobank.API.DAL.Interface
{
  public interface IComplainRepository : IDisposable
  {
    Task<IList<ComplainResponseViewModel>> GetComplains(Func<Complain, bool> predicate = null);
    Task<ComplainResponseViewModel> GetComplain(string complainId);
    Task<IdentityResult> CreateComplain(ComplainRequestViewModel complainModel);
    Task<IdentityResult> DeleteComplain(string complainId);
  }
}