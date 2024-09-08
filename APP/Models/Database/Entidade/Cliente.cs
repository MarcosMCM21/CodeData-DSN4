using MySqlX.XDevAPI;
using System.ComponentModel.DataAnnotations;

namespace CodeData_Connection.Models.Database.Entidade
{
    public class Cliente
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Nome { get; set; }

        [MaxLength(20)]
        public string CNPJ { get; set; }

        // Chave estrangeira
        public int EnderecoId { get; set; }
        public Endereco Endereco { get; set; }
    }
}
