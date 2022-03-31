using Users.Models;

namespace Users.Data
{
	// Database handler for user operations
	public class UserRepo : IUserRepo
	{
		private readonly AppDbContext _context;

		public UserRepo(AppDbContext context)
		{
			_context = context;
		}

		public void CreateUser(User user)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}

			_context.Users.Add(user);
		}

		public User GetUser(int id)
		{
			return _context.Users.FirstOrDefault(u => u.Id == id);
		}

		public IEnumerable<User> GetUsers()
		{
			throw new NotImplementedException();
		}

		public bool SaveChanges()
		{
			return (_context.SaveChanges() >= 0);
		}
	}
}