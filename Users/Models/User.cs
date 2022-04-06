using System.ComponentModel.DataAnnotations;

namespace Users.Models
{
	public class User
	{
		[Key]
		[Required]
		public int Id { get; set; }

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
