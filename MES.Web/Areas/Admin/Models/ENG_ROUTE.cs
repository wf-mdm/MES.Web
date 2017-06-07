namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ENG_ROUTE
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(12)]
        [Required, Display(Name = "工艺流程代码")]
        public string RT_NAME { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "numeric")]
        [Required, Display(Name = "顺序")]
        public decimal SEQNO { get; set; }

        [StringLength(20)]
        [Required, Display(Name = "产线代码")]
        public string LINENAME { get; set; }

        [StringLength(3)]
        [Required, Display(Name = "分支号")]
        public string SUBLINENO { get; set; }

        [StringLength(10)]
        [Required, Display(Name = "工序号")]
        public string L_OPNO { get; set; }

        [Display(Name = "工序代码")]
        [ForeignKey("LINENAME,L_OPNO")]
        public virtual ENG_LINEOP Op { get; set; }

        [StringLength(80)]
        [Display(Name = "前置工序")]
        public string PREV_OPLIST { get; set; }

        [StringLength(80)]
        [Display(Name = "后置工序")]
        public string NEXT_OPLIST { get; set; }

        [StringLength(1)]
        [Display(Name = "是否首工序")]
        public string IsFirst { get; set; }

        [StringLength(1)]
        [Display(Name = "是否末工序")]
        public string IsLast { get; set; }

        [StringLength(255)]
        [Display(Name = "备注")]
        public string COMMENTS { get; set; }
    }
}
