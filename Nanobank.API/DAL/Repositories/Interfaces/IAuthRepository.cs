using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Nanobank.API.DAL.Models;

namespace Nanobank.API.DAL.Repositories.Interfaces
{
  public interface IAuthRepository : IDisposable
  {
    Task<ApplicationUser> FindUser(string username, string password);
    Task<ClaimsIdentity> CreateClaimsIdentity(ApplicationUser user, string authenticationType);
  }
}
