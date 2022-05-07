using Users.Models;
using Microsoft.EntityFrameworkCore;

namespace Users.Data
{
	public static class PrepDb
	{
		public static void PrepPopulation(IApplicationBuilder app, bool isProd)
		{
			using (var serviceScope = app.ApplicationServices.CreateScope())
			{
				SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
			}
		}

		private static void SeedData(AppDbContext context, bool isProd)
		{
			// run migration if we are in production
			if (isProd)
			{
				Console.WriteLine("--> Attempting to apply migrations...");

				try
				{
					context.Database.Migrate();
				}
				catch (Exception ex)
				{
					Console.WriteLine($"--> Could not run migrations: {ex.Message}");
				}
			}

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