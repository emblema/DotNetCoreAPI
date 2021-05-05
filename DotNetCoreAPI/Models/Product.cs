using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace DotNetCoreAPI.Models
{
    public class Product
    {
        [SwaggerSchema("Product ID", ReadOnly = true)]
        public int ProductID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Description { get; set; }

        [Required]
        [Range(0.00, 999.99)]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(0, 100000)]
        public int Quantity { get; set; }
    }
}
