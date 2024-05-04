using Unity.Burst;
using Unity.Entities;

namespace Code.Gun
{
    [BurstCompile]
    [UpdateInGroup(typeof(SyncSystemGroup))]
    public partial struct ProjectileLiquidationSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndInitializationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            new ProjectileLiquidationJob
            {
                DT = SystemAPI.Time.fixedDeltaTime,
                ECB = ecb
            }.Schedule();
        }
    }

    [BurstCompile]
    public partial struct ProjectileLiquidationJob : IJobEntity
    {
        public float DT;
        public EntityCommandBuffer.ParallelWriter ECB;
        
        [BurstCompile]
        private void Execute([EntityIndexInQuery] int index, ref Bullet bullet, in Entity entity)
        {
            bullet.TimeLeft -= DT;
            if (bullet.TimeLeft < 0f)
            {
                ECB.DestroyEntity(index, entity);
            }
        }
    }
}