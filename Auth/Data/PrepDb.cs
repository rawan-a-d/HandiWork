using Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data
{
	public static class PrepDb
	{
		public async static void PrepPopulation(IApplicationBuilder app, bool isDevelopment)
		{
			using (var serviceScope = app.ApplicationServices.CreateScope())
			{
				await SeedData(
					serviceScope.ServiceProvider.GetService<UserManager<User>>(),
					serviceScope.ServiceProvider.GetService<RoleManager<Role>>()
				);
			}
		}
		private static async Task SeedData(UserManager<User> userManager, RoleManager<Role> roleManager)
		{
			// check if users table contains any users
			if (await userManager.Users.AnyAsync())
			{
				return;
			}

			// roles
			var roles = new List<Role> {
				new Role("User"),
				new Role("Admin"),
				new Role("Moderator")
			};
			// create roles
			foreach (var role in roles)
			{
				await roleManager.CreateAsync(role);
			}

			// Create an admin
			var admin = new User
			{
				Email = "admin@admin.com",
				UserName = "admin"
			};
			await userManager.CreateAsync(admin, "Pa$$w0rd");
			await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
		}
	}
}