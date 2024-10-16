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

        public IActionResult Usuarios()
        {
            return View();
        }

        public async Task<IActionResult> ObterDadosUsuario()
        {
            var usuarios = await _context.ApplicationUser.ToListAsync();

            return PartialView("_DadosUsuario", usuarios);
        }
    }
}
