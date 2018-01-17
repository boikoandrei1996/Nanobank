using System;
using System.ComponentModel.DataAnnotations;

namespace Nanobank.API.Models.ResponseViewModels
{
  [Serializable]
  public class ReportResponseViewModel
  {
    [Display(Name = "Complain Id")]
    public string ComplainId { get; set; }

    [Display(Name = "Complain Text")]
    public string ComplainText { get; set; }

    [Display(Name = "Deal Id")]
    public string DealId { get; set; }

    [Display(Name = "Deal Owner Username")]
    public string DealOwnerUsername { get; set; }

    [Display(Name = "Deal Creditor Username")]
    public string DealCreditorUsername { get; set; }

    [Display(Name = "Date of creating")]
    public DateTime DateOfCreating { get; set; }
  }
}