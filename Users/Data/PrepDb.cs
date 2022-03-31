using Users.Models;

namespace Users.Data
{
	public static class PrepDb
	{
		public static void PrepPopulation(IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices.CreateScope())
			{
				SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
			}
		}

		private static void SeedData(AppDbContext context)
		{
			// if no data
			if (!context.Users.Any())
			{
				Console.WriteLine("--> Seeding data...");

				context.Users.AddRange(
					new User() { Name = "Rawan", Email = "rawan@gmail.com", Password = "1234" },
					new User() { Name = "Omar", Email = "omar@gmail.com", Password = "1234" },
					new User() { Name = "Ranim", Email = "ranim@gmail.com", Password = "1234" }
				);

				context.SaveChanges();
			}
			else
			{
				Console.WriteLine("--> We already have data");
			}
		}
	}
}