using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using PROPERTY_SERVICE.Controllers;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace PROPERTY_SERVICE.Jobs
{
    public class SyncNewsVnexpress : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            string host = WebConfigurationManager.AppSettings["MainHost"];
            var apiUrl = host + "Umbraco/Sync/SyncNewsVnexpress/GetData";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                //HTTP GET
                var responseTask = client.GetAsync("GetData");

                var result = responseTask.Result;
                client.Dispose();
            }

            return Task.CompletedTask;
        }
    }
}