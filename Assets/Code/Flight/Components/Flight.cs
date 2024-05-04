using Unity.Entities;
using Unity.Mathematics;

namespace Code.Flight
{
    public struct Flight: IComponentData
    {
        public float MaxSpeed;
        public float StrafePower;
        public float MaxYaw; // rad
        public float InertialDamperPower;
    }
}