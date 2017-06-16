using OnlineLibrary.DAL.EF;
using OnlineLibrary.DAL.Entities;
using OnlineLibrary.DAL.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System;

namespace OnlineLibrary.DAL.Repositories
{
    public class GenreRepository : IRepository<Genre>
    {
        private LibraryContext db;
        public GenreRepository(LibraryContext context)
        {
            db = context;
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

        public IEnumerable<Genre> FindByAuthor(string itemName)
        {
            return null;
        }

        public Genre FindByName(string itemName)
        {
            return null;
        }

        public Genre Get(int? id)
        {
            return db.Genres.Find(id);
        }

        public IEnumerable<Genre> GetAll()
        {
            return db.Genres;
        }

        public void Update(Genre item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public IEnumerable<Genre> FindByGenre(string genre)
        {
            return null;
        }

        public IEnumerable<Genre> FindByPublisher(string itemName)
        {
            return null;
        }
    }
}
