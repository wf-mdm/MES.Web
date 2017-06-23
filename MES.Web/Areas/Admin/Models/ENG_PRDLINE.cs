namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ENG_PRDLINE
    {
        [NotMapped]
        public string CodeName { get { return String.Format("{0}:{1}", LINENAME, DISPLAYNAME); } }

        [Key]
        [StringLength(20)]
        [Required,Display(Name ="产线代码")]
        public string LINENAME { get; set; }

        [StringLength(12)]
        [Required, Display(Name = "公司")]
        public string BUNO { get; set; }

        [Display(Name = "公司")]
        [ForeignKey("BUNO")]
        public virtual ENG_BU BU { get; set; }

        [StringLength(80)]
        [Required, Display(Name = "产线名称")]
        public string DISPLAYNAME { get; set; }

        [Column(TypeName = "numeric")]
        [Required, Display(Name = "标准小时产出")]
        public decimal? STDYIELDRATE { get; set; }

        [StringLength(12)]
        [Required, Display(Name = "产线组")]
        public string LINEGRP { get; set; }
    }
}
