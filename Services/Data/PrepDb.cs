using Services.Data;
using Services.Models;

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
			if (!context.Users.Any() && !context.ServicesCategories.Any() && !context.Services.Any())
			{
				Console.WriteLine("--> Seeding data...");

				context.Users.AddRange(
					new User() { Name = "Rawan", ExternalId = 1 },
					new User() { Name = "Omar", ExternalId = 2 },
					new User() { Name = "Ranim", ExternalId = 3 }
				);

				context.ServicesCategories.AddRange(
					new ServiceCategory() { Name = "Painting" },
					new ServiceCategory() { Name = "Install Laminaat" },
					new ServiceCategory() { Name = "Plumbing" }
				);

				context.Services.AddRange(
					new Service() { Info = "I paint....", UserId = 1, ServiceCategoryId = 1 },
					new Service() { Info = "I install laminaat", UserId = 1, ServiceCategoryId = 2 },
					new Service() { Info = "I am a plumber", UserId = 2, ServiceCategoryId = 3 }
				);

				context.SaveChanges();

				//Console.WriteLine($"--> User: {context.Users.FirstOrDefault().Id}");
				//Console.WriteLine($"--> Service category: {context.ServicesCategories.FirstOrDefault().Name}");
				//Console.WriteLine($"--> Service: {context.Services.FirstOrDefault().ServiceCategory.Name}");
			}
			else
			{
				Console.WriteLine("--> We already have data");
			}
		}
	}
}