using System;
using System.Linq;
using System.Web.Configuration;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL;
using Nanobank.API.DAL.Loggers;
using Nanobank.API.DAL.Managers;
using Nanobank.API.DAL.Notifications;
using Nanobank.API.DAL.Repositories;
using Nanobank.API.DAL.Repositories.Interfaces;
using Nanobank.API.Infrastructure.Notifications;
using Unity;
using Unity.Injection;

namespace Nanobank.API
{
  /// <summary>
  /// Specifies the Unity configuration for the main container.
  /// </summary>
  public static class UnityConfig
  {
    private static Lazy<IUnityContainer> container =
      new Lazy<IUnityContainer>(() =>
      {
        var container = new UnityContainer();
        RegisterTypes(container);
        return container;
      });

    /// <summary>
    /// Configured Unity Container.
    /// </summary>
    public static IUnityContainer Container => container.Value;

    /// <summary>
    /// Registers the type mappings with the Unity container.
    /// </summary>
    /// <param name="container">The unity container to configure.</param>
    /// <remarks>
    /// There is no need to register concrete types such as controllers or
    /// API controllers (unless you want to change the defaults), as Unity
    /// allows resolving a concrete type even if it was not previously
    /// registered.
    /// </remarks>
    public static void RegisterTypes(IUnityContainer container)
    {
      // NOTE: To load from web.config uncomment the line below.
      // Make sure to add a Unity.Configuration to the using statements.
      // container.LoadConfiguration();

      RegisterLoggerType(container);

      container.RegisterType<ApplicationContext>();
      container.RegisterType<IIdentityMessageService, SendGridEmailService>();

      container.RegisterType<ApplicationUserManager>(
        new InjectionFactory(
          c => ApplicationUserManager.Create(
            c.Resolve<ApplicationContext>(),
            c.Resolve<IIdentityMessageService>()))
      );

      container.RegisterType<ApplicationRoleManager>(
        new InjectionFactory(c => ApplicationRoleManager.Create(c.Resolve<ApplicationContext>()))
      );

      container.RegisterType<IAuthRepository, AuthRepository>();
      container.RegisterType<IUserRepository, UserRepository>();
      container.RegisterType<IDealRepository, DealRepository>();
      container.RegisterType<IComplainRepository, ComplainRepository>();
      container.RegisterType<IReportRepository, ReportRepository>();
      container.RegisterType<IPhotoRepository, PhotoRepository>();
      container.RegisterType<ICreditCardRepository, CreditCardRepository>();

      container.RegisterType<IPushNotificationService, AndroidPushNotificationService>();

      //container.RegisterType<OrdersController>(new InjectionConstructor());
    }

    private static void RegisterLoggerType(IUnityContainer container)
    {
      bool isDebugLevel = false;
      if (WebConfigurationManager.AppSettings.AllKeys.Contains("DebugLevel"))
      {
        isDebugLevel = bool.Parse(WebConfigurationManager.AppSettings["DebugLevel"]);
      }

      container.RegisterType<NLogLogger>(
        new InjectionFactory(c => NLogLogger.Create(isDebugLevel))
      );
      container.RegisterType<ILogger, NLogLogger>();
    }
  }
}