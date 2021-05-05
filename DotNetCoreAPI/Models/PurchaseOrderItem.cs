using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace DotNetCoreAPI.Models
{
    public class PurchaseOrderItem
    {
        [SwaggerSchema("Purchase Order Item ID", ReadOnly = true)]
        public int PurchaseOrderItemID { get; set; }

        [Required]
        [SwaggerSchema("Purchase Order ID", ReadOnly = true)]
        public int PurchaseOrderID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        [Range(0.00, 999.99)]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(1, 1000)]
        public int Quantity { get; set; }

        [SwaggerSchema("Purchase Order", ReadOnly = true)]
        public PurchaseOrder PurchaseOrder { get; set; }

        [JsonIgnore]
        [SwaggerSchema("Product", ReadOnly = true)]
        public Product Product { get; set; }
    }
}
