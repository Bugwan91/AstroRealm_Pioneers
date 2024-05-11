using Code.Player;
using Unity.Entities;
using UnityEngine;
using UnityEngine.VFX;

namespace Code.Weapon
{
    public class WeaponAuthoring: MonoBehaviour
    {
        public bool isControlledByPlayer = false;
        public GameObject projectilePrefab;
        public float projectileSize = 0.1f;
        public Transform muzzlePosition;
        public VisualEffect muzzleFlash;
        public float fireRate = 4f;
        public float maxDistance = 3000f;
        public float bulletSpeed = 1000f;
        public float damage = 10f;
    }

    public class WeaponBaker : Baker<WeaponAuthoring>
    {
        public override void Bake(WeaponAuthoring authoring)
        {
            var gunEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(gunEntity, new Weapon
            {
                ProjectilePrefab = GetEntity(authoring.projectilePrefab, TransformUsageFlags.Dynamic),
                ProjectileSize = authoring.projectileSize,
                MuzzlePosition = authoring.muzzlePosition.localPosition,
                ShootDelay = 1f / authoring.fireRate,
                MaxDistance = authoring.maxDistance,
                BulletSpeed = authoring.bulletSpeed,
                Damage = authoring.damage
            });
            if (authoring.isControlledByPlayer)
            {
                AddComponent<GunControlledByPlayerTag>(gunEntity);
            }
            AddComponent<FireInput>(gunEntity);
        }
    }
}