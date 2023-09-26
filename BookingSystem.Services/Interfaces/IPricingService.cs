namespace BookingSystem.Services.Interfaces
{
    public interface IPricingService
    {
        decimal CalculatePrice(DateTime from, DateTime to);
    }
}
