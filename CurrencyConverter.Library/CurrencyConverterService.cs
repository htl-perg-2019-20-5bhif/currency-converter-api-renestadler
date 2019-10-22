using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyConverter.Library
{
    public class CurrencyConverterService
    {
        private readonly IEnumerable<ExchangeRate> exchangeRates;
        private readonly IEnumerable<Product> products;

        public CurrencyConverterService(IEnumerable<ExchangeRate> exchangeRates, IEnumerable<Product> products)
        {
            this.exchangeRates = exchangeRates;
            this.products = products;
        }

        public ProductPrice ConvertCurrency(string productName, string targetCurrency)
        {
            if (targetCurrency == "EUR")
            {
                Product product = products.First(p => p.Description == productName);
                if (product.Currency == "EUR")
                {
                    return new ProductPrice { Price = product.Price };
                }
                ExchangeRate rate = exchangeRates.First(e => e.Currency == product.Currency);
                return new ProductPrice { Price = Math.Round((1 / rate.Value) * product.Price, 2) };
            }
            else
            {
                ExchangeRate rate = exchangeRates.First(e => e.Currency == targetCurrency);
                return new ProductPrice { Price = Math.Round((ConvertCurrency(productName, "EUR").Price / (1 / rate.Value)), 2) };
            }
        }
    }
}
