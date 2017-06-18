using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using OnlineLibrary.DAL.EF;
using OnlineLibrary.DAL.Entities;
using OnlineLibrary.DAL.Identity;
using OnlineLibrary.DAL.Interfaces;
using System;
using System.Net.Mail;
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
            var provider = new DpapiDataProtectionProvider("OnlineLibrary");
            userManager.UserTokenProvider = new DataProtectorTokenProvider<LibraryUser>(provider.Create("EmailConfirmation"));
            userManager.EmailService = new IdentityEmailSender();
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
    public class IdentityEmailSender : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var from = "onlinelibrary17@mail.ru";
            var pass = "password123";

            SmtpClient client = new SmtpClient("smtp.mail.ru", 587);

            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(from, pass);
            client.EnableSsl = true;

            var mail = new MailMessage(from, message.Destination);
            mail.Subject = message.Subject;
            mail.Body = message.Body;
            mail.IsBodyHtml = true;

            return client.SendMailAsync(mail);
        }
    }
}
