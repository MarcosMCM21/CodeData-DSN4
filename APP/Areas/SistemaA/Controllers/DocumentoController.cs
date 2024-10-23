using CodeData_Connection.Areas.Identity.Data;
using CodeData_Connection.Models.Database.Entidade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeData_Connection.Areas.SistemaA.Controllers
{
    [Authorize]
    [Area("SistemaA")]
    public class DocumentoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DocumentoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ObterDadosDocumento()
        {
            // 1. Obter os documentos do banco de dados
            var documentos = await _context.Documentos
                .Select(d => new DadosDocumentoViewModel { Id = d.Id, Numero = d.Numero, Nome = d.Nome, Tipo = d.Tipo })
                .ToListAsync();

            if (documentos == null || !documentos.Any())
            {
                return NotFound();
            }

            ViewBag.Tipos = documentos.Select(d => d.Tipo).Distinct().ToList();

            return PartialView("_DadosDocumento", documentos);
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            // 1. Obter os documentos do banco de dados
            var documento = await _context.Documentos
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync();

            if (documento == null)
            {
                return NotFound();
            }

            return View(documento);
        }
    }

    public class DadosDocumentoViewModel
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
    }
}
