using AutoMapper;
using MessagingModels;
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
			CreateMap<ServiceUpdateDto, Service>();

			CreateMap<ServiceCategory, ServiceCategoryReadDto>();
			CreateMap<ServiceCategoryCreateDto, ServiceCategory>();
			CreateMap<ServiceCategoryUpdateDto, ServiceCategory>();

			CreateMap<Photo, PhotoDto>();

			CreateMap<UserCreated, User>()
				.ForMember(dest => dest.ExternalId, opt =>
					opt.MapFrom(src => src.Id)
				);
			CreateMap<UserUpdated, User>()
				.ForMember(dest => dest.ExternalId, opt =>
					opt.MapFrom(src => src.Id)
				);
			CreateMap<UserDeleted, User>()
				.ForMember(dest => dest.ExternalId, opt =>
					opt.MapFrom(src => src.Id)
				);
		}
	}
}