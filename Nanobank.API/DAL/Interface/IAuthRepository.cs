using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Nanobank.API.Infrastructure.Identity;
using Nanobank.API.Models;
using Nanobank.API.Models.ViewModels;

namespace Nanobank.API.DAL.Interface
{
  public interface IAuthRepository : IDisposable
  {
    Task<IList<UserViewModel>> GetUsers();
    Task<UserViewModel> GetUser(string userName);
    Task<IdentityResult> RegisterUser(UserModel userModel);
    Task<ApplicationUser> FindUser(string userName, string password);
    Task<ClaimsIdentity> CreateClaimsIdentity(ApplicationUser user, string authenticationType);
  }
}
