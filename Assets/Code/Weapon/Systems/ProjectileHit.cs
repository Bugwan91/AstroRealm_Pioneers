using Code.Damage;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

namespace Code.Weapon
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct ProjectileHitSystem : ISystem
    {
        private ComponentLookup<Destructable> _destructableLookup;
        private ComponentLookup<DamageDealer> _damageDealerLookup;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SimulationSingleton>();
            _destructableLookup = SystemAPI.GetComponentLookup<Destructable>(false);
            _damageDealerLookup = SystemAPI.GetComponentLookup<DamageDealer>(true);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _destructableLookup.Update(ref state);
            _damageDealerLookup.Update(ref state);

            var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
            
            state.Dependency = new ProjectileHitJob
            {
                Projectiles = _damageDealerLookup,
                Destructables = _destructableLookup
            }.Schedule(simulation, state.Dependency);
        }
    }

    [BurstCompile]
    public struct ProjectileHitJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<DamageDealer> Projectiles;
        public ComponentLookup<Destructable> Destructables;
            
        public void Execute(TriggerEvent triggerEvent)
        {
            var projectile = Entity.Null;
            var target = Entity.Null;
    
            if (Projectiles.HasComponent(triggerEvent.EntityA))
                projectile = triggerEvent.EntityA;
            if (Projectiles.HasComponent(triggerEvent.EntityB))
                projectile = triggerEvent.EntityB;
            if (Destructables.HasComponent(triggerEvent.EntityA))
                target = triggerEvent.EntityA;
            if (Destructables.HasComponent(triggerEvent.EntityB))
                target = triggerEvent.EntityB;
            if (Entity.Null.Equals(projectile) || Entity.Null.Equals(target)) return;
            
            Destructables.SetComponentEnabled(projectile, false);
        }
    }
}