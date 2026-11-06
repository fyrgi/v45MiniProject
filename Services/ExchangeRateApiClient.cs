using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using v45MiniProject.Models;

namespace v45MiniProject.Services
{
    public class ExchangeRateApiClient
    {
        private readonly HttpClient _httpClient;

        public ExchangeRateApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ExchangeRate?> GetHistoricalRateAsync(
            string fromCurrency,
            string toCurrency,
            DateTime date)
        {
            // Look for the requested date first.
            // If it is a weekend or holiday, try earlier dates.

            for (int i = 0; i <= 7; i++)
            {
                DateTime requestedDate = date.Date.AddDays(-i);

                string dateString = requestedDate.ToString(
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture);

                string url =
                    $"v2/rate/{fromCurrency}/{toCurrency}/{dateString}";

                using HttpResponseMessage response =
                    await _httpClient.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    continue;
                }

                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ApiRateResponse>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (result == null || result.Rate <= 0)
                    return null;

                return new ExchangeRate
                {
                    Rate = result.Rate,
                    Source = "API"
                };
            }

            return null;
        }

        private class ApiRateResponse
        {
            public string Date { get; set; } = string.Empty;
            public string Base { get; set; } = string.Empty;
            public string Quote { get; set; } = string.Empty;
            public decimal Rate { get; set; }
        }
    }
}