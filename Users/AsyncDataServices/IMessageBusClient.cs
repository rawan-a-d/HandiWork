using Users.Dtos;

namespace Users.AsyncDataServices
{
	public interface IMessageBusClient
	{
		void PublishUpdatedUser(UserPublishedDto userPublishedDto);
	}
}