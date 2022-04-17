using DataAccess.Models;
using System;
using System.Collections.Generic;

namespace RepoSystemWeb.Models.SearchModel
{
	public class DeliverySearch
	{
		public string Item { get; set; }

		public string Deliverer { get; set; }

		public DateTime? Date { get; set; }

		public List<Delivery> Result { get; set; }
	}
}
