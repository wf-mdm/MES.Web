using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MES.Web.Areas.Admin.Models
{
    public partial class ENG_VARIABLES
    {
        [Key]
        [Column(Order =0)]
        [Required, Display(Name = "产线代码")]
        [StringLength(20)]
        public String LINENAME { get; set; }

        [Display(Name = "产线")]
        [ForeignKey("LINENAME")]
        public virtual ENG_PRDLINE Line { get; set; }

        [Key]
        [Column(Order = 1)]
        [Required, Display(Name = "料号")]
        [StringLength(20)]
        public String PARTNO { get; set; }

        [Key]
        [Column(Order = 2)]
        [Required, Display(Name = "变量名")]
        [StringLength(20)]
        public String VARNAME { get; set; }


        [Display(Name = "变量类型")]
        [StringLength(20)]
        public String VARTYPE { get; set; }

        [Display(Name = "工站")]
        [StringLength(20)]
        public String L_STNO { get; set; }

        [Display(Name = "变量说明")]
        [StringLength(20)]
        public String VARDESC { get; set; }

        [Display(Name = "变量值")]
        [StringLength(20)]
        public String VARVALUE { get; set; }

        [Display(Name = "更新时间")]
        public DateTime? UPDATETIME { get; set; }
    }
}