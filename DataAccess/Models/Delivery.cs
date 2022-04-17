using System;

namespace DataAccess.Models
{
	public class Delivery : Base
	{
		public string Item { get; set; }

		public double Quantity { get; set; }

		public decimal PricePer { get; set; }

		public DateTime Date { get; set; }

		public Unit Unit { get; set; }
		public int UnitId { get; set; }

		public Deliverer Deliverer { get; set; }
		public int DelivererId { get; set; }
	}
}
