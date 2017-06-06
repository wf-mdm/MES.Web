namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ENG_LINEOP
    {
        public string CodeName { get { return String.Format("{0}:{1}", L_OPNO, DISPLAYNAME); } }

        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        [Required, Display(Name = "���ߴ���")]
        public string LINENAME { get; set; }

        [Display(Name = "����")]
        [ForeignKey("LINENAME")]
        public virtual ENG_PRDLINE Line { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        [Required, Display(Name = "�������")]
        public string L_OPNO { get; set; }

        [StringLength(80)]
        [Required, Display(Name = "��ʾ����")]
        public string DISPLAYNAME { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "���")]
        public decimal? OPDEFAULTSEQ { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "��׼Сʱ����")]
        public decimal? STDYIELDRATE { get; set; }


        [Column(TypeName = "numeric")]
        [Display(Name = "����ʱ��")]
        public decimal? CYCLETIME { get; set; }
    }
}
