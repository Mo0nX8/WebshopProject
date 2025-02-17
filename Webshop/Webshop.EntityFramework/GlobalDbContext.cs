using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework
{
    /// <summary>
    /// This class is the DBContext of the solution. This class define and control the database.
    /// </summary>
    public class GlobalDbContext : DbContext
    {
        /// <summary>
        /// User table
        /// </summary>
        public DbSet<UserData> Users { get; set; }
        /// <summary>
        /// Product table
        /// </summary>
        public DbSet<Products> StorageData {  get; set; }
        /// <summary>
        /// Order table
        /// </summary>
        public DbSet<Orders> Orders { get; set; }
        /// <summary>
        /// Cart table
        /// </summary>
        public DbSet<ShoppingCart> Carts { get; set; }
        /// <summary>
        /// Items of a cart. This is a help table
        /// </summary>
        public DbSet<CartItem> CartItems { get; set; }
        /// <summary>
        /// Items of an order. This is a help table.
        /// </summary>
        public DbSet<OrderItem> OrderItems { get; set; }

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
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);  
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

        }
    }
}
