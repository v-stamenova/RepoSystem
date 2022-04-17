using System;
using System.ComponentModel.DataAnnotations;

namespace RepoSystemWeb.Models
{
	public class CreateDeliveryViewModel
	{
		[Required(ErrorMessage = "Името на стоката не може да е празно")]
		public string Item { get; set; }

		public bool NewMeasurementUnit { get; set; }
		public string MeasurementUnitSelect { get; set; }
		public string MeasurementUnitText { get; set; }

		[Required(ErrorMessage = "Количеството на стоката е задължително")]
		public double Quantity { get; set; }

		[Required(ErrorMessage = "Единичната цена на стоката е задължителна")]
		public decimal PricePer { get; set; }

		public bool NewDeliverer { get; set; }
		public string DelivererSelect { get; set; }
		public string DelivererText { get; set; }

		public DateTime Date { get; set; }
	}
}
