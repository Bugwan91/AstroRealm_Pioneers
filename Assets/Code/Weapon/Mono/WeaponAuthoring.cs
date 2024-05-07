using Code.Player;
using Unity.Entities;
using UnityEngine;

namespace Code.Weapon
{
    public class WeaponAuthoring: MonoBehaviour
    {
        public bool isControlledByPlayer = false;
        public GameObject bulletPrefab;
        public Transform muzzlePosition;
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
                ProjectilePrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
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