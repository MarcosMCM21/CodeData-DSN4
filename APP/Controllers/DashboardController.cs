using Microsoft.AspNetCore.Mvc;

namespace CodeData_Connection.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
