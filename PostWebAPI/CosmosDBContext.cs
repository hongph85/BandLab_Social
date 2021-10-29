using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostWebAPI
{
    public class CosmosDBContext : DbContext
    {
        public CosmosDBContext() : base()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .ToContainer("Posts")
                .HasPartitionKey(x => x.PostId)
                .HasDiscriminator<string>(nameof(Post.Type));

            modelBuilder.Entity<Comment>()
                .ToContainer("Comments")
                .HasPartitionKey(x => x.PostId)
                .HasDiscriminator<string>(nameof(Comment.Type));
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(
                "https://localhost:8081",
                 "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", 
                 databaseName: "BandlabDB");
        }
    }
}
