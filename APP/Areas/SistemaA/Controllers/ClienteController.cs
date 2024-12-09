using CodeData_Connection.Areas.Identity.Data;
using CodeData_Connection.Models.Database.Entidade;
using CodeData_Connection.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MySql.Data.Types;
using CodeData_Connection.Controllers;
using MySqlX.XDevAPI;

namespace CodeData_Connection.Areas.SistemaA.Controllers
{
    [Authorize]
    [Area("SistemaA")]
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

        public IActionResult Cadastrar()
        {
            ViewBag.Post = "Cadastrar";
            return View("FormsCliente");
        }

        public IActionResult Editar(int id)
        {
            var cliente = _context.Clientes.Where(c => c.Id == id).FirstOrDefault();

            if (cliente == null) 
            {
                return NotFound();
            }

            var endereco = _context.Enderecos.Where(e => e.Id == cliente.EnderecoId).FirstOrDefault();

            if (endereco == null)
            {
                return NotFound();
            }

            var clienteEndereco = new FormsClienteViewModel
            {
                Nome = cliente.Nome,
                CNPJ = cliente.CNPJ,
                EnderecoId = cliente.EnderecoId,
                CEP = endereco.CEP,
                Rua = endereco.Rua,
                Numero = endereco.Numero,
                Bairro = endereco.Bairro,
                Cidade = endereco.Cidade,
                Estado = endereco.Estado,
                Complemento = endereco.Complemento
            };

            ViewBag.Post = "Editar";
            return View("FormsCliente", clienteEndereco);
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

        [HttpPost]
        public async Task<IActionResult> Cadastrar(FormsClienteViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Endereco endereco = new Endereco
                    {
                        CEP = model.CEP,
                        Rua = model.Rua,
                        Numero = model.Numero,
                        Bairro = model.Bairro,
                        Cidade = model.Cidade,
                        Estado = model.Estado,
                        Complemento = model.Complemento
                    };

                    _context.Add(endereco);
                    await _context.SaveChangesAsync();

                    var enderecoId = endereco.Id;

                    Cliente cliente = new Cliente
                    {
                        Nome = model.Nome,
                        CNPJ = model.CNPJ,
                        EnderecoId = enderecoId
                    };

                    _context.Add(cliente);
                    await _context.SaveChangesAsync();

                    TempData["Mensagem"] = "Cliente cadastrado com sucesso!";
                    TempData["TipoMensagem"] = "success";

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Tratar exceções específicas conforme necessário
                    Console.WriteLine($"Erro ao cadastrar: {ex.Message}");
                    TempData["Mensagem"] = "Erro ao cadastrar o cliente!";
                    TempData["TipoMensagem"] = "error";
                }
            }

            var viewModel = new FormsClienteViewModel
            {
                Nome = model.Nome,
                CNPJ = model.CNPJ,
                EnderecoId = model.EnderecoId,
                CEP = model.CEP,
                Rua = model.Rua,
                Numero = model.Numero,
                Bairro = model.Bairro,
                Cidade = model.Cidade,
                Estado = model.Estado,
                Complemento = model.Complemento
            };

            foreach (var erro in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(erro.ErrorMessage);
                Console.WriteLine(erro.Exception);
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, FormsClienteViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Cliente cliente = new Cliente
                    {
                        Id = (int) model.Id,
                        Nome = model.Nome,
                        CNPJ = model.CNPJ
                    };

                    // 1. Marcar a entidade como modificada
                    _context.Entry(cliente).State = EntityState.Modified;

                    _context.Entry(cliente).Property(e => e.EnderecoId).IsModified = false;
                    _context.Entry(cliente).Property(e => e.DataCadastro).IsModified = false;

                    await _context.SaveChangesAsync();

                    Endereco endereco = new Endereco
                    {
                        Id = (int) model.EnderecoId,
                        CEP = model.CEP,
                        Rua = model.Rua,
                        Numero = model.Numero,
                        Bairro = model.Bairro,
                        Cidade = model.Cidade,
                        Estado = model.Estado,
                        Complemento = model.Complemento
                    };

                    _context.Add(endereco);

                    _context.Entry(endereco).State = EntityState.Modified;

                    _context.Entry(endereco).Property(e => e.Localizacao).IsModified = false;
                    await _context.SaveChangesAsync();

                    TempData["Mensagem"] = "Cliente atualizado com sucesso!";
                    TempData["TipoMensagem"] = "success";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Clientes.Any(c => c.Id == id) || !_context.Enderecos.Any(e => e.Id == model.EnderecoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Re-lançar a exceção para tratamento em outro nível
                    }
                }
            }

            var clienteEndereco = new FormsClienteViewModel
            {
                Nome = model.Nome,
                CNPJ = model.CNPJ,
                EnderecoId = model.EnderecoId,
                CEP = model.CEP,
                Rua = model.Rua,
                Numero = model.Numero,
                Bairro = model.Bairro,
                Cidade = model.Cidade,
                Estado = model.Estado,
                Complemento = model.Complemento
            };

            TempData["Mensagem"] = "Erro ao atualizar o cliente!";
            TempData["TipoMensagem"] = "error";

            // Logar os erros de validação (opcional)
            foreach (var erro in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(erro.ErrorMessage);
                Console.WriteLine(erro.Exception);
            }

            return View(clienteEndereco);
        }
    }

    public class FormsClienteViewModel
    {
        public int? Id { get; set; }

        [MaxLength(255)]
        public string Nome { get; set; }

        [MaxLength(20)]
        public string CNPJ { get; set; }

        public int? EnderecoId { get; set; }

        [MaxLength(9)]
        public string CEP { get; set; }

        [MaxLength(255)]
        public string Rua { get; set; }

        public int Numero { get; set; }

        [MaxLength(255)]
        public string Bairro { get; set; }

        [MaxLength(255)]
        public string Cidade { get; set; }

        [MaxLength(30)]
        public string Estado { get; set; }

        [MaxLength(500)]
        public string Complemento { get; set; }

        [Column(TypeName = "POINT")]
        public MySqlGeometry Localizacao { get; set; }
    }

    public class DetalhesClienteViewModel
    {
        public Cliente Cliente { get; set; }
        public List<DadosSolicitacao> Solicitacoes { get; set; }
        public Endereco Endereco { get; set; }
    }
}
