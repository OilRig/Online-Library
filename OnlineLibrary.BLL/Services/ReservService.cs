using OnlineLibrary.BLL.Interfaces;
using OnlineLibrary.DAL.Interfaces;
using OnlineLibrary.DAL.Entities;
using OnlineLibrary.BLL.DTO;
using AutoMapper;
using System.Collections.Generic;
using System;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Linq;

namespace OnlineLibrary.BLL.Services
{
    public class ReservService : IReservService
    {
        IUnitOfWork Database { get; set; }
        public ReservService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public void Create(ReservDTO reservDto)
        {
            Reserv reserv = new Reserv
            {
                Date = reservDto.Date,
                UserName = reservDto.UserName,
                BookName = reservDto.BookName,
                FinishDate = reservDto.FinishDate
            };
            Database.ReserveManager.Create(reserv);
            Database.SaveAsync();
        }
        public async Task Delete(int id, string bookName)
        {
            List<LibraryUser> users = Database.UserManager.Users.ToList();
            foreach (LibraryUser user in users)
            {
                try
                {
                    MailMessage message = new MailMessage("onlinelibrary17@mail.ru", user.Email);
                    message.Subject = "Доступна книга";
                    message.Body = bookName + "- доступна для бронирования!";
                    using (SmtpClient client = new SmtpClient
                    {
                        EnableSsl = true,
                        Host = "smtp.mail.ru",
                        Port = 587,
                        Credentials = new NetworkCredential("onlinelibrary17@mail.ru", "password123")
                    })
                    {
                        await client.SendMailAsync(message);
                    }
                }
                catch
                {
                    
                }
            }
            Database.ReserveManager.Delete(id);
            await Database.SaveAsync();
        }
        public ReservDTO FindReservByBookName(string bookName)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Reserv, ReservDTO>());
            ReservDTO reservDto = Mapper.Map<Reserv, ReservDTO>(Database.ReserveManager.Find(r=>r.BookName == bookName));
            return reservDto;
        }
        public ReservDTO GetReserv(int id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Reserv, ReservDTO>());
            ReservDTO reservDto = Mapper.Map<Reserv, ReservDTO>(Database.ReserveManager.Get(id));
            return reservDto;
        }
        public List<ReservDTO> GetAllReserves()
        {
            IEnumerable<Reserv> reserves = Database.ReserveManager.GetAll();
            Mapper.Initialize(cfg => cfg.CreateMap<Reserv, ReservDTO>());
            List<ReservDTO> reservesDto = Mapper.Map<IEnumerable<Reserv>, List<ReservDTO>>(reserves);
            return reservesDto;
        }
        public bool? CheckReserv(string bookName)
        {
            if(Database.ReserveManager.Find(r=>r.BookName == bookName)==null)
            {
                return null;
            }
            else if(Database.ReserveManager.Find(r=>r.BookName == bookName).Resolution == false)
            {
                return false;
            }
            return true;
        } 
        public void Dispose()
        {
            Database.Dispose();
        }
        public void Update(int id)
        {
            Reserv reserv = Database.ReserveManager.Get(id);
            if (reserv != null)
            {
                reserv.Date = DateTime.UtcNow;
                reserv.Resolution = true;
                reserv.FinishDate = DateTime.UtcNow.AddMonths(3);
                Database.ReserveManager.Update(reserv);
                Database.SaveAsync();
            }
        }
    }
}
