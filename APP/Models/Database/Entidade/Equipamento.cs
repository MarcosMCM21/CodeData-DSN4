using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeData_Connection.Models.Database.Entidade
{
    public class Equipamento
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(45)]
        public string Codigo { get; set; }

        [MaxLength(70)]
        public string Modelo { get; set; }

        [Column(TypeName = "TEXT")]
        public string Descricao { get; set; }

        [MaxLength(45)]
        public string Marca { get; set; }

        [MaxLength(50)]
        public string SerialNumber { get; set; }

        [MaxLength(50)]
        public string PartNumber { get; set; }

        [Column(TypeName = "TINYINT(1)")]
        public bool Condicao { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime? DataCadastro { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime? DataAtualizado { get; set; }

        // Chaves estrangeiras
        public int EstoqueId { get; set; }
        [ForeignKey("EstoqueId")]
        public Estoque? Estoque { get; set; }

        public int DocumentoId { get; set; }
        [ForeignKey("DocumentoId")]
        public Documento? Documento { get; set; }
    }
}
