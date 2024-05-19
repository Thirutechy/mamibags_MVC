namespace mamibags.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_regCustomer
    {
        [Key]
        public int CustomerID { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public int? Mobile { get; set; }

        [StringLength(100)]
        public string Address { get; set; }
    }

    public partial class tbl_feedback
    {
        [Key]
        public int feedbackId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public string Mobilenumber { get; set; }

        [StringLength(500)]
        public string Message { get; set; }
    }


}
