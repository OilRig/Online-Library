using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineLibrary.DAL.Entities;
using System.Data.Entity;

namespace OnlineLibrary.DAL.EF
{
    public class LibraryContext : IdentityDbContext<LibraryUser>
    {
        public LibraryContext()
        {
        }
        public LibraryContext(string connectionString) : base (connectionString)
        {}

        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Reserv> Reserves { get; set; }
        public DbSet<Genre> Genres { get; set; }
    }
}
