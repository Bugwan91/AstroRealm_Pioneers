using Unity.Entities;
using Unity.Mathematics;

namespace Code.Camera
{
    public struct Camera : IComponentData
    {
        public float Height; // 0...1
        public float NeededHeight; // 0...1
        public float TiltAngle; // deg
        public float PointerTiltAngle; // deg
        public float3 Acceleration;
        public float3 OriginPosition;
    }
}