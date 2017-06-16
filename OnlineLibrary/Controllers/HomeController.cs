using OnlineLibrary.BLL.DTO;
using OnlineLibrary.BLL.Interfaces;
using OnlineLibrary.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using System;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace OnlineLibrary.Controllers
{
    public class HomeController : Controller
    {
        IReservService reservService;
        IBookService bookService;
        IGenreService genreService;
        IEmailService EmailService;
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }
        public HomeController()
        {
                
        }
        public HomeController(IReservService serv, IBookService bookserv, IGenreService genreserv, IEmailService emailServ)
        {
            EmailService = emailServ;
            genreService = genreserv;
            reservService = serv;
            bookService = bookserv;
        }
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult AllBooks(int page = 1)
        {
            List<BookDTO> booksDto = bookService.GetBooks();
            Mapper.Initialize(cfg => cfg.CreateMap<BookDTO, BookViewModel>());
            List<BookViewModel> books = Mapper.Map<List<BookDTO>, List<BookViewModel>>(booksDto);
            foreach (BookViewModel book in books)
            {
                book.Order = reservService.CheckReserv(book.Name);
                if(reservService.FindByBookName(book.Name) != null)
                    book.ReservUserName = reservService.FindByBookName(book.Name).UserName;
            }
            int pageSize = 3;
            IEnumerable<BookViewModel> booksPerPages = books.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = books.Count };
            AllBooksViewModel AllBooksView = new AllBooksViewModel { PageInfo = pageInfo, Books = booksPerPages };
            return View(AllBooksView);
        }
        public ActionResult List(string genre, int page = 1)
        {
            ViewBag.Genre = genre;
            List<BookDTO> booksDto = bookService.FindByGenre(genre);
            Mapper.Initialize(cfg => cfg.CreateMap<BookDTO, BookViewModel>());
            List<BookViewModel> books = Mapper.Map<List<BookDTO>, List<BookViewModel>>(booksDto);
            foreach (BookViewModel book in books)
            {
                book.Order = reservService.CheckReserv(book.Name);
                if(reservService.FindByBookName(book.Name) != null)
                    book.ReservUserName = reservService.FindByBookName(book.Name).UserName;
            }
            int pageSize = 3;
            IEnumerable<BookViewModel> booksPerPages = books.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = books.Count };
            AllBooksViewModel ListBooksView = new AllBooksViewModel { PageInfo = pageInfo, Books = booksPerPages };
            return View(ListBooksView);
        }
        public ActionResult MakeReserv(string bookName)
        {
           if (bookName == null)
            {
                HttpNotFound();
            }
           else
            {
                ReservDTO reserv = new ReservDTO()
                {
                    BookName = bookName,
                    UserName = User.Identity.Name,
                    Date = DateTime.UtcNow,
                    FinishDate = DateTime.UtcNow.AddSeconds(3)
                };
                if (reservService.CheckReserv(bookName) == null)
                        reservService.Create(reserv);
            }
            return RedirectToAction("AllBooks", "Home");
        }
        public async Task<ActionResult> DelReserv(string bookName)
        {
            ReservDTO reservDto = reservService.FindByBookName(bookName);
            if (reservDto != null && User.Identity.Name == reservDto.UserName)
            {
                IEnumerable<UserDTO> users =  UserService.AllUsers();
                foreach(UserDTO user in users)
                {
                   await EmailService.Send(user.Email, "Книга доступна", bookName + " снова доступна для бронирования!");
                }
                reservService.Delete(reservDto.Id);
            }
            return RedirectToAction("AllBooks", "Home");
        }
        public PartialViewResult _Menu()
        {
            List<string> genresNames = new List<string>();
            List<GenreDTO> genresDto = genreService.GetGenres();
            for(int i = 0; i < genresDto.Count; i++)
            {
                genresNames.Add(genresDto[i].Name);
            }
            return PartialView(genresNames);
        }
        [HttpPost]
        public ActionResult BookSearch(string author, string publisher, int page = 1)
        {
            ViewBag.Author = author;
            ViewBag.Publisher = publisher;
            List<BookViewModel> booksVM;
            if (author.Length > 0 && publisher.Length == 0)
            {
                List<BookDTO> Books = bookService.FindByAuthor(author); 
                Mapper.Initialize(cfg => cfg.CreateMap<BookDTO, BookViewModel>());
                List<BookViewModel> books = Mapper.Map<List<BookDTO>, List<BookViewModel>>(Books);
                booksVM = books;
            }
            else if(author.Length == 0 && publisher.Length > 0)
            {
                List<BookDTO> Books = bookService.FindByPublisher(publisher);
                Mapper.Initialize(cfg => cfg.CreateMap<BookDTO, BookViewModel>());
                List<BookViewModel> books = Mapper.Map<List<BookDTO>, List<BookViewModel>>(Books);
                booksVM = books;
            }
            else if(author.Length > 0 && publisher.Length > 0)
            {
                List<BookDTO> books = bookService.FindByAuthor(author);
                Mapper.Initialize(cfg => cfg.CreateMap<BookDTO, BookViewModel>());
                List<BookViewModel> booksAuthor = Mapper.Map<List<BookDTO>, List<BookViewModel>>(books);
                booksVM = booksAuthor.Where(b => b.Publisher.Contains(publisher)).ToList();
            }
            else
            {
                List<BookDTO> books = bookService.GetBooks();
                Mapper.Initialize(cfg => cfg.CreateMap<BookDTO, BookViewModel>());
                booksVM = Mapper.Map<List<BookDTO>, List<BookViewModel>>(books);
            }
            foreach(BookViewModel book in booksVM)
            {
                book.Order = reservService.CheckReserv(book.Name);
                if (reservService.FindByBookName(book.Name) != null)
                    book.ReservUserName = reservService.FindByBookName(book.Name).UserName;
            }
            int pageSize = 3;
            IEnumerable<BookViewModel> booksPerPages = booksVM.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = booksVM.Count };
            AllBooksViewModel ListBooksView = new AllBooksViewModel { PageInfo = pageInfo, Books = booksPerPages };
            return View(ListBooksView);
        }
    }
}