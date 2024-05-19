using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace mamibags.Models
{
    public partial class Products : DbContext
    {
        public Products()
            : base("name=Products")
        {
        }

        public virtual DbSet<tbl_Products> tbl_Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_Products>()
                .Property(e => e.Pname)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Products>()
                .Property(e => e.Size)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Products>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Products>()
                .Property(e => e.ImagePath)
                .IsUnicode(false);
        }
    }
}
