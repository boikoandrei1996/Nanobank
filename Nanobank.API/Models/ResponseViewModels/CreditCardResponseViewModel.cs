using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nanobank.API.Models.ResponseViewModels
{
  public class CreditCardResponseViewModel
  {
    [Display(Name = "Card number")]
    public string CardNumber { get; set; }

    [Display(Name = "Card balance")]
    public decimal Balance { get; set; }

    [Display(Name = "Users")]
    public ICollection<string> Users { get; set; }
  }
}