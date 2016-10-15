using System.Threading.Tasks;
using System.Web.Mvc;
using home.manager.Helpers;
using StructureMap;

namespace home.manager.Shared
{

    [Authorize]
    public abstract class SharedController<T, TC, TRepo> : Controller
        where T : class
        where TRepo : ISharedRepository<T, TC>
    {
        private readonly ISharedRepository<T, TC> _repository;

        protected SharedController()
        {
            _repository = ObjectFactory.GetInstance<TRepo>();
        }

        public virtual JsonNetResult GetItems(int id, int category, string search)
        {
            return new JsonNetResult(_repository.GetItems(id, category, search));
        }

        public virtual JsonNetResult GetCategories(bool withManageData = false)
        {
            return new JsonNetResult(_repository.GetCategories(withManageData));
        }

        public virtual async Task Delete(T item)
        {
            await _repository.DeleteItem(item);
        }

        public virtual async Task Update(T item)
        {
            await _repository.UpdateItem(item, User.Identity.Name);
        }

        public virtual void UpdateCategory(TC category)
        {
            _repository.UpdateCategory(category);
        }

        public virtual void DeleteCategory(TC category)
        {
            _repository.DeleteCategory(category);
        }
    }
}