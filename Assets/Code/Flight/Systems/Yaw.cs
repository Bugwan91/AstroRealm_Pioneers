using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Code.Flight
{
    public struct DirectionInput : IComponentData
    {
        public float3 Value;
    }
    
    [UpdateInGroup(typeof(FlightSystemGroup))]
    public partial struct YawSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new YawJob
            {
                DT = SystemAPI.Time.fixedDeltaTime
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct YawJob : IJobEntity
    {
        public float DT;
        
        [BurstCompile]
        private void Execute(FlightAspect flight, in DirectionInput input)
        {
            flight.ApplyYaw(input.Value, DT);
        }
    }
}