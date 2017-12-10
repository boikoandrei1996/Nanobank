using System;
using System.Collections.Generic;

namespace Nanobank.API.Models.ViewModels
{
  public class UserViewModel
  {
    public string UserName { get; set; }
    
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    
    public IList<string> Roles { get; set; }
    
    public bool IsApproved { get; set; }

    public long? RatingPositive { get; set; }
    public long? RatingNegative { get; set; }
  }
}