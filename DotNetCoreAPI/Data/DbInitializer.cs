using System.Linq;
using DotNetCoreAPI.Models;

namespace DotNetCoreAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (context.Products.Any())
            {
                return; 
            }

            var products = new Product[]
            {
                new Product() { ProductID = 1, Description = "Juguete 1", UnitPrice = 9.99M, Quantity = 100 },
                new Product() { ProductID = 2, Description = "Juguete 2", UnitPrice = 9.99M, Quantity = 200 },
                new Product() { ProductID = 3, Description = "Juguete 3", UnitPrice = 9.99M, Quantity = 300 },
                new Product() { ProductID = 4, Description = "Juguete 4", UnitPrice = 9.99M, Quantity = 400 },
                new Product() { ProductID = 5, Description = "Juguete 5", UnitPrice = 9.99M, Quantity = 500 },
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}