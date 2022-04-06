using AutoMapper;
using Services.Dtos;
using Services.Models;

namespace Services.Profiles
{
	public class ServicesProfile : Profile
	{
		public ServicesProfile()
		{
			// Source -> Target
			CreateMap<Service, ServiceReadDto>();
			CreateMap<ServiceCreateDto, Service>();
			CreateMap<UserPublishedDto, User>()
				.ForMember(dest => dest.ExternalId, opt =>
					opt.MapFrom(src => src.Id)
				);
		}
	}
}