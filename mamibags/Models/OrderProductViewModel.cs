using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mamibags.Models
{
    public class OrderProductViewModel
    {
        public tbl_Orders Orders { get; set; }  // The order information
        public tbl_Products Products { get; set; }  // The product information
    }
}