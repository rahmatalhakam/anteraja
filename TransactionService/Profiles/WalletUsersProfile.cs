using System;
using AutoMapper;
using TransactionService.Helpers;

namespace TransactionService.Profiles
{
  public class WalletUsersProfile : Profile
  {
    public WalletUsersProfile()
    {
      CreateMap<Models.WalletUser, Dtos.WalletUserOutput>();
    }
  }
}
