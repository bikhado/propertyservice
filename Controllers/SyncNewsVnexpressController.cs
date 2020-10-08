using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace PROPERTY_SERVICE.Controllers
{
    [PluginController("Sync")]
    public class SyncNewsVnexpressController : UmbracoApiController
    {
        [HttpGet]
        public void GetData()
        {
            SyncDataVnexpress();
        }

        public void SyncDataVnexpress()
        {
            IContentService contentService = Services.ContentService;
            var parent = contentService.GetById(1166);
            var news = CrawlerHtml();

            var pageNewsInDb = Umbraco.Content(1166).Children.ToList();

            try
            {
                foreach (var item in news)
                {                    
                    var anyNew = pageNewsInDb.Where(x => x.GetProperty("title").Value().Equals(item.Title))?.ToList();
                    if (anyNew.Count <= 0)
                    {
                        var message = contentService.CreateContent($"{item.Title}-{DateTime.Now}", parent.GetUdi(), "new");

                        message.SetValue("title", item.Title);
                        message.SetValue("description", item.Description);
                        message.SetValue("content", item.Content);
                        message.SetValue("image", item.LinkImage);

                        Services.ContentService.SaveAndPublish(message);
                    }
                }
            }
            catch(Exception ex)
            {

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
                    var image = item.QuerySelector("div a picture img")?.Attributes["src"].Value;
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
    }
}