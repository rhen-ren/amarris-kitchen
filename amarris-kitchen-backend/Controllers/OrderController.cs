using amarris_kitchen_backend.Data;
using amarris_kitchen_backend.DTOs;
using amarris_kitchen_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace amarris_kitchen_backend.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class OrderController : ControllerBase
    {
        private readonly AmarrisKitchenContext _context;
        public OrderController(AmarrisKitchenContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO orderDto)
        {
            if (orderDto == null || orderDto.Items == null || !orderDto.Items.Any())
            {
                return BadRequest();
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                decimal grandTotal = 0;
                //var orderItemsToSave = new List<OrderItem>();

                //foreach (var itemDto in orderDto.Items)
                //{
                //    var product = await _context.Products.FindAsync(itemDto.ProductId);
                //    if (product == null)
                //    {
                //        return NotFound();
                //    }

                //    decimal itemSubtotal = product.UnitPrice * itemDto.Qty;
                //    grandTotal += itemSubtotal;

                //    orderItemsToSave.Add(new OrderItem { ProductId = product.ProductId, Quantity = itemDto.Qty });

                //}

                var incomingProductIds = orderDto.Items.Select(i => i.ProductId).Distinct().ToList();

                var products = await _context.Products
                    .Where(p => incomingProductIds.Contains(p.ProductId))
                    .ToDictionaryAsync(p => p.ProductId);


                grandTotal = orderDto.Items.Sum(itemDto => products[itemDto.ProductId].UnitPrice * itemDto.Qty);

                var newOrder = new Order
                {
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    OrderTime = TimeOnly.FromDateTime(DateTime.Now),
                    OrderMode = orderDto.OrderMode,
                    Status = "Pending",
                    Price = grandTotal

                };

                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync();

                //foreach (var item in orderItemsToSave)
                //{
                //    item.OrderId = newOrder.OrderId;
                //    _context.OrderItems.Add(item);
                //}

                var orderItemsToSave = orderDto.Items.Select(itemDto => new OrderItem
                {
                    OrderId = newOrder.OrderId,
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Qty
                }).ToList;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();


                return Ok(new
                {
                    OrderId = newOrder.OrderId,
                    TotalAmount = grandTotal,
                    Message = "Order created successfully."
                });

            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500);
            }

        }
    }
}
