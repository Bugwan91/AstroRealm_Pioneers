using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace Code.Flight
{
    [UpdateInGroup(typeof(FlightSystemGroup), OrderFirst = true)]
    public partial struct ReduceToMaxVelocitySystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new ReduceToMaxVelocityJob().ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct ReduceToMaxVelocityJob : IJobEntity
    {
        [BurstCompile]
        private void Execute(in Flight flight, ref PhysicsVelocity velocity)
        {
            if (math.lengthsq(velocity.Linear) > flight.MaxSpeed * flight.MaxSpeed)
                velocity.Linear = math.normalize(velocity.Linear) * flight.MaxSpeed;
        }
    }
}