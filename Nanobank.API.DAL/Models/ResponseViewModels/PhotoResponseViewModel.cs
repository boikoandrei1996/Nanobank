using System.ComponentModel.DataAnnotations;

namespace Nanobank.API.DAL.Models.ResponseViewModels
{
  public class PhotoResponseViewModel
  {
    // As Base64 string
    [Display(Name = "Passport image as base64 string")]
    public string PassportImage { get; set; }

    [Required]
    [MaxLength(10)]
    public string ImageMimeType { get; set; }
  }
}