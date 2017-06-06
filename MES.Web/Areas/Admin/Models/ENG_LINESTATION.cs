namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ENG_LINESTATION
    {
        public string CodeName { get { return String.Format("{0}:{1}", L_STNO, DISPLAYNAME); } }

        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        [Required, Display(Name = "产线")]
        public string LINENAME { get; set; }

        [StringLength(10)]
        [Required, Display(Name = "工序代码")]
        public string L_OPNO { get; set; }

        [Display(Name = "工序代码")]
        [ForeignKey("LINENAME,L_OPNO")]
        public virtual ENG_LINEOP Op{ get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        [Required, Display(Name = "工站代码")]
        public string L_STNO { get; set; }

        [StringLength(80)]
        [Required, Display(Name = "显示名称")]
        public string DISPLAYNAME { get; set; }
/*
        [StringLength(255)]
        public string SCRIPTFILE { get; set; }

        [StringLength(80)]
        public string TYPENAME { get; set; }

        [StringLength(1024)]
        public string STICON { get; set; }

        [StringLength(1024)]
        public string STIMG { get; set; }
*/
    }
}
