using System.Linq;

namespace home.manager.Shared
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}