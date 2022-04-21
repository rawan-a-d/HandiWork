using Services.Models;

namespace Services.Dtos
{
	public class ServiceReadDto
	{
		public int Id { get; set; }

		public string Info { get; set; }

		public int UserId { get; set; }

		public int ServiceCategoryId { get; set; }

		//// navigation property
		//public ServiceCategory ServiceCategory { get; set; }
		//public User User { get; set; }
	}
}