using Unity.Entities;
using Unity.Mathematics;

namespace Code.Camera
{
    public struct VelocityPointer : IComponentData
    {
        public float3 ForwardPosition;
        public float3 BackwardPosition;
    }
}