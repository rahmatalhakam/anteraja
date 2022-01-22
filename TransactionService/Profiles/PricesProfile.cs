using System;
using AutoMapper;
using TransactionService.Helpers;

namespace TransactionService.Profiles
{
  public class PricesProfile : Profile
  {
    public PricesProfile()
    {
      CreateMap<Dtos.PriceInput, Models.Price>();
    }
  }
}
