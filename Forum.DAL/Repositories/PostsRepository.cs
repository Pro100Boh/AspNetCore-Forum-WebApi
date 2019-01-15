using Forum.DAL.EF;
using Forum.DAL.Entities;
using Forum.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forum.DAL.Repositories
{
    internal class PostsRepository : IRepository<Post>
    {
        private ForumContext db;

        public PostsRepository(ForumContext context)
        {
            this.db = context;
        }

        public IQueryable<Post> GetAll()
        {
            return db.Posts;
        }

        public Post Get(int id)
        {
            return db.Posts.Find(id);
        }

        public void Create(Post post)
        {
            db.Posts.Add(post);
        }

        public void Update(Post post)
        {
            db.Entry(post).State = EntityState.Deleted;

            db.Posts.Update(post);
        }

        public IEnumerable<Post> Find(Func<Post, bool> predicate)
        {
            return db.Posts.Where(predicate);
        }

        public void Delete(int id)
        {
            var post = Get(id);

            db.Posts.Remove(post);
        }
    }
}
