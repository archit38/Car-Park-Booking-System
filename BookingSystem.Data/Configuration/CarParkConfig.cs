namespace BookingSystem.API.Configuration
{
    public class CarParkConfig
    {
        public int Capacity { get; set; }
        public decimal WeekdayPrice { get; set; }
        public decimal WeekendPrice { get; set; }
        public decimal SummerPriceMultiplier { get; set; }
        public decimal WinterPriceMultiplier { get; set; }
    }

}
