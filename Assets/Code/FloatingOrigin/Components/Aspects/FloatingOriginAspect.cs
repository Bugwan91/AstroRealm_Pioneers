using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine.SocialPlatforms;

namespace Code.FloatingOrigin
{
    [BurstCompile]
    public readonly partial struct FloatingOriginAspect : IAspect
    {
        private readonly RefRW<FloatingOriginBase> _FO;
        private readonly RefRO<LocalTransform> _transform;
        private readonly RefRO<PhysicsVelocity> _velocity;

        public float3 Velocity => _velocity.ValueRO.Linear;
        public float3 Position => _transform.ValueRO.Position;
        public float3 DeltaPosition => _FO.ValueRO.DeltaPosition;
        public float3 DeltaVelocity => _FO.ValueRO.DeltaVelocity;
        public float3 TotalPosition => Position + DeltaPosition;
        public float3 TotalVelocity => Velocity + DeltaVelocity;
        
        private float PositionLimit => _FO.ValueRO.PositionLimit;
        private float VelocityLimit => _FO.ValueRO.VelocityLimit;

        public bool Enabled
        {
            get => _FO.ValueRO.Enabled;
            set => _FO.ValueRW.Enabled = value;
        }
        
        public void Update(float3 targetPosition, float3 targetVelocity)
        {
            ResetDeltas();
            ResetPosition(targetPosition);
            ResetVelocity(targetVelocity);
        }

        private void ResetDeltas()
        {
            _FO.ValueRW.DeltaPosition = float3.zero;
            _FO.ValueRW.DeltaVelocity = float3.zero;
        }

        private void ResetPosition(float3 position)
        {
            if (!CheckPositionLimit(position)) return;
            _FO.ValueRW.DeltaPosition = -position;
        }
        
        private bool CheckPositionLimit(float3 position)
        {
            var overLimit = math.abs(position.x) > PositionLimit || math.abs(position.z) > PositionLimit || math.abs(position.y) > PositionLimit;
            _FO.ValueRW.UpdatePosition = overLimit;
            return overLimit;
        }
        
        private void ResetVelocity(float3 velocity)
        {
            if (!CheckVelocityLimit(velocity)) return;
            _FO.ValueRW.DeltaVelocity = -velocity;
        }

        private bool CheckVelocityLimit(float3 velocity)
        {
            var overLimit = math.abs(velocity.x) > VelocityLimit || math.abs(velocity.z) > VelocityLimit || math.abs(velocity.y) > VelocityLimit;
            _FO.ValueRW.UpdateVelocity = overLimit;
            return overLimit;
        }
    }
}