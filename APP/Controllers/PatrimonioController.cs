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
            ViewBag.ExibirTelaDeCarregamento = true;

            // 1. Obter os equipamentos do banco de dados
            var equipamentos = await _context.Equipamentos.ToListAsync();

            if (equipamentos == null || !equipamentos.Any())
            {
                return NotFound("Nenhum equipamento encontrado!");
            }

            // 2. Criar uma instância de DadosPatrimonio e popular suas propriedades
            var dadosPatrimonio = new DadosPatrimonioViewModel
            {
                DadosEquipamento = equipamentos.Select(e => ObterDadosBasicosEquipamento(e)).ToList(),
                ListaEnderecos = ObterEnderecosEquipamentos()
            };

            return View(dadosPatrimonio);
        }

        public async Task<IActionResult> Detalhes(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // 1. Buscar o equipamento pelo ID, incluindo os dados relacionados (se necessário)
            var equipamento = await _context.Equipamentos
                // .Include(e => e.PropriedadeRelacionada) // Inclua propriedades relacionadas se necessário
                .FirstOrDefaultAsync(e => e.Id == id);

            if (equipamento == null)
            {
                return NotFound("Equipamento não encontrado!");
            }

            // 2. Obter os dados detalhados do equipamento
            var dadosEquipamento = ObterDetalhesEquipamento(equipamento);

            return View(dadosEquipamento);
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // 1. Buscar o equipamento pelo ID
            var equipamento = await _context.Equipamentos.FindAsync(id);

            if (equipamento == null)
            {
                return NotFound("Equipamento não encontrado!");
            }

            // 2. Criar um ViewModel para a view de edição (recomendado)
            var viewModel = new EditarEquipamentoViewModel
            {
                Equipamento = equipamento,
                Documentos = await _context.Documentos.Select(d => new EstoqueDocumento { Id = d.Id, Nome = d.Nome }).ToListAsync(),
                Estoques = await _context.Estoques.Select(e => new EstoqueDocumento { Id = e.Id, Nome = e.Nome }).ToListAsync()
            };

            return View(viewModel);
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
                    // 1. Marcar a entidade como modificada
                    _context.Entry(equipamento).State = EntityState.Modified;

                    // 2. Impedir a atualização da propriedade DataCadastro
                    _context.Entry(equipamento).Property(e => e.DataCadastro).IsModified = false;

                    // 3. Salvar as alterações no banco de dados
                    await _context.SaveChangesAsync();

                    TempData["Mensagem"] = "Patrimônio atualizado com sucesso!";
                    TempData["TipoMensagem"] = "success";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Equipamentos.Any(e => e.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Re-lançar a exceção para tratamento em outro nível
                    }
                }
            }

            // 4. Se houver erros de validação, exibir a view de edição novamente com as mensagens de erro
            var viewModel = new EditarEquipamentoViewModel
            {
                Equipamento = equipamento,
                Documentos = await _context.Documentos.Select(d => new EstoqueDocumento { Id = d.Id, Nome = d.Nome }).ToListAsync(),
                Estoques = await _context.Estoques.Select(e => new EstoqueDocumento { Id = e.Id, Nome = e.Nome }).ToListAsync()
            };

            TempData["Mensagem"] = "Erro ao atualizar o patrimônio!";
            TempData["TipoMensagem"] = "error";

            // Logar os erros de validação (opcional)
            foreach (var erro in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(erro.ErrorMessage);
                Console.WriteLine(erro.Exception);
            }

            return View(viewModel);
        }

        // Método para obter os dados básicos do equipamento, incluindo o status
        public DetalhesEquipamentoViewModel ObterDadosBasicosEquipamento(Equipamento equipamento)
        {
            var dadosEquipamento = new DetalhesEquipamentoViewModel { Equipamento = equipamento, Status = "ESTOQUE" };

            try
            {
                var movimentacao = _context.MovimentacoesEquipamentos
                    .FirstOrDefault(me => me.EquipamentoId == equipamento.Id);

                if (movimentacao != null)
                {
                    var solicitacao = _context.Solicitacoes.FirstOrDefault(s => s.Id == movimentacao.SolicitacaoId);

                    if (solicitacao != null)
                    {
                        dadosEquipamento.DadosSolicitacao = new DadosSolicitacao
                        {
                            Solicitacao = solicitacao,
                            Cliente = ObterNomeCliente(solicitacao.ClienteId),
                            Vendedor = ObterNomeVendedor(solicitacao.UserId)
                        };

                        dadosEquipamento.Status = solicitacao.Tipo ? "LOCAÇÃO" : "HOMOLOGAÇÃO";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()); // Log de erro
            }

            return dadosEquipamento;
        }

        // Método para obter os detalhes adicionais do equipamento, como endereço, estoque e documento
        public DetalhesEquipamentoViewModel ObterDetalhesEquipamento(Equipamento equipamento)
        {
            var dadosEquipamento = ObterDadosBasicosEquipamento(equipamento); // Reutiliza o método básico

            try
            {
                var movimentacao = _context.MovimentacoesEquipamentos
                    .FirstOrDefault(me => me.EquipamentoId == equipamento.Id);

                if (movimentacao != null)
                {
                    dadosEquipamento.Endereco = _context.Enderecos.FirstOrDefault(e => e.Id == movimentacao.EnderecoId);
                }
                else
                {
                    var estoque = _context.Estoques.FirstOrDefault(s => s.Id == equipamento.EstoqueId);
                    if (estoque != null)
                    {
                        dadosEquipamento.Estoque = estoque;
                        dadosEquipamento.Endereco = _context.Enderecos.FirstOrDefault(e => e.Id == estoque.EnderecoId);
                    }
                }

                dadosEquipamento.Documento = _context.Documentos.FirstOrDefault(d => d.Id == equipamento.DocumentoId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()); // Log de erro
            }

            return dadosEquipamento;
        }


        // Método para obter o nome do cliente
        private string ObterNomeCliente(int clienteId)
        {
            return _context.Clientes.FirstOrDefault(c => c.Id == clienteId)?.Nome;
        }

        // Método para obter o nome do vendedor
        private string ObterNomeVendedor(string userId)
        {
            var vendedor = _context.ApplicationUser
                .FirstOrDefault(v => v.Id == userId);

            return vendedor != null ? $"{vendedor.FirstName} {vendedor.LastName}" : string.Empty;
        }

        // Método para obter os endereços dos equipamentos
        public List<Endereco> ObterEnderecosEquipamentos()
        {
            // Utiliza uma consulta LINQ para obter os endereços dos equipamentos de forma eficiente
            var enderecos = _context.Equipamentos
                .Select(equipamento => _context.MovimentacoesEquipamentos
                    .Where(me => me.EquipamentoId == equipamento.Id)
                    .Select(me => me.EnderecoId)
                    .FirstOrDefault())
                .Where(enderecoId => enderecoId != null)
                .Select(enderecoId => _context.Enderecos.FirstOrDefault(e => e.Id == enderecoId))
                .ToList();

            return enderecos;
        }
    }

    // ViewModel para a view do Index
    public class DadosPatrimonioViewModel
    {
        public List<DetalhesEquipamentoViewModel> DadosEquipamento { get; set; }
        public List<Endereco> ListaEnderecos { get; set; }
    }

    // ViewModel para a view de detalhes
    public class DetalhesEquipamentoViewModel
    {
        public Equipamento Equipamento { get; set; }
        public Documento? Documento { get; set; }
        public DadosSolicitacao? DadosSolicitacao { get; set; }
        public Estoque? Estoque { get; set; }
        public Endereco? Endereco { get; set; }
        public string Status { get; set; }
    }

    // ViewModel para a view de edição
    public class EditarEquipamentoViewModel
    {
        public Equipamento Equipamento { get; set; }
        public List<EstoqueDocumento> Documentos { get; set; }
        public List<EstoqueDocumento> Estoques { get; set; }
    }
}