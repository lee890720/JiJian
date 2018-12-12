using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.AppIdentity
{
    [Table("User_BelongTo")]
    public partial class UserBelongTo
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BelongToId { get; set; }

        [Display(Name = "隶属")]
        public string BelongToName { get; set; }
    }
}
