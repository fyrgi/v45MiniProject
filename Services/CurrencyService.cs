using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using v45MiniProject.Models;

namespace v45MiniProject.Services
{
    public class CurrencyService
    {
        //private readonly IExchangeRateApi _rateProvider;

        private readonly Dictionary<string, decimal> _exchangeRates = new()
        {
            { "USD", 1.00m },
            { "SEK", 10.50m },
            { "TRY", 35.00m }
        };

        // Retrieves the exchange rate and its source
        public ExchangeRate GetExchangeRate(string currencyCode)
        {
            if (!_exchangeRates.TryGetValue(
                currencyCode, out decimal rate))
            {
                throw new ArgumentException(
                    $"Exchange rate for {currencyCode} not found.");
            }

            return new ExchangeRate
            {
                Rate = rate,
                Source = "Hardcoded"
            };
        }

        // Converts the original price into the local currency
        public decimal ConvertToLocalCurrency(
            decimal price,
            ExchangeRate exchangeRate)
        {
            return Math.Round(
                price * exchangeRate.Rate,
                2,
                MidpointRounding.AwayFromZero);
        }
    }


    /*
    public CurrencyService(IExchangeRateApi rateProvider)
    {
        _rateProvider = rateProvider;
    }

    public async Task<ExchangeRate> GetExchangeRateAsync(
        string fromCurrency,
        string toCurrency,
        DateTime purchaseDate)
    {
        // No conversion needed.
        if (fromCurrency == toCurrency)
        {
            return new ExchangeRate
            {
                Rate = 1m,
                Source = "Same currency"
            };
        }

        // First attempt: use the API.
        try
        {
            ExchangeRate? apiRate =
                await _rateProvider.GetHistoricalRateAsync(
                    fromCurrency,
                    toCurrency,
                    purchaseDate);

            if (apiRate != null)
                return apiRate;
        }
        catch (HttpRequestException)
        {
            // API unavailable.
        }
        catch (TaskCanceledException)
        {
            // API request timed out.
        }
        catch (JsonException)
        {
            // Invalid API response.
        }

        // Second attempt: use fallback.
        if (fromCurrency == "USD" &&
            _fallbackRates.TryGetValue(
                toCurrency, out decimal fallbackRate))
        {
            return new ExchangeRate
            {
                Rate = fallbackRate,
                Source = "Fallback"
            };
        }

        throw new InvalidOperationException(
            $"No exchange rate available for {fromCurrency}/{toCurrency}.");
    }*/

}