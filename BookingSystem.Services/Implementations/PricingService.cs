using BookingSystem.API.Configuration;
using BookingSystem.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Services.Implementations
{
    public class PricingService : IPricingService
    {
        private readonly CarParkConfig pricingConfig;

        public PricingService(IOptions<CarParkConfig> config)
        {
            this.pricingConfig = config.Value;
        }

        //The function is assuming is assuming that checkout date is not charged similar to hotel bookings so a booking from 01/01 - 03/01 is 2 days
        public decimal CalculatePrice(DateTime from, DateTime to)
        {
            TimeSpan duration = to - from;
            int totalDays = (int)Math.Ceiling(duration.TotalDays);
            decimal totalPrice = 0;

            DateTime currentDate = from;
            while (currentDate < to)
            {
                decimal dayPrice;

                // Check if the current day is a Saturday or Sunday
                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    dayPrice = pricingConfig.WeekendPrice;
                }
                else
                {
                    dayPrice = pricingConfig.WeekdayPrice;
                }

                // Apply summer and winter multipliers if applicable
                if (IsSummerSeason(from, to))
                {
                    dayPrice *= pricingConfig.SummerPriceMultiplier;
                }
                else if (IsWinterSeason(from, to))
                {
                    dayPrice *= pricingConfig.WinterPriceMultiplier;
                }

                totalPrice += dayPrice;

                currentDate = currentDate.AddDays(1);
            }

            return totalPrice;
        }


        private bool IsSummerSeason(DateTime from, DateTime to) => from.Month >= 6 && from.Month <= 8;

        private bool IsWinterSeason(DateTime from, DateTime to) => from.Month == 12 || from.Month <= 2;


    }

}
