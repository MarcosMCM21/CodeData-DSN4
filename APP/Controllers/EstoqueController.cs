using CodeData_Connection.Areas.Identity.Data;
using CodeData_Connection.Models.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeData_Connection.Controllers
{
    public class EstoqueController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstoqueController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Tabela()
        {
            var equipamentos = await _context.Equipamentos.ToListAsync();

            if (equipamentos == null)
            {
                Console.WriteLine("Nenhum equipamento encontrado!");
            }

            return View(equipamentos);
        }
    }
}
