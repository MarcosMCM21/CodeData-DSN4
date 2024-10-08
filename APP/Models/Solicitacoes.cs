using CodeData_Connection.Models.Database.Entidade;

namespace CodeData_Connection.Models
{
    public class Solicitacoes
    {
        public Solicitacao Solicitacao { get; set; }
        public string Cliente { get; set; }
        public string? Vendedor { get; set; }
    }
}
