using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using home.manager.Models.db;
using home.manager.Shared;

namespace home.manager.Repositories
{
    public class NoteRepository : EntityFrameworkRepository<Note>, ISharedRepository<Note, NoteCategory>
    {
        private readonly ISharedCategoryRepository<NoteCategory> _categoryRepository;
        private readonly IAccountRepository _accountRepository;

        public NoteRepository(DbContext context, ISharedCategoryRepository<NoteCategory> categoryRepository, IAccountRepository accountRepository)
            : base(context)
        {
            _categoryRepository = categoryRepository;
            _accountRepository = accountRepository;
        }

        public IQueryable<Note> GetItems(int userId, int categoryId, string search)
        {
            var all = GetAll();
            if (userId != 0) all = all.Where(x => x.Owner.UserId == userId);
            if (categoryId != 0) all = all.Where(x => x.Category.Id == categoryId);
            if (!String.IsNullOrEmpty(search)) all = all
                .Where(x => x.Name.Contains(search) ||
                    x.Description.Contains(search) );

            return all.OrderByDescending(x => x.Date);
        }

        public IQueryable<NoteCategory> GetCategories(bool withManageData = false)
        {
            var list = _categoryRepository.GetAll().OrderBy(x => x.Name);
            if (!withManageData) return list;

            foreach (var l in list)
            {
                l.IsEmpty = !GetItems(0, l.Id, string.Empty).Any();
            }

            return list;
        }

        public async Task DeleteItem(Note item)
        {
            if (item == null || item.Id == 0) return;

            var note = await GetAll().FirstOrDefaultAsync(x => x.Id == item.Id);
            if (note != null)
            {
                Delete(note);
            }

            Context.SaveChanges();
        }

        public async Task UpdateItem(Note item, string username)
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

        public void UpdateCategory(NoteCategory category)
        {
            _categoryRepository.AddOrUpdate(category);
        }

        public void DeleteCategory(NoteCategory category)
        {
            if (category == null || category.Id == 0) return;
            _categoryRepository.DeleteById(category.Id);
        }
    }

    public class NoteCategoryRepository : EntityFrameworkRepository<NoteCategory>, ISharedCategoryRepository<NoteCategory>
    {
        public NoteCategoryRepository(DbContext context) : base(context) { }
        public void AddOrUpdate(NoteCategory category)
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
