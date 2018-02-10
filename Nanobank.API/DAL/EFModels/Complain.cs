using System;
using System.ComponentModel.DataAnnotations;

namespace Nanobank.API.DAL.EFModels
{
  public class Complain
  {
    public Complain()
    {
      Id = Guid.NewGuid().ToString();
    }

    [Key]
    public string Id { get; set; }

    [Required]
    [DataType(DataType.Text)]
    [MaxLength(1000)]
    public string Text { get; set; }

    [Required]
    public string DealId { get; set; }
    public virtual Deal Deal { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfCreating { get; set; }
  }
}