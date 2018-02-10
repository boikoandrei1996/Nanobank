using System.ComponentModel.DataAnnotations;

namespace Nanobank.API.DAL.Models.EFModels
{
  public class CreditCard
  {
    [Key]
    public string CardNumber { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    public decimal Balance { get; set; }
  }
}