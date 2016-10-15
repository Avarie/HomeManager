using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using home.manager.Models.db;
using home.manager.Shared;

namespace home.manager.Repositories
{
    public class SecurityRepository : EntityFrameworkRepository<SecurityItem>, ISharedRepository<SecurityItem, SecurityCategory>
    {
        private readonly ISharedCategoryRepository<SecurityCategory> _categoryRepository;
        private readonly IAccountRepository _accountRepository;

        public SecurityRepository(DbContext context, ISharedCategoryRepository<SecurityCategory> categoryRepository, IAccountRepository accountRepository)
            : base(context)
        {
            _categoryRepository = categoryRepository;
            _accountRepository = accountRepository;
        }

        public IQueryable<SecurityItem> GetItems(int userId, int categoryId, string search)
        {
            var all = GetAll();
            if (userId != 0) all = all.Where(x => x.Owner.UserId == userId);
            if (categoryId != 0) all = all.Where(x => x.Category.Id == categoryId);
            if (!String.IsNullOrEmpty(search))
            {
                all = all
                .Where(x => x.Category.Name.Contains(search) ||
                    x.Category.Description.Contains(search) ||
                    x.Link.Contains(search) ||
                    x.Name.Contains(search) ||
                    x.Description.Contains(search)
                );
            }

            return all.OrderByDescending(x => x.Date);
        }

        public IQueryable<SecurityCategory> GetCategories(bool withManageData = false)
        {
            var list = _categoryRepository.GetAll().OrderBy(x => x.Name);
            if (!withManageData) return list;

            foreach (var l in list)
            {
                l.IsEmpty = !GetItems(0, l.Id, string.Empty).Any();
            }

            return list;
        }

        public async Task DeleteItem(SecurityItem item)
        {
            if (item == null || item.Id == 0) return;

            var note = await GetAll().FirstOrDefaultAsync(x => x.Id == item.Id);
            if (note != null)
            {
                Delete(note);
            }

            Context.SaveChanges();
        }

        public async Task UpdateItem(SecurityItem item, string username)
        {
            var user = _accountRepository.FindByName(username);
            if (user == null) return;

            item.Owner = user;
            item.Date = DateTime.Now;

            if (item.Category != null)
            {
                item.Category = _categoryRepository.GetAll().First(x => x.Id == item.Category.Id);
            }

            if (GetAll().Select(x => x.Id).ToList().Contains(item.Id))
            {
                Update(item);
            }
            else
            {
                Add(item);
            }
        }

        public void UpdateCategory(SecurityCategory category)
        {
            _categoryRepository.AddOrUpdate(category);
        }

        public void DeleteCategory(SecurityCategory category)
        {
            if (category == null || category.Id == 0) return;
            _categoryRepository.DeleteById(category.Id);
        }
    }
    public class SecurityCategoryRepository : EntityFrameworkRepository<SecurityCategory>, ISharedCategoryRepository<SecurityCategory>
    {
        public SecurityCategoryRepository(DbContext context) : base(context) { }
        public void AddOrUpdate(SecurityCategory category)
        {
            if (GetAll().Any(x => x.Id == category.Id))
            {
                Update(category);
            }
            else
            {
                Add(category);
            }
        }
        public void DeleteById(int id)
        {
            var category = GetAll().FirstOrDefault(x => x.Id == id);
            Delete(category);
            Context.SaveChanges();
        }
    }
}

