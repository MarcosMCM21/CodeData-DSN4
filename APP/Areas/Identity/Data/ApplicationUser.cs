using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeData_Connection.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(255)]
        public string FirstName { get; set; }

        [MaxLength(255)]
        public string LastName { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime DataCadastro { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime? DataAtualizado { get; set; }

        // Relacionamento com a entidade ASPNETUSERS
        [MaxLength(255)]
        public string? GerenteID { get; set; }
        [ForeignKey("GerenteID")]
        public ApplicationUser User { get; set; }
    }
}
