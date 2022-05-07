using Users.Models;
using Microsoft.EntityFrameworkCore;

namespace Users.Data
{
	public static class PrepDb
	{
		public static void PrepPopulation(IApplicationBuilder app, bool isDevelopment)
		{
			using (var serviceScope = app.ApplicationServices.CreateScope())
			{
				SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isDevelopment);
			}
		}

		private static void SeedData(AppDbContext context, bool isDevelopment)
		{
			// run migration if we are in production
			if (isDevelopment)
			{
				// if no data
				if (!context.Users.Any())
				{
					Console.WriteLine("--> Seeding data...");

					context.Users.AddRange(
						new User() { Name = "Rawan", Email = "rawan@gmail.com", Phone = "087667283", Address = "4783 GW 138" },
						new User() { Name = "Omar", Email = "omar@gmail.com", Phone = "087667283", Address = "4783 GW 138" },
						new User() { Name = "Ranim", Email = "ranim@gmail.com", Phone = "087667283", Address = "4783 GW 138" }
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
}