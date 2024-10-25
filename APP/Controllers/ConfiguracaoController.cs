using CodeData_Connection.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodeData_Connection.Models;
using NuGet.Protocol;
namespace CodeData_Connection.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ConfiguracaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConfiguracaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Usuarios()
        {

            return View();
        }

        public async Task<IActionResult> ObterDadosUsuario()
        {
            var dadosUsuarios = new List<DadosUsuario>();

            var usuarios = await _context.ApplicationUser.ToListAsync();

            if (usuarios == null)
            {
                return NotFound();
            }

            List<string> niveisAcesso = [];

            foreach (var usuario in usuarios)
            {
                var roleUser = await _context.UserRoles.Where(r => r.UserId == usuario.Id).FirstOrDefaultAsync();
                var role = await _context.Roles.Where(r => r.Id == roleUser.RoleId).FirstOrDefaultAsync();
                dadosUsuarios.Add(new DadosUsuario { Usuario = usuario, NivelAcesso = role.Name });

                if (!niveisAcesso.Contains(role.Name)) niveisAcesso.Add(role.Name);
            }

            ViewBag.NiveisAcesso = niveisAcesso;

            return PartialView("_DadosUsuario", dadosUsuarios);
        }

        public IActionResult DetalhesUsuario(string id)
        {
            // 1. Buscar o usuario pelo ID, incluindo os dados relacionados (se necessário)
            var dadosUsuario = ObterDetalhesUsuario(id);

            if (dadosUsuario == null)
            {
                return NotFound();
            }

            return View(dadosUsuario);
        }

        public DetalhesUsuarioViewModel ObterDetalhesUsuario(string usuarioId)
        {
            // 1. Obter a solicitação correspondente ao ID
            var usuario = _context.Users.FirstOrDefault(c => c.Id == usuarioId);

            if (usuario == null)
            {
                return null; // ou lançar uma exceção se preferir
            }

            // 2. Obter as solicitações associados ao cliente
            List<DadosSolicitacao> dadosSolicitacoes = new List<DadosSolicitacao>();
            var solicitacoes = _context.Solicitacoes
                .Include(c => c.Cliente)
                .Where(s => s.UserId == usuarioId)
                .ToList();

            foreach (var solicitacao in solicitacoes)
            {
                dadosSolicitacoes.Add(new DadosSolicitacao
                {
                    Solicitacao = solicitacao,
                    Cliente = solicitacao.Cliente.Nome,
                    Vendedor = ""
                });
            }

            var gerente = _context.Users.Where(g => g.Id == usuario.GerenteID).FirstOrDefault();

            // 3. Criar a instância do ViewModel e popular suas propriedades
            var detalhesUsuario = new DetalhesUsuarioViewModel
            {
                Usuario = usuario,
                Solicitacoes = dadosSolicitacoes,
                Gerente = gerente
            };

            return detalhesUsuario; // Retorna o ViewModel preenchido
        }

        [HttpPost]
        public IActionResult BroquearUsuario(string id)
        {
            Console.WriteLine("Usuário irá ser broqueado");
            var usuario = _context.Users.FirstOrDefault(u => u.Id == id);

            Console.WriteLine(usuario.ToJson()); 
            if (usuario == null)
            {
                Console.WriteLine("Deu ruim");
                return NotFound();
            }

            usuario.LockoutEnabled = true;
            usuario.LockoutEnd = DateTime.Now.AddYears(100);

            _context.SaveChanges();

            return View("Usuarios");
        }

        [HttpPost]
        public IActionResult DesbroquearUsuario(string id)
        {
            Console.WriteLine("Usuário irá ser debroqueado");
            var usuario = _context.Users.FirstOrDefault(u => u.Id == id);

            Console.WriteLine(usuario.ToJson());

            if (usuario == null)
            {
                Console.WriteLine("Deu ruim");
                return NotFound();
            }

            usuario.LockoutEnd = DateTime.Now;

            _context.SaveChanges();

            return View("Usuarios");
        }
    }

    public class DetalhesUsuarioViewModel
    {
        public ApplicationUser Usuario { get; set; }
        public ApplicationUser? Gerente { get; set; }
        public List<DadosSolicitacao> Solicitacoes { get; set; }
    }

    public class DadosUsuario
    {
        public ApplicationUser Usuario { get; set; }
        public string? NivelAcesso { get; set; }
    }
}
