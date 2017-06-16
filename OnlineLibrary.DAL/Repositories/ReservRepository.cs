using OnlineLibrary.DAL.EF;
using OnlineLibrary.DAL.Entities;
using OnlineLibrary.DAL.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;

namespace OnlineLibrary.DAL.Repositories
{
    public class ReservRepository : IRepository<Reserv>
    {
        private LibraryContext db;

        public ReservRepository(LibraryContext context)
        {
            db = context;
        }

        public void Create(Reserv item)
        {
            db.Reserves.Add(item);
        }

        public void Delete(int id)
        {
            Reserv reserv = db.Reserves.Find(id);
            if (reserv != null)
                db.Reserves.Remove(reserv);
        }
        public IEnumerable<Reserv> FindByAuthor(string userName)
        {
            return db.Reserves.Where(r => r.UserName == userName);
        }

        public IEnumerable<Reserv> FindByGenre(string genre)
        {
            return null;
        }

        public Reserv FindByName(string bookName)
        {
            return db.Reserves.FirstOrDefault(r => r.BookName == bookName);
        }
        public Reserv Get(int? id)
        {
            return db.Reserves.Find(id);
        }
        public IEnumerable<Reserv> GetAll()
        {
            return db.Reserves;
        }
        public void Update(Reserv item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public IEnumerable<Reserv> FindByPublisher(string publisher)
        {
            return null;
        }
    }
}
