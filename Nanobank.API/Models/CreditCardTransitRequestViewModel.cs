using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nanobank.API.Models
{
  public class CreditCardTransitRequestViewModel
  {
    public string FromCreditNumber { get; set; }
    public string ToCreditNumber { get; set; }
    public decimal Amount { get; set; }
    public string Username { get; set; }
  }
}