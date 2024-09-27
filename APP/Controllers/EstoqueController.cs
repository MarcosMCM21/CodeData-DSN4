using CodeData_Connection.Areas.Identity.Data;
using CodeData_Connection.Models;
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
            var equipamentosStatus = new List<EquipamentoStatus>();

            if (equipamentos == null)
            {
                Console.WriteLine("Nenhum equipamento encontrado!");
            }

            foreach (var equipamento in equipamentos)
            {
                try
                {
                    var status = _context.MovimentacoesEquipamentos
                    .Join(
                        _context.Solicitacoes,
                        me => me.SolicitacaoId,
                        s => s.Id,
                        (me, s) => new { me.EquipamentoId, s.Tipo }
                        )
                        .Where(joined => joined.EquipamentoId == equipamento.Id)
                        .ToList();

                    equipamentosStatus.Add(new EquipamentoStatus() { Equipamento = equipamento, Status = status[0].Tipo ? "LOCAÇÃO" : "HOMOLOGAÇÃO" });
                }
                catch (Exception ex) 
                {
                    equipamentosStatus.Add(new EquipamentoStatus() { Equipamento = equipamento, Status = "ESTOQUE" });
                }               
            }

            return View(equipamentosStatus);
        }

        //public JsonResult GetEquipamentoId(int id)
        //{
        //    var equipamento = _context.Equipamentos.Find(id);

        //    return Json(equipamento);
        //}
    }
}
