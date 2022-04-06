using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
	public class User
	{
		[Key]
		[Required]
		public int Id { get; set; }

		[Required]
		public int ExternalId { get; set; }

		[Required]
		public string? Name { get; set; }
	}
}