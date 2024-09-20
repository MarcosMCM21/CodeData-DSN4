using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeData_Connection.Models.Database.Entidade
{
    public class Estoque
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Nome { get; set; }

        // Chave estrangeira
        public int EnderecoId { get; set; }
        [ForeignKey("EnderecoId")]
        public Endereco Endereco { get; set; }
    }

}
