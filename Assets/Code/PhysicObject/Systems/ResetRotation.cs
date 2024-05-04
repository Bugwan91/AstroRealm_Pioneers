using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Code.PhysicObject
{
    [UpdateInGroup(typeof(ResetToPlaneSystemGroup))]
    public partial struct ResetRotationSystem : ISystem
    {

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ResetRotation>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new ResetRotationJob().ScheduleParallel();
        }
    }

    [BurstCompile]
    [WithAll(typeof(ResetRotation))]
    public partial struct ResetRotationJob : IJobEntity
    {
        [BurstCompile]
        private void Execute(ref LocalTransform transform)
        {
            var look = transform.Forward();
            look.y = 0f;
            transform.Rotation = quaternion.LookRotationSafe(look, math.up());
        }
    }
}