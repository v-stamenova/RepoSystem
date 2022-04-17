using System.Collections.Generic;

namespace DataAccess.Models
{
	public class Unit : Base
	{
		public Unit()
		{
			this.Deliveries = new List<Delivery>();
		}

		public string Name { get; set; }

		public ICollection<Delivery> Deliveries { get; set; }
	}
}
