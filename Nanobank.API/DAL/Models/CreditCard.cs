using System.ComponentModel.DataAnnotations;

namespace Nanobank.API.DAL.Models
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