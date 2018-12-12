using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data
{
    [Table("App_Menu")]
    public partial class AppMenu
    {
        //[Key]
        //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "菜单名称")]
        public string Name { get; set; }

        [Display(Name = "级别")]
        public int Grade { get; set; }

        [Display(Name = "顺序")]
        public int Sequence { get; set; }

        [Display(Name = "从属")]
        public int Follow { get; set; }

        [Display(Name="图标")]
        public string Ico { get; set; }

        [Display(Name = "超链接")]
        public string Url { get; set; }

        [Display(Name = "域")]
        public string Area { get; set; }

        [Display(Name = "控制器")]
        public string Controller { get; set; }

        [Display(Name = "功能")]
        public string Action { get; set; }

        [Display(Name = "有效性")]
        public bool Valid { get; set; }

        [Display(Name = "权限归属")]
        public string Description { get; set; }

        [Display(Name = "备注")]
        public string State { get; set; }
    }
}
