using OnlineLibrary.DAL.EF;
using OnlineLibrary.DAL.Entities;
using OnlineLibrary.DAL.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System;
using System.Linq;

namespace OnlineLibrary.DAL.Repositories
{
    public class GenreRepository : IRepository<Genre>
    {
        private LibraryContext db;
        public GenreRepository(LibraryContext context)
        {
            db = context;
        }
        public Genre Get(int id)
        {
            return db.Genres.Find(id);
        }
        public IEnumerable<Genre> GetAll()
        {
            return db.Genres;
        }
        public Genre Find(Func<Genre, bool> predicate)
        {
            return db.Genres.FirstOrDefault(predicate);
        }
        public IEnumerable<Genre> FindAll(Func<Genre, bool> predicate)
        {
            return db.Genres.Where(predicate);
        }
        public void Create(Genre item)
        {
            db.Genres.Add(item);
        }
        public void Delete(int id)
        {
            Genre genre = db.Genres.Find(id);
            if (genre != null)
                db.Genres.Remove(genre);
        }
        public void Update(Genre item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
