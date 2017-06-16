using System;
using System.Collections.Generic;

namespace OnlineLibrary.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int? id);
        IEnumerable<T> FindByAuthor(string itemName);
        IEnumerable<T> FindByPublisher(string itemName);
        T FindByName(string itemName);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        IEnumerable<T> FindByGenre(string genre);
    }
}
