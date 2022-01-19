using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TransactionService.Dtos
{
  public class WithdrawInput
  {

    [Required]
    public int WalletUserId { get; set; } //fk

    [Required]
    public float Debit { get; set; }

  }
}