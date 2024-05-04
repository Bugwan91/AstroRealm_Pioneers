using Unity.Entities;
using Unity.Mathematics;

namespace Code.Camera
{
    public struct CameraSettings : IComponentData
    {
        public float MinHeight;
        public float MinDistance;
        public float MaxHeight;
        public float MaxDistance;
        public float ScrollSpeed;
        public float ScrollSmooth;
        public bool InvertScroll;

        public float TiltSpeed;
        public float TiltUpLimit;
        public float ToCockpitHeightThreshold;

        // public float AccelerationMultiplier;
        // public float AccelerationMultiplierCockpit;
        public float AccelerationSmooth;
        public float AccelerationLimit;
        public float AccelerationLimitCockpit;
    }
}