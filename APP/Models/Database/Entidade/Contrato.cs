using System.ComponentModel.DataAnnotations.Schema;

namespace CodeData_Connection.Models.Database.Entidade
{
    public class Contrato
    {
        public int Id { get; set; }

        // Chave estrangeira
        public int SolicitacaoId { get; set; }
        public Solicitacao Solicitacao { get; set; }

        public int DocumentoId { get; set; }
        [ForeignKey("DocumentoId")]
        public Documento Documento { get; set; }
    }

}
