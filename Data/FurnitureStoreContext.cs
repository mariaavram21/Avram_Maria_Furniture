using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Avram_Maria_Furniture.Models;

namespace Avram_Maria_Furniture.Data
{
    public class FurnitureStoreContext : DbContext
    {
 public FurnitureStoreContext(DbContextOptions<FurnitureStoreContext> options) :
base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Furniture> Furnitures { get; set; }
        public DbSet<Factory> Factories { get; set; }
        public DbSet<ManufacturedFurniture> ManufacturedFurnitures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Furniture>().ToTable("Furniture");
            modelBuilder.Entity<Factory>().ToTable("Factory");
            modelBuilder.Entity<ManufacturedFurniture>().ToTable("Manufacture");
            modelBuilder.Entity<ManufacturedFurniture>().HasKey(c => new { c.FactoryID, c.FurnitureID });
        }
    }
}

