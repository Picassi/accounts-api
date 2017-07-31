using System.Data.Entity;
using System.Linq;
using Picassi.Core.Accounts.DAL;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Generator.Accounts
{
    public interface IDatabaseCleaner
    {
        void Clean();
    }

    public class DatabaseCleaner : IDatabaseCleaner
    {
        private readonly IAccountsDatabaseProvider _databaseProvider;

        public DatabaseCleaner(IAccountsDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public void Clean()
        {
            var context = _databaseProvider.GetDataContext();
            DropCollection(context.Accounts);
            context.SaveChanges();
            DropCollection(context.Categories);
            context.SaveChanges();
            DropCollection(context.Transactions);
            context.SaveChanges();
        }

        private static void DropCollection<T>(DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }
    }
}
