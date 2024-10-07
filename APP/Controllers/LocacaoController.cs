using CodeData_Connection.Areas.Identity.Data;
using CodeData_Connection.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CodeData_Connection.Controllers
{
    public class LocacaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocacaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var locacoesVC = new List<SolicitacaoVendedorCliente>();

            if (User.IsInRole("Usuario"))
            {
                var locacoes = await _context.Solicitacoes.Where(h => !h.Tipo && h.UserId == usuarioId).ToListAsync();

                foreach (var locacao in locacoes)
                {
                    string cliente = _context.Clientes.FindAsync(locacao.ClienteId).Result.Nome;
                    string vendedor = _context.ApplicationUser.FindAsync(locacao.UserId).Result.Email;

                    locacoesVC.Add(new SolicitacaoVendedorCliente() { Solicitacao = locacao, Cliente = cliente });
                }

                return View(locacoesVC);
            }

            if (User.IsInRole("Gerente"))
            {
                var vendedoresIds = await _context.ApplicationUser.Where(u => u.GerenteID == usuarioId).Select(u => u.Id).ToListAsync();
                var locacoes = await _context.Solicitacoes.Where(h => !h.Tipo && vendedoresIds.Contains(h.UserId)).ToListAsync();

                foreach (var locacao in locacoes)
                {
                    string cliente = _context.Clientes.FindAsync(locacao.ClienteId).Result.Nome;
                    string vendedor = _context.ApplicationUser.FindAsync(locacao.UserId).Result.Email;

                    locacoesVC.Add(new SolicitacaoVendedorCliente() { Solicitacao = locacao, Cliente = cliente });
                }

                return View(locacoesVC);
            }

            if (User.IsInRole("Administrador"))
            {
                var locacoes = await _context.Solicitacoes.Where(h => !h.Tipo).ToListAsync();

                foreach (var locacao in locacoes)
                {
                    string cliente = _context.Clientes.FindAsync(locacao.ClienteId).Result.Nome;
                    string vendedor = _context.ApplicationUser.FindAsync(locacao.UserId).Result.Email;

                    locacoesVC.Add(new SolicitacaoVendedorCliente() { Solicitacao = locacao, Cliente = cliente });
                }

                return View(locacoesVC);
            }

            return View();
        }
    }
}
