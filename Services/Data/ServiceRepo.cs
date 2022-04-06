using Services.Models;

namespace Services.Data
{
	public class ServiceRepo : IServiceRepo
	{
		private readonly AppDbContext _context;
		public ServiceRepo(AppDbContext context)
		{
			_context = context;
		}

		public void CreateService(int serviceCategoryId, Service service)
		{
			if (service == null)
			{
				throw new ArgumentNullException(nameof(service));
			}
			service.ServiceCategoryId = serviceCategoryId;

			_context.Services?.Add(service);
		}

		public IEnumerable<Service> GetAllServices()
		{
			throw new NotImplementedException();
		}

		public Service GetServiceById(int id)
		{
			throw new NotImplementedException();
		}

		public bool SaveChanges()
		{
			return (_context.SaveChanges() >= 0);
		}
	}
}