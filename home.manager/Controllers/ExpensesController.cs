using System;
using System.Linq;
using System.Web.Mvc;
using home.manager.Helpers;
using home.manager.Models.db;
using home.manager.Repositories;
using home.manager.Shared;

namespace home.manager.Controllers
{
    [Authorize]
    public class ExpensesController : SharedController<Expense, Category, ExpenseRepository>
    {
        private readonly ExpenseRepository _expenseRepository;
        private readonly ISharedCategoryRepository<SubCategory> _subCategoryRepository;
        public ExpensesController(ExpenseRepository expenseRepository, ISharedCategoryRepository<SubCategory> subCategoryRepository)
        {
            _expenseRepository = expenseRepository;
            _subCategoryRepository = subCategoryRepository;
        }

        [HttpGet]
        public JsonNetResult GetStatsData()
        {
            var now = DateTime.Now;
            var prev = now.AddMonths(-1);

            return new JsonNetResult(
                new
                {
                    now = now.ToString("Y"),
                    nowSpent = GetMonthlyExpenses(now),
                    prev = prev.ToString("Y"),
                    prevSpent = GetMonthlyExpenses(prev)
                });
        }

        private string GetMonthlyExpenses(DateTime now)
        {
            var all = _expenseRepository.GetAll().Where(x => x.Date.Year == now.Year && x.Date.Month == now.Month);
            return (all.Any() ? all.Sum(x => x.SpentMoney) : 0).ToString("# ###.##");
        }

        [HttpPost]
        public JsonNetResult GetSubCategoryNames(int categoryId)
        {
            return new JsonNetResult(_subCategoryRepository.GetAll()
                .Where(x => x.Category.Id == categoryId)
                .OrderBy(x => x.Name)
                .Select(x => x.Name));
        }
    }
}
