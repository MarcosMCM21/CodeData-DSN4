using CodeData_Connection.Models.Database.Entidade;

namespace CodeData_Connection.Models
{
    public class DadosEquipamento
    {
        public Equipamento Equipamento { get; set; }
        public Documento? Documento { get; set; }
        public DadosSolicitacao? DadosSolicitacao { get; set; }
        public Estoque? Estoque { get; set; }
        public Endereco? Endereco { get; set; }
        public string Status { get; set; }
    }
}
