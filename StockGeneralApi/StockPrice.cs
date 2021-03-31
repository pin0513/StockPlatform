namespace StockGeneralApi
{
    public class StockGeneralMessage
    {
        public bool success { get; set; }
        public string message { get; set; }
    }

    public class StockPrice : StockGeneralMessage
    {
        public decimal price { get; set; }
    }
}