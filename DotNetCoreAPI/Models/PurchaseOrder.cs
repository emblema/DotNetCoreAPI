using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace DotNetCoreAPI.Models
{
    public class PurchaseOrder
    {
        [SwaggerSchema("Purchase Order ID", ReadOnly = true)]
        public int PurchaseOrderID { get; set; }

        [Required]
        public int VendorID { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public virtual ICollection<PurchaseOrderItem> Items { get; set; }
    }
}
