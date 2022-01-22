using System.ComponentModel.DataAnnotations;

namespace UserService.Dtos.Transactions
{
  public class FeeInput
  {
    [Required]
    public string UserId { get; set; }

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