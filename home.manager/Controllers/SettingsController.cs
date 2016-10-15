using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using home.manager.Helpers;
using home.manager.Models.db;
using home.manager.Repositories;
using home.manager.Resources;

namespace home.manager.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly ExpenseRepository _expenseRepository;
        private readonly ExpenseSubCategoryRepository _subCategoryRepository;
        private readonly ExpenseCategoryRepository _categoryRepository;
        private readonly NoteRepository _noteRepository;

        public SettingsController(ExpenseRepository expenseRepository,
            ExpenseSubCategoryRepository subCategoryRepository,
            ExpenseCategoryRepository categoryRepository,
            NoteRepository noteRepository)
        {
            _expenseRepository = expenseRepository;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _noteRepository = noteRepository;
        }

        public JsonNetResult GetExpenseCategories()
        {
            return new JsonNetResult(_categoryRepository.GetAll()
                .ToArray()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                    Amount = _subCategoryRepository.GetAll().Count(s => s.Category.Id == x.Id)
                })
                .OrderBy(x => x.Name));
        }

        public JsonNetResult GetExpenseSubCategories(int category)
        {
            return new JsonNetResult(_subCategoryRepository.GetAll()
                .Where(x => x.Category.Id == category)
                .ToArray()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                    Amount = _expenseRepository.GetAll().Count(c => c.SubCategory.Id == x.Id)
                })
                .OrderBy(x => x.Name));
        }

        public void MergeSubCategory(int oldCategoryId, int oldSubCategoryId, int newCategoryId, int newSubCategoryId)
        {

            var expenses = _expenseRepository.GetAll()
                    .Where(x => x.Category.Id == oldCategoryId && x.SubCategory.Id == oldSubCategoryId);
            if (!expenses.Any()) return;

            var category = _categoryRepository.GetAll().First(x => x.Id == newCategoryId);
            var subCategory = _subCategoryRepository.GetAll().First(x => x.Id == newSubCategoryId);
            foreach (var expense in expenses.ToList())
            {
                expense.Category = category;
                expense.SubCategory = subCategory;
                _expenseRepository.Update(expense);
            }
        }

        public void AddOrUpdateExpenseSubCategory(SubCategory subCategory, int id)
        {
            if (subCategory == null || id == 0) return;
            subCategory.Category = _categoryRepository.GetAll().FirstOrDefault(x => x.Id == id);
            _subCategoryRepository.AddOrUpdate(subCategory);
        }

        public void DeleteExpenseSubCategory(SubCategory sub)
        {
            if (sub == null) return;
            _subCategoryRepository.Delete(sub);
        }

        public JsonNetResult GetResources()
        {
            return new JsonNetResult(
                Str.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true)
                );
        }

        [HttpGet]
        public JsonNetResult GetTitle()
        {
            return new JsonNetResult(GetTitleNote().Content);
        }

        private Note GetTitleNote()
        {
            var title = _noteRepository.GetItems(0, 0, String.Empty)
                    .Where(x => x.Name == "TITLE" && x.Description == "TITLE")
                    .OrderByDescending(x => x.Date)
                    .FirstOrDefault();
            return title ?? new Note
            {
                Name = "TITLE",
                Description = "TITLE"
            };
        }

        [HttpPost]
        public async Task<JsonNetResult> SaveTitle(string title)
        {
            var titleNote = GetTitleNote();
            titleNote.Content = title;
            await _noteRepository.UpdateItem(titleNote, User.Identity.Name);
            return new JsonNetResult(titleNote);
        }
    }
}
