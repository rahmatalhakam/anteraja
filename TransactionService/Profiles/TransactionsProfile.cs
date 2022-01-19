using System;
using AutoMapper;
using TransactionService.Helpers;

namespace TransactionService.Profiles
{
  public class TransactionsProfile : Profile
  {
    public TransactionsProfile()
    {
      CreateMap<Models.Transaction, Dtos.TransactionOutput>()
          .ForMember(dest => dest.Status,
          opt => opt.MapFrom(src => src.StatusOrder.Status))
          .ForMember(dest => dest.distance,
          opt => opt.MapFrom(src => Convert.ToInt32(Coordinate.DistanceTo(src.LatStart, src.LongStart, src.LatEnd, src.LongEnd, 'K'))));
      CreateMap<Dtos.TransactionInput, Models.Transaction>();
    }
  }
}
