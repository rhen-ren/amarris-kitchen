namespace amarris_kitchen_backend.DTOs
{
    public class XenditWebhookDTO
    {
        public string id {  get; set; }
        public string external_id { get; set; }
        public string status { get; set; }
        public decimal amount { get; set; }
    }
}
