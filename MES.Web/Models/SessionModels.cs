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
        public String ID { get; set; }
        [Column("OPERNAME")]
        public String Name{ get; set; }
        [Column("PWD")]
        public String Passwd{ get; set; }
    }
}