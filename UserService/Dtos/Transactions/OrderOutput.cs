using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dtos.Transactions
{
  public class OrderOutput
  {
    public string Message { get; set; }
    public FeeOutput Data { get; set; }
  }
}