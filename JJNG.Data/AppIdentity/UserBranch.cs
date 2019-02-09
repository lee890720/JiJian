using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.AppIdentity
{
    [Table("User_Branch")]
    public partial class UserBranch
    {
        public UserBranch()
        {
            UserBranchDetial = new HashSet<UserBranchDetial>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BranchId { get; set; }

        [Display(Name = "隶属")]
        public string BranchName { get; set; }
        public ICollection<UserBranchDetial> UserBranchDetial { get; set; }

    }
}
