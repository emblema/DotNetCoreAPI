using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace DotNetCoreAPI.Models
{
    public class SalesOrder
    {
        [SwaggerSchema("Sales Order ID", ReadOnly = true)]
        public int SalesOrderID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public virtual ICollection<SalesOrderItem> Items { get; set; }
    }
}
