namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ENG_PRDLINE
    {
        public string CodeName { get { return String.Format("{0}:{1}", LINENAME, DISPLAYNAME); } }

        [Key]
        [StringLength(20)]
        [Required,Display(Name ="���ߴ���")]
        public string LINENAME { get; set; }

        [StringLength(12)]
        [Required, Display(Name = "��˾")]
        public string BUNO { get; set; }

        [Display(Name = "��˾")]
        [Required, ForeignKey("BUNO")]
        public virtual ENG_BU BU { get; set; }

        [StringLength(80)]
        [Required, Display(Name = "��������")]
        public string DISPLAYNAME { get; set; }

        [Column(TypeName = "numeric")]
        [Required, Display(Name = "��׼Сʱ����")]
        public decimal? STDYIELDRATE { get; set; }

        [StringLength(12)]
        [Required, Display(Name = "������")]
        public string LINEGRP { get; set; }
    }
}
