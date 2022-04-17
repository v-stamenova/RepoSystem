using DataAccess;
using DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
	public class DelivererService
	{
		private RepoDbContext dbContext;

		public DelivererService(RepoDbContext _dbContext)
		{
			this.dbContext = _dbContext;
		}

		public List<Deliverer> GetDeliverers() => this.dbContext.Deliverers.ToList();

		public Deliverer GetDeliverer(string name) => this.dbContext.Deliverers.First(x => x.Name == name);

		public Deliverer AddDeliverer(string name)
		{
			Deliverer deliverer = new Deliverer
			{
				Name = name
			};

			this.dbContext.Deliverers.Add(deliverer);
			this.dbContext.SaveChanges();

			return deliverer;
		}
	}
}
