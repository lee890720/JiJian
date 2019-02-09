using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.AppIdentity
{
    [Table("User_BranchDetial")]
    public partial class UserBranchDetial
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BranchDetialId { get; set; }
        public int BranchId { get; set; }

        [Display(Name = "房号")]
        public string HouseNumber { get; set; }
        public UserBranch UserBranch { get; set; }
    }
}
