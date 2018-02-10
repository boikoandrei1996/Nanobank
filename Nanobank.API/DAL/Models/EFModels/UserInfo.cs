using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nanobank.API.DAL.Models.EFModels
{
  public class UserInfo
  {
    [Key, ForeignKey("User")]
    public string Id { get; set; }

    [Required]
    public virtual ApplicationUser User { get; set; }

    [Required]
    [MaxLength(100)]
    [Index("IX_UniqueFullName", 1, IsUnique = true)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(100)]
    [Index("IX_UniqueFullName", 2, IsUnique = true)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(100)]
    [Index("IX_UniqueFullName", 3, IsUnique = true)]
    public string Patronymic { get; set; }

    [Required]
    [DataType(DataType.CreditCard)]
    [ForeignKey("Card")]
    public string CardNumber { get; set; }
    
    public virtual CreditCard Card { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime CardDateOfExpire { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string CardOwnerFullName { get; set; }

    [Required]
    [MaxLength(3)]
    public string CardCVV2Key { get; set; }

    [Required]
    [DataType(DataType.Upload)]
    [Column(TypeName = "image")]
    public byte[] PassportImage { get; set; }
    
    [Required]
    [MaxLength(10)]
    public string ImageMimeType { get; set; }

    public long? RatingPositive { get; set; }
    
    public long? RatingNegative { get; set; }
  }
}