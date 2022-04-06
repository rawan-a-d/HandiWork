using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Users.AsyncDataServices;
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
		private readonly IMessageBusClient _messageBusClient;

		public UsersController(
			IUserRepo repository,
			IMapper mapper,
			IMessageBusClient messageBusClient
		)
		{
			_repository = repository;
			_mapper = mapper;
			_messageBusClient = messageBusClient;
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

		[HttpPut("{id}")]
		public ActionResult<UserReadDto> UpdateUser(int id, UserUpdateDto userUpdateDto)
		{
			if (!_repository.UserExists(id))
			{
				return BadRequest();
			}

			// map UserUpdateDto to User
			var userModel = _mapper.Map<User>(userUpdateDto);
			userModel.Id = id;

			// create user
			_repository.UpdateUser(userModel);
			// save to db
			_repository.SaveChanges();

			// get UserReadDto and return to user
			var userReadDto = _mapper.Map<UserReadDto>(userModel);

			try
			{
				var userPublishedDto = _mapper.Map<UserPublishedDto>(userReadDto);
				userPublishedDto.Event = "User_Updated";

				_messageBusClient.PublishUpdatedUser(userPublishedDto);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
			}

			return NoContent();
		}
	}
}