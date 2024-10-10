using CodeData_Connection.Areas.Identity.Data;
using CodeData_Connection.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Security.Claims;

namespace CodeData_Connection.Controllers
{
    public class SolicitacaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SolicitacaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Homologacao()
        {
            ViewData["Title"] = "Homologação";
            var homologacoes = await GetSolicitacoes(false);

            return View(homologacoes);
        }

        public async Task<IActionResult> Locacao()
        {
            ViewData["Title"] = "Locação";
            var locacoes = await GetSolicitacoes(true);

            return View(locacoes);
        }

        public async Task<List<DadosSolicitacao>> GetSolicitacoes(bool tipoSolicitacao)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var solicitacoesVC = new List<DadosSolicitacao>();

            if (User.IsInRole("Usuario"))
            {
                var solicitacoes = await _context.Solicitacoes.Where(h => h.Tipo == tipoSolicitacao && h.UserId == usuarioId).ToListAsync();

                foreach (var homologacao in solicitacoes)
                {
                    string cliente = _context.Clientes.FindAsync(homologacao.ClienteId).Result.Nome;
                    string vendedor = _context.ApplicationUser.FindAsync(homologacao.UserId).Result.Email;

                    solicitacoesVC.Add(new DadosSolicitacao() { Solicitacao = homologacao, Cliente = cliente });
                }
            }

            if (User.IsInRole("Gerente"))
            {
                var vendedoresIds = await _context.ApplicationUser.Where(u => u.GerenteID == usuarioId).Select(u => u.Id).ToListAsync();
                var solicitacoes = await _context.Solicitacoes.Where(h => h.Tipo == tipoSolicitacao && vendedoresIds.Contains(h.UserId)).ToListAsync();

                foreach (var homologacao in solicitacoes)
                {
                    string cliente = _context.Clientes.FindAsync(homologacao.ClienteId).Result.Nome;
                    string vendedor = _context.ApplicationUser.FindAsync(homologacao.UserId).Result.Email;

                    solicitacoesVC.Add(new DadosSolicitacao() { Solicitacao = homologacao, Cliente = cliente, Vendedor = vendedor });
                }
            }

            if (User.IsInRole("Administrador"))
            {
                var solicitacoes = await _context.Solicitacoes.Where(h => h.Tipo == tipoSolicitacao).ToListAsync();

                foreach (var homologacao in solicitacoes)
                {
                    string cliente = _context.Clientes.FindAsync(homologacao.ClienteId).Result.Nome;
                    string vendedor = _context.ApplicationUser.FindAsync(homologacao.UserId).Result.Email;

                    solicitacoesVC.Add(new DadosSolicitacao() { Solicitacao = homologacao, Cliente = cliente, Vendedor = vendedor });
                }
            }

            return solicitacoesVC;
        }
    }
}