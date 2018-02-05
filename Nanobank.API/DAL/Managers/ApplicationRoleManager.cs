using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Nanobank.API.DAL.Managers
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