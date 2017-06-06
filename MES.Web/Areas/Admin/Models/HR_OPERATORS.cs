namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HR_OPERATORS
    {
        [Key]
        [Column(Order =0)]
        [StringLength(20)]
        [Display(Name = "用户ID")]
        [Required]
        public string OPERID { get; set; }

        [StringLength(80)]
        [Column(Order = 1)]
        [Display(Name = "用户名")]
        [Required]
        public string OPERNAME { get; set; }

        [StringLength(128)]
        [Column(Order = 2)]
        [Display(Name = "密码")]
        [Required]
        [DataType(DataType.Password)]
        public string PWD { get; set; }

        [Display(Name = "公司")]
        [ForeignKey("BUNO")]
        public virtual ENG_BU BU { get; set; }

        [Column(Order = 3)]
        [StringLength(12)]
        [Required]
        [Display(Name = "公司")]
        public string BUNO { get; set; }

        [Column(Order = 4)]
        [StringLength(20)]
        [Display(Name = "部门代码")]
        public string DEPTNO { get; set; }
        
        [StringLength(1)]
        [Column(Order = 5)]
        [Display(Name = "有效用户")]
        public String ACTIVE { get; set; }


        [Column(Order = 6)]
        [StringLength(120)]
        [Display(Name = "电子邮件")]
        public string EMAIL { get; set; }

        [Column(Order = 7)]
        [StringLength(80)]
        [Display(Name = "手机号")]
        public string MOBILE { get; set; }

        [Column(Order = 8)]
        [StringLength(255)]
        [Display(Name = "备注")]
        public string COMMENTS { get; set; }

    }
}
