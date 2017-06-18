using OnlineLibrary.BLL.DTO;
using OnlineLibrary.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;

namespace OnlineLibrary.Modules
{
    public class TimerModule : IHttpModule
    {
        IReservService reservService;
        static Timer timer;
        long interval = 86400000;
        public TimerModule(IReservService reserv)
        {
            reservService = reserv;
        }
        public void Init(HttpApplication context)
        {
            timer = new Timer(new TimerCallback(SendEmail), null, 0, interval);
        }
        private void SendEmail(object obj)
        {
            DateTime dd = DateTime.Now;
            IEnumerable<ReservDTO> reserves = reservService.GetAllReserves();
            if (reserves != null)
            {
                foreach (ReservDTO reserv in reserves)
                {
                    if (!reserv.Resolution && reserv.FinishDate <= dd)
                    {
                        reservService.Delete(reserv.Id, reserv.BookName);
                    }
                }
            }
        }
        public void Dispose()
        {
            
        }
    }
}