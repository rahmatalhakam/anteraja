using System.ComponentModel.DataAnnotations;

namespace TransactionService.Models
{
  public class StatusOrder
  {
    [Key]
    public int Id { get; set; }

    [Required]
    public string Status { get; set; }
  }
}