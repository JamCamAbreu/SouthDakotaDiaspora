using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class SqlBookData : IBookData
    {
        private readonly SouthDakotaDiasporaDbContext database;
        public SqlBookData(SouthDakotaDiasporaDbContext db)
        {
            database = db;
        }

        public void Add(Book book)
        {
            database.Activities.Add(book);
            database.SaveChanges();
        }

        public Book Get(int id)
        {
            return database.Books.FirstOrDefault(g => g.ActivityId == id);
        }

        public IEnumerable<Book> GetAll()
        {
            return database.Books.OrderBy(g => g.Name);
        }

        public void Update(Book book)
        {
            Book existing = this.Get(book.ActivityId);
            if (existing != null)
            {
                existing.Name = book.Name;
                existing.Description = book.Description;
                existing.WebsiteUrl = book.WebsiteUrl;
                existing.Author = book.Author;
                existing.NumberPages = book.NumberPages;
                existing.PublishDate = book.PublishDate;
                existing.Platform = book.Platform;
                database.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            Book book = this.Get(id);
            if (book != null)
            {
                database.Activities.Remove(book);
                database.SaveChanges();
            }
        }
    }
}
