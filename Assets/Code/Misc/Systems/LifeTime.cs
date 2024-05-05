using Unity.Burst;
using Unity.Entities;

namespace Code
{
    [UpdateInGroup(typeof(SyncSystemGroup))]
    public partial struct LifeTimeSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<LifeTime>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var dt = SystemAPI.Time.fixedDeltaTime;
            // TODO: Move this to separate parallel Job System if performance will be not satisfied
            foreach (var (lifeTimeData, lifeTimeState) in SystemAPI.Query<RefRW<LifeTime>, EnabledRefRW<LifeTime>>())
            {
                lifeTimeData.ValueRW.TimeLeft -= dt;
                if (lifeTimeData.ValueRO.TimeLeft < 0)
                {
                    lifeTimeState.ValueRW = false;
                }
            }
            state.EntityManager.DestroyEntity(SystemAPI.QueryBuilder()
                .WithDisabled<LifeTime>()
                // TODO: Remove `ToEntityArray(state.WorldUpdateAllocator)` after upgrade to Entity 1.2
                .Build().ToEntityArray(state.WorldUpdateAllocator));
        }
    }
}