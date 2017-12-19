using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nanobank.API.Models
{
  public class CreditCardTransitRequestViewModel
  {
    public string DealId { get; set; }
    public decimal Amount { get; set; }
    public string Username { get; set; }
  }
}