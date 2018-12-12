using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.Areas.AppIdentity.Models
{
    public class UserCreateModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string Name { get; set; }

        [Display(Name = "联系方式")]
        public string PhoneNumber { get; set; }

        [Display(Name = "部门")]
        public string Department { get; set; }

        [Display(Name = "职位")]
        public string Position { get; set; }

        [Display(Name = "隶属")]
        public string BelongTo { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "重复密码")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
