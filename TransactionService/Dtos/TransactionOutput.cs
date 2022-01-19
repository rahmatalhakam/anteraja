using System;
using System.ComponentModel.DataAnnotations;

namespace TransactionService.Dtos
{
  public class TransactionOutput
  {
    public int Id { get; set; }
    public string UserId { get; set; }
    public string DriverId { get; set; }

    public double LatStart { get; set; }

    public double LongStart { get; set; }

    public double LatEnd { get; set; }

    public double LongEnd { get; set; }

    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }
    public int distance { get; set; }
    public float Billing { get; set; }
  }
}