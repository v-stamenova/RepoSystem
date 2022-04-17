using DataAccess;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
	public class DeliveryService
	{
		private RepoDbContext dbContext;

		public DeliveryService(RepoDbContext _dbContext)
		{
			this.dbContext = _dbContext;
		}

		public Delivery GetDelivery(string id) => this.GetDeliveries().FirstOrDefault(x => x.Id == int.Parse(id));

		public List<Delivery> GetDeliveries() => this.dbContext.Deliveries.Include(x => x.Deliverer).Include(x => x.Unit).ToList();

		public void AddDelivery(Delivery delivery)
		{
			this.dbContext.Add(delivery);
			this.dbContext.SaveChanges();
		}

		public void DeleteDelivery(string id)
		{
			Delivery delivery = this.GetDelivery(id);
			this.dbContext.Deliveries.Remove(delivery);
			this.dbContext.SaveChanges();
		}

		public void UpdateDelivery(string idForUpdate, Delivery newDelivery)
		{
			Delivery deliveryForUpdate = this.GetDelivery(idForUpdate);

			deliveryForUpdate.Date = newDelivery.Date;
			deliveryForUpdate.Deliverer = newDelivery.Deliverer;
			deliveryForUpdate.Item = newDelivery.Item;
			deliveryForUpdate.PricePer = newDelivery.PricePer;
			deliveryForUpdate.Quantity = newDelivery.Quantity;
			deliveryForUpdate.Unit = newDelivery.Unit;

			this.dbContext.SaveChanges();
		}
	}
}
