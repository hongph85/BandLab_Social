using BandLab_Social.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BandLab_Social.Infrastructure
{
    public class CosmosDBContext : DbContext
    {
        public string endPoint;
        public string dataBaseId;
        public string token;

        public CosmosDBContext(string endPoint, string dataBaseId, string token) : base()
        {
            this.endPoint = endPoint;
            this.dataBaseId = dataBaseId;
            this.token = token;

            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .ToContainer("Posts")
                .HasPartitionKey(x => x.PostId).OwnsMany(x => x.RecentComments);

            modelBuilder.Entity<Comment>()
                .ToContainer("Posts")
                .HasPartitionKey(x => x.PostId).OwnsMany(x => x.Comments);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(
                endPoint,
                 token, 
                 databaseName: dataBaseId);
        }
    }
}
