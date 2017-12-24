﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nanobank.API.Models
{
  public class CreditCardTransitRequestViewModel
  {
    [Required]
    public string DealId { get; set; }
    
    public decimal Amount { get; set; }

    [Required]
    public string Username { get; set; }
  }
}