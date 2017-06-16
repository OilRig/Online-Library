using OnlineLibrary.DAL.Entities;
using OnlineLibrary.DAL.Identity;
using System;
using System.Threading.Tasks;

namespace OnlineLibrary.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        LibraryUserManager UserManager { get; }
        IClientManager ClientManager { get; }
        LibraryRoleManager RoleManager { get; }
        IRepository<Book> BookManager { get; }
        IRepository<Reserv> ReserveManager { get; }
        IRepository<Genre> GenreManager { get; }
        Task SaveAsync();
    }
}
