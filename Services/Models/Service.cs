using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
	public class Service
	{
		[Key]
		[Required]
		public int Id { get; set; }

		[Required]
		public string? Info { get; set; }

		[Required]
		public int UserId { get; set; }

		[Required]
		public int ServiceCategoryId { get; set; }

		// navigation property
		public ServiceCategory? ServiceCategory { get; set; }
		public User? User { get; set; }
	}
}