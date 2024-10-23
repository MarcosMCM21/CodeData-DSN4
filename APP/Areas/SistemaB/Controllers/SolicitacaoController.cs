using CodeData_Connection.Areas.Identity.Data;
using CodeData_Connection.Models;
using CodeData_Connection.Models.Database.Entidade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CodeData_Connection.Areas.SistemaB.Controllers
{
    [Authorize]
    [Area("SistemaB")]
    public class SolicitacaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SolicitacaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(bool tipoSolicitacao)
        {
            ViewBag.TipoSolicitacao = tipoSolicitacao;

            return View();
        }

        public async Task<IActionResult> ObterDadosSolicitacoes(bool tipoSolicitacao)
        {
            // 1. Obter as locações do banco de dados
            var solicitacoes = await ObterDadosBasicosSolicitacoes(tipoSolicitacao); // Usar o método assíncrono

            if (solicitacoes.Any() == false)
            {
                return NotFound();
            }

            // 2. Criar uma instância de DadosPatrimonio e popular suas propriedades
            var dadosSolicitacoes = new DadosSolicitacaoViewModel
            {
                DadosSolicitacao = solicitacoes,
                ListaEnderecos = ObterEnderecosSolicitacoes(false)
            };

            return PartialView("_DadosSolicitacoes", dadosSolicitacoes);
        }

        public IActionResult Detalhes(int id)
        {
            // 1. Buscar a solicitação pelo ID, incluindo os dados relacionados (se necessário)
            var dadosSolicitacao = ObterDetalhesSolicitacao(id);

            if (dadosSolicitacao == null)
            {
                return NotFound();
            }

            return View(dadosSolicitacao);
        }

        public async Task<List<DadosSolicitacao>> ObterDadosBasicosSolicitacoes(bool tipoSolicitacao)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 1. Obter as solicitações com base na regra do usuário (com Include)
            List<Solicitacao> solicitacoes = [];
            List<string> clientes = [];
            List<string> vendedores = [];

            if (User.IsInRole("Usuario"))
            {
                solicitacoes = await _context.Solicitacoes
                    .Include(s => s.Cliente)
                    .Where(h => h.Tipo == tipoSolicitacao && h.UserId == usuarioId)
                    .ToListAsync();
            }
            else if (User.IsInRole("Gerente"))
            {
                var vendedoresIds = await _context.ApplicationUser
                    .Where(u => u.GerenteID == usuarioId)
                    .Select(u => u.Id)
                    .ToListAsync();

                solicitacoes = await _context.Solicitacoes
                    .Include(s => s.Cliente)
                    .Include(s => s.User)
                    .Where(h => h.Tipo == tipoSolicitacao && vendedoresIds.Contains(h.UserId))
                    .ToListAsync();
            }
            else if (User.IsInRole("Administrador"))
            {
                solicitacoes = await _context.Solicitacoes
                    .Include(s => s.Cliente)
                    .Include(s => s.User)
                    .Where(h => h.Tipo == tipoSolicitacao)
                    .ToListAsync();
            }

            List<DadosSolicitacao> dadosSolicitacoes = [];

            foreach (var solicitacao in solicitacoes)
            {
                var vendedor = solicitacao.User != null ? $"{solicitacao.User.FirstName} {solicitacao.User.LastName}" : "";

                dadosSolicitacoes.Add(new DadosSolicitacao
                {
                    Solicitacao = solicitacao,
                    Cliente = solicitacao.Cliente.Nome,
                    Vendedor = vendedor
                });

                if (!vendedores.Contains(vendedor)) clientes.Add(solicitacao.Cliente.Nome);

                if (!vendedores.Contains(vendedor)) vendedores.Add(vendedor);
            }

            ViewBag.Clientes = clientes;
            ViewBag.Vendedores = vendedores;

            // 2. Mapear as solicitações para o ViewModel
            return dadosSolicitacoes;
        }

        public DetalhesSolicitacaoViewModel ObterDetalhesSolicitacao(int solicitacaoId)
        {
            // 1. Obter a solicitação correspondente ao ID
            var solicitacao = _context.Solicitacoes
                    .Include(s => s.Cliente)
                    .Include(s => s.User)
                    .FirstOrDefault(s => s.Id == solicitacaoId);

            if (solicitacao == null)
            {
                return null; // ou lançar uma exceção se preferir
            }

            // 2. Obter os documentos associados à solicitação
            var documentos = ObterDocumentosSolicitacao(solicitacaoId);

            // 3. Obter os documentos associados à solicitação
            var equipamentos = ObterEquipamentosSolicitacao(solicitacaoId);

            // 4. Obter o endereço associado à solicitação
            var endereco = ObterEnderecoSolicitacao(solicitacaoId);

            // 5. Criar a instância do ViewModel e popular suas propriedades
            var detalhesSolicitacao = new DetalhesSolicitacaoViewModel
            {
                DadosSolicitacao = new DadosSolicitacao
                {
                    Solicitacao = solicitacao,
                    Cliente = solicitacao.Cliente.Nome,
                    Vendedor = solicitacao.User != null ? $"{solicitacao.User.FirstName} {solicitacao.User.LastName}" : ""
                },
                Documentos = documentos,
                Equipamentos = equipamentos,
                Endereco = endereco
            };

            return detalhesSolicitacao; // Retorna o ViewModel preenchido
        }

        // Método para obter os endereço da solicitação
        public Endereco ObterEnderecoSolicitacao(int solicitacaoId)
        {
            // Obtém o EnderecoId diretamente da tabela MovimentacoesEquipamentos
            var enderecoId = _context.MovimentacoesEquipamentos
                .Where(me => me.SolicitacaoId == solicitacaoId) // Filtra pela solicitação ID
                .Select(me => me.EnderecoId)
                .FirstOrDefault(); // Pega o primeiro EnderecoId correspondente ou null

            // Retorna o endereço correspondente ou null caso não encontre
            return enderecoId != null ? _context.Enderecos.FirstOrDefault(e => e.Id == enderecoId) : null;
        }

        // Método para obter os endereços das solicitações
        public List<Endereco> ObterEnderecosSolicitacoes(bool tipoSolicitacao)
        {
            // Utiliza uma consulta LINQ para obter os endereços das solicitacoes de forma eficiente
            var enderecos = _context.MovimentacoesEquipamentos
                   .Join(
                       _context.Solicitacoes,
                       movimentacao => movimentacao.SolicitacaoId,
                       solicitacao => solicitacao.Id,
                       (movimentacao, solicitacao) => new { movimentacao.EnderecoId, solicitacao.Tipo }
                   )
                   .Where(ms => ms.Tipo == tipoSolicitacao) // Filtra pelo tipo de solicitação (booleano)
                   .Select(ms => ms.EnderecoId)
                   .Distinct() // Remove duplicatas de EnderecoId, se houver
                   .Select(enderecoId => _context.Enderecos.FirstOrDefault(e => e.Id == enderecoId))
                   .Where(endereco => endereco != null)
                   .ToList();

            return enderecos;
        }

        public List<Documento> ObterDocumentosSolicitacao(int solicitacaoId)
        {
            // Obtém todos os IDs de documentos da tabela MovimentacoesEquipamentos
            var documentoIds = _context.MovimentacoesEquipamentos
                .Where(me => me.SolicitacaoId == solicitacaoId) // Filtra pela solicitação ID
                .Select(me => me.DocumentoId) // Supondo que você tenha um campo DocumentoId
                .Distinct() // Remove duplicatas, se houver
                .ToList(); // Converte para lista

            // Busca os documentos correspondentes na tabela Documentos
            return _context.Documentos
                .Where(d => documentoIds.Contains(d.Id)) // Filtra os documentos pelo ID
                .ToList(); // Retorna a lista de documentos
        }

        public List<Equipamento> ObterEquipamentosSolicitacao(int solicitacaoId)
        {
            // Obtém todos os IDs de documentos da tabela MovimentacoesEquipamentos
            var equipamentosIds = _context.MovimentacoesEquipamentos
                .Where(me => me.SolicitacaoId == solicitacaoId) // Filtra pela solicitação ID
                .Select(me => me.EquipamentoId) // Supondo que você tenha um campo DocumentoId
                .Distinct() // Remove duplicatas, se houver
                .ToList(); // Converte para lista

            // Busca os documentos correspondentes na tabela Documentos
            return _context.Equipamentos
                .Where(e => equipamentosIds.Contains(e.Id)) // Filtra os documentos pelo ID
                .ToList(); // Retorna a lista de documentos
        }
    }

    public class DadosSolicitacaoViewModel
    {
        public List<DadosSolicitacao> DadosSolicitacao { get; set; }
        public List<Endereco> ListaEnderecos { get; set; }
    }

    public class DetalhesSolicitacaoViewModel
    {
        public DadosSolicitacao DadosSolicitacao { get; set; }
        public List<Documento>? Documentos { get; set; }
        public List<Equipamento>? Equipamentos { get; set; }
        public Endereco? Endereco { get; set; }
    }
}