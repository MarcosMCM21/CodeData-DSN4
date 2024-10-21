using CodeData_Connection.Areas.Identity.Data;
using CodeData_Connection.Models.Database.Entidade;
using CodeData_Connection.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CodeData_Connection.Controllers
{
    [Authorize]
    public class ClienteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClienteController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ObterDadosCliente()
        {
            var clientes = await _context.Clientes.ToListAsync();

            return PartialView("_DadosCliente", clientes);
        }

        public IActionResult Detalhes(int id)
        {
            // 1. Buscar o cliente pelo ID, incluindo os dados relacionados (se necessário)
            var dadosCliente = ObterDetalhesCliente(id);

            if (dadosCliente == null)
            {
                return NotFound();
            }

            return View(dadosCliente);
        }

        public DetalhesClienteViewModel ObterDetalhesCliente(int clienteId)
        {
            // 1. Obter a solicitação correspondente ao ID
            var cliente = _context.Clientes.FirstOrDefault(c => c.Id == clienteId);

            if (cliente == null)
            {
                return null; // ou lançar uma exceção se preferir
            }

            // 2. Obter as solicitações associados ao cliente
            List<DadosSolicitacao> dadosSolicitacoes = new List<DadosSolicitacao>();
            var solicitacoes = _context.Solicitacoes
                .Include(s => s.User)
                .Where(s => s.ClienteId == clienteId)
                .ToList();

            foreach (var solicitacao in solicitacoes)
            {
                dadosSolicitacoes.Add(new DadosSolicitacao
                {
                    Solicitacao = solicitacao,
                    Cliente = "",
                    Vendedor = solicitacao.User != null ? $"{solicitacao.User.FirstName} {solicitacao.User.LastName}" : ""
                });
            }

            var endereco = _context.Enderecos.Where(e => e.Id == cliente.EnderecoId).FirstOrDefault();

            // 3. Criar a instância do ViewModel e popular suas propriedades
            var detalhesCliente = new DetalhesClienteViewModel
            {
                Cliente = cliente,
                Solicitacoes = dadosSolicitacoes,
                Endereco = endereco
            };

            return detalhesCliente; // Retorna o ViewModel preenchido
        }
    }

    public class DetalhesClienteViewModel
    {
        public Cliente Cliente { get; set; }
        public List<DadosSolicitacao> Solicitacoes { get; set; }
        public Endereco Endereco { get; set; }
    }
}
