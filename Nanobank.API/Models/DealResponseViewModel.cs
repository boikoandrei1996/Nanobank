using System;
using System.ComponentModel.DataAnnotations;

namespace Nanobank.API.Models
{
  public class DealResponseViewModel
  {
    [Display(Name = "Deal id")]
    public string Id { get; set; }

    [Display(Name = "Title")]
    public string Title { get; set; }

    [Display(Name = "Origin amount")]
    public decimal StartAmount { get; set; }
    
    [Display(Name = "Deal duration(month)")]
    public short DealDurationInMonth { get; set; }

    [Display(Name = "Percent rate")]
    public decimal PercentRate { get; set; }

    [Display(Name = "Owner user name")]
    public string OwnerUserName { get; set; }
    
    [Display(Name = "Creditor user name")]
    public string CreditorUserName { get; set; }

    [Display(Name = "Date of deal has been started")]
    public DateTime? DealStartDate { get; set; }

    [Display(Name = "Date of deal has been closed")]
    public DateTime? DealClosedDate { get; set; }

    [Display(Name = "Is deal has been closed")]
    public bool IsClosed { get; set; }

    [Display(Name = "Positive rating")]
    public short? RatingPositive { get; set; }

    [Display(Name = "Negative rating")]
    public short? RatingNegative { get; set; }
  }
}