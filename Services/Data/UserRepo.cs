using Services.Models;

namespace Services.Data
{
	public class UserRepo : IUserRepo
	{
		private readonly AppDbContext _context;
		public UserRepo(AppDbContext context)
		{
			_context = context;
		}

		public bool SaveChanges()
		{
			return (_context.SaveChanges() > 0);
		}

		public bool ExternalUserExists(int externalPlatformId)
		{
			return _context.Users.Any(p => p.ExternalId == externalPlatformId);
		}


		public void UpdateUser(User user)
		{
			_context.Users.Update(user);
		}

		public User GetUser(int id)
		{
			return _context.Users.Find(id);
		}
	}
}