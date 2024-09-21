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

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Tabela()
        {
            var equipamentos = await _context.Equipamentos.ToListAsync();

            Console.WriteLine(equipamentos);

            return PartialView("~/Views/Estoque/Tabela.cshtml", equipamentos);
        }

        public JsonResult GetEquipamentoId(int id)
        {
            var equipamento = _context.Equipamentos.Find(id);

            return Json(equipamento);
        }
    }
}
