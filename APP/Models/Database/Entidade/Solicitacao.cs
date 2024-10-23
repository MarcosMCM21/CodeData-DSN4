using CodeData_Connection.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeData_Connection.Models.Database.Entidade
{
    public class Solicitacao
    {
        public int Id { get; set; }

        [Column(TypeName = "TINYINT(1)")]
        public bool Tipo { get; set; }

        [MaxLength(50)]
        public string Numero { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime DataInicio { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime DataFinal { get; set; }

        [Column(TypeName = "TEXT")]
        public string Descricao { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime? DataCadastro { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime? DataAtualizado { get; set; }

        [MaxLength(255)]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public int ClienteId { get; set; }
        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }
    }

}
