using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using GMCBookApp.Models;

namespace GMCBookApp.Data
{
    public class BookDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public BookDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Book>().Wait();
        }

        public Task<List<Book>> GetBooksAsync()
        {
            return _database.Table<Book>().ToListAsync();
        }

        public Task<Book> GetBookAsync(int id)
        {
            return _database.Table<Book>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveBookAsync(Book book)
        {
            if (book.ID != 0)
            {
                return _database.UpdateAsync(book);
            }
            else
            {
                return _database.InsertAsync(book);
            }
        }

        public Task<int> DeleteBookAsync(Book book)
        {
            return _database.DeleteAsync(book);
        }
    }
}
