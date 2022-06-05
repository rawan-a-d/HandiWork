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
					new User() { ExternalId = 1, Name = "Admin", Email = "admin@admin.com", Phone = "081234566", Address = "7653 OT 123" },
					new User() { ExternalId = 2, Name = "Rawan", Email = "rawan@gmail.com", Phone = "087667283", Address = "4783 GW 138" },
					new User() { ExternalId = 3, Name = "Omar", Email = "omar@gmail.com", Phone = "087667283", Address = "4783 GW 138" },
					new User() { ExternalId = 4, Name = "Ranim", Email = "ranim@gmail.com", Phone = "087667283", Address = "4783 GW 138" }
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