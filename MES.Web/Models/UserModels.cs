using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MES.Web.Models
{
    /// <summary>
    /// 登录
    /// </summary>
    public class LoginModel
    {
        [Display(Name = "登录到")]
        [Required]
        public String AppId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name ="用户名")]
        [Required]
        public String UserId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        [Required]
        public String Password { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "记住我?")]
        public Boolean RememberMe { get; set; }
    }

    /// <summary>
    /// 用户表
    /// </summary>
    [Table("HR_OPERATORS")]
    public class SysUserModel
    {
        [Key]
        [Column("OPERID")]
        [Display(Name="用户名")]
        [Required, MaxLength(20)]
        public String ID { get; set; }

        [Display(Name = "姓名")]
        [Column("OPERNAME")]
        [Required, MaxLength(80)]
        public String Name{ get; set; }

        [Display(Name="密码")]
        [Column("PWD")]
        [Required, MaxLength(128)]
        public String Passwd{ get; set; }

        [Display(Name = "部门代码")]
        [Column("DEPTNO")]
        [Required, MaxLength(20)]
        public String DeptNo { get; set; }

        [Display(Name = "公司代码")]
        [Column("BUNO")]
        [Required, MaxLength(20)]
        public String BuNo { get; set; }

        [Display(Name = "Email")]
        [Column("Email")]
        [MaxLength(120)]
        public String Email { get; set; }

        [Display(Name = "手机号")]
        [Column("MOBILE")]
        [MaxLength(80)]
        public String Mobile { get; set; }

        [Display(Name = "备注")]
        [Column("COMMENTS")]
        [MaxLength(250)]
        public String Comments { get; set; }
    }
}