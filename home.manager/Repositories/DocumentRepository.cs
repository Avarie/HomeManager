using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using home.manager.Models.db;
using home.manager.Shared;

namespace home.manager.Repositories
{
    public class DocumentRepository : EntityFrameworkRepository<Document>, ISharedRepository<Document, SharedCategory>
    {
        private readonly IAccountRepository _accountRepository;
        public DocumentRepository(DbContext context, IAccountRepository accountRepository)
            : base(context)
        {
            _accountRepository = accountRepository;
        }

        public IQueryable<Document> GetItems(int userId, int categoryId, string search)
        {
            var all = GetAll();
            if (userId != 0) all = all.Where(x => x.Owner.UserId == userId);

            if (!string.IsNullOrEmpty(search)) all = all
                .Where(x => x.FileName.Contains(search) || x.Description.Contains(search));

            return all.OrderByDescending(x => x.CreatedTime);
        }

        public async Task DeleteItem(Document item)
        {
            if (item == null || item.Id == 0) return;

            var doc = await GetAll().FirstOrDefaultAsync(x => x.Id == item.Id);
            if (doc != null)
            {
                Delete(doc);
                Context.SaveChanges();
            }
        }

        // only update here for document
        public async Task UpdateItem(Document item, string username)
        {
            var user = _accountRepository.FindByName(username);
            if (user == null) return;

            var document = await GetAll().FirstOrDefaultAsync(x => x.Id == item.Id);
            if (document == null) return;

            document.Owner = user;
            document.Description = item.Description;
            document.CreatedTime = DateTime.Now;
            Update(document);
        }

        // As designed: No categories here
        public IQueryable<SharedCategory> GetCategories(bool withManageData = false)
        {
            throw new NotImplementedException();
        }

        public void UpdateCategory(SharedCategory category)
        {
            throw new NotImplementedException();
        }

        public void DeleteCategory(SharedCategory category)
        {
            throw new NotImplementedException();
        }
    }

}