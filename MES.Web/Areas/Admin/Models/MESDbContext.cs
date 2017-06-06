namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MESDbContext : DbContext
    {
        public MESDbContext()
            : base("DefaultConnection")
        {
        }

        public virtual DbSet<ENG_LINEOP> ENG_LINEOP { get; set; }
        public virtual DbSet<ENG_LINESTATION> ENG_LINESTATION { get; set; }
        public virtual DbSet<ENG_PRDLINE> ENG_PRDLINE { get; set; }
        public virtual DbSet<ENG_ROUTE> ENG_ROUTE { get; set; }
        public virtual DbSet<ENG_RWKSCRCODE> ENG_RWKSCRCODE { get; set; }
        public virtual DbSet<HR_OPERATORS> HR_OPERATORS { get; set; }
        public virtual DbSet<HR_ROLES> HR_ROLES { get; set; }
        public virtual DbSet<ENG_LINEOPPARAMCONF> ENG_LINEOPPARAMCONF { get; set; }
        public virtual DbSet<APP_MASTDATA> APP_MASTDATA { get; set; }
        public virtual DbSet<ENG_BU> ENG_BU { get; set; }
        public virtual DbSet<HR_OPERCAPBMATRIX> HR_OPERCAPBMATRIX { get; set; }
        public virtual DbSet<HR_PERMISSIONS> HR_PERMISSIONS { get; set; }
    }
}
