using System;
using System.Collections.Generic;
using System.Linq;
using home.manager.Models.db;
using home.manager.Repositories;
using home.manager.Resources;

namespace home.manager.Helpers
{
    public class ChartCalculationHelper
    {
        private readonly ExpenseRepository _expenseRepository;
        private readonly ExpenseCategoryRepository _categoryRepository;

        public ChartCalculationHelper(ExpenseRepository expenseRepository, ExpenseCategoryRepository categoryRepository)
        {
            _expenseRepository = expenseRepository;
            _categoryRepository = categoryRepository;
        }

        public JsonNetResult GetData(int months = 0)
        {
            var categories = _categoryRepository.GetAll().OrderBy(x => x.Name).ToList();
            var gridList = new List<Dictionary<String, Object>>();
            var chartsData = new Dictionary<String, List<double[]>>();
            DateTime start;

            if (months == 0)
            {
                var first = _expenseRepository.GetAll().OrderBy(x => x.Date).FirstOrDefault();
                if (first == null) return new JsonNetResult();
                start = first.Date.Date;
            }
            else
            {
                start = DateTime.Now.Date.AddMonths(-months);
            }

            var now = DateTime.Now.Date;

            foreach (var category in categories)
            {
                chartsData[category.Name] = new List<double[]>();
            }

            var gridLineId = 0;
            for (var month = now; !(month.Month == start.Month && month.Year == start.Year); month = month.AddMonths(-1))
            {
                gridList.Add(CreateMonthLine(month, categories, chartsData, ref gridLineId));
            }

            return new JsonNetResult(
                new
                {
                    titles = CreateGridTitles(categories),
                    grid = gridList,
                    chart = CreateChartData(chartsData)
                });
        }

        private Dictionary<string, object> CreateMonthLine(DateTime month, IEnumerable<Category> categories, Dictionary<string, List<double[]>> chartsData, ref int gridLineId)
        {
            var gridColId = 1;

            var gridLine = new Dictionary<String, Object>
            {
                {"Id", gridLineId++},
                {"Col0", month.ToString("Y")},
            };

            var unixtime =
                DateTimeToUnixTimestamp(new DateTime(month.Year, month.Month, 1).AddMonths(1).AddDays(-1));

            double all = 0;
            foreach (var category in categories)
            {
                var monthlyExpenses = _expenseRepository.GetAll()
                    .Where(x =>
                        x.Date.Month == month.Month &&
                        x.Date.Year == month.Year &&
                        x.Category.Id == category.Id)
                    .ToList();

                var sum = monthlyExpenses.Any() ? monthlyExpenses.Sum(x => x.SpentMoney) : 0;
                all += sum;

                AddGridLineValue(gridLine, gridColId++, FormatGridCell(sum));
                chartsData[category.Name].Add(new[] { unixtime, Math.Round(sum, 2) });
            }
            AddGridLineValue(gridLine, gridColId, FormatGridCell(all));
            return gridLine;
        }

        private static string FormatGridCell(double sum)
        {
            return sum > 0 ? sum.ToString("# ###.##") : "-";
        }

        private static List<object> CreateChartData(Dictionary<string, List<double[]>> chartsData)
        {
            var charts = new List<object>();
            foreach (var chart in chartsData)
            {
                charts.Add(new
                {
                    name = chart.Key,
                    data = chart.Value.OrderBy(x => x[0])
                });
            }
            return charts;
        }

        private static List<object> CreateGridTitles(List<Category> categories)
        {
            var titles = new List<Object>();
            var titleId = 1;
            AddGridTitleValue(titles, 0, Str.grid_Date);
            categories.ForEach(x => AddGridTitleValue(titles, titleId++, x.Name));
            AddGridTitleValue(titles, titleId, Str.grid_All);
            return titles;
        }

        private static void AddGridTitleValue(ICollection<object> titles, int id, string title)
        {
            titles.Add(new
            {
                field = String.Format("Col{0}", id),
                displayName = title
            });
        }

        private static void AddGridLineValue(Dictionary<string, object> lines, int id, string value)
        {
            lines.Add(String.Format("Col{0}", id), value);
        }

        private static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds * 1000;
        }
    }
}