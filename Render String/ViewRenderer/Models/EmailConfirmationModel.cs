using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewRendererSamples.Models
{
    public class EmailConfirmationModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public decimal OrderTotal { get; set; }
        public string  InvoiceNo { get; set; }

        public string ItemSku { get; set; }
        public string ItemDescription { get; set; }

        public string StoreName { get; set; }        
    }
}