using OnlineLibrary.BLL.DTO;
using OnlineLibrary.BLL.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;

namespace OnlineLibrary.jobs
{
    public class TimeReserv : IJob
    {
        IReservService reservService;
        IEmailService EmailService;
        IUserService UserService;
        public TimeReserv(IReservService reservServ, IEmailService emailServ, IUserService userServ)
        {
            UserService = userServ;
            EmailService = emailServ;
            reservService = reservServ;
        }
        public void Execute(IJobExecutionContext context)
        {
            List<ReservDTO> reserves = reservService.GetAllReserves();
            foreach(ReservDTO reserv in reserves)
            {
                if (!reserv.Resolution && reserv.FinishDate <= DateTime.UtcNow)
                {
                    reservService.Delete(reserv.Id);
                    IEnumerable<UserDTO> users = UserService.AllUsers();
                    foreach (UserDTO user in users)
                    {
                        EmailService.Send(user.Email, "Книга доступна", reserv.BookName + "- доступна для бронированя!");
                    }
                }
            }
        }
    }
}