using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.Data;
using Services.Dtos;
using Services.Models;

namespace Services.Controllers
{
	[Route("api/users/{userId}/[controller]")]
	[ApiController]
	public class ServicesController : ControllerBase
	{
		private readonly IServiceRepo _serviceRepository;
		private readonly IServiceCategoryRepo _serviceCategoryRepository;
		private readonly IPhotoRepo _photoRepository;
		private readonly IPhotoService _photoService;
		private readonly IMapper _mapper;

		public ServicesController(
			IServiceRepo serviceRepository,
			IServiceCategoryRepo serviceCategoryRepository,
			IPhotoRepo photoRepository,
			IPhotoService photoService,
			IMapper mapper)
		{
			_serviceRepository = serviceRepository;
			_serviceCategoryRepository = serviceCategoryRepository;
			_photoRepository = photoRepository;
			_photoService = photoService;
			_mapper = mapper;
		}

		/// <summary>
		/// Create new service
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="serviceCreateDto"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult CreateService(int userId, ServiceCreateDto serviceCreateDto)
		{
			// TODO: validate user is logged in

			// check if category exists
			var serviceCategory = _serviceCategoryRepository.GetServiceCategory(serviceCreateDto.ServiceCategoryId);

			if (serviceCategory == null)
			{
				return BadRequest("Service category does not exist");
			}

			var serviceModel = _mapper.Map<Service>(serviceCreateDto);

			_serviceRepository.CreateService(userId, serviceModel);
			_serviceRepository.SaveChanges();

			var serviceReadDto = _mapper.Map<ServiceReadDto>(serviceModel);

			return CreatedAtRoute(
				nameof(GetService),
				new { userId = userId, serviceId = serviceReadDto.Id },
				serviceReadDto
			);
		}

		/// <summary>
		/// Get services for a user
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult<IEnumerable<ServiceReadDto>> GetServices(int userId)
		{
			// TODO: validate user is logged in

			var services = _mapper.Map<IEnumerable<ServiceReadDto>>(_serviceRepository.GetServicesForUser(userId));

			return Ok(services);
		}

		/// <summary>
		/// Get service by id and user id
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="serviceId"></param>
		/// <returns></returns>
		[HttpGet("{serviceId}", Name = "GetService")]
		public ActionResult<IEnumerable<ServiceReadDto>> GetService(int userId, int serviceId)
		{
			// TODO: validate user is logged in

			// get service
			var service = _serviceRepository.GetService(serviceId, userId);

			if (service == null)
			{
				return NotFound();
			}

			return Ok(_mapper.Map<ServiceReadDto>(service));
		}

		/// <summary>
		/// Update service
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="serviceId"></param>
		/// <param name="serviceUpdateDto"></param>
		/// <returns></returns>
		[HttpPut("{serviceId}")]
		public ActionResult<ServiceReadDto> UpdateService(int userId, int serviceId, ServiceUpdateDto serviceUpdateDto)
		{
			// TODO: validate user is logged in (JWT)
			// TODO: validate user is owner (if not unauthorized)

			var service = _serviceRepository.GetService(serviceId, userId);
			if (service == null)
			{
				return NotFound();
			}

			// map ServiceUpdateDto to Service
			_mapper.Map(serviceUpdateDto, service);

			// update service
			_serviceRepository.UpdateService(service);
			// save to db
			_serviceRepository.SaveChanges();

			return NoContent();
		}

		/// <summary>
		/// Delete service
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="serviceId"></param>
		/// <returns></returns>
		[HttpDelete("{serviceId}")]
		public ActionResult DeleteService(int userId, int serviceId)
		{
			// TODO: validate user is logged in (JWT)
			// TODO: validate user is owner (if not unauthorized)

			// get service
			var service = _serviceRepository.GetService(serviceId, userId);

			_serviceRepository.DeleteService(service);

			if (_serviceRepository.SaveChanges())
			{
				return Ok();
			}

			return BadRequest("Service cannot be removed");
		}

		[HttpPost("{serviceId}/photos")]
		public async Task<ActionResult<PhotoDto>> AddPhoto(int userId, int serviceId, IFormFile file)
		{
			// get service object with the photos
			var service = _serviceRepository.GetService(serviceId, userId);

			if (service == null)
			{
				return NotFound("Service does not exist");
			}

			// add new photo to Cloudinary
			var result = await _photoService.AddPhotoAsync(file);

			// if there was an error
			if (result.Error != null)
			{
				return BadRequest(result.Error.Message);
			}

			// Create new photo object using the result
			var photo = new Photo
			{
				Url = result.SecureUrl.AbsoluteUri,
				PublicId = result.PublicId
			};

			// add photo to photos array
			//_photoRepository.CreatePhoto(photo);
			service.Photos.Add(photo);

			// save changes to db
			if (_photoRepository.SaveChanges())
			{
				return CreatedAtRoute("GetService", new { userId = userId, serviceId = serviceId }, _mapper.Map<PhotoDto>(photo));
			}

			return BadRequest("Problem adding photos");
		}

		/// <summary>
		/// Delete a photo
		/// </summary>
		/// <param name="photoId">photo id</param>
		/// <returns></returns>
		[HttpDelete("{serviceId}/photos/{photoId}")]
		public async Task<ActionResult> DeletePhoto(int userId, int serviceId, int photoId)
		{
			// get service object with the photos
			var service = _serviceRepository.GetService(serviceId, userId);

			if (service == null)
			{
				return NotFound("Service does not exist");
			}

			// find photo
			var photo = service.Photos.FirstOrDefault(x => x.Id == photoId);

			if (photo == null)
			{
				return NotFound();
			}

			// remove photo
			// 1. from Cloudinary
			if (photo.PublicId != null)
			{
				// delete photo from Cloudinary
				var result = await _photoService.DeletePhotoAsync(photo.PublicId);

				if (result.Error != null)
				{
					return BadRequest(result.Error.Message);
				}
			}
			// 2. from DB
			_photoRepository.DeletePhoto(photo);

			// save to db
			if (_photoRepository.SaveChanges())
			{
				return Ok();
			}

			return BadRequest("Failed to delete photo");
		}
	}
}