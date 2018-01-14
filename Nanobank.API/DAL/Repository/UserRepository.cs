﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL.Interface;
using Nanobank.API.DAL.Models;
using Nanobank.API.Infrastructure.Identity;
using Nanobank.API.Infrastructure.Notifications;
using Nanobank.API.Models;

namespace Nanobank.API.DAL.Repository
{
  public class UserRepository : IUserRepository
  {
    private readonly ApplicationUserManager _userManager;
    private readonly ApplicationRoleManager _roleManager;
    private readonly IPushNotificationService _pushService;

    public UserRepository(
      ApplicationUserManager userManager,
      ApplicationRoleManager roleManager,
      IPushNotificationService pushService)
    {
      _userManager = userManager;
      _roleManager = roleManager;
      _pushService = pushService;
    }

    public UserRepository(
      ApplicationUserManager userManager,
      ApplicationRoleManager roleManager) : this(userManager, roleManager, null)
    {
    }

    public async Task<IList<UserResponseViewModel>> GetUsers(Func<ApplicationUser, bool> predicate = null)
    {
      var resultUsers = new List<UserResponseViewModel>();

      var users = await _userManager.Users.ToListAsync();
      if (predicate != null)
      {
        users = users.Where(predicate).ToList();
      }

      foreach (var user in users)
      {
        resultUsers.Add(await MapUserAsync(user));
      }

      return resultUsers;
    }

    public async Task<UserResponseViewModel> GetUser(string username)
    {
      var user = await _userManager.FindByNameAsync(username);
      if (user == null)
      {
        return null;
      }

      return await MapUserAsync(user);
    }

    public async Task<IdentityResult> RegisterUser(UserRequestViewModel userModel)
    {
      byte[] image;
      try
      {
        image = Convert.FromBase64String(userModel.PassportImage);
      }
      catch (FormatException ex)
      {
        return IdentityResult.Failed(ex.Message);
      }

      var user = MapUser(userModel, image);

      IdentityResult result;

      try
      {
        result = await _userManager.CreateAsync(user, userModel.Password);
      }
      catch (DbUpdateException ex)
      {
        return IdentityResult.Failed(ex.InnerException.InnerException.Message);
      }

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

    public async Task<IdentityResult> ApproveUser(string username)
    {
      ApplicationUser user = await _userManager.FindByNameAsync(username);
      if (user == null)
      {
        return IdentityResult.Failed($"User '{username}' not found.");
      }

      user.IsApproved = true;
      // TODO: The hook should be deleted.
      // The hook for load lazy property UserInfo.
      user.UserInfo.ToString();

      try
      {
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
          if (!string.IsNullOrWhiteSpace(user.FCMPushNotificationToken) && _pushService != null)
          {
            await _pushService.SendAsync(
              user.FCMPushNotificationToken,
              "Approved by admin in Nanobank.",
              $"Account '{user.UserName}' have been approved by admin.");
          }

          await _userManager.SendEmailAsync(
            user.Id,
            "Approved by admin in Nanobank.",
            $"<p>Account <i>{user.UserName}</i> have been <b>approved</b> by admin.</p>");
        }

        return result;
      }
      catch (DbUpdateException ex)
      {
        return IdentityResult.Failed(ex.InnerException.InnerException.Message);
      }
    }

    public async Task<IdentityResult> AddRoleToUser(string username, string rolename)
    {
      ApplicationUser user = await _userManager.FindByNameAsync(username);
      if (user == null)
      {
        return IdentityResult.Failed($"User '{username}' not found.");
      }

      if (!RoleTypes.AllRoles.Contains(rolename))
      {
        return IdentityResult.Failed($"Role '{rolename}' not found.");
      }

      // TODO: The hook should be deleted.
      // The hook for load lazy property UserInfo.
      user.UserInfo.ToString();

      try
      {
        return await _userManager.AddToRoleAsync(user.Id, rolename);
      }
      catch (DbUpdateException ex)
      {
        return IdentityResult.Failed(ex.InnerException.InnerException.Message);
      }
    }

    public async Task<IdentityResult> DeleteUser(string username)
    {
      ApplicationUser user = await _userManager.FindByNameAsync(username);
      if (user == null)
      {
        return IdentityResult.Failed($"User '{username}' not found.");
      }

      // TODO: The hook should be deleted.
      // The hook for load lazy property UserInfo.
      user.UserInfo.ToString();

      try
      {
        return await _userManager.DeleteAsync(user);
      }
      catch (DbUpdateException ex)
      {
        return IdentityResult.Failed(ex.InnerException.InnerException.Message);
      }
    }

    public void Dispose()
    {
      _userManager.Dispose();
      _roleManager.Dispose();
    }

    private async Task<UserResponseViewModel> MapUserAsync(ApplicationUser user)
    {
      return new UserResponseViewModel
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
        Roles = await _userManager.GetRolesAsync(user.Id),
        CardNumber = user.UserInfo.CardNumber
      };
    }

    private ApplicationUser MapUser(UserRequestViewModel user, byte[] image)
    {
      return new ApplicationUser
      {
        UserName = user.UserName,
        Email = user.Email,
        PhoneNumber = user.PhoneNumber,
        IsApproved = false,
        FCMPushNotificationToken = user.FCMPushNotificationToken,
        UserInfo = new UserInfo
        {
          FirstName = user.FirstName,
          LastName = user.LastName,
          Patronymic = user.Patronymic,
          CardNumber = user.CardNumber,
          CardOwnerFullName = user.CardOwnerFullName,
          CardDateOfExpire = DateTime.ParseExact(user.CardDateOfExpire, "MM/yy", CultureInfo.InvariantCulture).Date,
          CardCVV2Key = user.CardCVV2Key,
          PassportImage = image,
          ImageMimeType = user.ImageMimeType
        }
      };
    }
  }
}