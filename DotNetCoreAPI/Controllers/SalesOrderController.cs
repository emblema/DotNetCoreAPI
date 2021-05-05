using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using DotNetCoreAPI.Models;
using DotNetCoreAPI.Data;

namespace DotNetCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public SalesOrderController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<SalesOrder>))]
        public IEnumerable<SalesOrder> GetSalesOrders()
        {
            return _dbContext.SalesOrders.ToList();
        }

        [HttpGet("{id}", Name = "GetSalesOrder")]
        [SwaggerResponse(200, "Success", typeof(SalesOrder))]
        [SwaggerResponse(404, "Not Found")]
        public ActionResult<SalesOrder> GetSalesOrder(int id)
        {
            var order = _dbContext.SalesOrders.SingleOrDefault(p => p.SalesOrderID == id);
            if (order == null) return NotFound();

            return order;
        }

        [HttpPost]
        [SwaggerResponse(201, "Created", typeof(SalesOrder))]
        [SwaggerResponse(400, "Bad Request")]
        public ActionResult<SalesOrder> CreateSalesOrder([FromBody] SalesOrder order)
        {
            if (!ValidItems(order.Items)) return BadRequest();

            var newOrder = new SalesOrder
            {
                CustomerID = order.CustomerID,
                OrderDate = order.OrderDate
            };

            _dbContext.SalesOrders.Add(newOrder);
            _dbContext.SaveChanges();

            foreach (var item in order.Items)
            {
                var product = _dbContext.Products.SingleOrDefault(p => p.ProductID == item.ProductID);
                if (product == null) return NotFound();

                product.Quantity -= item.Quantity;

                var newOrderItem = new SalesOrderItem
                {
                    SalesOrderID = newOrder.SalesOrderID,
                    ProductID = item.ProductID,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity
                };

                _dbContext.SalesOrderItems.Add(newOrderItem);
            }

            _dbContext.SaveChanges();

            order.SalesOrderID = newOrder.SalesOrderID;

            return CreatedAtRoute("GetSalesOrder", new { id = order.SalesOrderID }, order);
        }

        private bool ValidItems(ICollection<SalesOrderItem> orderItems)
        {
            foreach (var item in orderItems)
            {
                var product = _dbContext.Products.SingleOrDefault(p => p.ProductID == item.ProductID);
                if (product == null || product.Quantity < item.Quantity) return false;
            }

            return true;
        }
    }
}