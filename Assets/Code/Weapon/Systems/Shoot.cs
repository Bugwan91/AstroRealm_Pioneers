using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

namespace Code.Weapon
{
    [BurstCompile]
    [UpdateInGroup(typeof(SyncSystemGroup))]
    public partial struct ShootSystem : ISystem
    {
        private ComponentLookup<PhysicsVelocity> _velocityLookup;
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndInitializationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<FireInput>();
            _velocityLookup = SystemAPI.GetComponentLookup<PhysicsVelocity>(true);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var fireInput = SystemAPI.GetSingleton<FireInput>().Value;
            if (!fireInput) return;
            var ecb = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            _velocityLookup.Update(ref state);
            new ShootJob
            {
                Time = SystemAPI.Time.ElapsedTime,
                ECB = ecb,
                VelocityLookup = _velocityLookup
            }.ScheduleParallel();
        }
        
        [BurstCompile]
        public partial struct ShootJob : IJobEntity
        {
            public double Time;
            public EntityCommandBuffer.ParallelWriter ECB;
            [Unity.Collections.ReadOnly]
            public ComponentLookup<PhysicsVelocity> VelocityLookup;
        
            [BurstCompile]
            private void Execute([EntityIndexInQuery] int index, GunAspect gun, Parent parent)
            {
                VelocityLookup.TryGetComponent(parent.Value, out var velocity);
                gun.Shoot(velocity.Linear, Time, ECB, index);
            }
        }
    }
}