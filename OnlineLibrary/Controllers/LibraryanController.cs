using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using OnlineLibrary.BLL.DTO;
using OnlineLibrary.BLL.Interfaces;
using OnlineLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnlineLibrary.Controllers
{
    [Authorize(Roles ="librarian")]
    public class LibraryanController : Controller
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
        public LibraryanController()
        {

        }
        public LibraryanController(IReservService serv, IBookService bookserv, IGenreService genreServ, IEmailService emailServ)
        {
            EmailService = emailServ;
            genreService = genreServ;
            reservService = serv;
            bookService = bookserv;
        }
        public ActionResult AllBooks()
        {
            List<BookDTO> booksDTO = bookService.GetBooks();
            Mapper.Initialize(cfg => cfg.CreateMap<BookDTO, BookViewModel>());
            var books = Mapper.Map<List<BookDTO>, List<BookViewModel>>(booksDTO);
            return View(books);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            BookDTO bookDTO = bookService.GetBook(id);
            Mapper.Initialize(cfg => cfg.CreateMap<BookDTO, BookViewModel>());
            var book = Mapper.Map<BookDTO, BookViewModel>(bookDTO);
            List<GenreDTO> genres = genreService.GetGenres();
            string[] genresArray = new string[genres.Count-1];
            for (int i = 0; i < genres.Count-1; i++)
            {
                if(genres[i].Name != book.Genre)
                    genresArray[i] = genres[i].Name;
            }
            ViewBag.Genres = genresArray;
            return View(book);
        }
        [HttpPost]
        public ActionResult Edit(BookViewModel model, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                model.Image = imageData;
                Mapper.Initialize(cfg => cfg.CreateMap<BookViewModel, BookDTO>());
                BookDTO bookDto = Mapper.Map<BookViewModel, BookDTO>(model);
                bookService.Update(bookDto);
                return RedirectToAction("AllBooks", "Libraryan");
            }
            else
            {
                ModelState.AddModelError("", "Заполните поля");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            List<GenreDTO> genres = genreService.GetGenres();
            string[] genresArray = new string[genres.Count];
            for(int i = 0; i < genres.Count; i++)
            {
                genresArray[i] = genres[i].Name;
            }
            ViewBag.Genres = genresArray;
            return View();
        }
        [HttpPost]
        public ActionResult Create(BookViewModel model, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                model.Image = imageData;
                Mapper.Initialize(cfg => cfg.CreateMap<BookViewModel, BookDTO>());
                BookDTO bookDto = Mapper.Map<BookViewModel, BookDTO>(model);
                bookService.Create(bookDto);
                return RedirectToAction("AllBooks", "Libraryan");
            }
            else
            {
                ModelState.AddModelError("", "Заполните все необходимые поля!");
            }
            return View();
        }
        public ActionResult Delete(int id)
        {
            bookService.Delete(id);
            return RedirectToAction("AllBooks", "Libraryan");
        }
        public ActionResult AllReserves()
        {
            IEnumerable<ReservDTO> reservs = reservService.GetReserves();
            Mapper.Initialize(cfg => cfg.CreateMap<ReservDTO, ReservViewModel>());
            List<ReservViewModel> reserves = Mapper.Map<IEnumerable<ReservDTO>, List<ReservViewModel>>(reservs);
            return View(reserves);
        }
        public ActionResult Accept(int? id)
        {
            ReservDTO reserv = reservService.GetReserv(id);
            reservService.Update(reserv);
            return RedirectToAction("AllReserves", "Libraryan");
        }
        public async Task<ActionResult> Cancel(int id)
        {
            ReservDTO reserv = reservService.GetReserv(id);
            if (reserv != null)
            {
                IEnumerable<UserDTO> users = UserService.AllUsers();
                foreach(UserDTO user in users)
                {
                    await EmailService.Send(user.Email ,"Книга доступна", reserv.BookName + "- доступна для бронирования!");
                } 
                reservService.Delete(id);
            }
            return RedirectToAction("AllReserves", "Libraryan");
        }
        public ActionResult AllGenres()
        {
            return View(genreService.GetGenres());
        }
        [HttpGet]
        public ActionResult EditGenre(int id)
        {
            GenreDTO genre = genreService.GetGenre(id);
            return View(genre);
        }
        [HttpPost]
        public ActionResult EditGenre(GenreDTO model)
        {
            if (ModelState.IsValid)
            {
                genreService.Update(model);
                return RedirectToAction("AllGenres", "Libraryan");
            }
            else
            {
                ModelState.AddModelError("", "Заполните поля");
            }
            return View();
        }
        public ActionResult DeleteGenre(int id)
        {
            genreService.Delete(id);
            return RedirectToAction("AllGenres", "Libraryan");
        }
        [HttpGet]
        public ActionResult CreateGenre()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateGenre(GenreDTO model)
        {
            if (ModelState.IsValid)
            {
                genreService.Create(model);
                return RedirectToAction("AllGenres", "Libraryan");
            }
            else
            {
                ModelState.AddModelError("", "Заполните все необходимые поля!");
            }
            return View();
        }
    }
}