using System;
using Nanobank.API.DAL;
using Nanobank.API.DAL.Interface;
using Nanobank.API.Infrastructure.Identity;
using Unity;
using Unity.Injection;

namespace Nanobank.API
{
  /// <summary>
  /// Specifies the Unity configuration for the main container.
  /// </summary>
  public static class UnityConfig
  {
    #region Unity Container
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
    #endregion

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

      // TODO: Register your type's mappings here.

      container.RegisterType<ApplicationContext>();
      
      container.RegisterType<ApplicationUserManager>(
        new InjectionFactory(c => ApplicationUserManager.Create(c.Resolve<ApplicationContext>()))
      );

      container.RegisterType<ApplicationRoleManager>(
        new InjectionFactory(c => ApplicationRoleManager.Create(c.Resolve<ApplicationContext>()))
      );

      container.RegisterType<IAuthRepository, AuthRepository>();
      container.RegisterType<IDealRepository, DealRepository>();
      container.RegisterType<ICreditCardRepository, CreditCardRepository>();

      //container.RegisterType<OrdersController>(new InjectionConstructor());
    }
  }
}