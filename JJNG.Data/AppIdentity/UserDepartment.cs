using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.AppIdentity
{
    [Table("User_Department")]
    public partial class UserDepartment
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }

        [Display(Name = "部门")]
        public string DepartmentName { get; set; }
    }
}
