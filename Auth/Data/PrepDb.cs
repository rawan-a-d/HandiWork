using Auth.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data
{
	public static class PrepDb
	{
		public static void PrepPopulation(IApplicationBuilder app, bool isDevelopment)
		{

			using (var serviceScope = app.ApplicationServices.CreateScope())
			{
				SeedData(
					serviceScope.ServiceProvider.GetService<UserManager<IdentityUser>>(),
					serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>(),
					serviceScope.ServiceProvider.GetService<AppDbContext>(),
					isDevelopment
				);
			}
		}

		private async static void SeedData(
			UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> roleManager,
			AppDbContext context,
			bool isDevelopment)
		{
			if (isDevelopment)
			{
				// if no data
				if (!context.Users.Any())
				{
					Console.WriteLine("--> Seeding data...");

					await roleManager.CreateAsync(new IdentityRole("Moderator"));
					await roleManager.CreateAsync(new IdentityRole("AppUser"));

					var user1 = new UserCreateDto { Name = "Rawan", Email = "rawan@gmail.com", Password = "Pa$$w0rd" };
					var user1Identity = new IdentityUser { Email = user1.Email, UserName = user1.Email };

					await userManager.CreateAsync(user1Identity, "Pa$$w0rd");

					await userManager.AddToRoleAsync(user1Identity, "Moderator");
				}
				else
				{
					Console.WriteLine("--> We already have data");
				}
			}
		}
	}
}