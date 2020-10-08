using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;

namespace PROPERTY_SERVICE.Controllers
{
    public class ProjectController : SurfaceController
    {
        // GET: Project
        [HttpGet]
        public ActionResult SubmitComment(int idProject, string userComment, string contentComment, string typeSubmit = "main-submit",
            int idMainPage = 0)
        {
            IContentService contentService = Services.ContentService;

            try
            {
                if (typeSubmit == "main-submit")
                {
                    var parent = contentService.GetById(idProject);
                    var comment = contentService.CreateContent($"{userComment}-{DateTime.Now}", parent.GetUdi(), "comment");
                    comment.SetValue("nameComment", userComment);
                    comment.SetValue("time", DateTime.Now);
                    comment.SetValue("idComment", Guid.NewGuid());
                    comment.SetValue("commentContent", contentComment);

                    Services.ContentService.SaveAndPublish(comment);
                }
                else
                {
                    var parent = contentService.GetById(idProject);
                    var comment = contentService.CreateContent($"{userComment}-{DateTime.Now}", parent.GetUdi(), "replyComment");
                    comment.SetValue("nameUserReply", userComment);
                    comment.SetValue("timeReply", DateTime.Now);
                    comment.SetValue("iDReplyComment", Guid.NewGuid());
                    comment.SetValue("contentReply", contentComment);

                    Services.ContentService.SaveAndPublish(comment);
                    return RedirectToUmbracoPage(idMainPage);
                }
            }
            catch (Exception)
            {
                TempData["error-message"] = "Submit Failed !";
            }

            return RedirectToUmbracoPage(idProject);
        }
    }
}