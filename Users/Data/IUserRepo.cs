using Users.Models;

namespace Users.Data
{
	public interface IUserRepo
	{
		/// <summary>
		/// Persist changes to the database
		/// </summary>
		/// <returns></returns>
		bool SaveChanges();

		/// <summary>
		/// Get users
		/// </summary>
		/// <returns></returns>
		IEnumerable<User> GetUsers();

		/// <summary>
		/// Get user by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		User GetUser(int id);

		void UpdateUser(User user);

		/// <summary>
		/// Check if user exists
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Boolean UserExists(int id);
	}
}