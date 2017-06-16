using OnlineLibrary.DAL.EF;
using OnlineLibrary.DAL.Entities;
using OnlineLibrary.DAL.Interfaces;

namespace OnlineLibrary.DAL.Repositories
{
    public class ClientManager : IClientManager
    {
        public LibraryContext Database { get; set; }
        public ClientManager()
        {

        }
        public ClientManager(LibraryContext db)
        {
            Database = db;
        }
        public void Create(ClientProfile item)
        {
            Database.ClientProfiles.Add(item);
            Database.SaveChanges();
        }
        public ClientProfile FindById(string id)
        {
            return Database.ClientProfiles.Find(id);
        }
        public void DeleteProfile(ClientProfile profile)
        {
            if (profile != null)
                Database.ClientProfiles.Remove(profile);
            Database.SaveChanges();
        }
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
