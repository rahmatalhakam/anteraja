using System;
using System.ComponentModel.DataAnnotations;

namespace TransactionService.Dtos
{
  public class FinishTransInput
  {
    [Required]
    public int TransactionId { get; set; }
    [Required]
    public string DriverId { get; set; }

  }
}