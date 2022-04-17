using DataAccess;
using DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
	public class UnitService
	{
		private RepoDbContext dbContext;

		public UnitService(RepoDbContext _dbContext)
		{
			this.dbContext = _dbContext;
		}

		public List<Unit> GetUnits() => this.dbContext.Units.ToList();

		public Unit GetUnit(string name) => this.dbContext.Units.First(x => x.Name == name);

		public Unit AddUnit(string name)
		{
			Unit unit = new Unit
			{
				Name = name
			};

			this.dbContext.Units.Add(unit);
			this.dbContext.SaveChanges();

			return unit;
		}
	}
}
