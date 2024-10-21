using CodeData_Connection.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeData_Connection.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Perfil()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var usuario = _context.Users.Find(usuarioId);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        public IActionResult Configuracoes()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var usuario = _context.Users.Find(usuarioId);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }
    }
}
