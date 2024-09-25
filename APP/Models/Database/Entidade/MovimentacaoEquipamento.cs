using System.ComponentModel.DataAnnotations.Schema;

namespace CodeData_Connection.Models.Database.Entidade
{
    public class MovimentacaoEquipamento
    {
        public int Id { get; set; }

        // Chaves estrangeiras
        public int EquipamentoId { get; set; }
        [ForeignKey("EquipamentoId")]
        public Equipamento Equipamento { get; set; }

        public int EnderecoId { get; set; }
        [ForeignKey("EnderecoId")]
        public Endereco Endereco { get; set; }

        public int SolicitacaoId { get; set; }
        [ForeignKey("SolicitacaoId")]
        public Solicitacao Solicitacao { get; set; }

        public int DocumentoId { get; set; }
        [ForeignKey("DocumentoId")]
        public Documento Documento { get; set; }
    }
}
