using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Nanobank.API.DAL;

namespace Nanobank.API.Infrastructure.Identity
{
  public class ApplicationRoleManager : RoleManager<IdentityRole>
  {
    public ApplicationRoleManager(RoleStore<IdentityRole> store)
      : base(store)
    {
    }

    public static ApplicationRoleManager Create(ApplicationContext context)
    {
      var manager = new ApplicationRoleManager(
        new RoleStore<IdentityRole>(context));

      return manager;
    }
  }
}