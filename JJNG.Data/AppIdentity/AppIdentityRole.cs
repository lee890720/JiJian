using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.AppIdentity
{
    public class AppIdentityRole
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name ="描述")]
        public string Description { get; set; }
        [Display(Name ="权限")]
        public string Permission { get; set; }

        public IdentityRole IdentityRole { get; set; }
    }
}
