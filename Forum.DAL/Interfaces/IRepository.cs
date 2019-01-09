using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forum.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        T Get(int id);

        IEnumerable<T> Find(Func<T, bool> predicate);

        void Create(T item);

        void Update(T item);

        void Delete(int id);
    }
}
