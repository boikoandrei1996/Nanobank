using System;
using System.Collections.Generic;

namespace Nanobank.API.Models.ViewModels
{
  public class UserViewModel
  {
    public string UserName { get; set; }
    public string Email { get; set; }
    public IList<string> Roles { get; set; }
  }
}