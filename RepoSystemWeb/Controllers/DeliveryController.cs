using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using RepoSystemWeb.Models;
using RepoSystemWeb.Models.SearchModel;
using Services;
using Services.Helpers;
using System;
using System.Linq;

namespace RepoSystemWeb.Controllers
{
	public class DeliveryController : Controller
	{
		private DelivererService delivererService;
		private UnitService unitService;
		private DeliveryService deliveryService;

		public DeliveryController(DelivererService _delivererService, UnitService _unitService, DeliveryService _deliveryService)
		{
			this.delivererService = _delivererService;
			this.unitService = _unitService;
			this.deliveryService = _deliveryService;
		}

		#region IndexAndSearch

		[HttpGet]
		public IActionResult Index()
		{
			if (Logged.IsLogged())
			{
				DeliverySearch search = new DeliverySearch();
				search.Date = DateTime.Now;
				search.Result = this.deliveryService.GetDeliveries();
				return View(search);
			}

			return Unauthorized();
		}

		[HttpGet]
		public IActionResult Search(DeliverySearch search)
		{
			search.Result = this.deliveryService.GetDeliveries();
			if (search.Item is not null)
			{
				search.Result = search.Result.Where(x => x.Item.ToLower().Contains(search.Item.ToLower())).ToList();
			}
			if (search.Deliverer != "Доставчик...")
			{
				search.Result = search.Result.Where(x => x.Deliverer.Name.ToLower().Contains(search.Deliverer.ToLower())).ToList();
			}
			if (search.Date.HasValue)
			{
				search.Result = search.Result.Where(x => x.Date <= search.Date).ToList();
			}

			return View("Index", search);
		}

		#endregion

		#region Create

		[HttpGet]
		public IActionResult Create()
		{
			if (Logged.IsLogged())
			{
				CreateDeliveryViewModel model = new CreateDeliveryViewModel
				{
					Date = DateTime.Now
				};

				return View(model);
			}

			return Unauthorized();
		}

		[HttpPost]
		public IActionResult Create(CreateDeliveryViewModel model)
		{
			if (ModelState.IsValid)
			{
				Delivery newDelivery = new Delivery();

				if (!string.IsNullOrWhiteSpace(model.DelivererSelect) || !string.IsNullOrWhiteSpace(model.DelivererText))
				{
					if (model.NewDeliverer)
					{
						if (!string.IsNullOrWhiteSpace(model.DelivererText))
						{
							if (delivererService.GetDeliverers().Any(x => x.Name == model.DelivererText))
							{
								newDelivery.Deliverer = this.delivererService.GetDeliverer(model.DelivererSelect);
							}
							else
							{
								newDelivery.Deliverer = this.delivererService.AddDeliverer(model.DelivererText);
							}
						}
						else
						{
							ModelState.AddModelError("deError", "Няма въведен нов доставчик");
						}
					}
					else
					{
						newDelivery.Deliverer = this.delivererService.GetDeliverer(model.DelivererSelect);
					}
				}
				else
				{
					ModelState.AddModelError("delivererError", "Няма въведен доставчик");
				}

				if (!string.IsNullOrWhiteSpace(model.MeasurementUnitSelect) || !string.IsNullOrWhiteSpace(model.MeasurementUnitText))
				{
					if (model.NewMeasurementUnit)
					{
						if (!string.IsNullOrWhiteSpace(model.MeasurementUnitText))
						{
							if (this.unitService.GetUnits().Any(x => x.Name == model.MeasurementUnitText))
							{
								newDelivery.Unit = this.unitService.GetUnit(model.MeasurementUnitSelect);
							}
							else
							{
								newDelivery.Unit = this.unitService.AddUnit(model.MeasurementUnitText);
							}
						}
						else
						{
							ModelState.AddModelError("mesError", "Няма въведенa нова мерна единица");
						}
					}
					else
					{
						newDelivery.Unit = this.unitService.GetUnit(model.MeasurementUnitSelect);
					}
				}
				else
				{
					ModelState.AddModelError("unitError", "Няма въведена мерна единица");
				}

				if (model.Quantity <= 0)
				{
					ModelState.AddModelError("quantityError", "Количеството на стоката не може да е по-малко или равно на 0");
				}

				if (model.PricePer <= 0)
				{
					ModelState.AddModelError("priceError", "Единичната цена не може да е по-малка или равна на 0");
				}

				if (ModelState.ErrorCount > 0)
				{
					return View(model);
				}

				newDelivery.Item = model.Item;
				newDelivery.Date = model.Date;
				newDelivery.Quantity = model.Quantity;
				newDelivery.PricePer = model.PricePer;

				this.deliveryService.AddDelivery(newDelivery);

				return RedirectToAction("Index", "Home");
			}

			return View(model);
		}

