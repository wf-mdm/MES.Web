namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HR_ROLES
    {
        [Key]
        [Column(Order = 0)]
        [Required, Display(Name = "ÓÃ»§")]
        [StringLength(20)]
        public string USERID { get; set; }

        [ForeignKey("USERID")]
        public virtual HR_OPERATORS User { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        [Required,Display(Name ="½ÇÉ«")]
        public string ROLEID { get; set; }
    }
}
