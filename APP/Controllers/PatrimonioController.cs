using CodeData_Connection.Areas.Identity.Data;
using CodeData_Connection.Models.Database.Entidade;
using CodeData_Connection.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

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
            var equipamentosStatus = new List<DadosEquipamento>();

            if (equipamentos == null)
            {
                Console.WriteLine("Nenhum equipamento encontrado!");
            }

            foreach (var equipamento in equipamentos)
            {
                equipamentosStatus.Add(DadosByEquip(equipamento, false));
            }

            return View(equipamentosStatus);
        }

        public IActionResult Equipamento(int id)
        {
            var equipamento = _context.Equipamentos.Find(id);

            return View(DadosByEquip(equipamento, true));
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

        public DadosEquipamento DadosByEquip(Equipamento equipamento, bool getEndereco)
        {
            DadosEquipamento dadosEquipamento = new DadosEquipamento();
            
            try
            {
                var movimentacaoSolicitacao = _context.MovimentacoesEquipamentos
                    .Where(me => me.EquipamentoId == equipamento.Id)
                    .Select(me => me.SolicitacaoId)
                    .FirstOrDefault();

                // Verifica se encontrou uma SolicitacaoId
                if (movimentacaoSolicitacao != default(int)) // Aqui, assumimos que SolicitacaoId é do tipo int. Ajuste o tipo conforme necessário.
                {
                    // Usa o SolicitacaoId para obter o tipo de solicitacao
                    bool status = _context.Solicitacoes.FirstOrDefault(s => s.Id == movimentacaoSolicitacao).Tipo;
                    dadosEquipamento = new DadosEquipamento() { Equipamento = equipamento, Status = status ? "LOCAÇÃO" : "HOMOLOGAÇÃO" };
                }
                else
                {
                    dadosEquipamento = new DadosEquipamento() { Equipamento = equipamento, Status = "ESTOQUE" };
                    Console.WriteLine("Solicitação não encontrada para o equipamento especificado.");
                }

                if (getEndereco)
                {
                    Console.WriteLine("Procurar Endereço do Equipamento");
                    // Primeiro, filtra MovimentacoesEquipamentos para obter o EnderecoId relacionado ao EquipamentoId
                    var movimentacaoEndereco = _context.MovimentacoesEquipamentos
                        .Where(me => me.EquipamentoId == equipamento.Id)
                        .Select(me => me.EnderecoId)
                        .FirstOrDefault();

                    Console.WriteLine(movimentacaoEndereco.ToJson());

                    // Verifica se encontrou um EnderecoId
                    if (movimentacaoEndereco != default(int)) // Aqui, assumimos que EnderecoId é do tipo int. Ajuste o tipo conforme necessário.
                    {
                        // Usa o EnderecoId para obter os dados completos do endereço
                        var enderecoEquipamento = _context.Enderecos.FirstOrDefault(e => e.Id == movimentacaoEndereco);
                        Console.WriteLine(enderecoEquipamento.ToJson());

                        dadosEquipamento.Endereco = enderecoEquipamento;
                    }
                    else
                    {
                        Console.WriteLine("Endereço não encontrado para o equipamento especificado.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine(dadosEquipamento.ToJson());

            return dadosEquipamento;
        }
    }
}
