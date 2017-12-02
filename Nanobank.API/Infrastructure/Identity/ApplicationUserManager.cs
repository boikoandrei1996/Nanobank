using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Nanobank.API.DAL;

namespace Nanobank.API.Infrastructure.Identity
{
  public class ApplicationUserManager : UserManager<IdentityUser>
  {
    public ApplicationUserManager(IUserStore<IdentityUser> store)
      : base(store)
    {
    }

    public static ApplicationUserManager Create(ApplicationContext context)
    {
      var manager = new ApplicationUserManager(
        new UserStore<IdentityUser>(context));

      /*manager.UserValidator = new UserValidator<IdentityUser>(manager)
      {
        AllowOnlyAlphanumericUserNames = false,
        RequireUniqueEmail = true
      };

      manager.PasswordValidator = new PasswordValidator
      {
        RequiredLength = 6,
        //RequireNonLetterOrDigit = true,
        RequireDigit = true,
        //RequireLowercase = true,
        //RequireUppercase = true,
      };*/

      return manager;
    }
  }
}