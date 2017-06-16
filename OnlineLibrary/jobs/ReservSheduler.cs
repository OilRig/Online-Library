using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineLibrary.jobs
{
    public class ReservSheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<TimeReserv>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")  
                .StartNow()                          
                .WithSimpleSchedule(x => x         
                    .WithIntervalInMinutes(1)        
                    .RepeatForever())                 
                .Build();                            

            scheduler.ScheduleJob(job, trigger);      
        }
    }
}