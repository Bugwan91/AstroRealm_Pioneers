using Code.Utils;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Code.Camera
{
    [BurstCompile]
    public readonly partial struct CameraAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRW<Camera> _camera;
        private readonly RefRO<CameraSettings> _settings;

        private float Height
        {
            get => _camera.ValueRO.Height;
            set => _camera.ValueRW.Height = value;
        }
        
        private float NeededHeight
        {
            get => _camera.ValueRO.NeededHeight;
            set => _camera.ValueRW.NeededHeight = value;
        }

        public float3 Position => _transform.ValueRO.Position;
        public float3 OriginPosition => _camera.ValueRO.OriginPosition;
        public quaternion Rotation => _transform.ValueRO.Rotation;
        public quaternion PointerRotation => quaternion.RotateX(math.radians(PointerTiltAngle));
        private float MinDistance => _settings.ValueRO.MinDistance;
        private float DistanceDelta => _settings.ValueRO.MaxDistance - MinDistance;

        private float MinHeight => _settings.ValueRO.MinHeight;
        private float HeightDelta => _settings.ValueRO.MaxHeight - MinHeight;
        
        private float CurrentHeight => MinHeight + HeightDelta * math.pow(Height, 3f);
        private float CurrentDistance => MinDistance + DistanceDelta * math.pow(Height, 3f);
        private float ScrollSpeed => _settings.ValueRO.ScrollSpeed;
        private float ScrollSmooth => _settings.ValueRO.ScrollSmooth;
        private float InvertScroll => _settings.ValueRO.InvertScroll ? -1f : 1f;

        private float TiltAngle
        {
            get => _camera.ValueRO.TiltAngle;
            set => _camera.ValueRW.TiltAngle = value;
        }

        private float PointerTiltAngle
        {
            get => _camera.ValueRO.PointerTiltAngle;
            set => _camera.ValueRW.PointerTiltAngle = value;
        }

        private float3 a
        {
            get => _camera.ValueRO.Acceleration;
            set => _camera.ValueRW.Acceleration = value;
        }

        private float TiltSpeed => _settings.ValueRO.TiltSpeed;
        
        private float PointerTiltUpLimit => math.degrees(math.atan(CurrentHeight / 100000f));
        
        private float TiltUpLimit => math.max(_settings.ValueRO.TiltUpLimit * Height, PointerTiltUpLimit);

        private float TiltDownLimit => InCockpit ? 20f : math.degrees(math.atan(CurrentHeight / MinDistance));

        public bool InCockpit => CurrentHeight - MinHeight < _settings.ValueRO.ToCockpitHeightThreshold;

        public float3 Acceleration => MathUtil.ClampVector(_camera.ValueRO.Acceleration * (InCockpit ? 0.02f : 1f), InCockpit ? 0.2f : 10f);

        [BurstCompile]
        public void Update(float scrollInput, float tiltInput, float delta, float3 acceleration)
        {
            var scroll = scrollInput * ScrollSpeed * InvertScroll * delta;
            var tilt = PointerTiltAngle - tiltInput * TiltSpeed;
            UpdateHeight(scroll, delta);
            UpdateTilt(tilt);
            UpdateAcceleration(acceleration, delta);
            var camera = _camera.ValueRO;
            _transform.ValueRW.Position = camera.OriginPosition + Acceleration;
        }

        [BurstCompile]
        private void UpdateHeight(float scroll, float delta)
        {
            if (scroll != 0f)
                NeededHeight = math.clamp(NeededHeight + scroll, 0f, 1f);
            if (Mathf.Approximately(Height, NeededHeight)) return;
            Height = math.lerp(Height, NeededHeight, ScrollSmooth * delta);
            UpdateDistance(-CurrentDistance);
        }
        
        [BurstCompile]
        private void UpdateTilt(float angle)
        {
            TiltAngle = math.clamp(angle, TiltUpLimit, TiltDownLimit);
            PointerTiltAngle = math.clamp(angle, PointerTiltUpLimit, TiltDownLimit);
            _transform.ValueRW.Rotation = quaternion.RotateX(math.radians(TiltAngle));
            var tiltDist = CurrentHeight / math.tan(math.radians(TiltAngle));
            var dist = math.clamp(tiltDist - MinDistance, 0f, CurrentDistance);
            UpdateDistance(-dist);
        }

        [BurstCompile]
        private void UpdateAcceleration(float3 acceleration, float dt)
        {
            a = math.lerp(a, -acceleration, 3f * dt);
        }

        [BurstCompile]
        private void UpdateDistance(float dist)
        {
            _camera.ValueRW.OriginPosition = InCockpit ? float3.zero : new float3(0f, CurrentHeight, dist);
        }
    }
}