using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
	public class RepoDbContext : DbContext
	{
		public RepoDbContext(DbContextOptions<RepoDbContext> options)
			: base(options)
		{
		}

		public DbSet<Deliverer> Deliverers { get; set; }

		public DbSet<Delivery> Deliveries { get; set; }

		public DbSet<Unit> Units { get; set; }

		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Deliverer>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.Property(e => e.Name)
					.IsRequired();
			});

			modelBuilder.Entity<Unit>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.Property(e => e.Name)
					.IsRequired();
			});

			modelBuilder.Entity<Delivery>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.Property(e => e.Item)
					.IsRequired();

				entity.Property(e => e.PricePer)
					.IsRequired();

				entity.Property(e => e.Quantity)
					.IsRequired();

				entity.HasOne(e => e.Unit)
					.WithMany(u => u.Deliveries)
					.HasForeignKey(e => e.UnitId);

				entity.HasOne(e => e.Deliverer)
					.WithMany(d => d.Deliveries)
					.HasForeignKey(e => e.DelivererId);
			});

			modelBuilder.Entity<User>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.Property(e => e.Username)
					.IsRequired();

				entity.Property(e => e.Password)
					.IsRequired();
			});

			modelBuilder.Entity<User>().HasData(
				new User()
				{
					Id = 1,
					Username = "Admin",
					Password = "@dm1nP@$$w0rd"
				}
			);
		}

	}
}