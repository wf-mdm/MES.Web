namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HR_OPERATORS
    {
        [Key]
        [Column(Order =0)]
        [StringLength(20)]
        [Display(Name = "�û�ID")]
        [Required]
        public string OPERID { get; set; }

        [StringLength(80)]
        [Column(Order = 1)]
        [Display(Name = "�û���")]
        [Required]
        public string OPERNAME { get; set; }

        [NotMapped]
        public String Name { get { return String.Format("{0}:{1}", OPERID, OPERNAME); } }

        [StringLength(128)]
        [Column(Order = 2)]
        [Display(Name = "����")]
        [Required]
        [DataType(DataType.Password)]
        public string PWD { get; set; }

        [Display(Name = "��˾")]
        [ForeignKey("BUNO")]
        public virtual ENG_BU BU { get; set; }

        [Column(Order = 3)]
        [StringLength(12)]
        [Required]
        [Display(Name = "��˾")]
        public string BUNO { get; set; }

        [Column(Order = 4)]
        [StringLength(20)]
        [Display(Name = "���Ŵ���")]
        public string DEPTNO { get; set; }
        
        [StringLength(1)]
        [Column(Order = 5)]
        [Display(Name = "��Ч�û�")]
        public String ACTIVE { get; set; }


        [Column(Order = 6)]
        [StringLength(120)]
        [Display(Name = "�����ʼ�")]
        public string EMAIL { get; set; }

        [Column(Order = 7)]
        [StringLength(80)]
        [Display(Name = "�ֻ���")]
        public string MOBILE { get; set; }

        [Column(Order = 8)]
        [StringLength(255)]
        [Display(Name = "��ע")]
        public string COMMENTS { get; set; }

    }
}
