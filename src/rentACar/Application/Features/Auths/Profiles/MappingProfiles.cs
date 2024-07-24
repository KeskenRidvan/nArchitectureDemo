using Application.Features.Auths.Commands.RevokeToken;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Auths.Profiles;
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Core.Security.Entities.RefreshToken<int, int>, RefreshToken>().ReverseMap();
        CreateMap<RefreshToken, RevokedTokenResponse>().ReverseMap();
    }
}