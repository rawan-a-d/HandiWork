using AutoMapper;
using MessagingModels;
using Users.Dtos;
using Users.Models;

namespace Users.Profiles
{
	// AutoMapper: maps from one class to another
	public class UsersProfile : Profile
	{
		public UsersProfile()
		{
			// Source -> Target
			CreateMap<User, UserReadDto>();
			CreateMap<UserUpdateDto, User>();
			CreateMap<UserCreated, User>();
		}
	}
}