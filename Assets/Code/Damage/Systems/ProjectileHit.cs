using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
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

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SimulationSingleton>();
            _destructableLookup = SystemAPI.GetComponentLookup<Destructable>(false);
            _damageDealerLookup = SystemAPI.GetComponentLookup<DamageDealer>(true);
            _takeDamageLookup = SystemAPI.GetComponentLookup<TakingDamage>(false);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _destructableLookup.Update(ref state);
            _damageDealerLookup.Update(ref state);
            _takeDamageLookup.Update(ref state);

            var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
            
            state.Dependency = new ProjectileHitJob
            {
                Damage = _damageDealerLookup,
                Destructables = _destructableLookup,
                TakeDamage = _takeDamageLookup
            }.Schedule(simulation, state.Dependency);
        }
    }

    [BurstCompile]
    public struct ProjectileHitJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<DamageDealer> Damage;
        public ComponentLookup<Destructable> Destructables;
        public ComponentLookup<TakingDamage> TakeDamage;
            
        public void Execute(TriggerEvent triggerEvent)
        {
            var projectile = Entity.Null;
            var target = Entity.Null;

            if (Damage.HasComponent(triggerEvent.EntityA)) {
                projectile = triggerEvent.EntityA;
                if (Destructables.HasComponent(triggerEvent.EntityB))
                    target = triggerEvent.EntityB;
            } else if (Damage.HasComponent(triggerEvent.EntityB)) {
                projectile = triggerEvent.EntityB;
                if (Destructables.HasComponent(triggerEvent.EntityA))
                    target = triggerEvent.EntityA;
            }
            
            if (Entity.Null.Equals(projectile) || Entity.Null.Equals(target)) return;
            TakeDamage.GetRefRW(target).ValueRW.Value = Damage.GetRefRO(projectile).ValueRO.Value;
            Destructables.SetComponentEnabled(projectile, false);
        }
    }
}