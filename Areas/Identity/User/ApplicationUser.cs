using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CodeData_Connection.Areas.Identity.User
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(255)]
        public string FirstName { get; set; }

        [MaxLength(255)]
        public string LastName { get; set; }


        // Relacionamento com a entidade ASPNETUSERS
        [MaxLength(255)]
        public string? GerenteID { get; set; }
        public ApplicationUser User { get; set; }
    }
}
