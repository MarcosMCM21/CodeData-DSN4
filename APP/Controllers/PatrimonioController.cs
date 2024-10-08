using CodeData_Connection.Areas.Identity.Data;
using CodeData_Connection.Models.Database.Entidade;
using CodeData_Connection.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeData_Connection.Controllers
{
    public class PatrimonioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatrimonioController(ApplicationDbContext context)
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

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var equipamento = _context.Equipamentos.Find(id);

            return Json(equipamento);
        }

        [HttpPost]
        public IActionResult Update(Equipamento model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var equipamento = _context.Equipamentos.Find(model.Id);

                    if (equipamento == null)
                    {
                        return NotFound();
                    }

                    // Atualizar os campos necessários
                    equipamento.Codigo = model.Codigo;
                    equipamento.Modelo = model.Modelo;
                    equipamento.Descricao = model.Descricao;
                    equipamento.Marca = model.Marca;
                    equipamento.SerialNumber = model.SerialNumber;
                    equipamento.PartNumber = model.PartNumber;

                    // Salvar as alterações
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest($"Erro ao atualizar: {ex.Message}");
                }
            }

            return BadRequest("Dados inválidos");
        }
    }
}
