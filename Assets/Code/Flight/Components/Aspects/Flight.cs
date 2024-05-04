using Code.Utils;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

namespace Code.Flight
{
    [BurstCompile]
    public readonly partial struct FlightAspect : IAspect
    {
        public readonly Entity Entity;
        
        private readonly RefRW<Flight> _flight;
        
        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRW<PhysicsVelocity> _velocity;
        private readonly RefRO<PhysicsMass> _mass;
        
        [BurstCompile]
        public void ApplyStrafe(float3 input, float dT)
        {
            var strafe = math.normalize(input) * _flight.ValueRO.StrafePower * dT;
            var force = math.rotate(_transform.ValueRO.Rotation, strafe);
            _velocity.ValueRW.ApplyLinearImpulse(_mass.ValueRO, force);
        }

        [BurstCompile]
        public void ApplyInertialDamper(float input, float3 velocityFO, float dT)
        {
            var dumperStep = _flight.ValueRO.InertialDamperPower * (1 / _mass.ValueRO.InverseMass) * dT;
            var absoluteV = _velocity.ValueRO.Linear - velocityFO;
            if (math.lengthsq(absoluteV) < dumperStep * dumperStep)
            {
                _velocity.ValueRW.Linear = velocityFO;
            } else
            {
                var stopForce = -input * _flight.ValueRO.InertialDamperPower * dT * math.normalize(absoluteV);
                _velocity.ValueRW.ApplyLinearImpulse(_mass.ValueRO, stopForce);
            }
        }
        
        [BurstCompile]
        public void ApplyYaw(float3 direction, float dt)
        {
            var forward = _transform.ValueRO.Forward();
            // Accelerated turn
            var d = MathUtil.AngleY(forward, direction);
            var v = _velocity.ValueRW.Angular.y;
            if (float.IsNaN(d) || (math.abs(d) < 0.01f && math.abs(v) < 0.01f))
            {
                _velocity.ValueRW.Angular = float3.zero;
                return;
            }
            var a = _flight.ValueRO.MaxYaw;
            var vLimit = 100f * a;
            var resV = math.clamp(v, -vLimit, vLimit);
            var ad = a * dt;
            var vt = 0.5f * (math.sqrt(ad * (ad + 8f * math.abs(d))) - ad) * math.sign(d);
            var dv = vt - v * dt;
            resV += math.min(ad, math.abs(dv)) * math.sign(dv) / dt;
            _velocity.ValueRW.Angular = new float3(0f, resV, 0f);
            
            // Constant speed turn
            // var cross = math.cross(forward, direction);
            // if (cross.y == 0f) return float3.zero;
            // var yaw = math.acos(forward.x * direction.x + forward.z * direction.z);
            // if (float.IsNaN(yaw) || yaw == 0f) return float3.zero;
            // var yawMax = _flight.ValueRO.MaxYaw;
            // return (yawMax * dt < yaw ? yawMax : yaw / dt) * math.normalize(cross);
        }
    }
}