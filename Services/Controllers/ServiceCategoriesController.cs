using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.Data;
using Services.Dtos;
using Services.Models;

namespace Services.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ServiceCategoriesController : ControllerBase
	{
		private readonly IServiceCategoryRepo _repository;
		private readonly IMapper _mapper;

		public ServiceCategoriesController(IServiceCategoryRepo repository, IMapper mapper)
		{
			_mapper = mapper;
			_repository = repository;
		}

		/// <summary>
		/// Create new service category
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult<ServiceCategoryReadDto> CreateServiceCategory(ServiceCategoryCreateDto serviceCategoryCreateDto)
		{
			// TODO: check if admin, if not unauthorized

			if (_repository.DoesServiceCategoryExist(serviceCategoryCreateDto.Name))
			{
				return Conflict("Service category already exists");
			}

			var serviceCategoryModel = _mapper.Map<ServiceCategory>(serviceCategoryCreateDto);
			_repository.CreateServiceCategory(serviceCategoryModel);
			_repository.SaveChanges();

			var serviceCategoryReadDto = _mapper.Map<ServiceCategoryReadDto>(serviceCategoryModel);

			return CreatedAtRoute(
				nameof(GetServiceCategory),
				new { Id = serviceCategoryReadDto.Id },
				serviceCategoryReadDto
			);
		}

		/// <summary>
		/// Get service categories
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult<IEnumerable<ServiceCategoryReadDto>> GetServiceCategories()
		{
			var categories = _repository.GetServiceCategories();

			return Ok(_mapper.Map<IEnumerable<ServiceCategoryReadDto>>(categories));
		}

		/// <summary>
		/// Get service category by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}", Name = "GetServiceCategory")]
		public ActionResult<ServiceCategoryReadDto> GetServiceCategory(int id)
		{
			var categoryModel = _repository.GetServiceCategory(id);

			if (categoryModel == null)
			{
				return NotFound();
			}

			return Ok(_mapper.Map<ServiceCategoryReadDto>(categoryModel));
		}

		/// <summary>
		/// Update service category
		/// </summary>
		/// <param name="id"></param>
		/// <param name="serviceCategoryUpdateDto"></param>
		/// <returns></returns>
		[HttpPut("{id}")]
		public ActionResult UpdateServiceCategory(int id, ServiceCategoryUpdateDto serviceCategoryUpdateDto)
		{
			// TODO: check if admin, if not unauthorized

			var serviceCategoryModel = _repository.GetServiceCategory(id);

			if (serviceCategoryModel == null)
			{
				return NotFound();
			}

			// map ServiceCategoryUpdateDto to ServiceCategory
			_mapper.Map(serviceCategoryUpdateDto, serviceCategoryModel);

			// update service
			_repository.UpdateServiceCategory(serviceCategoryModel);
			// save to db
			_repository.SaveChanges();

			return NoContent();
		}

		/// <summary>
		/// Delete service category by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("{id}")]
		public ActionResult DeleteServiceCategory(int id)
		{
			// TODO: check if admin, if not unauthorized

			var serviceCategoryModel = _repository.GetServiceCategory(id);

			if (serviceCategoryModel == null)
			{
				return NotFound();
			}

			_repository.DeleteServiceCategory(serviceCategoryModel);

			if (_repository.SaveChanges())
			{
				return Ok();
			}

			return BadRequest("Service category cannot be removed");
		}
	}
}