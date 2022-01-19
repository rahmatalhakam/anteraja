using System;
using System.ComponentModel.DataAnnotations;

namespace TransactionService.Dtos
{
  public class PriceInput
  {
    [Required]
    public float PricePerKM { get; set; }
    [Required]
    public string Area { get; set; }

  }
}