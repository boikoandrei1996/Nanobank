﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OAuth;
using Nanobank.API.DAL.Interface;

namespace Nanobank.API.Infrastructure.Providers
{
  public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider, IDisposable
  {
    private readonly IAuthRepository _authRepo;

    public SimpleAuthorizationServerProvider(IAuthRepository authRepo)
    {
      _authRepo = authRepo;
    }

    public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
    {
      return Task.Run(() => context.Validated());
    }

    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
    {
      context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

      var user = await _authRepo.FindUser(context.UserName, context.Password);

      if (user == null)
      {
        context.SetError("invalid_grant", "The user name or password is incorrect.");
        return;
      }

      if (!user.IsApproved)
      {
        context.SetError("invalid_access", "The user is not approved by administrator.");
        return;
      }

      // var identity = new ClaimsIdentity(context.Options.AuthenticationType);
      // identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
      // identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
      var identity = await _authRepo.CreateClaimsIdentity(user, DefaultAuthenticationTypes.ExternalBearer);

      context.Validated(identity);
    }

    public void Dispose()
    {
      _authRepo.Dispose();
    }
  }
}