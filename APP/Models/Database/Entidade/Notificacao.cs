using CodeData_Connection.Areas.Identity.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeData_Connection.Models.Database.Entidade
{
    public class Notificacao
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Titulo { get; set; }

        [Column(TypeName = "TEXT")]
        public string Mensagem { get; set; }

        [Column(TypeName = "TINYINT(1)")]
        public bool Visualizado { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime? DataHora { get; set; }

        [MaxLength(255)]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}