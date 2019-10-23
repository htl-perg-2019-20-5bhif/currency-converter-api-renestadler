using CurrencyConverter.Library;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyConverter.Controllers
{
    [ApiController]
    public class CurrencyConverterController : ControllerBase
    {
        private readonly HttpClient client;

        public CurrencyConverterController(IHttpClientFactory clientFactory)
        {
            this.client = clientFactory.CreateClient("data");
        }


        [HttpGet]
        [Route("/api/products/{product}/price")]
        public async Task<ProductPrice> Get([FromRoute]string product, [FromQuery]string targetCurrency)
        {
            string exchangeRatesPath = "ExchangeRates.csv";
            IEnumerable<ExchangeRate> exchangeRates = (await (await client.GetAsync(exchangeRatesPath))
                .EnsureSuccessStatusCode().Content.ReadAsStringAsync()).
                Replace("\r", string.Empty).Split("\n").Select((s, id) =>
                {
                    Console.WriteLine(id);
                    if (id != 0)
                    {
                        string[] args = s.Split(",");
                        if (args.Count() == 2)
                        {
                            ExchangeRate exchangeRate = new ExchangeRate();
                            exchangeRate.Currency = args[0];
                            exchangeRate.Value = Decimal.Parse(args[1], CultureInfo.InvariantCulture);
                            return exchangeRate;
                        }
                    }
                    return null;
                }).Where(e => e != null);
            string pricesPath = "Prices.csv";
            IEnumerable<Product> products = (await (await client.GetAsync(pricesPath))
                .EnsureSuccessStatusCode().Content.ReadAsStringAsync()).
                Replace("\r", string.Empty).Split("\n").Select((s, id) =>
                {
                    Console.WriteLine(id);
                    if (id != 0)
                    {
                        string[] args = s.Split(",");
                        if (args.Count() == 3)
                        {
                            Product exchangeRate = new Product();
                            exchangeRate.Description = args[0];
                            exchangeRate.Currency = args[1];
                            exchangeRate.Price = Decimal.Parse(args[2], CultureInfo.InvariantCulture);
                            return exchangeRate;
                        }
                    }
                    return null;
                }).Where(e => e != null);
            CurrencyConverterService currencyConverter = new CurrencyConverterService(exchangeRates, products);
            return currencyConverter.ConvertCurrency(product, targetCurrency);
        }
    }
}
