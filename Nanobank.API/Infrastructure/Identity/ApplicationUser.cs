using Microsoft.AspNet.Identity.EntityFramework;

namespace Nanobank.API.Infrastructure.Identity
{
  public class ApplicationUser : IdentityUser
  {
    public string UserInfoId { get; set; }
  }
}