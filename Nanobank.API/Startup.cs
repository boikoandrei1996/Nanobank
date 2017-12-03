﻿using System;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Nanobank.API.DAL.Interface;
using Nanobank.API.Infrastructure.Providers;
using Owin;
using Unity;

[assembly: OwinStartup(typeof(Nanobank.API.Startup))]
namespace Nanobank.API
{
  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      ConfigureOAuth(app);

      HttpConfiguration config = new HttpConfiguration();
      WebApiConfig.Register(config);
      
      config.DependencyResolver = new UnityResolver(UnityConfig.Container);

      // https://metanit.com/sharp/aspnet_webapi/5.1.php
      // Класс HostAuthenticationFilter подключает аутентификацию токенов, 
      // а метод SuppressDefaultHostAuthentication() указывает Web API игнорировать любую аутентификацию, 
      // которая происходит до того, как обработка запроса достигнет конвейера Web API. 
      // Это позволяет отключить атуентификацию на основе кук, и тем самым защитить приложение от CSRF-атак.
      config.SuppressDefaultHostAuthentication();
      config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

      app.UseWebApi(config);
      app.UseCors(CorsOptions.AllowAll);
    }

    public void ConfigureOAuth(IAppBuilder app)
    {
      var oAuthServerOptions = new OAuthAuthorizationServerOptions()
      {
        AllowInsecureHttp = true,
        TokenEndpointPath = new PathString("/api/token"),
        AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
        Provider = new SimpleAuthorizationServerProvider(UnityConfig.Container.Resolve<IAuthRepository>())
      };

      // Token Generation
      app.UseOAuthAuthorizationServer(oAuthServerOptions);
      app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
    }
  }
}