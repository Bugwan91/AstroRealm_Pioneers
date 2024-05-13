using Code.Damage;
using Code.Misc.Mono;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace Code.Weapon
{
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(SyncSystemGroup))]
    public partial class ShootProjectilesSystem : SystemBase
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FireInput>();
        }

        protected override void OnUpdate()
        {
            var fireInput = SystemAPI.GetSingleton<FireInput>().Value;
            if (!fireInput) return;

            var time = SystemAPI.Time.ElapsedTime;
            foreach (var (weapon, parent) in SystemAPI.Query<WeaponAspect, RefRO<Parent>>())
            {
                if (!weapon.CanShoot(time)) continue;
                weapon.LastShootTime = time;
                var projectile = EntityManager.Instantiate(weapon.ProjectilePrefab);
                EntityManager.SetComponentData(projectile, new DamageDealer
                {
                    Value = weapon.Damage
                });
                EntityManager.SetComponentData(projectile, new Destructable
                {
                    ByTimer = true,
                    TimeLeft = weapon.BulletLifeTime
                });
                EntityManager.SetComponentData(projectile, weapon.ProjectilePosition);
                var parentV = EntityManager.GetComponentData<PhysicsVelocity>(parent.ValueRO.Value).Linear;
                var v = PhysicsVelocity.Zero;
                v.Linear = weapon.Direction * weapon.BulletSpeed + parentV;
                EntityManager.SetComponentData(projectile, v);
                
                Entities.WithoutBurst().WithAll<VFXTag>().ForEach((UnityEngine.VFX.VisualEffect vfx ) =>
                {
                    vfx.Play();
                }).Run();
            }
        }
    }
}