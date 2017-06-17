using OnlineLibrary.BLL.Interfaces;
using OnlineLibrary.DAL.Interfaces;
using OnlineLibrary.BLL.DTO;
using AutoMapper;
using OnlineLibrary.DAL.Entities;
using System.Collections.Generic;

namespace OnlineLibrary.BLL.Services
{
    public class BookService : IBookService
    {
        IUnitOfWork Database { get; set; }
        public BookService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public void Create(BookDTO bookDto)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<BookDTO, Book>());
            var book = Mapper.Map<BookDTO, Book>(bookDto);
            Database.BookManager.Create(book);
            Database.SaveAsync();
        }
        public void Delete(int id)
        {
            Database.BookManager.Delete(id);
            Database.SaveAsync();
        }
        public void Update(BookDTO bookDto)
        {
            Book book = Database.BookManager.Find(b=>b.Id == bookDto.Id);
            book.Name = bookDto.Name;
            book.Author = bookDto.Author;
            if(bookDto.Genre != null)
                book.Genre = bookDto.Genre;
            book.Description = bookDto.Description;
            book.Publisher = bookDto.Publisher;
            if(bookDto.Image.Length != 0)
                book.Image = bookDto.Image;
            Database.BookManager.Update(book);
            Database.SaveAsync();
        }
        public BookDTO GetBook(int id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Book, BookDTO>());
            var bookDto = Mapper.Map<Book, BookDTO>(Database.BookManager.Get(id));
            return bookDto;
        }
        public List<BookDTO> GetAllBooks()
        {
            IEnumerable<Book> books = Database.BookManager.GetAll();
            Mapper.Initialize(cfg => cfg.CreateMap<Book, BookDTO>());
            List<BookDTO> booksDto = Mapper.Map<IEnumerable<Book>, List<BookDTO>>(books);
            return booksDto;
        }
        public BookDTO FindBookByName(string bookName)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Book, BookDTO>());
            BookDTO bookDto = Mapper.Map<Book, BookDTO>(Database.BookManager.Find(b=>b.Name == bookName));
            return bookDto;
        }
        public List<BookDTO> FindBooksByGenre(string genre)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Book, BookDTO>());
            List<BookDTO> booksDto = Mapper.Map<IEnumerable<Book>, List<BookDTO>>(Database.BookManager.FindAll(b=>b.Genre == genre));
            return booksDto;
        }
        public List<BookDTO> FindBooksByAuthor(string authorName)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Book, BookDTO>());
            List<BookDTO> booksDto = Mapper.Map<IEnumerable<Book>, List<BookDTO>>(Database.BookManager.FindAll(b=>b.Author == authorName));
            return booksDto;
        }
        public List<BookDTO> FindBooksByPublisher(string publisher)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Book, BookDTO>());
            List<BookDTO> booksDto = Mapper.Map<IEnumerable<Book>, List<BookDTO>>(Database.BookManager.FindAll(b=>b.Publisher == publisher));
            return booksDto;
        }
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
