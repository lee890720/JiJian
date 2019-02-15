using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Finance
{
    [Table("Fnc_HouseType")]
    public partial class FncHouseType
    {
        public FncHouseType()
        {
            FncHouseNumber = new HashSet<FncHouseNumber>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int HouseTypeId { get; set; }
        public int BranchId { get; set; }

        [Display(Name = "房型")]
        public string HouseType { get; set; }
        public string Order { get; set; }
        public FncBranch FncBranch { get; set; }
        public ICollection<FncHouseNumber> FncHouseNumber { get; set; }

    }
}
