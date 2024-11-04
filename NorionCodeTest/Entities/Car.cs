using NorionCodeTest.Enums;

namespace NorionCodeTest.Entities;

public class Car : IVehicle
{
    public string GetVehicleType()
    {
        return "Car";
    }
}