using OnlineLibrary.BLL.Interfaces;
using OnlineLibrary.DAL.Interfaces;
using OnlineLibrary.DAL.Entities;
using OnlineLibrary.BLL.DTO;
using AutoMapper;
using System.Collections.Generic;
using System;

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
        public void Delete(int id)
        {
            Database.ReserveManager.Delete(id);
            Database.SaveAsync();
        }
        public ReservDTO FindByBookName(string bookName)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Reserv, ReservDTO>());
            ReservDTO reservDto = Mapper.Map<Reserv, ReservDTO>(Database.ReserveManager.FindByName(bookName));
            return reservDto;
        }
        public ReservDTO GetReserv(int? id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Reserv, ReservDTO>());
            ReservDTO reservDto = Mapper.Map<Reserv, ReservDTO>(Database.ReserveManager.Get(id));
            return reservDto;
        }
        public List<ReservDTO> GetReserves()
        {
            IEnumerable<Reserv> reserves = Database.ReserveManager.GetAll();
            Mapper.Initialize(cfg => cfg.CreateMap<Reserv, ReservDTO>());
            List<ReservDTO> reservesDto = Mapper.Map<IEnumerable<Reserv>, List<ReservDTO>>(reserves);
            return reservesDto;
        }
        public bool? CheckReserv(string bookName)
        {
            if(Database.ReserveManager.FindByName(bookName)==null)
            {
                return null;
            }
            else if(Database.ReserveManager.FindByName(bookName).Resolution == false)
            {
                return false;
            }
            return true;
        } 
        public void Dispose()
        {
            Database.Dispose();
        }
        public void Update(ReservDTO reservDto)
        {
            Reserv reserv = Database.ReserveManager.Get(reservDto.Id);
            reserv.Date = DateTime.Now;
            reserv.Resolution = true;
            Database.ReserveManager.Update(reserv);
            Database.SaveAsync();
        }
    }
}
