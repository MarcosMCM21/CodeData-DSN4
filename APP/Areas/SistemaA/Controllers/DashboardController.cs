using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeData_Connection.Areas.SistemaA.Controllers
{
    [Authorize]
    [Area("SistemaA")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
