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
        public Reserv Get(int id)
        {
            return db.Reserves.Find(id);
        }
        public IEnumerable<Reserv> GetAll()
        {
            return db.Reserves;
        }
        public Reserv Find(Func<Reserv, bool> predicate)
        {
            return db.Reserves.FirstOrDefault(predicate);
        }
        public IEnumerable<Reserv> FindAll(Func<Reserv, bool> predicate)
        {
            return db.Reserves.Where(predicate);
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
        public void Update(Reserv item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}