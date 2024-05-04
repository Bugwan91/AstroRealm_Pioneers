using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace Code.PhysicObject
{
    [UpdateInGroup(typeof(ResetToPlaneSystemGroup))]
    public partial struct ResetAngularVelocitySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ResetAngularVelocity>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new ResetAngularVelocityJob().ScheduleParallel();
        }
    }

    [BurstCompile]
    [WithAll(typeof(ResetAngularVelocity))]
    public partial struct ResetAngularVelocityJob : IJobEntity
    {
        [BurstCompile]
        private void Execute(ref PhysicsVelocity velocity)
        {
            velocity.Angular.x = 0f;
            velocity.Angular.z = 0f;
        }
    }
}