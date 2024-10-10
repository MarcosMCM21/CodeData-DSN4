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

            if (equipamentos == null) return NotFound("Nenhum equipamento encontrado!");

            foreach (var equipamento in equipamentos)
            {
                equipamentosStatus.Add(DadosEquipamento(equipamento, false));
            }

            return View(equipamentosStatus);
        }

        public async Task<IActionResult> Detalhes(int? id)
        {
            if (id == null) return NotFound();
            
            var equipamento = await _context.Equipamentos.FirstOrDefaultAsync(e => e.Id == id);

            if (equipamento == null) return NotFound("Equipamento não encontrado!");

            return View(DadosEquipamento(equipamento, true));
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();

            var equipamento = await _context.Equipamentos.FirstOrDefaultAsync(e => e.Id == id);

            if (equipamento == null) return NotFound("Equipamento não encontrado!");

            // Buscar documentos e estoques do banco de dados
            ViewBag.Documentos = await _context.Documentos.Select(d => new EstoqueDocumento { Id = d.Id, Nome = d.Nome }).ToListAsync();
            ViewBag.Estoques = await _context.Estoques.Select(e => new EstoqueDocumento { Id = e.Id, Nome = e.Nome }).ToListAsync();

            return View(equipamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("Id, Codigo, Modelo, Descricao, Marca, SerialNumber, PartNumber, Condicao, EstoqueId, DocumentoId")] Equipamento equipamento)
        {
            if (id != equipamento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipamento);

                    _context.Entry(equipamento).State = EntityState.Modified;
                    _context.Entry(equipamento).Property(e => e.DataCadastro).IsModified = false; // Impede atualização

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Equipamentos.Contains(equipamento))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var erro in ModelState.Values.SelectMany(v => v.Errors))
                {
                    // Acesse a mensagem de erro: erro.ErrorMessage
                    // Acesse a exceção, se houver: erro.Exception
                    Console.WriteLine(erro.ErrorMessage);
                    Console.WriteLine(erro.Exception);
                }
            }

            // Buscar documentos e estoques do banco de dados
            ViewBag.Documentos = await _context.Documentos.Select(d => new EstoqueDocumento { Id = d.Id, Nome = d.Nome }).ToListAsync();
            ViewBag.Estoques = await _context.Estoques.Select(e => new EstoqueDocumento { Id = e.Id, Nome = e.Nome }).ToListAsync();

            return View(equipamento);
        }

        public DadosEquipamento DadosEquipamento(Equipamento equipamento, bool getDetalhes)
        {
            DadosEquipamento dadosEquipamento = new DadosEquipamento() { Equipamento = equipamento, Status = "ESTOQUE" };
            
            try
            {
                var detalhes = _context.MovimentacoesEquipamentos
                    .Where(me => me.EquipamentoId == equipamento.Id)
                    .Select(me => new { SolicitacaoId = me.SolicitacaoId, EnderecoId = me.EnderecoId })
                    .FirstOrDefault();

                if (detalhes == null)
                {
                    var estoque = _context.Estoques.FirstOrDefault(s => s.Id == equipamento.EstoqueId);
                    if (estoque != null) dadosEquipamento.Estoque = estoque;
                } 
                else
                {
                    var solicitacaoEquipamento = _context.Solicitacoes.FirstOrDefault(s => s.Id == detalhes.SolicitacaoId);

                    if (solicitacaoEquipamento != null) 
                    {
                        dadosEquipamento.DadosSolicitacao = new DadosSolicitacao();

                        dadosEquipamento.DadosSolicitacao.Solicitacao = solicitacaoEquipamento;
                        dadosEquipamento.Status = dadosEquipamento.DadosSolicitacao.Solicitacao.Tipo ? "LOCAÇÃO" : "HOMOLOGAÇÃO";
                    }
                }

                if (getDetalhes)
                {
                    if (dadosEquipamento.DadosSolicitacao != null)
                    {
                        string cliente = _context.Clientes.FirstOrDefault(c => c.Id == dadosEquipamento.DadosSolicitacao.Solicitacao.ClienteId).Nome;
                        string vendedor = _context.ApplicationUser.FirstOrDefault(v => v.Id == dadosEquipamento.DadosSolicitacao.Solicitacao.UserId).Email;

                        dadosEquipamento.DadosSolicitacao.Cliente = cliente;
                        dadosEquipamento.DadosSolicitacao.Vendedor = vendedor;

                        var enderecoEquipamento = _context.Enderecos.FirstOrDefault(e => e.Id == detalhes.EnderecoId);
                        if (enderecoEquipamento != null) dadosEquipamento.Endereco = enderecoEquipamento;
                    }
                    else
                    {
                        var enderecoEquipamento = _context.Enderecos.FirstOrDefault(e => e.Id == dadosEquipamento.Estoque.EnderecoId);
                        if (enderecoEquipamento != null) dadosEquipamento.Endereco = enderecoEquipamento;
                    }

                    var documentoEquipamento = _context.Documentos.FirstOrDefault(d => d.Id == equipamento.DocumentoId);
                    if (documentoEquipamento != null) dadosEquipamento.Documento = documentoEquipamento;
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