using NorionCodeTest.Entities;
using NorionCodeTest.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NorionCodeTest.Services;

public interface ITollCalculatorService
{
    public int GetTollFee(Vehicle vehicle, DateTime[] dates);
}

public class TollCalculatorService : ITollCalculatorService
{
    private static readonly List<VehicleType> _tollFreeVehicles =
    [
        VehicleType.Motorbike,
        VehicleType.Tractor,
        VehicleType.Emergency,
        VehicleType.Diplomat,
        VehicleType.Foreign,
        VehicleType.Military
    ];

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
    public int GetTollFee(Vehicle vehicle, DateTime[] passageDates)
    {
        var firstPassageDate = passageDates[0];
        var totalFee = 0;
        var maxHourlyFee = GetTollFee(vehicle, firstPassageDate);
        const int maxTotalFee = 60;
        foreach (var passageDate in passageDates)
        {
            var currentFee = GetTollFee(vehicle, passageDate);
            var dateDifference = passageDate - firstPassageDate; // If a vehicle reaches 2 payment stations with less than 30 seconds in between this would cause inaccuracies due to rounding to an int. However I do not know how they are placed in reality so will keep it as is 
            (totalFee, maxHourlyFee) = CalculateMaxHourlyFee(dateDifference, currentFee, maxHourlyFee, totalFee);

            if (totalFee >= maxTotalFee)
                return maxTotalFee;
        }

        return totalFee >= maxTotalFee
            ? maxTotalFee
            : totalFee;
    }

    private static (int, int) CalculateMaxHourlyFee(TimeSpan dateDifference, int currentFee, int maxHourlyFee, int totalFee)
    {
        if (dateDifference.Minutes <= 60)
        {
            if (totalFee > 0)
            {
                totalFee -= maxHourlyFee;
            }
            if (currentFee >= maxHourlyFee)
            {
                maxHourlyFee = currentFee;
            }
            
            totalFee += maxHourlyFee;
        }
        else
        {
            totalFee += currentFee;
        }

        return (totalFee, maxHourlyFee);
    }

    private static int GetTollFee(Vehicle vehicle, DateTime passageDate)
    {
        if (IsTollFreeDate(passageDate) || IsTollFreeVehicle(vehicle))
            return 0;

        var hour = passageDate.Hour;
        var minute = passageDate.Minute;

        if (hour == 6 && minute <= 29)
            return 8;

        else if (hour == 6 && minute >= 30 && minute <= 59)
            return 13;

        else if (hour == 7 && minute <= 59)
            return 18;

        else if (hour == 8 && minute <= 29)
            return 13;

        else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59)
            return 8;

        else if (hour == 15 && minute <= 29)
            return 13;

        else if (hour == 15 || hour == 16 && minute <= 59)
            return 18;

        else if (hour == 17 && minute <= 59)
            return 13;

        else if (hour == 18 && minute <= 29)
            return 8;

        else
            return 0;
    }

    private static bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle is null)
            return false;

        return _tollFreeVehicles.Contains(vehicle.VehicleType);
    }

    private static bool IsTollFreeDate(DateTime date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            return true;

        var year = date.Year;
        var month = date.Month;
        var day = date.Day;

        if (year == 2013)   // If this was a real world case one would have to perform this kind of a calculation for every year due to holidays etc. changing
        {
            if (month == 1 && day == 1
                || month == 3 && (day == 28 || day == 29)
                || month == 4 && (day == 1 || day == 30)
                || month == 5 && (day == 1 || day == 8 || day == 9)
                || month == 6 && (day == 5 || day == 6 || day == 21)
                || month == 7
                || month == 11 && day == 1
                || month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
            {
                return true;
            }
        }

        return false;
    }
}