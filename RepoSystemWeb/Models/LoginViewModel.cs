using System.ComponentModel.DataAnnotations;

namespace RepoSystemWeb.Models
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Потребителското име е задължително")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Паролата е задължителна")]
		public string Password { get; set; }
	}
}
