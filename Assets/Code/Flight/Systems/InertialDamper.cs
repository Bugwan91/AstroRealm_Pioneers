using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Code.FloatingOrigin;
using Unity.Physics;

namespace Code.Flight
{
    public struct InertialDamperInput : IComponentData
    {
        public float Value;
    }
    
    [UpdateInGroup(typeof(FlightSystemGroup))]
    [UpdateAfter(typeof(StrafeSystem))]
    public partial struct InertialDumperSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FloatingOriginBase>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new InertialDumperJob
            {
                Velocity = SystemAPI.GetComponent<PhysicsVelocity>(SystemAPI.GetSingletonEntity<FloatingOriginBase>()).Linear,
                DT = SystemAPI.Time.fixedDeltaTime
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct InertialDumperJob : IJobEntity
    {
        public float DT;
        public float3 Velocity;
        
        [BurstCompile]
        private void Execute(FlightAspect flight, in InertialDamperInput input)
        {
            if (input.Value != 0) flight.ApplyInertialDamper(input.Value, Velocity, DT);
        }
    }
}