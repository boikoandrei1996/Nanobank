using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
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
        resultUsers.Add(await MapUserAsync(user));
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

      return await MapUserAsync(user);
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

    private async Task<UserViewModel> MapUserAsync(ApplicationUser user)
    {
      return new UserViewModel
      {
        UserName = user.UserName,
        Email = user.Email,
        PhoneNumber = user.PhoneNumber,
        FirstName = user.UserInfo.FirstName,
        LastName = user.UserInfo.LastName,
        Patronymic = user.UserInfo.Patronymic,
        IsApproved = user.IsApproved,
        RatingPositive = user.UserInfo.RatingPositive,
        RatingNegative = user.UserInfo.RatingNegative,
        Roles = await _userManager.GetRolesAsync(user.Id)
      };
    }
  }
}