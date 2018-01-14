using System.ComponentModel.DataAnnotations;

namespace Nanobank.API.Models.RequestViewModels
{
  public class ComplainRequestViewModel
  {
    [Required]
    public string DealId { get; set; }

    [Required]
    [MaxLength(1000, ErrorMessage = "Too much symbols, max length is 1000")]
    public string ComplainText { get; set; }
  }
}