using Unity.Burst;
using Unity.Entities;

namespace Code.Damage
{
    [UpdateInGroup(typeof(DamageSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(ApplyDamage))]
    public partial struct DestructSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Destructable>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var dt = SystemAPI.Time.fixedDeltaTime;
            // TODO: Move this to separate parallel Job System if performance will be not satisfied
            foreach (var (destructable, status)
                     in SystemAPI.Query<RefRW<Destructable>, EnabledRefRW<Destructable>>())
            {
                if (destructable.ValueRO.ByTimer) {
                    if (destructable.ValueRO.TimeLeft <= 0)
                        status.ValueRW = false;
                    else
                        destructable.ValueRW.TimeLeft -= dt;
                } else {
                    if (destructable.ValueRO.Health <= 0)
                        status.ValueRW = false;
                }
            }
            state.EntityManager.DestroyEntity(SystemAPI.QueryBuilder()
                .WithDisabled<Destructable>()
                // TODO: Remove `ToEntityArray(state.WorldUpdateAllocator)` after upgrade to Entity 1.2
                .Build().ToEntityArray(state.WorldUpdateAllocator));
        }
    }
}