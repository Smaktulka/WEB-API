using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using fridge.Models;
using Entities.Models;
using Entities.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Entities.DataTransferObjects;

namespace Entities
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new FridgesConfiguration());
            modelBuilder.ApplyConfiguration(new FridgeModelConfiguration());
            modelBuilder.ApplyConfiguration(new FridgeProductsConfiguration());
            modelBuilder.ApplyConfiguration(new ProductsConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }

        public DbSet<Fridges> Fridges { get; set; } = null!;
        public DbSet<FridgeModels> FridgeModels { get; set; } = null!;
        public DbSet<FridgeProducts> FridgeProducts { get; set; } = null!;
        public DbSet<Products> Products { get; set; } = null!;

    }
}
