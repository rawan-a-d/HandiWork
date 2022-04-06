using System.ComponentModel.DataAnnotations;

namespace Services.Dtos
{
	public class ServiceCreateDto
	{
		[Required]
		public string? Info { get; set; }

		[Required]
		public int UserId { get; set; }

		[Required]
		public int ServiceCategoryId { get; set; }
	}
}