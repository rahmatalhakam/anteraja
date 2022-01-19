using System.ComponentModel.DataAnnotations;

namespace TransactionService.Dtos
{
  public class TransactionInput
  {
    [Required]
    public string UserId { get; set; }
    [Required]
    public string DriverId { get; set; }
    [Required]
    public double LatStart { get; set; }

    [Required]
    public double LongStart { get; set; }

    [Required]
    public double LatEnd { get; set; }

    [Required]
    public double LongEnd { get; set; }

    public string Area { get; set; }
  }
}