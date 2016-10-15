using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using home.manager.Models.db;
using home.manager.Shared;

namespace home.manager.Repositories
{
    public class ExpenseRepository : EntityFrameworkRepository<Expense>, ISharedRepository<Expense, Category>
    {
        private readonly ISharedCategoryRepository<Category> _categoryRepository;
        private readonly ISharedCategoryRepository<SubCategory> _subCategoryRepository;
        private readonly IAccountRepository _accountRepository;

        public ExpenseRepository(DbContext context, 
            ISharedCategoryRepository<Category> categoryRepository, 
            ISharedCategoryRepository<SubCategory> subCategoryRepository,
            IAccountRepository accountRepository)
            : base(context)
        {
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _accountRepository = accountRepository;
        }

        public IQueryable<Expense> GetItems(int userId, int categoryId, string search)
        {
            var all = GetAll();
            if (userId != 0) all = all.Where(x => x.Owner.UserId == userId);
            if (categoryId != 0) all = all.Where(x => x.Category.Id == categoryId);
            if (!String.IsNullOrEmpty(search)) all = all
                .Where(x => x.SubCategory.Name.Contains(search) || x.Description.Contains(search));

            return all.OrderByDescending(x => x.Date).Take(200);
        }

        public async Task DeleteItem(Expense item)
        {
            if (item == null || item.Id == 0) return;

            var entity = await GetAll().FirstOrDefaultAsync(x => x.Id == item.Id);
            if (entity != null)
            {
                Delete(entity);
                Context.SaveChanges();
            }
        }

        public async Task UpdateItem(Expense item, string username)
        {
            var user = _accountRepository.FindByName(username);
            if (user == null) return;

            item.Owner = user;
            item.Date = DateTime.Now;

            if (item.Category == null) return;

            item.Category = await _categoryRepository.GetAll().FirstAsync(x => x.Id == item.Category.Id);
            if (GetAll().Select(x => x.Id).ToList().Contains(item.Id))
            {
                item.SubCategory = GetSubCategory(item.Category.Id, item.SubCategory.Name);
                Update(item);
            }
            else
            {
                Add(item);
            }
        }

        private SubCategory GetSubCategory(int categoryId, string subCategoryName)
        {
            var subs = _subCategoryRepository.GetAll()
                .Where(x => x.Category.Id == categoryId && x.Name == subCategoryName);

            if (!subs.Any())
            {
                _subCategoryRepository.Add(new SubCategory
                {
                    Name = subCategoryName,
                    Description = String.Empty,
                    Category = _categoryRepository.GetAll().First(x => x.Id == categoryId),
                });
            }

            return (subs.Any())
                ? subs.First()
                : _subCategoryRepository.GetAll().First(x => x.Category.Id == categoryId && x.Name == subCategoryName);
        }

        public IQueryable<Category> GetCategories(bool withManageData = false)
        {
            return _categoryRepository.GetAll().OrderBy(x => x.Name);
        }

        public void UpdateCategory(Category category)
        {
            _categoryRepository.AddOrUpdate(category);
        }

        public void DeleteCategory(Category category)
        {
            if (category == null || category.Id == 0) return;
            _categoryRepository.DeleteById(category.Id);
        }
    }

    public class ExpenseCategoryRepository : EntityFrameworkRepository<Category>, ISharedCategoryRepository<Category>
    {
        public ExpenseCategoryRepository(DbContext context) : base(context) { }
        public void AddOrUpdate(Category category)
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

    public class ExpenseSubCategoryRepository : EntityFrameworkRepository<SubCategory>, ISharedCategoryRepository<SubCategory>
    {
        public ExpenseSubCategoryRepository(DbContext context) : base(context) { }
        public void AddOrUpdate(SubCategory category)
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
//            DbSet.Attach(entity);
//            base.Delete(entity);

            var category = GetAll().FirstOrDefault(x => x.Id == id);
            Delete(category);
            Context.SaveChanges();
        }
    }
}
