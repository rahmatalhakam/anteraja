using System.ComponentModel.DataAnnotations;

namespace TransactionService.Dtos
{
  public class FeeOutput
  {
    public string UserId { get; set; }

    public int distance { get; set; }
    public float Billing { get; set; }
    public float PricePerKM { get; set; }


  }
}