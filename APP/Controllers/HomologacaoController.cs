using CodeData_Connection.Areas.Identity.Data;
using CodeData_Connection.Models;
using CodeData_Connection.Models.Database.Entidade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Security.Claims;

namespace CodeData_Connection.Controllers
{
    public class HomologacaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomologacaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var homologacoesVC = new List<SolicitacaoVendedorCliente>();

            if (User.IsInRole("Usuario"))
            {
                var homologacoes = await _context.Solicitacoes.Where(h => !h.Tipo && h.UserId == usuarioId).ToListAsync();

                foreach (var homologacao in homologacoes)
                {
                    string cliente = _context.Clientes.FindAsync(homologacao.ClienteId).Result.Nome;
                    string vendedor = _context.ApplicationUser.FindAsync(homologacao.UserId).Result.Email;

                    homologacoesVC.Add(new SolicitacaoVendedorCliente() { Solicitacao = homologacao, Cliente = cliente });
                }

                return View(homologacoesVC);
            }

            if (User.IsInRole("Gerente"))
            {
                var vendedoresIds = await _context.ApplicationUser.Where(u => u.GerenteID == usuarioId).Select(u => u.Id).ToListAsync();
                var homologacoes = await _context.Solicitacoes.Where(h => !h.Tipo && vendedoresIds.Contains(h.UserId)).ToListAsync();

                foreach (var homologacao in homologacoes)
                {
                    string cliente = _context.Clientes.FindAsync(homologacao.ClienteId).Result.Nome;
                    string vendedor = _context.ApplicationUser.FindAsync(homologacao.UserId).Result.Email;

                    homologacoesVC.Add(new SolicitacaoVendedorCliente() { Solicitacao = homologacao, Cliente = cliente, Vendedor = vendedor });
                }

                return View(homologacoesVC);
            }

            if (User.IsInRole("Administrador"))
            {
                var homologacoes = await _context.Solicitacoes.Where(h => !h.Tipo).ToListAsync();

                foreach (var homologacao in homologacoes)
                {
                    string cliente = _context.Clientes.FindAsync(homologacao.ClienteId).Result.Nome;
                    string vendedor = _context.ApplicationUser.FindAsync(homologacao.UserId).Result.Email;

                    homologacoesVC.Add(new SolicitacaoVendedorCliente() { Solicitacao = homologacao, Cliente = cliente, Vendedor = vendedor });
                }

                return View(homologacoesVC);
            }

            return View();
        }
    }
}
