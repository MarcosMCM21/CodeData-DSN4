using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeData_Connection.Models.Database.Entidade
{
    public class Endereco
    {
        [Key]
        public int Id { get; set; }

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
        public string Localizacao { get; set; }
    }

}
