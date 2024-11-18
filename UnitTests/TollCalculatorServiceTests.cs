using NorionCodeTest.Entities;
using NorionCodeTest.Enums;
using NorionCodeTest.Services;

namespace UnitTests;

[TestClass]
public class TollCalculatorServiceTests
{
    private TollCalculatorService _target = new();
    private static readonly DateTime _startPassageDate = DateTime.Parse("2013-02-04 08:00:00");

    [TestMethod]
    public void GetTollFee_SinglePass()
    {
        var vehicle = new Vehicle()
        {
            VehicleType = VehicleType.Car
        };

        var actual = _target.GetTollFee(vehicle, [_startPassageDate]);

        Assert.AreEqual(13, actual);
    }

    [TestMethod]
    [DataRow("2013-02-04 06:00:00", 8)]
    [DataRow("2013-02-04 06:30:00", 13)]
    [DataRow("2013-02-04 07:00:00", 18)]
    [DataRow("2013-02-04 08:00:00", 13)]
    [DataRow("2013-02-04 08:30:00", 8)]
    [DataRow("2013-02-04 15:00:00", 13)]
    [DataRow("2013-02-04 15:30:00", 18)]
    [DataRow("2013-02-04 17:00:00", 13)]
    [DataRow("2013-02-04 18:00:00", 8)]
    [DataRow("2013-02-04 18:30:00", 0)]
    public void GetTollFee_CorrectTimeFee(string date, int expectedFee)
    {
        var vehicle = new Vehicle()
        {
            VehicleType = VehicleType.Car
        };

        var actual = _target.GetTollFee(vehicle, [DateTime.Parse(date)]);

        Assert.AreEqual(expectedFee, actual);
    }

    [TestMethod]
    [DataRow(VehicleType.Motorbike)]
    [DataRow(VehicleType.Tractor)]
    [DataRow(VehicleType.Emergency)]
    [DataRow(VehicleType.Diplomat)]
    [DataRow(VehicleType.Foreign)]
    [DataRow(VehicleType.Military)]
    public void GetTollFee_TollFreeVehicles(VehicleType vehicleType)
    {
        var vehicle = new Vehicle()
        {
            VehicleType = vehicleType
        };

        var actual = _target.GetTollFee(vehicle, [_startPassageDate]);

        Assert.AreEqual(0, actual);
    }

    [TestMethod]
    [DataRow("2013-02-02 08:00:00")] // Saturday
    [DataRow("2013-02-03 08:00:00")] // Sunday
    [DataRow("2013-01-01 08:00:00")]
    [DataRow("2013-03-28 08:00:00")]
    [DataRow("2013-03-29 08:00:00")]
    [DataRow("2013-04-01 08:00:00")]
    [DataRow("2013-04-30 08:00:00")]
    [DataRow("2013-05-01 08:00:00")]
    [DataRow("2013-05-08 08:00:00")]
    [DataRow("2013-05-09 08:00:00")]
    [DataRow("2013-06-05 08:00:00")]
    [DataRow("2013-06-06 08:00:00")]
    [DataRow("2013-06-21 08:00:00")]
    [DataRow("2013-11-01 08:00:00")]
    [DataRow("2013-12-24 08:00:00")]
    [DataRow("2013-12-25 08:00:00")]
    [DataRow("2013-12-26 08:00:00")]
    [DataRow("2013-12-31 08:00:00")]
    public void GetTollFee_TollFreeTimeSlots(string date)
    {
        var vehicle = new Vehicle()
        {
            VehicleType = VehicleType.Car
        };

        var actual = _target.GetTollFee(vehicle, [DateTime.Parse(date)]);

        Assert.AreEqual(0, actual);
    }

    [TestMethod]
    public void GetTollFee_TollFreeTimeSlots_JulyMonth()
    {
        var vehicle = new Vehicle()
        {
            VehicleType = VehicleType.Car
        };

        var numberOfDaysInJuly = DateTime.DaysInMonth(2013, 7);
        for (int i = 1; i <= numberOfDaysInJuly; i++)
        {
            var actual = _target.GetTollFee(vehicle, [DateTime.Parse($"2013-07-{i} 08:00:00")]);
            Assert.AreEqual(0, actual);
        }
    }

    [TestMethod]
    public void GetTollFee_MaxHourlyFee()
    {
        var vehicle = new Vehicle()
        {
            VehicleType = VehicleType.Car
        };

        var passageDates = new DateTime[]
        {
            _startPassageDate,
            _startPassageDate.AddMinutes(5),
            _startPassageDate.AddMinutes(6),
            _startPassageDate.AddMinutes(7),
        };

        var actual = _target.GetTollFee(vehicle, passageDates);

        Assert.AreEqual(13, actual);
    }

    [TestMethod]
    public void GetTollFee_MaxTotalFee()
    {
        var vehicle = new Vehicle()
        {
            VehicleType = VehicleType.Car
        };

        var passageDates = new DateTime[]
        {
            _startPassageDate,
            _startPassageDate.AddMinutes(5),
            _startPassageDate.AddMinutes(60),
            _startPassageDate.AddMinutes(61),
            _startPassageDate.AddMinutes(62),
            _startPassageDate.AddMinutes(63),
            _startPassageDate.AddMinutes(64),
            _startPassageDate.AddMinutes(65),
            _startPassageDate.AddMinutes(66),
            _startPassageDate.AddMinutes(67),
            _startPassageDate.AddMinutes(68),
        };

        var actual = _target.GetTollFee(vehicle, passageDates);

        Assert.AreEqual(60, actual);
    }
}
