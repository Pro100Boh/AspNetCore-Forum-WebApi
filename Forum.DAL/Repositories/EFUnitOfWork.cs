using Forum.DAL.EF;
using Forum.DAL.Entities;
using Forum.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.DAL.Repositories
{
    internal class EFUnitOfWork : IUnitOfWork
    {
        private ForumContext db;

        private PostsRepository postsRepository;

        private UsersRepository usersRepository;

        private CommentsRepository commentsRepository;

        public EFUnitOfWork()
        {
            db = new ForumContext();
        }

        public IRepository<Post> Posts
        {
            get
            {
                if (postsRepository == null)
                {
                    postsRepository = new PostsRepository(db);
                }
                return postsRepository;
            }
        }

        public IRepository<Comment> Comments
        {
            get
            {
                if (commentsRepository == null)
                {
                    commentsRepository = new CommentsRepository(db);
                }
                return commentsRepository;
            }
        }

        public IRepository<User> Users
        {
            get
            {
                if (usersRepository == null)
                {
                    usersRepository = new UsersRepository(db);
                }
                return usersRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
