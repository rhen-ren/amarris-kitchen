using System.ComponentModel.DataAnnotations;

namespace amarris_kitchen_backend.DTOs
{
    public class CreatePaymentDTO
    {
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }
        
    }
}
