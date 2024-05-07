using Code.FloatingOrigin;
using Code.Weapon;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Code.Flight
{
    public struct Acceleration : IComponentData
    {
        public float3 PreviousVelocity;
        public float3 Value;
    }
    
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    [UpdateAfter(typeof(ProjectileHitSystem))]
    public partial struct UpdateAccelerationValuesSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FloatingOriginBase>();
            state.RequireForUpdate<FloatingOriginBodyTag>();
            state.RequireForUpdate<Acceleration>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new UpdateAccelerationValueJob
            {
                Shift = SystemAPI.GetSingleton<FloatingOriginBase>().DeltaVelocity
            }.ScheduleParallel();
        }
    }
    
    [BurstCompile]
    [WithAll(typeof(FloatingOriginBodyTag))]
    public partial struct UpdateAccelerationValueJob : IJobEntity
    {
        public float3 Shift;
        
        [BurstCompile]
        private void Execute(ref Acceleration acceleration,
            in PhysicsVelocity velocity)
        {
            var pastV = acceleration.PreviousVelocity + Shift;
            acceleration.Value = velocity.Linear - pastV;
            acceleration.PreviousVelocity = velocity.Linear;
        }
    }
}