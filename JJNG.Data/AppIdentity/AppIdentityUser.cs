using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace JJNG.Data.AppIdentity
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class AppIdentityUser : IdentityUser
    {
        [Display(Name ="部门")]
        public string Department { get; set; }
        [Display(Name ="职位")]
        public string Position { get; set; }
        [Display(Name = "隶属")]
        public string Branch { get; set; }
        [Display(Name = "隶属ID")]
        public int BranchId { get; set; }
        [Display(Name = "注册时间")]
        public DateTime RegisterDate { get; set; }
        [Display(Name = "头像")]
        public string UserImage { get; set; }
    }
    //public enum SexType
    //{
    //    [Display(Name = "男")]
    //    男,
    //    [Display(Name = "女")]
    //    女
    //}
}
