using System;
using System.ComponentModel.DataAnnotations;

namespace Nanobank.API.Models
{
  public class UserRequestViewModel
  {
    [Required]
    public string UserName { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
    
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(100)]
    public string Patronymic { get; set; }
    
    [Required]
    [MaxLength(20)]
    [DataType(DataType.CreditCard)]
    public string CardNumber { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime CardDateOfExpire { get; set; }

    [Required]
    [MaxLength(255)]
    public string CardOwnerFullName { get; set; }

    [Required]
    [MaxLength(3)]
    public string CardCVV2Key { get; set; }

    //[Required]
    //public byte[] PassportImage { get; set; }

    //[Required]
    //[MaxLength(10)]
    //public string ImageMimeType { get; set; }
  }
}