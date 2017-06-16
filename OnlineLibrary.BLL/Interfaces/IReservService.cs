using OnlineLibrary.BLL.DTO;
using System.Collections.Generic;

namespace OnlineLibrary.BLL.Interfaces
{
    public interface IReservService
    {
        void Create(ReservDTO reservDto);
        void Delete(int id);
        ReservDTO GetReserv(int? id);
        List<ReservDTO> GetReserves();
        bool? CheckReserv(string bookName);
        ReservDTO FindByBookName(string bookName);
        void Update(ReservDTO reservDto);
        void Dispose();
    }
}
