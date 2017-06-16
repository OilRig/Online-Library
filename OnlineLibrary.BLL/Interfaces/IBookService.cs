using OnlineLibrary.BLL.DTO;
using System.Collections.Generic;

namespace OnlineLibrary.BLL.Interfaces
{
    public interface IBookService
    {
        void Create(BookDTO bookDto);
        void Delete(int id);
        void Update(BookDTO bookDto);
        BookDTO GetBook(int? id);
        List<BookDTO> GetBooks();
        List<BookDTO> FindByGenre(string genre);
        List<BookDTO> FindByAuthor(string authorName);
        List<BookDTO> FindByPublisher(string publisher);
        void Dispose();
    }
}
