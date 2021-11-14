using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteRESTAPI.Entity;

namespace WebsiteRESTAPI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            // ...
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ImagesVendues>(
                    eb =>
                    {
                        eb.HasNoKey();
                        
                    });
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Coeff> Coeffs { get; set; }
        public DbSet<ImagesVendues> ImagesVendues { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Reportage> Reportages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AuthTokens> AuthTokens { get; set; }
    }
}
