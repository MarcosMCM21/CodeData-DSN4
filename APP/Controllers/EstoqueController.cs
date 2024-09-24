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
            Console.WriteLine("CONTROLLER: Estoque; ACTION: Tabela");

            var equipamentos = await _context.Equipamentos.ToListAsync();

            Console.WriteLine("\n\nEQUIPAMENTOS:\n\n");

            if (equipamentos.Any() || equipamentos == null)
            {
                Console.WriteLine("Nenhum equipamento encontrado!");

                return View("Nenhum equipamento encontrado!");
            }


            Console.WriteLine(equipamentos.ToJson());

            return View(equipamentos);
        }

        //public JsonResult GetEquipamentoId(int id)
        //{
        //    var equipamento = _context.Equipamentos.Find(id);

        //    return Json(equipamento);
        //}
    }
}
