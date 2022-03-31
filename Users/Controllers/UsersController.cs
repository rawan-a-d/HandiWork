using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Users.Data;
using Users.Dtos;
using Users.Models;

namespace Users.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserRepo _repository;
		private readonly IMapper _mapper;

		public UsersController(IUserRepo repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		[HttpGet("{id}", Name = "GetUser")]
		public ActionResult<UserReadDto> GetUser(int id)
		{
			var userItem = _repository.GetUser(id);

			if (userItem != null)
			{
				// return UserReadDto
				return Ok(_mapper.Map<UserReadDto>(userItem));
			}

			return NotFound();
		}

		[HttpPost]
		public ActionResult<UserReadDto> CreateUser(UserCreateDto userCreateDto)
		{
			// map UserCreateDto to User
			var userModel = _mapper.Map<User>(userCreateDto);

			// create user
			_repository.CreateUser(userModel);
			// save to db
			_repository.SaveChanges();

			// get UserReadDto and return to user
			var userReadDto = _mapper.Map<UserReadDto>(userModel);

			return CreatedAtRoute(
				nameof(GetUser),
				new { Id = userReadDto.Id },
				userReadDto
			);
		}
	}
}