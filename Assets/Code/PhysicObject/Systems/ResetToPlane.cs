using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Code.PhysicObject
{
    [UpdateInGroup(typeof(ResetToPlaneSystemGroup))]
    public partial struct ResetToPlaneSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ResetToPlane>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new ResetToPlaneJob().ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct ResetToPlaneJob : IJobEntity
    {
        [BurstCompile]
        private void Execute(ref LocalTransform transform, ref PhysicsVelocity velocity, in ResetToPlane reset)
        {
            transform.Position.y = 0f;
            velocity.Linear.y = 0f;
            if (reset.AndRotation)
            {
                var look = transform.Forward();
                look.y = 0f;
                transform.Rotation = quaternion.LookRotationSafe(look, math.up());
                velocity.Angular.x = 0f;
                velocity.Angular.z = 0f;
            }
        }
    }
}