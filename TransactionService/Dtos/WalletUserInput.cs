using System;
using System.ComponentModel.DataAnnotations;

namespace TransactionService.Dtos
{
  public class WalletUserInput
  {
    [Required]
    public string CustomerId { get; set; }


  }
}