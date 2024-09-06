using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeData_Connection.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ConfigController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
