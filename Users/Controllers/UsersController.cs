using AutoMapper;
using MassTransit;
using MessagingModels;
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
		private readonly IPublishEndpoint _publishEndPoint;

		public UsersController(
			IUserRepo repository,
			IMapper mapper,
			IPublishEndpoint publishEndPoint
		)
		{
			_repository = repository;
			_mapper = mapper;
			_publishEndPoint = publishEndPoint;
		}

		[HttpGet]
		public ActionResult<IEnumerable<UserReadDto>> GetUsers()
		{
			var users = _repository.GetUsers();

			return Ok(users);
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
		public async Task<ActionResult> UpdateUser(int id, UserUpdateDto userUpdateDto)
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

			try
			{
				// Publish UserUpdate event
				await _publishEndPoint.Publish<UserUpdated>(new
				{
					Id = userModel.Id,
					Name = userModel.Name
				});
			}
			catch (Exception ex)
			{
				Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
			}

			return NoContent();
		}

		[HttpDelete("{id}")]
		public ActionResult DeleteUser(int id)
		{
			var user = _repository.GetUser(id);

			_repository.DeleteUser(user);

			// save to db
			if (_repository.SaveChanges())
			{
				return Ok();
			}

			return BadRequest("User cannot be removed");
		}
	}
}