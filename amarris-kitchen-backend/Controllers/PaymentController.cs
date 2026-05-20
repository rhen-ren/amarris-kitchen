using amarris_kitchen_backend.Data;
using amarris_kitchen_backend.DTOs;
using amarris_kitchen_backend.Models;
using amarris_kitchen_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.EntityFrameworkCore;

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
        //xendit will call this
        //[HttpPost("webhook")]
        //public async Task<IActionResult> XenditCallback([FromBody] XenditWebhookDTO callbackData)
        //{
        //    if(callbackData == null || callbackData.status != "PAID")
        //    {
        //        return BadRequest();
        //    }

        //    if(!int.TryParse(callbackData.external_id, out int orderId))
        //    {
        //        return BadRequest();
        //    }

        //    using var transaction = await _context.Database.BeginTransactionAsync();

        //    try
        //    {
        //        var payment = await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
        //        var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);

        //        if (payment != null)
        //        {
        //            payment.AmountPaid = callbackData.amount;
        //            payment.Trn = callbackData.id;
        //            payment.PaymentDate = DateTime.Now;




        //            await _context.SaveChangesAsync();
        //            await transaction.CommitAsync();


        //            return Ok();
        //        }
        //        if (order != null)
        //        {
        //            order.Status = "Complete";
        //        }
        //        else
        //        {
        //            Console.WriteLine("This Payment has no existing Order for #${orderId}");
        //        }
        //        return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        return StatusCode(500, "Internal error updating records.");
        //    }
        //}
    }
}
