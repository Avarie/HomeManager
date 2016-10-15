using System.Linq;
using System.Web.Mvc;
using home.manager.Helpers;
using home.manager.Models.db;
using home.manager.Repositories;
using home.manager.Shared;

namespace home.manager.Controllers
{
    [Authorize]
    public class NotesController : SharedController<Note, NoteCategory, NoteRepository>
    {
        private readonly ISharedRepository<Note, NoteCategory> _repository;
        public NotesController(ISharedRepository<Note, NoteCategory> repository)
        {
            _repository = repository;
        }

        public override JsonNetResult GetItems(int id, int category, string search)
        {
            return new JsonNetResult(
                _repository.GetItems(id, category, search)
                .Where(x => x.Name != "TITLE" && x.Description != "TITLE")
                );
        }
    }
}