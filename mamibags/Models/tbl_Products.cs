namespace mamibags.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Products
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName ("Product Name")]
        public string Pname { get; set; }

        [StringLength(100)]
        public string Size { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        [StringLength(255)]
        [DisplayName("Image")]
        public string ImagePath { get; set; }
    }

}
