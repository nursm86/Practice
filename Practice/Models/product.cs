//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Practice.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class product
    {
        public int productId { get; set; }
        public int userId { get; set; }
        [DisplayName("Product Name")]
        public string productName { get; set; }
        [DisplayName("Quantity")]
        public int quantity { get; set; }
        [DisplayName("Price")]
        public double price { get; set; }
        public System.DateTime createdDate { get; set; }
        public System.DateTime updatedDate { get; set; }
        public virtual user user { get; set; }
    }
}
