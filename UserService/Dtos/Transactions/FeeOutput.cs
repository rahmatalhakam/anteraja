using System.ComponentModel.DataAnnotations;

namespace UserService.Dtos.Transactions
{
  public class FeeOutput
  {
    public string userId { get; set; }

    public int distance { get; set; }
    public float billing { get; set; }
    public float pricePerKM { get; set; }

    public string area { get; set; }
    public bool canOrder { get; set; }

  }
}