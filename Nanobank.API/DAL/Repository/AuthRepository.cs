using System.Security.Claims;
using System.Threading.Tasks;
using Nanobank.API.DAL.Interface;
using Nanobank.API.DAL.Models;

namespace Nanobank.API.DAL.Repository
{
  public class AuthRepository : IAuthRepository
  {
    private readonly ApplicationUserManager _userManager;

    public AuthRepository(
      ApplicationUserManager userManager)
    {
      _userManager = userManager;
    }

    public async Task<ApplicationUser> FindUser(string username, string password)
    {
      ApplicationUser user = await _userManager.FindAsync(username, password);

      return user;
    }

    public async Task<ClaimsIdentity> CreateClaimsIdentity(ApplicationUser user, string authenticationType)
    {
      ClaimsIdentity identity = await _userManager.CreateIdentityAsync(user, authenticationType);

      return identity;
    }

    public void Dispose()
    {
      _userManager.Dispose();
    }
  }
}