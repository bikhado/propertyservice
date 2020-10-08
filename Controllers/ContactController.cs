using HtmlAgilityPack;
using PROPERTY_SERVICE.Models;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;
using Fizzler.Systems.HtmlAgilityPack;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using Umbraco.Core.Logging.Viewer;
using System.Collections.Generic;
using Umbraco.Web.PublishedModels;
using Umbraco.Core.Composing;
using NPoco;

namespace PROPERTY_SERVICE.Controllers
{
    public class ContactController : SurfaceController
    {
        private readonly ILogViewer _logViewer = null;

        public ContactController()
        {

        }

        public ContactController(ILogViewer logViewer)
        {
            _logViewer = logViewer;
        }

        public void SendNotiIntoUmbra(ContactModel contactModel)
        {
            IContentService contentService = Services.ContentService;
            var parent = contentService.GetById(1098);

            var message = contentService.CreateContent($"{contactModel.Name}-{DateTime.Now}", parent.GetUdi(), "contactContent");
            message.SetValue("nameUser", contactModel.Name);
            message.SetValue("email", contactModel.Email);
            message.SetValue("phoneNumber", contactModel.PhoneNumber);
            message.SetValue("message", contactModel.Message);

            Services.ContentService.SaveAndPublish(message);
        }

        public void SyncDataVnexpress()
        {
            IContentService contentService = Services.ContentService;
            var parent = contentService.GetById(1166);
            var news = CrawlerHtml();

            foreach (var item in news)
            {
                var message = contentService.CreateContent($"{item.Title}-{DateTime.Now}", parent.GetUdi(), "new");
                message.SetValue("title", item.Title);
                message.SetValue("description", item.Description);
                message.SetValue("content", item.Content);
                message.SetValue("image", item.LinkImage);

                Services.ContentService.SaveAndPublish(message);
            }
        }

        public List<VnexpressModel> CrawlerHtml()
        {
            var results = new List<VnexpressModel>();

            try
            {
                var web = new HtmlWeb();
                var document = web.Load("https://vnexpress.net/kinh-doanh/bat-dong-san");
                var articles = document.DocumentNode.QuerySelectorAll(".has-border-right article").ToList();

                foreach (var item in articles)
                {
                    var title = item.QuerySelector("h2 a")?.InnerText;
                    var image = item.QuerySelector("div a picture img").Attributes["src"]?.Value;
                    var linkDetail = item.QuerySelector("h2")?.FirstChild.GetAttributeValue("href", "");
                    var documentDetail = web.Load(linkDetail);
                    var htmlDetail = documentDetail.DocumentNode.QuerySelectorAll(".top-detail .container .sidebar-1").FirstOrDefault();
                    var description = htmlDetail.QuerySelector("p")?.InnerText;
                    var contentHtml = htmlDetail.QuerySelector("article")?.InnerHtml;

                    results.Add(new VnexpressModel()
                    {
                        Title = title,
                        LinkImage = image,
                        Description = description,
                        Content = contentHtml
                    });
                }
            }
            catch (Exception ex)
            {

            }

            return results;
        }

        [HttpPost]
        public ActionResult SubmitForm(ContactModel contactModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var fromEmail = new MailAddress(ConfigurationManager.AppSettings["SendEmailFrom"]);
                    var toAddress = new MailAddress(ConfigurationManager.AppSettings["SendEmailTo"]);
                    string subject = ConfigurationManager.AppSettings["EmailSubject"];
                    string body = contactModel.Message;
                    var message = new MailMessage(fromEmail, toAddress)
                    {
                        Subject = subject,
                        Body = createEmailBody(contactModel, subject)
                    };
                    var smtp = new SmtpClient();
                    message.IsBodyHtml = true;
                    smtp.Send(message);
                    SendNotiIntoUmbra(contactModel);

                    TempData["message-noti"] = "Has been sent ...";
                    return RedirectToCurrentUmbracoPage();
                }

                TempData["message-noti"] = "Cannot send email. Please check your input ...";
            }
            catch (Exception ex)
            {
                TempData["message-noti"] = "Error server ...";
            }

            return RedirectToCurrentUmbracoPage();
        }

        private string createEmailBody(ContactModel contactModel, string title)
        {
            var logoUrl = ConfigurationManager.AppSettings["LogoUrl"];
            string body = string.Empty;

            using (StreamReader reader = new StreamReader(Server.MapPath("~/mail/single-news.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{name}", contactModel.Name); //replacing the required things  
            body = body.Replace("{phonenumber}", contactModel.PhoneNumber);
            body = body.Replace("{email}", contactModel.Email);
            body = body.Replace("{logo}", logoUrl);
            body = body.Replace("{message}", contactModel.Message);

            var monthString = DateTime.Now.ToString("D", DateTimeFormatInfo.InvariantInfo);
            body = body.Replace("{date}", monthString);

            return body;

        }
    }

    public class VnexpressModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string LinkImage { get; set; }
        public string Content { get; set; }
    }
}