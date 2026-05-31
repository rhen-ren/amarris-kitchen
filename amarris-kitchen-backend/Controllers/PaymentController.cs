using amarris_kitchen_backend.Data;
using amarris_kitchen_backend.DTOs;
using amarris_kitchen_backend.Models;
using amarris_kitchen_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace amarris_kitchen_backend.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly AmarrisKitchenContext _context;
        private readonly IXenditService _xenditService;
        public PaymentController(AmarrisKitchenContext context, IXenditService xenditService)
        {
            _context = context;
            _xenditService = xenditService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody]CreatePaymentDTO paymentDto)
        {
            if (paymentDto == null)
            {
                return BadRequest();
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
          
            try
            {
                var order = await _context.Orders.FindAsync(paymentDto.OrderId);
                if(order == null)
                {
                    return NotFound($"Order: {paymentDto.OrderId}");
                }
                var newPayment = new Payment
                {
                    OrderId = paymentDto.OrderId,
                    PaymentMethod = paymentDto.PaymentMethod,
                    Trn = null,
                    AmountPaid = 0,
                    PaymentDate = DateTime.Now,
                    Discount = 0,
                    Vat = 0

                };

                _context.Payments.Add(newPayment);
                await _context.SaveChangesAsync();

                string checkoutUrl = null;

                if (paymentDto.PaymentMethod.Equals("Online", StringComparison.OrdinalIgnoreCase))
                {
                    checkoutUrl = await _xenditService.CreateInvoiceAsync(
                        externalId: order.OrderId.ToString(),
                        amount: order.Price);
                }


                await transaction.CommitAsync();

                return Ok(new
                {
                    paymentId = newPayment.PaymentId,
                    checkoutUrl = checkoutUrl,
                    queueNumber = order.OrderId
                });
            }

  
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"[SYSTEM CRASH DEATILS]: {ex.ToString()}");
                return StatusCode(500, "Error while processing request");
            }

        }
        [HttpPost("finalize")]
        public async Task<IActionResult> FinalizePayment([FromBody] CreatePaymentDTO paymentDto)
        {
            if (paymentDto == null)
            {
                return BadRequest();
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var order = await _context.Orders.FindAsync(paymentDto.OrderId);
                if (order == null)
                {
                    return NotFound();
                }

                order.Status = "Paid";
                _context.Orders.Update(order);


                var payment = await _context.Payments
                    .FirstOrDefaultAsync(p => p.OrderId == paymentDto.OrderId);

                if(payment == null)
                {
                    payment = new Payment
                    {
                        OrderId = paymentDto.OrderId,
                        Order = order,
                        PaymentMethod = paymentDto.PaymentMethod,
                        PaymentDate = DateTime.Now,
                        Discount = 0,
                        Vat = 0
                    };

                    _context.Payments.Add(payment);
                }

                payment.AmountPaid = order.Price;
                payment.PaymentDate = DateTime.Now;


                payment.Trn = "XND-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                _context.Payments.Update(payment);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok();

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Internal error while finalizing transaction");
            }
        }
        
    }
}
