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
        public LibraryanController(IReservService serv, IBookService bookserv, IGenreService genreServ)
        {
            genreService = genreServ;
            reservService = serv;
            bookService = bookserv;
        }
        public ActionResult AllBooks()
        {
            List<BookDTO> booksDTO = bookService.GetAllBooks();
            Mapper.Initialize(cfg => cfg.CreateMap<BookDTO, BookViewModel>());
            var books = Mapper.Map<List<BookDTO>, List<BookViewModel>>(booksDTO);
            return View(books);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                BookDTO bookDTO = bookService.GetBook(Convert.ToInt32(id));
                Mapper.Initialize(cfg => cfg.CreateMap<BookDTO, BookViewModel>());
                var book = Mapper.Map<BookDTO, BookViewModel>(bookDTO);
                List<GenreDTO> genres = genreService.GetAllGenres();
                List<string> genresNames = new List<string>();
                foreach(GenreDTO genre in genres)
                {
                    if (genre.Name != book.Genre)
                        genresNames.Add(genre.Name);
                }
                ViewBag.Genres = genresNames;
                return View(book);
            }
            return RedirectToAction("AllBooks", "Libraryan");
        }
        [HttpPost]
        public ActionResult Edit(BookViewModel model, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                if (uploadImage != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    model.Image = imageData;
                }
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
            List<GenreDTO> genres = genreService.GetAllGenres();
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
        public ActionResult Delete(int? id)
        {
            if (id != null)
                bookService.Delete(Convert.ToInt32(id));
            return RedirectToAction("AllBooks", "Libraryan");
        }
        public ActionResult AllReserves()
        {
            IEnumerable<ReservDTO> reservs = reservService.GetAllReserves();
            Mapper.Initialize(cfg => cfg.CreateMap<ReservDTO, ReservViewModel>());
            List<ReservViewModel> reserves = Mapper.Map<IEnumerable<ReservDTO>, List<ReservViewModel>>(reservs);
            return View(reserves);
        }
        public ActionResult Accept(int? id)
        {
            if (id != null)
            {
                reservService.Update(Convert.ToInt32(id));
            }
            return RedirectToAction("AllReserves", "Libraryan");
        }
        public async Task<ActionResult> Cancel(int? id, string bookName)
        {
            if(reservService.GetReserv(Convert.ToInt32(id))!=null)
                await reservService.Delete(Convert.ToInt32(id), bookName);
            return RedirectToAction("AllReserves", "Libraryan");
        }
        public ActionResult AllGenres()
        {
            return View(genreService.GetAllGenres());
        }
        [HttpGet]
        public ActionResult EditGenre(int? id)
        {
            if (id != null)
            {
                GenreDTO genre = genreService.GetGenre(Convert.ToInt32(id));
                return View(genre);
            }
            return RedirectToAction("AllGenres", "Libraryan");
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
        public ActionResult DeleteGenre(int? id)
        {
            if (id != null)
                genreService.Delete(Convert.ToInt32(id));
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