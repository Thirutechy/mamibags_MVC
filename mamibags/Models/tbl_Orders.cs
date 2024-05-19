namespace mamibags.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Orders
    {
        [Key]
        public int OrderID { get; set; }

        public int? ProductID { get; set; }

        public DateTime? OrderDate { get; set; }

        public int? Quantity { get; set; }

        [StringLength(100)]
        public string CustomerName { get; set; }

        [StringLength(11)]
        public string CustomerMobile { get; set; }

        [StringLength(300)]
        public string Address { get; set; }
    }
}
