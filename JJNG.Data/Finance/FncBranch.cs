using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Finance
{
    [Table("Fnc_Branch")]
    public partial class FncBranch
    {
        public FncBranch()
        {
            FncHouseType = new HashSet<FncHouseType>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BranchId { get; set; }

        [Display(Name = "名称")]
        public string BranchName { get; set; }
        public ICollection<FncHouseType> FncHouseType { get; set; }

    }
}
