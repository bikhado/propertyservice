using PROPERTY_SERVICE.Jobs;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Core.Services.Implement;

namespace PROPERTY_SERVICE.Umbraco.Composing
{
    public class ApplicationComposer : ComponentComposer<ApplicationComponent>, IUserComposer
    {
        public override void Compose(Composition composition)
        {
            // ApplicationStarting event in V7: add IContentFinders, register custom services and more here

            base.Compose(composition);
        }
    }

    public class ApplicationComponent : IComponent
    {
        public async void Initialize()
        {
            // construct a scheduler factory
            StdSchedulerFactory factory = new StdSchedulerFactory();

            // get a scheduler
            IScheduler scheduler = await factory.GetScheduler();
            await scheduler.Start();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<SyncNewsVnexpress>()
                .WithIdentity("myJob", "group1")
                .Build();

            // Trigger the job to run now, and then every 40 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1").WithCronSchedule("* 0/30 * * * ?").Build();

            await scheduler.ScheduleJob(job, trigger);
        }

        public void Terminate()
        { }
    }
}