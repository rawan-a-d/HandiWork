using Microsoft.EntityFrameworkCore;
using Services.Models;

namespace Services.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
		{

		}

		public DbSet<Service>? Services { get; set; }
		public DbSet<ServiceCategory>? ServicesCategories { get; set; }
		public DbSet<User>? Users { get; set; }
	}
}