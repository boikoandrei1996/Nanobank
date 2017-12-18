using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nanobank.API.Models
{
  public class CreditCardResponseViewModel
  {
    public string CardNumber { get; set; }
    public decimal Balance { get; set; }
    public ICollection<string> Users { get; set; }
  }
}