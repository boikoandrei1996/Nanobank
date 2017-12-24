using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nanobank.API.Models
{
  public class ComplainRequestViewModel
  {
    [Required]
    public string DealId { get; set; }

    [Required]
    [MaxLength(1000, ErrorMessage = "Too much symbols, max length is 1000")]
    public string ComplainText { get; set; }

    [Required]
    public string Username { get; set; }
  }
}