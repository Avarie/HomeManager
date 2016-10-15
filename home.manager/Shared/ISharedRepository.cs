using System.Linq;
using System.Threading.Tasks;

namespace home.manager.Shared
{
    public interface ISharedRepository<T, TC>
    {
        IQueryable<T> GetItems(int userId, int categoryId, string search);
        IQueryable<TC> GetCategories(bool withManageData = false);
        Task DeleteItem(T item);
        Task UpdateItem(T item, string username);
        void UpdateCategory(TC category);
        void DeleteCategory(TC category);
    }

    public interface ISharedCategoryRepository<T> : IRepository<T> where T : class
    {
        void DeleteById(int id);
        void AddOrUpdate(T category);
    }
}