using Code.Damage;
using Code.FloatingOrigin;
using Unity.Entities;
using UnityEngine;

namespace Code.Weapon
{
    public class ProjectileAuthoring : MonoBehaviour
    {
        public GameObject hitEffect;
        public float ricochetImpulse = 1f;
    }

    public class ProjectileBaker : Baker<ProjectileAuthoring>
    {
        public override void Bake(ProjectileAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<FloatingOriginBodyTag>(entity);
            AddComponent(entity, new Projectile
            {
                HitEffectPrefab = GetEntity(authoring.hitEffect, TransformUsageFlags.Dynamic),
                RicochetImpulse = authoring.ricochetImpulse 
            });
            AddComponent(entity, new DamageDealer
            {
                Value = 10f,
            });
            AddComponent(entity, new Destructable
            {
                ByTimer = true,
                TimeLeft = 1f,
            });
        }
    }
}