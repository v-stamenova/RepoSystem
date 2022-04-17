using System.Collections.Generic;

namespace DataAccess.Models
{
	public class Deliverer : Base
	{
		public Deliverer()
		{
			this.Deliveries = new List<Delivery>();
		}

		public string Name { get; set; }

		public ICollection<Delivery> Deliveries { get; set; }
	}
}
