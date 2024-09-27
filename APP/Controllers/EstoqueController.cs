using CodeData_Connection.Areas.Identity.Data;
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

            if (equipamentos == null)
            {
                Console.WriteLine("Nenhum equipamento encontrado!");

                return View("Nenhum equipamento encontrado!");
            }

            return View(equipamentos);
        }

        //public JsonResult GetEquipamentoId(int id)
        //{
        //    var equipamento = _context.Equipamentos.Find(id);

        //    return Json(equipamento);
        //}
    }
}
