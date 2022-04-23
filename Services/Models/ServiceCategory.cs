using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
	public class ServiceCategory
	{
		[Key]
		[Required]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }
	}
}