using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using home.manager.Models.db;
using home.manager.Repositories;
using home.manager.Shared;

namespace home.manager.Controllers
{
    [Authorize]
    public class DocumentsController : SharedController<Document, SharedCategory, DocumentRepository>
    {
        private readonly EntityFrameworkRepository<Document> _repository;
        private readonly IAccountRepository _accountRepository;

        public DocumentsController(EntityFrameworkRepository<Document> repository, IAccountRepository accountRepository)
        {
            _repository = repository;
            _accountRepository = accountRepository;
        }

        [HttpPost]
        public JsonResult Upload(HttpPostedFileBase file)
        {
            if (_repository.GetAll().Any( x => x.FileName == file.FileName && x.ContentLength == file.ContentLength))
                return Json(_repository.GetAll().Count());

            using (var b = new BinaryReader(file.InputStream))
            {
                var binData = b.ReadBytes(file.ContentLength);

                _repository.Add(new Document
                {
                    ContentLength = binData.Count(),
                    ContentType = file.ContentType,
                    CreatedTime = DateTime.Now,
                    Data = binData,
                    Description = String.Empty,
                    FileName = file.FileName,
                    Owner = _accountRepository.FindByName(User.Identity.Name)
                });
            }

            return Json(_repository.GetAll().Count());
        }
    }
}