namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ENG_RWKSCRCODE
    {
        [Key]
        [StringLength(20)]
        [Required, Display(Name = "质量代码")]
        public string QCCODE { get; set; }

        [StringLength(1)]
        [Required, Display(Name = "类型")]
        public string QCTYPE { get; set; }

        [StringLength(255)]
        [Required, Display(Name = "说明")]
        public string DESCRIPTION { get; set; }

        [StringLength(20)]
        [Display(Name = "产线")]
        public string LINENAME { get; set; }

        [StringLength(10)]
        [Display(Name = "返工到")]
        public string DEFAULTOOP { get; set; }

        [StringLength(10)]
        [Display(Name = "来源工序")]
        public string FROMOP { get; set; }

        [StringLength(255)]
        [Display(Name = "备注")]
        public string COMMENTS { get; set; }
    }
}
