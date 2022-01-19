using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TransactionService.Models;

namespace TransactionService.Dtos
{
  public class MutationOutput
  {

    public int Id { get; set; }

    public int WalletUserId { get; set; } //fk

    public float Saldo { get; set; }

    public DateTime CreatedAt { get; set; }

    public WalletUserOutput WalletUserOutput { get; set; }

  }
}