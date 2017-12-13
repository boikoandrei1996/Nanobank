﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Nanobank.API.DAL.Models
{
  public class Deal
  {
    [Key]
    public string Id { get; set; }

    [Required]
    [DataType(DataType.Text)]
    [MaxLength(1000)]
    public string Title { get; set; }

    [DataType(DataType.Currency)]
    public decimal StartAmount { get; set; }
    
    [Range(1, 24, ErrorMessage = "Deal duration should be [1 - 24] month(s)")]
    public short DealDurationInMonth { get; set; }

    public decimal PercentRate { get; set; }

    [Required]
    public string UserOwnerId { get; set; }
    public virtual ApplicationUser UserOwner { get; set; }

    public string UserCreditorId { get; set; }
    public virtual ApplicationUser UserCreditor { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DealStartDate { get; set; }
    
    [Range(0, 5, ErrorMessage = "Positive rating should be [0 - 5] point(s)")]
    public short? RatingPositive { get; set; }

    [Range(0, 5, ErrorMessage = "Negative rating should be [0 - 5] point(s)")]
    public short? RatingNegative { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DealClosedDate { get; set; }

    public bool IsClosed { get; set; }
  }
}