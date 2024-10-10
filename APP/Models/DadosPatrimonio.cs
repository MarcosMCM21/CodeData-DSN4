using CodeData_Connection.Models.Database.Entidade;

namespace CodeData_Connection.Models
{
    public class DadosPatrimonio
    {
        public List<DadosEquipamento> DadosEquipamento { get; set; }
        public List<Endereco> ListaEnderecos { get; set; }
    }
}
