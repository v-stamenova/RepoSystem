using DataAccess.Models;

namespace Services.Helpers
{
	public static class Logged
	{
		public static User User { get; set; }

		public static bool IsAdmin()
		{
			if(IsLogged())
			{
				if (User.Username == "Admin")
				{
					return true;
				}
			}

			return false;
		}

		public static bool IsLogged()
		{
			if (Logged.User is not null)
			{
				return true;
			}

			return false;
		}
	}
}
