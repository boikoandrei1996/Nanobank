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

    public async Task<IdentityResult> RegisterUser(UserModel userModel)
    {
      IdentityUser user = new IdentityUser
      {
        UserName = userModel.UserName
      };

      var result = await _userManager.CreateAsync(user, userModel.Password);

      return result;
    }

    public async Task<IdentityUser> FindUser(string userName, string password)
    {
      IdentityUser user = await _userManager.FindAsync(userName, password);

      return user;
    }

    public void Dispose()
    {
      _userManager.Dispose();
      _roleManager.Dispose();
    }
  }
}