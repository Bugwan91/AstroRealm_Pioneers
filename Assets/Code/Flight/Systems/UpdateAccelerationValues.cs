using Code.FloatingOrigin;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Code.Flight
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct UpdateAccelerationValuesSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FloatingOriginBase>();
            state.RequireForUpdate<FloatingOriginBody>();
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
    public partial struct UpdateAccelerationValueJob : IJobEntity
    {
        public float3 Shift;
        
        [BurstCompile]
        private void Execute(ref Acceleration acceleration,
            in PhysicsVelocity velocity,
            in FloatingOriginBody floatingOriginBody)
        {
            var pastV = acceleration.PreviousVelocity + Shift;
            acceleration.Value = velocity.Linear - pastV;
            acceleration.PreviousVelocity = velocity.Linear;
        }
    }
}