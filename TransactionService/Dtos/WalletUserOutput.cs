using System;

namespace TransactionService.Dtos
{
  public class WalletUserOutput
  {
    public int Id { get; set; }

    public string Rolename { get; set; }

    public string CustomerId { get; set; }

    public DateTime CreatedAt { get; set; }

  }
}