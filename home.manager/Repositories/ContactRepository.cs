using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using home.manager.Models.db;
using home.manager.Shared;

namespace home.manager.Repositories
{
    public class ContactRepository : EntityFrameworkRepository<Contact>, ISharedRepository<Contact, ContactCategory>
    {
        private readonly ISharedCategoryRepository<ContactCategory> _categoryRepository;
        private readonly IAccountRepository _accountRepository;

        public ContactRepository(DbContext context, 
            ISharedCategoryRepository<ContactCategory> categoryRepository, 
            IAccountRepository accountRepository)
            : base(context)
        {
            _categoryRepository = categoryRepository;
            _accountRepository = accountRepository;
        }

        public IQueryable<Contact> GetItems(int userId, int categoryId, string search)
        {
            var all = GetAll();
            if (userId != 0) all = all.Where(x => x.Owner.UserId == userId);
            if (categoryId != 0) all = all.Where(x => x.Category.Id == categoryId);
            if (!String.IsNullOrEmpty(search)) all = all
                .Where(x => x.Name.Contains(search) ||
                    x.Description.Contains(search) ||
                    x.ContactLines.Any(z => z.Description.Contains(search) || z.Name.Contains(search))
                );

            return all.OrderByDescending(x => x.Date);
        }

        public IQueryable<ContactCategory> GetCategories(bool withManageData = false)
        {
            var list = _categoryRepository.GetAll().OrderBy(x => x.Name);
            if (!withManageData) return list;

            foreach (var l in list)
            {
                l.IsEmpty = !GetItems(0, l.Id, string.Empty).Any();
            }

            return list;
        }

        public async Task DeleteItem(Contact item)
        {
            if (item == null || item.Id == 0) return;

            var contact = await GetAll().FirstOrDefaultAsync(x => x.Id == item.Id);
            if (contact != null)
            {
                Delete(contact);
            }

            Context.SaveChanges();
        }

        public async Task UpdateItem(Contact item, string username)
        {
            var user = _accountRepository.FindByName(username);
            if (user == null) return;
            
            item.Owner = user;

            var dbSetContactLine = Context.Set<ContactLine>();
            var addedEntities = new List<string>();

            var category = await _categoryRepository.GetAll().FirstOrDefaultAsync(x => x.Id == item.Category.Id);
            if (category == null) return;

            item.Category = category;
            if (item.ContactLines == null) item.ContactLines = new Collection<ContactLine>();


            if (GetAll().Select(x => x.Id).ToList().Contains(item.Id))
            {
                var parent = GetAll().FirstOrDefault(x => x.Id == item.Id);
                if (parent == null) return; // we don't have a parent entity to update

                parent.Owner = item.Owner;

                foreach (var child in parent.ContactLines.ToList())
                {
                    var detachedChild = item.ContactLines
                        .FirstOrDefault(x => x.Id == child.Id);
                    if (detachedChild != null) continue;

                    parent.ContactLines.Remove(child);
                    dbSetContactLine.Remove(child);
                }

                foreach (var line in item.ContactLines)
                {
                    var attached = parent.ContactLines.FirstOrDefault(x => x.Id == line.Id);

                    if (attached != null)
                    {
                        Context.Entry(attached).CurrentValues.SetValues(line);
                    }
                    else
                    {
                        var cLine = line.Clone();
                        addedEntities.Add(cLine.Id);
                        parent.ContactLines.Add(cLine);
                        dbSetContactLine.Add(cLine);
                    }
                }

                parent.Name = item.Name;
                parent.Description = item.Description;

                Context.SaveChanges();
            }
            else
            {
                Add(item);
            }
        }

        public void UpdateCategory(ContactCategory category)
        {
            _categoryRepository.AddOrUpdate(category);
        }

        public void DeleteCategory(ContactCategory category)
        {
            if (category == null || category.Id == 0) return;
            _categoryRepository.DeleteById(category.Id);
        }
    }

    public class ContactCategoryRepository : EntityFrameworkRepository<ContactCategory>, ISharedCategoryRepository<ContactCategory>
    {
        public ContactCategoryRepository(DbContext context) : base(context) { }
        public void AddOrUpdate(ContactCategory category)
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
        }
    }
}
