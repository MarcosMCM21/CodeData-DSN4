using CodeData_Connection.Areas.Identity.Data;
using CodeData_Connection.Models.Database.Entidade;
using CodeData_Connection.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using NuGet.Protocol;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace CodeData_Connection.Areas.SistemaA.Controllers
{
    [Authorize]
    [Area("SistemaA")]
    public class PatrimonioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatrimonioController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detalhes(int id)
        {
            var dadosEquipamento = ObterDetalhesEquipamento(id);

            if (dadosEquipamento == null)
            {
                return NotFound();
            }

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
                return NotFound();
            }

            // 2. Criar um ViewModel para a view de edição (recomendado)
            var viewModel = new EditarEquipamentoViewModel
            {
                Equipamento = equipamento,
                Documentos = await _context.Documentos.Select(d => new EstoqueDocumento { Id = d.Id, Nome = d.Nome }).Distinct().ToListAsync(),
                Estoques = await _context.Estoques.Select(e => new EstoqueDocumento { Id = e.Id, Nome = e.Nome }).Distinct().ToListAsync()
            };

            return View(viewModel);
        }

        public IActionResult ImportarEquipamentos() 
        {
            return View();
        }

        // Crie uma nova Action para buscar os dados do banco de dados
        public async Task<IActionResult> ObterDadosPatrimonio()
        {
            // Obtém os equipamentos sem tracking para evitar overhead do Entity Framework
            var equipamentos = await _context.Equipamentos
                .AsNoTracking()
                .Select(e => new { e.Id, e.Marca, e.Modelo })
                .ToListAsync();

            // Se não houver equipamentos, retorna 404
            if (!equipamentos.Any())
            {
                return NotFound();
            }

            // Popula marcas e modelos sem duplicação
            ViewBag.Marcas = equipamentos.Select(e => e.Marca).Distinct().ToList();
            ViewBag.Modelos = equipamentos.Select(e => e.Modelo).Distinct().ToList();

            var dadosPatrimonio = new DadosPatrimonioViewModel
            {
                DadosEquipamento = equipamentos.Select(e => ObterDadosBasicosEquipamento(e.Id)).ToList(),
                ListaEnderecos = ObterEnderecosEquipamentos()
            };

            return PartialView("_DadosPatrimonio", dadosPatrimonio);
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
        public DetalhesEquipamentoViewModel? ObterDadosBasicosEquipamento(int id)
        {
            var equipamento = _context.Equipamentos.Find(id);

            if (equipamento == null)
            {
                return null;
            }

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
        public DetalhesEquipamentoViewModel ObterDetalhesEquipamento(int id)
        {
            var dadosEquipamento = ObterDadosBasicosEquipamento(id); // Reutiliza o método básico

            try
            {
                var movimentacao = _context.MovimentacoesEquipamentos
                    .FirstOrDefault(me => me.EquipamentoId == id);

                if (movimentacao != null)
                {
                    dadosEquipamento.Endereco = _context.Enderecos.FirstOrDefault(e => e.Id == movimentacao.EnderecoId);
                }
                else
                {
                    var estoque = _context.Estoques.FirstOrDefault(s => s.Id == dadosEquipamento.Equipamento.EstoqueId);
                    if (estoque != null)
                    {
                        dadosEquipamento.Estoque = estoque;
                        dadosEquipamento.Endereco = _context.Enderecos.FirstOrDefault(e => e.Id == estoque.EnderecoId);
                    }
                }

                dadosEquipamento.Documento = _context.Documentos.FirstOrDefault(d => d.Id == dadosEquipamento.Equipamento.DocumentoId);
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
            var enderecos = _context.MovimentacoesEquipamentos.AsNoTracking()
            .Where(me => me.EnderecoId != null)
            .Select(me => _context.Enderecos.AsNoTracking().FirstOrDefault(e => e.Id == me.EnderecoId))
            .ToList();

            return enderecos;
        }

        // Action para lidar com o upload
        [HttpPost]
        public async Task<IActionResult> ImportarEquipamentos(IFormFile pdfFile)
        {
            if (pdfFile != null && pdfFile.Length > 0)
            {
                // Defina o caminho onde o arquivo será salvo temporariamente
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", pdfFile.FileName);

                // Salvando o arquivo no diretório especificado

                try
                {
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await pdfFile.CopyToAsync(stream);
                    }
                } catch { }

                // Você pode processar o arquivo PDF aqui, como analisar com iTextSharp ou outra lógica
                var dadosPDF = ExtrairDadosNotaFiscal(path);

                Console.WriteLine(dadosPDF.ToJson());

                return RedirectToAction("ImportarEquipamentos");
            }

            return BadRequest("Nenhum arquivo foi enviado.");
        }

        public static List<Produto> ExtrairDadosNotaFiscal(string caminhoArquivoPdf)
        {
            List<Produto> produtos = new List<Produto>();
            var patternProduto = @"(?<=DADOS DO PRODUTO/SERVIÇOS)(.*?)(?=DADOS ADICIONAIS)";
            var patternDadosProduto = @"^(\d{7})";
            var patternSeriais = @"PEDIDO\sCLIENTE\s-\s(.+?)\n\s+CODIGO\s\d+\s-\sSERIE\s(.+?)\n\s+CODIGO\s\d+\s-\sSERIE\s(.+)"; //Simple

            // Usando PdfReader e PdfDocument do iText 7+
            using (var reader = new PdfReader(caminhoArquivoPdf))
            using (var pdfDoc = new PdfDocument(reader))
            {
                Console.WriteLine("Documento aberto com sucesso");
                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    LocationTextExtractionStrategy strategy1 = new LocationTextExtractionStrategy();
                    SimpleTextExtractionStrategy strategy2 = new SimpleTextExtractionStrategy();
                    PdfCanvasProcessor parser1 = new PdfCanvasProcessor(strategy1);
                    PdfCanvasProcessor parser2 = new PdfCanvasProcessor(strategy2);

                    var page = pdfDoc.GetPage(i);

                    Console.WriteLine("\n\nLocationTextExtractionStrategy");
                    parser1.ProcessPageContent(page);
                    string content1 = strategy1.GetResultantText();
                    Console.WriteLine(content1 + "\n\n");

                    Console.WriteLine("\n\nSimpleTextExtractionStrategy");
                    parser2.ProcessPageContent(page);
                    string content2 = strategy2.GetResultantText();
                    Console.WriteLine(content2 + "\n\n");

                    var contentProduto = Regex.Match(content2, patternProduto, RegexOptions.Singleline).Value;
                    Console.WriteLine("\n\nPatternProduto");
                    Console.WriteLine(contentProduto + "\n\n");

                    if (!string.IsNullOrEmpty(contentProduto))
                    {
                        // Aplicar o regex para capturar os dados dos produtos dentro do intervalo
                        foreach (Match match in Regex.Matches(contentProduto, patternDadosProduto))
                        {
                            Console.WriteLine("\n\nPatternProduto");

                            for (int groupIndex = 1; groupIndex < match.Groups.Count; groupIndex++)
                            {
                                Console.WriteLine($"Group {groupIndex}: {match.Groups[groupIndex].Value}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nenhum dado encontrado entre as seções DADOS DO PRODUTO/SERVIÇOS e DADOS ADICIONAIS.");
                    }

                    // Extrair os números de série
                    foreach (Match match in Regex.Matches(content1, patternSeriais))
                    {
                        Console.WriteLine("\nnPatternSerials");
                        Console.WriteLine(match);

                        string codigoProduto = match.Groups[1].Value;
                        Console.WriteLine("CÓD. PRODUTO: " + codigoProduto);

                        string[] seriais = match.Groups[2].Value.Split(',');
                        Console.WriteLine("SERIAIS NUMBER: " + codigoProduto);

                        var produto = produtos.Find(p => p.Codigo == codigoProduto);
                        if (produto != null)
                        {
                            produto.Seriais.AddRange(seriais);
                        }
                    }
                }
            }

            return produtos;
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

    public class Produto
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public List<string> Seriais { get; set; } = new List<string>();
    }
}