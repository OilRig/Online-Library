using OnlineLibrary.BLL.DTO;
using System.Collections.Generic;

namespace OnlineLibrary.BLL.Interfaces
{
    public interface IReservService
    {
        void Create(ReservDTO reservDto);
        void Delete(int id);
        ReservDTO GetReserv(int id);
        List<ReservDTO> GetAllReserves();
        bool? CheckReserv(string bookName);
        ReservDTO FindReservByBookName(string bookName);
        void Update(ReservDTO reservDto);
        void Dispose();
    }
}
