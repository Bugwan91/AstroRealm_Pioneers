using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Code.Camera
{
    [BurstCompile]
    public readonly partial struct CameraOrbitAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRW<CameraOrbit> _orbit;
        private readonly RefRO<CameraOrbitSettings> _settings;

        private float RotationAngle
        {
            get => _orbit.ValueRO.RotationAngle;
            set => _orbit.ValueRW.RotationAngle = value;
        }

        private float RotationSpeed => _settings.ValueRO.RotationSpeed;

        [BurstCompile]
        public void UpdateRotation(float input)
        {
            RotationAngle += input * RotationSpeed;
            _transform.ValueRW.Rotation = quaternion.RotateY(math.radians(RotationAngle));
        }

        [BurstCompile]
        public void UpdatePosition(float3 position)
        {
            _transform.ValueRW.Position = position;
        }
    }
}