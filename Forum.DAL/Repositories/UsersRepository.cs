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
    internal class UsersRepository : IRepository<User>
    {
        private ForumContext db;

        public UsersRepository(ForumContext context)
        {
            this.db = context;
        }

        public IQueryable<User> GetAll()
        {
            return db.Users;
        }

        public User Get(int id)
        {
            return db.Users.Find(id);
        }

        public void Create(User user)
        {
            db.Users.Add(user);
        }

        public void Update(User user)
        {
            //db.Entry(user).State = EntityState.Deleted;

            db.Users.Update(user);
        }

        public IEnumerable<User> Find(Func<User, bool> predicate)
        {
            return db.Users.Where(predicate);
        }

        public void Delete(int id)
        {
            var user = Get(id);

            db.Users.Remove(user);

        }
    }
}
