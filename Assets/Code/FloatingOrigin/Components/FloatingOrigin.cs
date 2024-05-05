using Unity.Entities;
using Unity.Mathematics;

namespace Code.FloatingOrigin
{
    public struct FloatingOriginBase : IComponentData
    {
        public bool Enabled;
        
        public float PositionLimit;
        public bool UpdatePosition;
        public float3 DeltaPosition;
        
        public float VelocityLimit;
        public bool UpdateVelocity;
        public float3 DeltaVelocity;
    }

    public struct FloatingOriginTargetTag : IComponentData {}

    public struct FloatingOriginBodyTag : IComponentData {}
}