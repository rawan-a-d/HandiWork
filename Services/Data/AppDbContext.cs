using Microsoft.EntityFrameworkCore;
using Services.Models;

namespace Services.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
		{

		}

		public DbSet<Service> Services { get; set; }
		public DbSet<ServiceCategory> ServicesCategories { get; set; }
		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// User
			modelBuilder
				.Entity<User>()
				.HasMany(u => u.Services)
				.WithOne(s => s.User)
				.HasForeignKey(s => s.UserId);

			// Service
			modelBuilder
				.Entity<Service>()
				.HasOne(s => s.User)
				.WithMany(u => u.Services)
				.HasForeignKey(s => s.UserId);

			//modelBuilder
			//	.Entity<Service>()
			//	.HasOne(s => s.ServiceCategory)
			//	.WithMany(u => u.Services)
			//	.HasForeignKey(s => s.ServiceCategoryId);
		}
	}
}