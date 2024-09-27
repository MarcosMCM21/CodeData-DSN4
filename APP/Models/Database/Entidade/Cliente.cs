using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeData_Connection.Models.Database.Entidade
{
    public class Cliente
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Nome { get; set; }

        [MaxLength(20)]
        public string CNPJ { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime DataCadastro { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime? DataAtualizado { get; set; }

        public int EnderecoId { get; set; }
        [ForeignKey("EnderecoId")]
        public Endereco Endereco { get; set; }
    }
}
