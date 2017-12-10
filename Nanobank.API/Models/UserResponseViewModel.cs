using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nanobank.API.Models
{
  public class UserResponseViewModel
  {
    [Display(Name = "User name")]
    public string UserName { get; set; }

    [Display(Name = "Email")]
    public string Email { get; set; }

    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; }
 
    [Display(Name = "First name")]
    public string FirstName { get; set; }

    [Display(Name = "Last name")]
    public string LastName { get; set; }

    [Display(Name = "Patronymic")]
    public string Patronymic { get; set; }

    [Display(Name = "Roles")]
    public IList<string> Roles { get; set; }

    [Display(Name = "Is approved")]
    public bool IsApproved { get; set; }

    [Display(Name = "Rating positive")]
    public long? RatingPositive { get; set; }

    [Display(Name = "Rating negative")]
    public long? RatingNegative { get; set; }
  }
}