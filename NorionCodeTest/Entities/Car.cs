using NorionCodeTest.Enums;

namespace NorionCodeTest.Entities;

public class Car : IVehicle
{
    public VehicleType GetVehicleType()
    {
        return VehicleType.Car;
    }
}