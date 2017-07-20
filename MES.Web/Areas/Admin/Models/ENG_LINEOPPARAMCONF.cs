namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ENG_LINEOPPARAMCONF
    {
        [Key]
        [StringLength(20)]
        [Required, Display(Name = "参数集id")]
        [Column(Order = 0)]
        public string CONFNAME { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "numeric")]
        [Display(Name = "流水码")]
        public decimal? CONFID { get; set; }

        [StringLength(20)]
        [Display(Name = "产线")]
        public string LINENAME { get; set; }

        [StringLength(10)]
        [Display(Name = "工序")]
        public string L_OPNO { get; set; }

        [StringLength(10)]
        [Display(Name = "工站")]
        public string L_STNO { get; set; }

        [StringLength(30)]
        [Display(Name = "参数id")]
        public string PARAM_ID { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "值")]
        public decimal? PARAM_VAL { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "值上限")]
        public decimal? PARAM_UPPER { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "值下限")]
        public decimal? PARAM_LOWER{ get; set; }

        [StringLength(120)]
        [Display(Name = "文本值")]
        public string PARAM_TEXT { get; set; }

        [StringLength(12)]
        [Display(Name = "数据类型")]
        [Column("DATA_TYPE")]
        public string DATA_TYPE { get; set; }

        [StringLength(5)]
        [Column("PARAM_TYPE")]
        [Display(Name = "参数类型")]
        public string PARAM_TYPE { get; set; }

        [StringLength(255)]
        [Display(Name = "备注")]
        public string COMMENTS { get; set; }
    }
}
