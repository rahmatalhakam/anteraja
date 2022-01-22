using System.ComponentModel.DataAnnotations;

namespace UserService.Dtos.Transactions
{
  public class FeeOutput
  {
    public string UserId { get; set; }

    public int distance { get; set; }
    public float Billing { get; set; }
    public float PricePerKM { get; set; }

    public string Area { get; set; }
    public bool canOrder { get; set; }

  }
}