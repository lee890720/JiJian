using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Personnel
{
    [Table("Psn_Note")]
    public partial class PsnNote
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }
        public int NoteAccountId { get; set; }
        [Display(Name = "账号")]
        public string Account { get; set; }
        [Display(Name ="密码")]
        public string Password { get; set; }
        [Display(Name ="平台")]
        public string Platform { get; set; }
        [Display(Name ="手机号")]
        public string Phone { get; set; }
        [Required]
        [Display(Name = "录单人")]
        public string EnteringStaff { get; set; }
        [Required]
        [Display(Name = "分店")]
        public string Branch { get; set; }
        [Display(Name = "备注")]
        public string Note { get; set; }
        public PsnNoteAccount PsnNoteAccount { get; set; }
    }
}
