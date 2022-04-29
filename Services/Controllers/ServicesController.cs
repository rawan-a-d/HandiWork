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
		private readonly IServiceRepo _repository;
		private readonly IServiceCategoryRepo _serviceCategoryRepository;
		private readonly IMapper _mapper;

		public ServicesController(IServiceRepo repository, IServiceCategoryRepo serviceCategoryRepository, IMapper mapper)
		{
			_repository = repository;
			_serviceCategoryRepository = serviceCategoryRepository;
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

			_repository.CreateService(userId, serviceModel);
			_repository.SaveChanges();

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

			var services = _mapper.Map<IEnumerable<ServiceReadDto>>(_repository.GetServicesForUser(userId));

			//"dfkgc".StartsWithUpper()

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
			var service = _repository.GetService(serviceId, userId);

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

			var service = _repository.GetService(serviceId, userId);
			if (service == null)
			{
				return NotFound();
			}

			// map ServiceUpdateDto to Service
			_mapper.Map(serviceUpdateDto, service);

			// update service
			_repository.UpdateService(service);
			// save to db
			_repository.SaveChanges();

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
			var service = _repository.GetService(serviceId, userId);

			_repository.DeleteService(service);

			if (_repository.SaveChanges())
			{
				return Ok();
			}

			return BadRequest("Service cannot be removed");
		}
	}
}