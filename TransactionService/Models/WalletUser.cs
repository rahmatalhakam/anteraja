using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TransactionService.Models
{

  public class WalletUser
  {
    //   id, role, dan customer id
    [Key]
    public int Id { get; set; }

    [Required]
    public string Rolename { get; set; }

    [Required]
    public int CustomerId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public ICollection<WalletMutation> WalletMutations { get; set; }
  }
}