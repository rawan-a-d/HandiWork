using Users.Models;
using Microsoft.EntityFrameworkCore;

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
					new User() { Name = "admin", Email = "admin@admin.com", Phone = "087667283", Address = "4543 YH 2", ExternalId = 1 },
					new User() { Name = "Rawan", Email = "rawan@gmail.com", Phone = "087667283", Address = "3496 WJ 100", ExternalId = 2 },
					new User() { Name = "Omar", Email = "omar@gmail.com", Phone = "087667283", Address = "4783 GW 138", ExternalId = 3 },
					new User() { Name = "Ranim", Email = "ranim@gmail.com", Phone = "087667283", Address = "2395 WQ 45", ExternalId = 4 }
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