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
    internal class CommentsRepository : IRepository<Comment>
    {
        private ForumContext db;

        public CommentsRepository(ForumContext context)
        {
            this.db = context;
        }

        public IQueryable<Comment> GetAll()
        {
            return db.Comments;
        }

        public Comment Get(int id)
        {
            return db.Comments.Find(id);
        }

        public void Create(Comment comment)
        {
            db.Comments.Add(comment);
        }

        public void Update(Comment comment)
        {
            db.Entry(comment).State = EntityState.Deleted;

            db.Comments.Update(comment);
        }

        public IEnumerable<Comment> Find(Func<Comment, bool> predicate)
        {
            return db.Comments.Where(predicate);
        }

        public void Delete(int id)
        {
            var comment = Get(id);

            db.Comments.Remove(comment);
        }
    }
}
