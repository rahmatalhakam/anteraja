using System;
using AutoMapper;
using TransactionService.Helpers;

namespace TransactionService.Profiles
{
  public class WalletMutationsProfile : Profile
  {
    public WalletMutationsProfile()
    {
      CreateMap<Models.WalletMutation, Dtos.MutationOutput>()
      .ForMember(dest => dest.Rolename,
          opt => opt.MapFrom(src => src.WalletUser.Rolename));
      CreateMap<Dtos.TopUpInput, Models.WalletMutation>();
      CreateMap<Dtos.WithdrawInput, Models.WalletMutation>();
    }
  }
}
