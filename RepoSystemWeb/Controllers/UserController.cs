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
	public class UserController : Controller
	{
		private UserService userService;

		public UserController(UserService _userService)
		{
			this.userService = _userService;
		}

		#region IndexAndSearch
		[HttpGet]
		public IActionResult Index()
		{
			if (Logged.IsLogged())
			{
				UserSearch search = new UserSearch();
				search.Results = this.userService.GetUsers();
				return View(search);
			}

			return Unauthorized();
		}

		[HttpGet]
		public IActionResult Search(UserSearch search)
		{
			if (Logged.IsLogged())
			{
				search.Results = this.userService.GetUsers();
				if (search.Username is not null)
				{
					search.Results = search.Results.Where(x => x.Username.ToLower().Contains(search.Username.ToLower())).ToList();
				}

				return View("Index", search);
			}

			return Unauthorized();
		}
		#endregion

		#region Create
		[HttpGet]
		public IActionResult Create()
		{
			if (Logged.IsAdmin())
			{
				CreateUserViewModel model = new CreateUserViewModel();

				return View(model);
			}

			return Unauthorized();
		}

		[HttpPost]
		public IActionResult Create(CreateUserViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					string username = model.Username;
					string password = model.Password;
					string confirmedPassword = model.ConfirmPassword;
					
					if (password != confirmedPassword)
					{
						ModelState.AddModelError("loginError", "Паролите не съвпадат");
						return View(model);
					}
					else
					{
						this.userService.Create(username, password);
					}
				}
				catch (Exception e)
				{
					ModelState.AddModelError(e.GetType().ToString(), e.Message);
					return View(model);
				}
			}

			return RedirectToAction("Index");
		}
		#endregion

		#region Login
		[HttpGet]
		public IActionResult Login()
		{
			LoginViewModel model = new LoginViewModel();

			return View(model);
		}

		[HttpPost]
		public IActionResult Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					string username = model.Username;
					string password = model.Password;
					this.userService.Login(username, password);
				}
				catch (Exception)
				{
					ModelState.AddModelError("loginError", "Невалидно потребителско име или парола");
					return View(model);
				}
			}
			else
			{
				return View(model);
			}

			return RedirectToAction("Index", "Home");
		}
		#endregion

		#region Delete

		[HttpGet]
		[Route("User/Delete/{username}")]
		public IActionResult Delete([FromRoute] string username)
		{
			if (Logged.IsAdmin())
			{
				if(this.userService.GetUsers().Any(x => x.Username == username))
				{
					this.userService.Delete(username);

					return RedirectToAction("Index");
				}

				return NotFound();
			}

			return Unauthorized();
		}

		#endregion

		#region Edit

		[HttpGet]
		[Route("User/Edit/{username}")]
		public IActionResult Edit([FromRoute] string username)
		{
			if (Logged.IsAdmin())
			{
				if (this.userService.GetUsers().Any(x => x.Username == username))
				{
					EditUserViewModel model = new EditUserViewModel
					{
						Username = username
					};

					return View(model);
				}

				return NotFound();
			}

			return Unauthorized();
		}

		[HttpPost]
		public IActionResult Edit(EditUserViewModel model)
		{
			if (Logged.IsAdmin() || Logged.User.Username == model.Username)
			{
				if (ModelState.IsValid)
				{
					try
					{
						string username = model.Username;
						string password = model.Password;
						string confirmedPassword = model.ConfirmPassword;

						if(Logged.User.Username == model.Username)
						{
							if(Logged.User.Password == password)
							{
								this.userService.ChangePassword(Logged.User.Username, confirmedPassword);
								return RedirectToAction("Index");
							}
							else
							{
								ModelState.AddModelError("loginError", "Паролата на потребителя не съвпада");
								return View(model);
							}
						}
						else
						{
							if (password != confirmedPassword)
							{
								ModelState.AddModelError("loginError", "Паролите не съвпадат");
								return View(model);
							}
							else
							{
								this.userService.ChangePassword(username, password);
								return RedirectToAction("Index");
							}
						}
					}
					catch (Exception e)
					{
						ModelState.AddModelError(e.GetType().ToString(), e.Message);
						return View(model);
					}
				}

				return RedirectToAction("Index");
			}

			return Unauthorized();
		}
		
		#endregion

		[HttpGet]
		[Route("User/ChangePassword/{username}")]
		public IActionResult ChangePassword([FromRoute] string username)
		{
			if(Logged.User.Username == username)
			{
				EditUserViewModel model = new EditUserViewModel
				{
					Username = username
				};

				return View("Edit", model);
			}

			return Unauthorized();
		}
	}
}
