using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MES.Web.Models
{
    public class MESDbContext: DbContext
    {
        public MESDbContext()
            :base("DefaultConnection")
        {
        }

        public DbSet<SysUserModel> AppUsers { get; set; }
        public DbSet<LineModel> Lines { get; set; }
        public DbSet<OpModel> Ops { get; set; }
        public DbSet<StnModel> Stns { get; set; }
    }
}