using System.ComponentModel.DataAnnotations;

namespace Nanobank.API.Models
{
  public class DealRequestViewModel
  {
    [Required]
    [MaxLength(1000)]
    public string Title { get; set; }

    [DataType(DataType.Currency)]
    public decimal StartAmount { get; set; }

    [Range(1, 24, ErrorMessage = "Deal duration should be [1 - 24] month(s)")]
    public short DealDurationInMonth { get; set; }

    public decimal PercentRate { get; set; }

    public string CreditorUserName { get; set; }
  }
}