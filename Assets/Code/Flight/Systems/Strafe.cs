using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Code.Flight
{
    public struct StrafeInput : IComponentData
    {
        public float3 Value;
    }
    
    [UpdateInGroup(typeof(FlightSystemGroup))]
    public partial struct StrafeSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new StrafeJob
            {
                DT = SystemAPI.Time.fixedDeltaTime
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct StrafeJob : IJobEntity
    {
        public float DT;
    
        [BurstCompile]
        private void Execute(FlightAspect flight, in StrafeInput input)
        {
            if (input.Value is { x: 0f, z: 0f }) return;
            flight.ApplyStrafe(input.Value, DT);
        }
    }
}