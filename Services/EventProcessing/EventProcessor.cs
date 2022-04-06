using System.Text.Json;
using AutoMapper;
using Services.Data;
using Services.Dtos;
using Services.Models;

namespace Services.EventProcessing
{
	public class EventProcessor : IEventProcessor
	{
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly IMapper _mapper;
		public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
		{
			_scopeFactory = scopeFactory;
			_mapper = mapper;
		}

		public void ProcessEvent(string message)
		{
			var eventType = DetermineEvent(message);

			switch (eventType)
			{
				case EventType.UserUpdated:
					UpdateUser(message);
					break;
				default:
					break;
			}
		}

		private EventType DetermineEvent(string notificationMessage)
		{
			var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

			switch (eventType?.Event)
			{
				case "User_Updated":
					Console.WriteLine("--> User Updated Event Detected");
					return EventType.UserUpdated;
				default:
					Console.WriteLine("--> Could not determine the event type");
					return EventType.Undetermined;
			}
		}

		private void UpdateUser(string userPublishedMessage)
		{
			using (var scope = _scopeFactory.CreateScope())
			{
				var repo = scope.ServiceProvider.GetRequiredService<IUserRepo>();

				var userPublishedDto = JsonSerializer.Deserialize<UserPublishedDto>(userPublishedMessage);

				Console.WriteLine($"--> User will update: {userPublishedDto.Id}, {userPublishedDto.Name}");

				try
				{
					var userModel = _mapper.Map<User>(userPublishedDto);

					// update user if it exists
					if (repo.ExternalUserExists(userModel.ExternalId))
					{
						repo.UpdateUser(userModel);
						repo.SaveChanges();

						Console.WriteLine($"--> User updated: {repo.GetUser(userPublishedDto.Id).Id}, {repo.GetUser(userPublishedDto.Id).Name}");
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"--> Could not add User to DB {ex.Message}");
				}
			}
		}
	}
}