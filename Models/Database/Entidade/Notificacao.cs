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
        public DateTime DataHora { get; set; }

        // Relacionamento com a entidade ASPNETUSERS
        [MaxLength(255)]
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }

        public Notificacao(int id, string titulo, string mensagem, bool visualizado, DateTime dataHora)
        {
            Id = id;
            Titulo = titulo;
            Mensagem = mensagem;
            Visualizado = visualizado;
            DataHora = dataHora;
        }
    }
}