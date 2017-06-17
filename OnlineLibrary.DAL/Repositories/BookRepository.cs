using OnlineLibrary.DAL.EF;
using OnlineLibrary.DAL.Entities;
using OnlineLibrary.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OnlineLibrary.DAL.Repositories
{
    public class BookRepository : IRepository<Book>
    {
        private LibraryContext db;
        public BookRepository(LibraryContext context)
        {
            this.db = context;
        }
        public Book Get(int id)
        {
            return db.Books.Find(id);
        }
        public IEnumerable<Book> GetAll()
        {
            return db.Books;
        }
        public Book Find(Func<Book, bool> predicate)
        {
            return db.Books.FirstOrDefault(predicate);
        }
        public IEnumerable<Book> FindAll(Func<Book, bool> predicate)
        {
            return db.Books.Where(predicate);
        }
        public void Create(Book item)
        {
            db.Books.Add(item);
        }
        public void Delete(int id)
        {
            Book book = db.Books.Find(id);
            if (book != null)
                db.Books.Remove(book);
        }
        public void Update(Book item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
