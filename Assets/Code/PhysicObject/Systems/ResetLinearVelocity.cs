using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

namespace Code.PhysicObject
{
    [UpdateInGroup(typeof(ResetToPlaneSystemGroup))]
    public partial struct ResetLinearVelocitySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ResetLinearVelocity>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new ResetLinearVelocityJob().ScheduleParallel();
        }
    }

    [BurstCompile]
    [WithAll(typeof(ResetLinearVelocity))]
    public partial struct ResetLinearVelocityJob : IJobEntity
    {
        [BurstCompile]
        private void Execute(ref PhysicsVelocity velocity)
        {
            velocity.Linear.y = 0f;
        }
    }
}