using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace home.manager.Controllers
{
    public class TemplatesController : Controller
    {
        public TemplatesController()
        {
            ViewFolders = new List<string>
            {
                "~/Scripts"
            };
        }

        public List<string> ViewFolders { get; set; }

        [OutputCache(CacheProfile = "Templates")]
        public ActionResult Render(string templatePath)
        {
            return DoRender(templatePath);
        }

        private ActionResult DoRender(string templatePath)
        {
            var viewNames = new List<string>();

            for (int i = 0; i < ViewFolders.Count; i++)
            {
                string fmt = ViewFolders[i] + "/{0}.cshtml";
                viewNames.Add(string.Format(fmt, templatePath));
            }

            for (int i = 0; i < viewNames.Count; i++)
            {
                string viewName = viewNames[i];

                if (!ViewExists(viewName)) continue;

                return View(viewName);
            }

            throw new HttpException(404, string.Format("404 - View for the template '{0}' not found", templatePath));
        }

        private bool ViewExists(string name)
        {
            var result = ViewEngines.Engines.FindView(ControllerContext, name, null);
            return (result.View != null);
        }
    }
}