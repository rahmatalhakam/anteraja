using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TransactionService.Models
{
  public class WalletMutation
  {
    //   id, customer id, debit, credit, saldo, 
    [Key]
    public int Id { get; set; }

    [Required]
    public int WalletUserId { get; set; } //fk

    public float Debit { get; set; }

    public float Credit { get; set; }

    public float Saldo { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public WalletUser WalletUser { get; set; }


  }
}