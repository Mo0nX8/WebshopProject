using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework
{
    public class GlobalDbContext : DbContext
    {
        public DbSet<UserData> Users { get; set; }
        public DbSet<Products> StorageData {  get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<ShoppingCart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=UserDatabase");
            }
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserData>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<ShoppingCart>(c => c.UserId);
            modelBuilder.Entity<Orders>()
                .HasOne(u => u.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);
            modelBuilder.Entity<CartItem>()
                .HasKey(ci=>new {ci.CartId, ci.ProductId});
            modelBuilder.Entity<CartItem>()
                .HasOne(ci=>ci.Cart)
                .WithMany(ci=>ci.CartItems)
                .HasForeignKey(ci=>ci.CartId);
            modelBuilder.Entity<CartItem>()
                .HasOne(ci=>ci.Product)
                .WithMany(p=>p.CartItems)
                .HasForeignKey(ci=>ci.ProductId);
                
        }
    }
}
