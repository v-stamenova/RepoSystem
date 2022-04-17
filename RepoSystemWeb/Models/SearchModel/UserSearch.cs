using DataAccess.Models;
using System.Collections.Generic;

namespace RepoSystemWeb.Models.SearchModel
{
	public class UserSearch
	{
		public string Username { get; set; }

		public List<User> Results { get; set; }

	}
}
