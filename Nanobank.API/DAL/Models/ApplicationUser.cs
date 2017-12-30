using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Nanobank.API.DAL.Models
{
  public class ApplicationUser : IdentityUser
  {
    [Required]
    public virtual UserInfo UserInfo { get; set; }

    public string FCMPushNotificationToken { get; set; }

    public bool IsApproved { get; set; }
  }
}