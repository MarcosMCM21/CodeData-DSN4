using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeData_Connection.Areas.SistemaB.Controllers
{
    [Authorize]
    [Area("SistemaB")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
