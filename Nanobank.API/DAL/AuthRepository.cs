using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Nanobank.API.Models;
using Nanobank.API.DAL.Interface;
using Nanobank.API.Infrastructure.Identity;
using Nanobank.API.Models.ViewModels;
using Nanobank.API.DAL.Models;
using System.Security.Claims;

namespace Nanobank.API.DAL
{
  public class AuthRepository : IAuthRepository
  {
    private readonly ApplicationUserManager _userManager;
    private readonly ApplicationRoleManager _roleManager;

    public AuthRepository(
      ApplicationUserManager userManager,
      ApplicationRoleManager roleManager)
    {
      _userManager = userManager;
      _roleManager = roleManager;
    }

    public async Task<IList<UserViewModel>> GetUsers()
    {
      var resultUsers = new List<UserViewModel>();

      foreach (var user in _userManager.Users.ToList())
      {
        resultUsers.Add(new UserViewModel
        {
          UserName = user.UserName,
          Email = user.Email,
          Roles = await _userManager.GetRolesAsync(user.Id)
        });
      }

      return resultUsers;
    }

    public async Task<UserViewModel> GetUser(string userName)
    {
      var user = await _userManager.FindByNameAsync(userName);
      if (user == null)
      {
        return null;
      }

      return new UserViewModel
      {
        UserName = user.UserName,
        Email = user.Email,
        Roles = await _userManager.GetRolesAsync(user.Id)
      };
    }

    public async Task<IdentityResult> RegisterUser(UserModel userModel)
    {
      var user = new ApplicationUser
      {
        UserName = userModel.UserName
      };

      var result = await _userManager.CreateAsync(user, userModel.Password);
      if (!result.Succeeded)
      {
        return result;
      }

      result = await _userManager.AddToRoleAsync(user.Id, RoleTypes.User);
      if (!result.Succeeded)
      {
        // TODO: setting Logger
        /*Logger.Error($"Can not add role: {role} for userId: {user.Id}");
        IdentityResult tempResult = await UserManager.DeleteAsync(user);
        if (!tempResult.Succeeded)
        {
          Logger.Error($"Can not delete user with Id: {user.Id}, we have extra users, that should be deleted");
        }*/
        throw new InvalidOperationException($"Can not add role '{RoleTypes.User}' for user '{user.UserName}'");
      }

      return result;
    }

    public async Task<ApplicationUser> FindUser(string userName, string password)
    {
      ApplicationUser user = await _userManager.FindAsync(userName, password);

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
      _roleManager.Dispose();
    }
  }
}