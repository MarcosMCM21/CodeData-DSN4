using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeData_Connection.Controllers
{
    public class EstoqueController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
