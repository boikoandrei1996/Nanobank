﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL.Models;
using Nanobank.API.Models;

namespace Nanobank.API.DAL.Interface
{
  public interface IAuthRepository : IDisposable
  {
    Task<IList<UserResponseViewModel>> GetUsers(Func<ApplicationUser, bool> predicate = null);
    Task<UserResponseViewModel> GetUser(string username);
    Task<IdentityResult> RegisterUser(UserRequestViewModel userModel);
    Task<IdentityResult> ApproveUser(string username);
    Task<IdentityResult> AddRoleToUser(string username, string rolename);
    Task<IdentityResult> DeleteUser(string username);
    Task<ApplicationUser> FindUser(string username, string password);
    Task<ClaimsIdentity> CreateClaimsIdentity(ApplicationUser user, string authenticationType);
    Task<PhotoResponseViewModel> GetPhoto(string username);
  }
}
