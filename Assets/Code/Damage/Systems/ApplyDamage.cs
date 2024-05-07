using Unity.Burst;
using Unity.Entities;

namespace Code.Damage
{
    [UpdateInGroup(typeof(SyncSystemGroup), OrderFirst = true)]
    [UpdateBefore(typeof(DestructSystem))]
    public partial struct ApplyDamage : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new ApplyDamageJob().ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct ApplyDamageJob : IJobEntity
    {
        private void Execute(ref TakingDamage damage, ref Destructable health)
        {
            health.Health -= damage.Value;
            damage.Value = 0f;
        }
    }
}