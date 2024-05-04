using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Code.PhysicObject
{
    [UpdateInGroup(typeof(ResetToPlaneSystemGroup))]
    public partial struct ResetPositionSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ResetPosition>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new ResetPositionJob().ScheduleParallel();
        }
    }

    [BurstCompile]
    [WithAll(typeof(ResetPosition))]
    public partial struct ResetPositionJob : IJobEntity
    {
        [BurstCompile]
        private void Execute(ref LocalTransform transform)
        {
            transform.Position.y = 0f;
        }
    }
}