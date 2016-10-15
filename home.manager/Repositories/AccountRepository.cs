using System.Data.Entity;
using System.Linq;
using home.manager.Models;
using home.manager.Shared;

namespace home.manager.Repositories
{
    public interface IAccountRepository : IRepository<UserProfile>
    {
        UserProfile FindByName(string name);
        void DeleteById(int id);
    }

    public class AccountRepository : EntityFrameworkRepository<UserProfile>, IAccountRepository
    {
        public AccountRepository(DbContext context) : base(context) { }

        public UserProfile FindByName(string name)
        {
            var account = GetAll().Where(x => x.UserName == name);
            return account.Any() ? account.First() : null;
        }

        public void DeleteById(int id)
        {
            var account = GetAll().FirstOrDefault(x => x.UserId == id);
            if (account == null) return;

            Delete(account);
            Context.SaveChanges();
        }
    }
}