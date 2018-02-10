using System.ComponentModel.DataAnnotations;

namespace Nanobank.API.DAL.Models.RequestViewModels
{
  public class CreditCardTransitRequestViewModel
  {
    [Required]
    public string DealId { get; set; }
    
    public decimal Amount { get; set; }
  }
}