using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Nanobank.API.DAL;

namespace Nanobank.API.Infrastructure.Identity
{
  public class ApplicationUserManager : UserManager<ApplicationUser>
  {
    public ApplicationUserManager(IUserStore<ApplicationUser> store)
      : base(store)
    {
    }

    public static ApplicationUserManager Create(ApplicationContext context)
    {
      var manager = new ApplicationUserManager(
        new UserStore<ApplicationUser>(context));

      /*manager.UserValidator = new UserValidator<ApplicationUser>(manager)
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