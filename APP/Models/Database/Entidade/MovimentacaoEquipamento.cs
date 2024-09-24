namespace CodeData_Connection.Models.Database.Entidade
{
    public class MovimentacaoEquipamento
    {
        public int Id { get; set; }

        // Chaves estrangeiras
        public int EquipamentoId { get; set; }
        public Equipamento Equipamento { get; set; }

        public int EnderecoId { get; set; }
        public Endereco Endereco { get; set; }

        public int SolicitacaoId { get; set; }
        public Solicitacao Solicitacao { get; set; }

        public int DocumentoId { get; set; }
        public Documento Documento { get; set; }
    }
}
