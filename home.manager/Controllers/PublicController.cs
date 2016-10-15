using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using home.manager.Repositories;

namespace home.manager.Controllers
{
    [AllowAnonymous]
    public class PublicController : Controller
    {
        private readonly NoteRepository _noteRepository;
        private readonly DocumentRepository _documentRepository;

        public PublicController(NoteRepository noteRepository, DocumentRepository documentRepository)
        {
            _noteRepository = noteRepository;
            _documentRepository = documentRepository;
        }

        public ActionResult Note(string id)
        {
            var item = _noteRepository.GetAll().FirstOrDefault(x => x.PublicId == id);
            if (item == null) return Get404();

            ViewBag.Note = item;
            return View("Note", "_NoLayout");
        }

        public ActionResult Document(String name)
        {
            var doc = _documentRepository.GetAll().FirstOrDefault(x => x.FileName == name);
            return doc != null ? File(doc.Data, doc.ContentType) : Get404();
        }

        private ActionResult Get404()
        {
            var file = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("home.manager.Content.images.404.jpg");

            var buffer = new byte[16*1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = file.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return File(ms.ToArray(), "image/jpeg");
            }
        }
    }
}

// http://www.codeproject.com/Articles/996879/Various-ways-of-using-Shared-Layout-in-ASP-NET-MVC