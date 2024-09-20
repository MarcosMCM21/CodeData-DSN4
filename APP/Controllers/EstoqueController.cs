using CodeData_Connection.Areas.Identity.Data;
using CodeData_Connection.Models.Database;
using CodeData_Connection.Models.Database.Entidade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace CodeData_Connection.Controllers
{
    public class EstoqueController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstoqueController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var equipamentos = await _context.Equipamentos.ToListAsync();

            return View(equipamentos);
        }
    }
}
