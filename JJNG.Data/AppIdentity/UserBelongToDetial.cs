using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.AppIdentity
{
    [Table("User_BelongToDetial")]
    public partial class UserBelongToDetial
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BelongToDetialId { get; set; }
        public int BelongToId { get; set; }

        [Display(Name = "房号")]
        public string HouseNumber { get; set; }
        public UserBelongTo UserBelongTo { get; set; }
    }
}
