using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace mamibags.Models
{
    public partial class Orders : DbContext
    {
        public Orders()
            : base("name=Orders")
        {
        }

        public virtual DbSet<tbl_Orders> tbl_Orders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_Orders>()
                .Property(e => e.CustomerName)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Orders>()
                .Property(e => e.CustomerMobile)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Orders>()
                .Property(e => e.Address)
                .IsUnicode(false);
        }
    }
}
