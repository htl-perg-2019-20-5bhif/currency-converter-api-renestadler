namespace CurrencyConverter.Library
{
    public class Product
    {
        public string Description { get; set; }
        public string Currency { get; set; }
        public decimal Price { get; set; }

    }
    public class ExchangeRate
    {
        public string Currency { get; set; }
        public decimal Value { get; set; }
    }

    public class ProductPrice
    {
        public decimal Price { get; set; }
    }
}
