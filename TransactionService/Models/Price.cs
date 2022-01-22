using System.ComponentModel.DataAnnotations;

namespace TransactionService.Models
{
  public class Price
  {
    [Key]
    public int Id { get; set; }

    [Required]
    public float PricePerKM { get; set; }

    [Required]
    public string Area { get; set; }
  }
}