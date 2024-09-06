using Microsoft.AspNetCore.Mvc;

namespace CodeData_Connection.Controllers
{
    public class ClientesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
