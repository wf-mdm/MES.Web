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
        [Required, Display(Name = "�������̴���")]
        public string RT_NAME { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "numeric")]
        [Required, Display(Name = "˳��")]
        public decimal SEQNO { get; set; }

        [StringLength(20)]
        [Required, Display(Name = "���ߴ���")]
        public string LINENAME { get; set; }

        [StringLength(3)]
        [Required, Display(Name = "��֧��")]
        public string SUBLINENO { get; set; }

        [StringLength(10)]
        [Required, Display(Name = "�����")]
        public string L_OPNO { get; set; }

        [Display(Name = "�������")]
        [ForeignKey("LINENAME,L_OPNO")]
        public virtual ENG_LINEOP Op { get; set; }

        [StringLength(80)]
        [Required, Display(Name = "ǰ�ù���")]
        public string PREV_OPLIST { get; set; }

        [StringLength(80)]
        [Display(Name = "���ù���")]
        public string NEXT_OPLIST { get; set; }

        [StringLength(1)]
        [Display(Name = "�Ƿ��׹���")]
        public string IsFirst { get; set; }

        [StringLength(1)]
        [Display(Name = "�Ƿ�ĩ����")]
        public string IsLast { get; set; }

        [StringLength(255)]
        [Display(Name = "��ע")]
        public string COMMENTS { get; set; }
    }
}
