using Services.Models;

namespace Services.Data
{
	public interface IUserRepo
	{
		void UpdateUser(User user);

		bool SaveChanges();

		User GetUser(int id);

		bool ExternalUserExists(int externalPlatformId);
	}
}