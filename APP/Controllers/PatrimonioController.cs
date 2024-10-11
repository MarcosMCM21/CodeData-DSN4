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
            var dadosPatrimonio = new DadosPatrimonio { DadosEquipamento = new List<DadosEquipamento>(), ListaEnderecos = new List<Endereco>()};

            var equipamentos = await _context.Equipamentos.ToListAsync();

            if (equipamentos == null) return NotFound("Nenhum equipamento encontrado!");

            foreach (var equipamento in equipamentos)
            {
                dadosPatrimonio.DadosEquipamento.Add(DadosEquipamento(equipamento, false));
            }

            dadosPatrimonio.ListaEnderecos = EnderecosEquipamento();

            return View(dadosPatrimonio);
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

                TempData["Mensagem"] = "Patrimônio atualizado criado com sucesso!";
                TempData["TipoMensagem"] = "success"; // ou "error" para fracasso

                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Mensagem"] = "Erro ao atualizar o patrimônio!";
                TempData["TipoMensagem"] = "error";

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
                        var vendedor = _context.ApplicationUser
                            .Where(v => v.Id == dadosEquipamento.DadosSolicitacao.Solicitacao.UserId)
                            .Select(v => new { FirstName = v.FirstName, LastName = v.LastName })
                            .FirstOrDefault();

                        dadosEquipamento.DadosSolicitacao.Cliente = cliente;
                        dadosEquipamento.DadosSolicitacao.Vendedor = $"{vendedor.FirstName} {vendedor.LastName}";

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

        public List<Endereco> EnderecosEquipamento()
        {
            var equipamentos = _context.Equipamentos.ToList();
            var listaEnderecos = new List<Endereco>();

            foreach (var equipamento in equipamentos) 
            {
                var enderecoId = _context.MovimentacoesEquipamentos
                    .Where(me => me.EquipamentoId == equipamento.Id)
                    .Select(me => me.EnderecoId)
                    .FirstOrDefault();

                if (enderecoId != null)
                {
                    var endereco = _context.Enderecos.FirstOrDefault(e => e.Id == enderecoId);
                    listaEnderecos.Add(endereco);
                }
                else
                {
                    var enderecoIdEstoque = _context.Estoques
                    .Where(e => e.Id == equipamento.EstoqueId)
                    .Select(e => e.EnderecoId)
                    .FirstOrDefault();

                    var endereco = _context.Enderecos.FirstOrDefault(e => e.Id == enderecoId);
                    listaEnderecos.Add(endereco);
                }
            }

            return listaEnderecos;
        }
    }
}