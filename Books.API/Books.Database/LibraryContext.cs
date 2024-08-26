using Books.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Database
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book>? Books { get; set; }
        public DbSet<Author>? Authors { get; set; }
        public DbSet<Publisher>? Publisher { get; set; }

        private readonly string connectionString = "Data Source=LAPTOP-GAETV9LK; Initial Catalog=LibraryBD; Integrated Security=true; TrustServerCertificate=true";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString).UseLazyLoadingProxies();  
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Publisher>().HasMany(x => x.Books)
                .WithOne(y => y.Publisher);
        }
    }
}
