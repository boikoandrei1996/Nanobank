using System.ComponentModel.DataAnnotations;

namespace Nanobank.API.Models
{
  public class ComplainResponseViewModel
  {
    [Display(Name = "Complain Id")]
    public string ComplainId { get; set; }

    [Display(Name = "Complain Text")]
    public string ComplainText { get; set; }

    [Display(Name = "Deal Id")]
    public string DealId { get; set; }
  }
}