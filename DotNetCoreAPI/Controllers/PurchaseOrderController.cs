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
    public class PurchaseOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public PurchaseOrderController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<PurchaseOrder>))]
        public IEnumerable<PurchaseOrder> GetPurchaseOrders()
        {
            return _dbContext.PurchaseOrders.ToList();
        }

        [HttpGet("{id}", Name = "GetPurchaseOrder")]
        [SwaggerResponse(200, "Success", typeof(PurchaseOrder))]
        [SwaggerResponse(404, "Not Found")]
        public ActionResult<PurchaseOrder> GetPurchaseOrder(int id)
        {
            var order = _dbContext.PurchaseOrders.SingleOrDefault(p => p.PurchaseOrderID == id);
            if (order == null) return NotFound();

            return order;
        }

        [HttpPost]
        [SwaggerResponse(201, "Created", typeof(PurchaseOrder))]
        [SwaggerResponse(400, "Bad Request")]
        public ActionResult<PurchaseOrder> CreatePurchaseOrder([FromBody] PurchaseOrder order)
        {
            if (!ValidItems(order.Items)) return BadRequest();

            var newOrder = new PurchaseOrder
            {
                VendorID = order.VendorID,
                OrderDate = order.OrderDate
            };

            _dbContext.PurchaseOrders.Add(newOrder);
            _dbContext.SaveChanges();

            foreach (var item in order.Items)
            {
                var product = _dbContext.Products.SingleOrDefault(p => p.ProductID == item.ProductID);
                if (product == null) return NotFound();

                product.Quantity += item.Quantity;

                var newOrderItem = new PurchaseOrderItem
                {
                    PurchaseOrderID = newOrder.PurchaseOrderID,
                    ProductID = item.ProductID,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity
                };

                _dbContext.PurchaseOrderItems.Add(newOrderItem);
            }

            _dbContext.SaveChanges();

            order.PurchaseOrderID = newOrder.PurchaseOrderID;

            return CreatedAtRoute("GetPurchaseOrder", new { id = order.PurchaseOrderID }, order);
        }

        private bool ValidItems(ICollection<PurchaseOrderItem> orderItems)
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