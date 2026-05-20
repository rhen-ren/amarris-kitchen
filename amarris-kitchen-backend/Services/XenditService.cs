using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace amarris_kitchen_backend.Services
{
    public class XenditService : IXenditService
    {
        private readonly HttpClient _httpClient;
        private readonly string _secretKey;

        public XenditService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _secretKey = configuration["Xendit:SecretKey"] ?? string.Empty;
        }
        public async Task<string> CreateInvoiceAsync(string externalId, decimal amount)
        {
            var requestUrl = "https://api.xendit.co/v2/invoices";

            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_secretKey}:"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            var payload = new Dictionary<string, object>
            {
                { "external_id", externalId },
                { "amount", amount },
                { "currency", "PHP" },
                { "description", $"Amarri's Kitchen Kiosk Order #{externalId}" },
                { "success_redirect_url", "http://localhost:5250//amarris-kitchen-sample-kiosk/templates/success.html" },
                { "failure_redirect_url", "http://localhost:5250/amarris-kitchen-sample-kiosk/templates/payment.html" }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");


            var response = await _httpClient.PostAsync(requestUrl, jsonContent);

            if(!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"{response.StatusCode} - {errorContent}");

            }

            var responseString = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseString);

            if(jsonDoc.RootElement.TryGetProperty("invoice_url", out var urlElement))
            {
                return urlElement.GetString()!;
            }

            throw new Exception();
        }
    }
}
