using Unity.Entities;
using Unity.Mathematics;

namespace Code.Flight
{
    public struct StrafeInput : IComponentData
    {
        public float3 Value;
    }

    public struct DirectionInput : IComponentData
    {
        public float3 Value;
    }

    public struct InertialDamperInput : IComponentData
    {
        public float Value;
    }
}