using Microsoft.AspNet.Identity.EntityFramework;
using OnlineLibrary.DAL.EF;
using OnlineLibrary.DAL.Entities;
using OnlineLibrary.DAL.Identity;
using OnlineLibrary.DAL.Interfaces;
using System;
using System.Threading.Tasks;

namespace OnlineLibrary.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private LibraryContext db;
        private LibraryUserManager userManager;
        private LibraryRoleManager roleManager;
        private IClientManager clientManager;
        private BookRepository bookRepository;
        private ReservRepository reservRepository;
        private GenreRepository genreRepository;
        public UnitOfWork(string connectionString)
        {
            db = new LibraryContext(connectionString);
            userManager = new LibraryUserManager(new UserStore<LibraryUser>(db));
            roleManager = new LibraryRoleManager(new RoleStore<LibraryRole>(db));
            clientManager = new ClientManager(db);
        }
        public IRepository<Genre> GenreManager
        {
            get
            {
                if (genreRepository == null)
                    genreRepository = new GenreRepository(db);
                return genreRepository;
            }
        }
        public LibraryUserManager UserManager
        {
            get
            {
                return userManager;
            }
        }
        public IClientManager ClientManager
        {
            get
            {

                if (clientManager == null)
                    clientManager = new ClientManager(db);
                return clientManager;
            }
        }

        public LibraryRoleManager RoleManager
        {
            get
            {
                return roleManager;
            }
        }

        public IRepository<Book> BookManager
        {
            get
            {
                if (bookRepository == null)
                    bookRepository = new BookRepository(db);
                return bookRepository;
            }
        }
        public IRepository<Reserv> ReserveManager
        {
            get
            {
                if (reservRepository == null)
                    reservRepository = new ReservRepository(db);
                return reservRepository;
            }
        }
        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    userManager.Dispose();
                    roleManager.Dispose();
                    clientManager.Dispose();
                    db.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}
