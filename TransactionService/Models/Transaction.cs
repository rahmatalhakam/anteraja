using System;
using System.ComponentModel.DataAnnotations;

namespace TransactionService.Models
{
  public class Transaction
  {
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public string DriverId { get; set; }

    [Required]
    public int PriceId { get; set; } //fk

    [Required]
    public int StatusOrderId { get; set; } //fk

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public double LatStart { get; set; }

    [Required]
    public double LongStart { get; set; }

    [Required]
    public double LatEnd { get; set; }

    [Required]
    public double LongEnd { get; set; }

    public float Billing { get; set; }

    public StatusOrder StatusOrder { get; set; }

    public Price Price { get; set; }

  }
}