using CodeData_Connection.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeData_Connection.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ConfiguracaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConfiguracaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Usuarios()
        {
            var usuarios = await _context.ApplicationUser.ToListAsync();

            return View(usuarios);
        }
    }
}
