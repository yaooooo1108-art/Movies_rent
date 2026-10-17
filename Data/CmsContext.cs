using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using _1131754.Models;

namespace _1131754.Data
{
    public class CmsContext : DbContext
    {
        public CmsContext(DbContextOptions<CmsContext> options) : base(options)
        {

        }

        public DbSet<User> TableUsers1131754 { get; set; }

        public DbSet<Movie> TableMovies1131754 { get; set; }
        public DbSet<Order> TableOrders1131754 { get; set; }

        public DbSet<OrderDetail> TableOrderDetails1131754 { get; set; }
        public DbSet<Cart> TableCartItems1131754 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}