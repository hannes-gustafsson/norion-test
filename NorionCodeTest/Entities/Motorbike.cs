using NorionCodeTest.Enums;

namespace NorionCodeTest.Entities;

public class Motorbike : IVehicle
{
    public VehicleType GetVehicleType()
    {
        return VehicleType.Motorbike;
    }
}
