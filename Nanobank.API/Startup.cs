using System;
using System.Data.Entity.Migrations;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Nanobank.API.DAL.Interface;
using Nanobank.API.Infrastructure.Providers;
using Nanobank.API.Migrations;
using Owin;
using Unity;

[assembly: OwinStartup(typeof(Nanobank.API.Startup))]
namespace Nanobank.API
{
  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      // Должен быть в начале, т.к. иначе будут ошибки связанные с CORS доступом
      app.UseCors(CorsOptions.AllowAll);

      ConfigureOAuth(app);

      HttpConfiguration config = new HttpConfiguration
      {
        DependencyResolver = new UnityResolver(UnityConfig.Container)
      };
      
      WebApiConfig.Register(config);
      
      // https://metanit.com/sharp/aspnet_webapi/5.1.php
      // Класс HostAuthenticationFilter подключает аутентификацию токенов, 
      // а метод SuppressDefaultHostAuthentication() указывает Web API игнорировать любую аутентификацию, 
      // которая происходит до того, как обработка запроса достигнет конвейера Web API. 
      // Это позволяет отключить атуентификацию на основе кук, и тем самым защитить приложение от CSRF-атак.
      config.SuppressDefaultHostAuthentication();
      config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

      app.UseWebApi(config);
      
      var migrator = new DbMigrator(new Configuration());
      migrator.Update();
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