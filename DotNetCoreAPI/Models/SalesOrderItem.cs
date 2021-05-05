using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace DotNetCoreAPI.Models
{
    public class SalesOrderItem
    {
        [SwaggerSchema("Sales Order Item ID", ReadOnly = true)]
        public int SalesOrderItemID { get; set; }

        [Required]
        [SwaggerSchema("Sales Order ID", ReadOnly = true)]
        public int SalesOrderID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        [Range(0.00, 999.99)]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(1, 1000)]
        public int Quantity { get; set; }

        [SwaggerSchema("Sales Order", ReadOnly = true)]
        public SalesOrder SalesOrder { get; set; }

        [JsonIgnore]
        [SwaggerSchema("Product", ReadOnly = true)]
        public Product Product { get; set; }
    }
}
