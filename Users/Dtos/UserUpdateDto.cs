using System.ComponentModel.DataAnnotations;

namespace Users.Dtos
{
	public class UserUpdateDto
	{
		[Required]
		public string? Name { get; set; }

		[Required]
		[EmailAddress]
		public string? Email { get; set; }

		[Phone]
		public string? Phone { get; set; }

		public string? Address { get; set; }
	}
}