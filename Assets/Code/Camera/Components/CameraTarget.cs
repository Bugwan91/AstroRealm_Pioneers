using Unity.Entities;
using Unity.Mathematics;

namespace Code.Camera
{
    public struct CameraTarget : IComponentData
    {
        public float3 Acceleration;
    }
}