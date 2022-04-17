using System.ComponentModel.DataAnnotations;

namespace RepoSystemWeb.Models
{
	public class CreateUserViewModel
	{
		[Required(ErrorMessage = "Потребителското име е задължително")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Паролата е задължителна")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Паролата за потвърждение не може да е празна")]
		public string ConfirmPassword { get; set; }
	}
}
