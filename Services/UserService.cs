using DataAccess;
using DataAccess.Models;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
	public class UserService
	{
		private RepoDbContext dbContext;

		public UserService(RepoDbContext _dbContext)
		{
			this.dbContext = _dbContext;
		}

		public List<User> GetUsers() => this.dbContext.Users.ToList();

		public User GetUser(string username) => this.dbContext.Users.First(x => x.Username == username);

		public void Login(string username, string password)
		{
			if (this.dbContext.Users.Any(x => x.Username == username))
			{
				if (this.dbContext.Users.FirstOrDefault(x => x.Username == username).Password == password)
				{
					Logged.User = this.dbContext.Users.FirstOrDefault(x => x.Username == username);
				}
				else
				{
					throw new ArgumentException("Incorrect username or password");
				}
			}
			else
			{
				throw new ArgumentException("Incorrect username or password");
			}
		}

		public void Create(string username, string password)
		{
			if(this.dbContext.Users.Any(x => x.Username != username))
			{
				this.dbContext.Add(new User
				{
					Username = username,
					Password = password
				});
				this.dbContext.SaveChanges();
			}
			else
			{
				throw new ArgumentException("A user with the same username already exists");
			}
		}

		public void Delete(string username)
		{
			User user = this.GetUser(username);
			this.dbContext.Users.Remove(user);
			this.dbContext.SaveChanges();
		}

		public void ChangePassword(string username, string newPassword)
		{
			User user = this.GetUser(username);
			user.Password = newPassword;

			this.dbContext.SaveChanges();
		}
	}
}
