using Unity.Entities;
using Unity.Mathematics;

namespace Code.Flight
{
    public struct Acceleration : IComponentData
    {
        public float3 PreviousVelocity;
        public float3 Value;
    }
}