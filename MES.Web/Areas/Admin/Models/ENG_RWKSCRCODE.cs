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
        [Required, Display(Name = "��������")]
        public string QCCODE { get; set; }

        [StringLength(1)]
        [Required, Display(Name = "����")]
        public string QCTYPE { get; set; }

        [StringLength(255)]
        [Required, Display(Name = "˵��")]
        public string DESCRIPTION { get; set; }

        [StringLength(20)]
        [Required, Display(Name = "����")]
        public string LINENAME { get; set; }

        [StringLength(10)]
        [Required, Display(Name = "������")]
        public string DEFAULTOOP { get; set; }

        [StringLength(10)]
        [Display(Name = "��Դ����")]
        public string FROMOP { get; set; }

        [StringLength(255)]
        [Display(Name = "��ע")]
        public string COMMENTS { get; set; }
    }
}
