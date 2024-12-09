using CodeData_Connection.Areas.Identity.Data;
using CodeData_Connection.Areas.SistemaA.Controllers;
using CodeData_Connection.Models;
using CodeData_Connection.Models.Database.Entidade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using SendGrid.Helpers.Mail;
using System.Text;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace CodeData_Connection.Controllers
{
    [Authorize]
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

        public IActionResult Cadastrar()
        {
            ViewBag.Post = "Cadastrar";
            return View("FormsDocumento");
        }

        public IActionResult Editar(int id)
        {
            var documentoDb = _context.Documentos
                .Where(d => d.Id == id)
                .FirstOrDefault();

            if (documentoDb == null)
            {
                return NotFound();
            }

            IFormFile anexo;

            var byteArray = System.Text.Encoding.UTF8.GetBytes(documentoDb.Anexo);
            using (var stream = new MemoryStream(byteArray))
            {
                anexo = new FormFile(stream, 0, stream.Length, "name", documentoDb.Nome)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "text/plain"
                };
            }

            var documento = new FormsDocumentoViewModel
            {
                Id = documentoDb.Id,
                Numero = documentoDb.Numero,
                Nome = documentoDb.Nome,
                Tipo = documentoDb.Tipo
            };

            ViewBag.Post = "Editar";
            return View("FormsDocumento", documento);
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
            var documento = await _context.Documentos
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync();

            if (documento == null)
            {
                return NotFound();
            }

            return View(documento);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(FormsDocumentoViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var anexo = "";

                    if (model.Anexo != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            model.Anexo.CopyTo(memoryStream);
                            var fileBytes = memoryStream.ToArray();
                            anexo = Convert.ToBase64String(fileBytes);
                        }
                    }

                    Documento documento = new Documento
                    {
                        Numero = model.Numero,
                        Nome = model.Nome,
                        Tipo = model.Tipo,
                        Anexo = anexo
                    };

                    _context.Add(documento);

                    // 3. Salvar as alterações no banco de dados
                    await _context.SaveChangesAsync();

                    TempData["Mensagem"] = "Documento cadastrado com sucesso!";
                    TempData["TipoMensagem"] = "success";

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Tratar exceções específicas conforme necessário
                    Console.WriteLine($"Erro ao cadastrar: {ex.Message}");
                    TempData["Mensagem"] = "Erro ao cadastrar o documento!";
                    TempData["TipoMensagem"] = "error";
                }
            }

            var viewModel = new FormsDocumentoViewModel
            {
                Numero = model.Numero,
                Nome = model.Nome,
                Tipo = model.Tipo
            };

            foreach (var erro in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(erro.ErrorMessage);
                Console.WriteLine(erro.Exception);
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, FormsDocumentoViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Documento documento = new Documento
                    {
                        Id = (int) model.Id,
                        Numero = model.Numero,
                        Nome = model.Nome,
                        Tipo = model.Tipo
                    };

                    // 1. Marcar a entidade como modificada
                    _context.Entry(documento).State = EntityState.Modified;

                    if (model.Anexo == null)
                    {
                        // 2. Impedir a atualização da propriedade DataCadastro
                        _context.Entry(documento).Property(e => e.Anexo).IsModified = false;

                    } else {
                        var anexo = "";

                        using (var memoryStream = new MemoryStream())
                        {
                            model.Anexo.CopyTo(memoryStream);
                            var fileBytes = memoryStream.ToArray();
                            anexo = Convert.ToBase64String(fileBytes);
                        }

                        documento.Anexo = anexo;
                    }

                    _context.Entry(documento).Property(e => e.DataCadastro).IsModified = false;

                    // 3. Salvar as alterações no banco de dados
                    await _context.SaveChangesAsync();

                    TempData["Mensagem"] = "Documento atualizado com sucesso!";
                    TempData["TipoMensagem"] = "success";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Documentos.Any(e => e.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Re-lançar a exceção para tratamento em outro nível
                    }
                }
            }

            var viewModel = new FormsDocumentoViewModel
            {
                Id = model.Id,
                Numero = model.Numero,
                Nome = model.Nome,
                Tipo = model.Tipo
            };

            TempData["Mensagem"] = "Erro ao atualizar o documento!";
            TempData["TipoMensagem"] = "error";

            // Logar os erros de validação (opcional)
            foreach (var erro in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(erro.ErrorMessage);
                Console.WriteLine(erro.Exception);
            }

            return View(viewModel);
        }
    }

    public class FormsDocumentoViewModel
    {
        public int? Id { get; set; }
        public string Numero { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public IFormFile? Anexo {  get; set; }
    }

    public class DadosDocumentoViewModel
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
    }
}
