namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HR_OPERCAPBMATRIX
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        [Required, Display(Name = "用户ID")]
        public string OPERID { get; set; }

        [ForeignKey("OPERID")]
        [Display(Name = "用户ID")]
        public virtual V_USERANDROLES Oper { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        [Display(Name = "产线")]
        public string LINENAME { get; set; }

        [StringLength(10)]
        [Display(Name = "工序")]
        public string L_OPNO { get; set; }

        [StringLength(10)]
        [Display(Name = "工站")]
        public string L_STNO { get; set; }

        [Display(Name ="技能等级")]
        [StringLength(3)]
        public string CAPBLEVEL { get; set; }

        [Display(Name = "备注")]
        [StringLength(255)]
        public string COMMENTS { get; set; }
    }
}
