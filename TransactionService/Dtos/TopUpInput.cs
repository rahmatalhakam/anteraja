using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TransactionService.Dtos
{
  public class TopUpInput
  {

    [Required]
    public int WalletUserId { get; set; } //fk

    [Required]
    public float Credit { get; set; }

  }
}