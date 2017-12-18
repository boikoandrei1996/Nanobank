﻿using System.ComponentModel.DataAnnotations;

namespace Nanobank.API.Models
{
  public class RatingRequestViewModel
  {
    public string CreditorUsername { get; set; }

    [Range(0, 5, ErrorMessage = "Positive rating should be [0 - 5] point(s)")]
    public short Positive { get; set; }

    [Range(0, 5, ErrorMessage = "Negative rating should be [0 - 5] point(s)")]
    public short Negative { get; set; }
  }
}