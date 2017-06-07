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
        [Required, Display(Name = "������id")]
        [Column(Order = 0)]
        public string CONFNAME { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "numeric")]
        [Required, Display(Name = "��ˮ��")]
        public decimal CONFID { get; set; }

        [StringLength(20)]
        [Display(Name = "����")]
        public string LINENAME { get; set; }

        [StringLength(10)]
        [Display(Name = "����")]
        public string L_OPNO { get; set; }

        [StringLength(10)]
        [Display(Name = "��վ")]
        public string L_STNO { get; set; }

        [StringLength(30)]
        [Display(Name = "����id")]
        public string PARAM_ID { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "ֵ")]
        public decimal? PARAM_VAL { get; set; }

        [StringLength(120)]
        [Display(Name = "�ı�ֵ")]
        public string PARAM_TEXT { get; set; }

        [StringLength(12)]
        [Display(Name = "��������")]
        [Column("DATA_TYPE")]
        public string DATA_TYPE { get; set; }

        [StringLength(5)]
        [Column("PARAM_TYPE")]
        [Display(Name = "��������")]
        public string PARAM_TYPE { get; set; }

        [StringLength(255)]
        [Display(Name = "��ע")]
        public string COMMENTS { get; set; }
    }
}
