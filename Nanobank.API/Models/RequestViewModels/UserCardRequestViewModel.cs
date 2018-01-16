using System;
using System.ComponentModel.DataAnnotations;

namespace Nanobank.API.Models.RequestViewModels
{
  public class UserCardRequestViewModel
  {
    [Required]
    [MaxLength(20)]
    [DataType(DataType.CreditCard)]
    public string CardNumber { get; set; }

    [Required]
    [RegularExpression(
      @"((0[1-9])|(1[0-2]))/(\d\d)",
      ErrorMessage = "Date format is incorrect. You should use format 'mm/yy'")]
    public string CardDateOfExpire { get; set; }

    [Required]
    [MaxLength(255)]
    public string CardOwnerFullName { get; set; }

    [Required]
    [MaxLength(3)]
    public string CardCVV2Key { get; set; }
  }
}