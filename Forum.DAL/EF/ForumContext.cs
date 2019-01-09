using Forum.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.DAL.EF
{
    internal class ForumContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public DbSet<User> Users { get; set; }

        public ForumContext()
        {
            Database.Migrate();
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ForumDBv1;Trusted_Connection=True;");
        }

        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().HasData(
                new Post[]
                {
                new Post { PostId = 1, Header = "Header1", Content = "Content1", Published = DateTime.Now },
                new Post { Header = "Header2", Content = "Content2", Published = DateTime.Now },
                new Post { PostId = 1, Header = "Header3", Content = "Content3", Published = DateTime.Now }
                });
            base.OnModelCreating(modelBuilder);
        }
        */
    }
}