		#endregion

		#region Delete

		[HttpGet]
		[Route("Delivery/Delete/{id}")]
		public IActionResult Delete([FromRoute]string id)
		{
			if (Logged.IsLogged())
			{
				this.deliveryService.DeleteDelivery(id);

				return RedirectToAction("Index", "Delivery");
			}

			return Unauthorized();
		}

		#endregion

		#region Details

		[HttpGet]
		[Route("Delivery/Details/{id}")]
		public IActionResult Details([FromRoute] string id)
		{
			if (Logged.IsLogged())
			{
				Delivery delivery = this.deliveryService.GetDelivery(id);

				return View(delivery);
			}

			return Unauthorized();
		}

		#endregion

		#region Edit

		[HttpGet]
		[Route("Delivery/Edit/{id}")]
		public IActionResult Edit([FromRoute] string id)
		{
			if (Logged.IsLogged())
			{
				if(this.deliveryService.GetDelivery(id) is not null)
				{
					Delivery delivery = this.deliveryService.GetDelivery(id);

					CreateDeliveryViewModel model = new CreateDeliveryViewModel
					{
						Item = delivery.Item,
						Quantity = delivery.Quantity,
						PricePer = delivery.PricePer,
						Date = delivery.Date,
						DelivererSelect = delivery.Deliverer.Name,
						MeasurementUnitSelect = delivery.Unit.Name,
						NewDeliverer = false,
						NewMeasurementUnit = false
					};

					return View(model);
				}
			}

			return Unauthorized();
		}

		[HttpPost]
		[Route("Delivery/Edit/{id}")]
		public IActionResult Edit(CreateDeliveryViewModel model, [FromRoute] string id)
		{
			if (ModelState.IsValid)
			{
				Delivery newDelivery = new Delivery();

				if (!string.IsNullOrWhiteSpace(model.DelivererSelect) || !string.IsNullOrWhiteSpace(model.DelivererText))
				{
					if (model.NewDeliverer)
					{
						if (!string.IsNullOrWhiteSpace(model.DelivererText))
						{
							if (delivererService.GetDeliverers().Any(x => x.Name == model.DelivererText))
							{
								newDelivery.Deliverer = this.delivererService.GetDeliverer(model.DelivererSelect);
							}
							else
							{
								newDelivery.Deliverer = this.delivererService.AddDeliverer(model.DelivererText);
							}
						}
						else
						{
							ModelState.AddModelError("deError", "Няма въведен нов доставчик");
						}
					}
					else
					{
						newDelivery.Deliverer = this.delivererService.GetDeliverer(model.DelivererSelect);
					}
				}
				else
				{
					ModelState.AddModelError("delivererError", "Няма въведен доставчик");
				}

				if (!string.IsNullOrWhiteSpace(model.MeasurementUnitSelect) || !string.IsNullOrWhiteSpace(model.MeasurementUnitText))
				{
					if (model.NewMeasurementUnit)
					{
						if (!string.IsNullOrWhiteSpace(model.MeasurementUnitText))
						{
							if (this.unitService.GetUnits().Any(x => x.Name == model.MeasurementUnitText))
							{
								newDelivery.Unit = this.unitService.GetUnit(model.MeasurementUnitSelect);
							}
							else
							{
								newDelivery.Unit = this.unitService.AddUnit(model.MeasurementUnitText);
							}
						}
						else
						{
							ModelState.AddModelError("mesError", "Няма въведенa нова мерна единица");
						}
					}
					else
					{
						newDelivery.Unit = this.unitService.GetUnit(model.MeasurementUnitSelect);
					}
				}
				else
				{
					ModelState.AddModelError("unitError", "Няма въведена мерна единица");
				}

				if (model.Quantity <= 0)
				{
					ModelState.AddModelError("quantityError", "Количеството на стоката не може да е по-малко или равно на 0");
				}

				if (model.PricePer <= 0)
				{
					ModelState.AddModelError("priceError", "Единичната цена не може да е по-малка или равна на 0");
				}

				if (ModelState.ErrorCount > 0)
				{
					return View(model);
				}

				newDelivery.Item = model.Item;
				newDelivery.Date = model.Date;
				newDelivery.Quantity = model.Quantity;
				newDelivery.PricePer = model.PricePer;

				this.deliveryService.UpdateDelivery(id, newDelivery);

				return RedirectToAction("Index", "Home");
			}

			return View(model);
		}

		#endregion
	}
}
