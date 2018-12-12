using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.AppIdentity
{
    [Table("User_Position")]
    public partial class UserPosition
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PositionId { get; set; }

        [Display(Name = "职位")]
        public string PositionName { get; set; }
    }
}
