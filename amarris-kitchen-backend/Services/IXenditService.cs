namespace amarris_kitchen_backend.Services
{
    public interface IXenditService
    {
        Task<string> CreateInvoiceAsync(string externalId, decimal amount);
    }
}
