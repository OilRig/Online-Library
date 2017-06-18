using OnlineLibrary.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineLibrary.BLL.Interfaces
{
    public interface IReservService
    {
        void Create(ReservDTO reservDto);
        Task Delete(int id, string bookName);
        ReservDTO GetReserv(int id);
        List<ReservDTO> GetAllReserves();
        bool? CheckReserv(string bookName);
        ReservDTO FindReservByBookName(string bookName);
        void Update(int id);
        void Dispose();
    }
}
