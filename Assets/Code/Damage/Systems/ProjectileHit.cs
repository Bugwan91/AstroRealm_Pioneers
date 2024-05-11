using Code.Weapon;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

namespace Code.Damage
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct ProjectileHitSystem : ISystem
    {
        private ComponentLookup<Destructable> _destructableLookup;
        private ComponentLookup<DamageDealer> _damageDealerLookup;
        private ComponentLookup<TakingDamage> _takeDamageLookup;
        private ComponentLookup<Projectile> _projectileLookup;
        private ComponentLookup<LocalTransform> _transformLookup;
        private ComponentLookup<PhysicsVelocity> _velocityLookup;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsWorldSingleton>();
            state.RequireForUpdate<SimulationSingleton>();
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            _destructableLookup = SystemAPI.GetComponentLookup<Destructable>(false);
            _damageDealerLookup = SystemAPI.GetComponentLookup<DamageDealer>(true);
            _takeDamageLookup = SystemAPI.GetComponentLookup<TakingDamage>(false);
            _projectileLookup = SystemAPI.GetComponentLookup<Projectile>(true);
            _transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
            _velocityLookup = SystemAPI.GetComponentLookup<PhysicsVelocity>(true);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _destructableLookup.Update(ref state);
            _damageDealerLookup.Update(ref state);
            _takeDamageLookup.Update(ref state);
            _projectileLookup.Update(ref state);
            _transformLookup.Update(ref state);
            _velocityLookup.Update(ref state);
            
            var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
            
            var ecbBOS = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var physWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld;
            state.Dependency = new ProjectileCollisionJob
            {
                Damage = _damageDealerLookup,
                Destructables = _destructableLookup,
                TakeDamage = _takeDamageLookup,
                Projectiles = _projectileLookup,
                Velocities = _velocityLookup,
                PhysWorld = physWorld,
                ECB = ecbBOS
            }.Schedule(simulation, state.Dependency);
        }
    }
    
    public partial struct ProjectileCollisionJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentLookup<DamageDealer> Damage;
        public ComponentLookup<Destructable> Destructables;
        public ComponentLookup<TakingDamage> TakeDamage;
        [ReadOnly] public ComponentLookup<Projectile> Projectiles;
        [ReadOnly] public ComponentLookup<PhysicsVelocity> Velocities;
        [ReadOnly] public PhysicsWorld PhysWorld;
        
        public EntityCommandBuffer ECB;
        public void Execute(CollisionEvent collisionEvent)
        {
            Debug.Log("CollisionEvent");
            var projectile = Entity.Null;
            var target = Entity.Null;

            if (Damage.HasComponent(collisionEvent.EntityA)) {
                projectile = collisionEvent.EntityA;
                if (TakeDamage.HasComponent(collisionEvent.EntityB))
                    target = collisionEvent.EntityB;
            } else if (Damage.HasComponent(collisionEvent.EntityB)) {
                projectile = collisionEvent.EntityB;
                if (TakeDamage.HasComponent(collisionEvent.EntityA))
                    target = collisionEvent.EntityA;
            }

            Debug.Log("Checking collision");
            if (Entity.Null.Equals(projectile) || Entity.Null.Equals(target))
            {
                Debug.Log("NoCollision");
                return;
            }
            TakeDamage.GetRefRW(target).ValueRW.Value = Damage.GetRefRO(projectile).ValueRO.Value;
            Destructables.SetComponentEnabled(projectile, false);

            var details = collisionEvent.CalculateDetails(ref PhysWorld);
            
            var hitVFX = ECB.Instantiate(Projectiles[projectile].HitEffectPrefab);
            var hitPosition = LocalTransform.FromPosition(details.AverageContactPointPosition);
            ECB.SetComponent(hitVFX, hitPosition);
            var velocity = PhysicsVelocity.Zero;
            velocity.Linear = Velocities[target].Linear;
            ECB.SetComponent(hitVFX, velocity);
        }
    }
}