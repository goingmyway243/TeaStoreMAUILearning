using GoingMyTeaStore.Models;
using SQLite;

namespace GoingMyTeaStore.Services
{
    public class BookmarkItemService
    {
        private readonly SQLiteConnection _database;
        public BookmarkItemService()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "entities.db");
            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<BookmarkedProduct>();
        }

        public BookmarkedProduct GetById(int id)
        {
            return _database.Table<BookmarkedProduct>().FirstOrDefault(x => x.Id == id);
        }

        public List<BookmarkedProduct> GetAll()
        {
            return _database.Table<BookmarkedProduct>().ToList();
        }

        public void Insert(BookmarkedProduct bookmarkedProduct)
        {
            _database.Insert(bookmarkedProduct);
        }

        public void Delete(BookmarkedProduct bookmarkedProduct)
        {
            _database.Delete(bookmarkedProduct);
        }
    }
}
