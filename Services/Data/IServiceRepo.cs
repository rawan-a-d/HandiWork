using Services.Models;

namespace Services.Data
{
	public interface IServiceRepo
	{
		bool SaveChanges();

		IEnumerable<Service> GetAllServices();

		Service GetServiceById(int id);

		void CreateService(int serviceCategoryId, Service service);
	}
}