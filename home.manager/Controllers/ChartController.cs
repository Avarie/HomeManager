using System.Web.Mvc;
using home.manager.Helpers;

namespace home.manager.Controllers
{
    [Authorize]
    public class ChartController : Controller
    {
        private readonly ChartCalculationHelper _helper;

        public ChartController(ChartCalculationHelper helper)
        {
            _helper = helper;
        }

        public JsonNetResult GetData(int months = 0)
        {
            return _helper.GetData(months);
        }
    }
}
