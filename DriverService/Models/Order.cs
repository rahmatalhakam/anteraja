using System.ComponentModel.DataAnnotations;


namespace DriverService.Models
{
  public class Order
  {
    [Key]
    public int OrderId { get; set; }

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