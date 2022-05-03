using Auth.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Auth.Profiles
{
	public class AuthProfile : Profile
	{
		public AuthProfile()
		{
			// Source -> Target
			CreateMap<UserCreateDto, IdentityUser>()
				.ForMember(dest => dest.UserName, opt =>
					opt.MapFrom(src => src.Email)
				);
		}
	}
}