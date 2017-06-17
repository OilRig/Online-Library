using OnlineLibrary.BLL.DTO;
using System.Collections.Generic;

namespace OnlineLibrary.BLL.Interfaces
{
    public interface IBookService
    {
        void Create(BookDTO bookDto);
        void Delete(int id);
        void Update(BookDTO bookDto);
        BookDTO GetBook(int id);
        List<BookDTO> GetAllBooks();
        List<BookDTO> FindBooksByGenre(string genre);
        List<BookDTO> FindBooksByAuthor(string authorName);
        List<BookDTO> FindBooksByPublisher(string publisher);
        void Dispose();
    }
}
