using Microsoft.EntityFrameworkCore;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DBContext
{
    public class CosmosDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }


        public CosmosDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("WidgetStore"); // sets the default container

            modelBuilder.Entity<Order>()
                .ToContainer(nameof(Orders))

                // EF Core adds a discriminator value to identify the entity type that a given item represent
                .HasNoDiscriminator() // HasNoDiscriminator() removes the discriminator since no other entity type will be stored in this container
                .HasPartitionKey(d => d.PartitionKey)
                .UseETagConcurrency();


            modelBuilder.Entity<Product>()
               .ToContainer(nameof(Products))
               .HasNoDiscriminator() 
               .HasPartitionKey(d => d.PartitionKey)
               .UseETagConcurrency();

            modelBuilder.Entity<User>()
               .ToContainer(nameof(Users))
               .HasNoDiscriminator()
               .HasPartitionKey(d => d.PartitionKey)
               .UseETagConcurrency();

            modelBuilder.Entity<ProductReview>()
               .ToContainer(nameof(ProductReviews))
               .HasNoDiscriminator()
               .HasPartitionKey(d => d.PartitionKey)
               .UseETagConcurrency();


        }
    }
}
