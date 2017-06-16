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
        public void Create(Book item)
        {
            db.Books.Add(item);
        }
        public IEnumerable<Book> FindByAuthor(string AuthorName)
        {
            return db.Books.Where(a => a.Author.Contains(AuthorName)).ToList();
        }
        public IEnumerable<Book> FindByPublisher(string publisher)
        {
            return db.Books.Where(a => a.Publisher.Contains(publisher)).ToList();
        }
        public Book FindByName(string BookName)
        {
            return db.Books.FirstOrDefault(b => b.Name == BookName);
        }
        public void Delete(int id)
        {
            Book book = db.Books.Find(id);
            if (book != null)
                db.Books.Remove(book);
        }
        public Book Get(int? id)
        {
            return db.Books.Find(id);
        }

        public IEnumerable<Book> GetAll()
        {
            return db.Books;
        }
        public void Update(Book item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<Book> FindByGenre(string genre)
        {
            return db.Books.Where(b => b.Genre == genre);
        }
    }
}
