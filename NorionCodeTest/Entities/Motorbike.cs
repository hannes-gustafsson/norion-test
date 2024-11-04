using NorionCodeTest.Enums;

namespace NorionCodeTest.Entities;

public class Motorbike : IVehicle
{
    public string GetVehicleType()
    {
        return "Motorbike";
    }
}
