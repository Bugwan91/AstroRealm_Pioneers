using Code.Player;
using Unity.Entities;
using UnityEngine;

namespace Code.Gun
{
    public class GunAuthoring: MonoBehaviour
    {
        public bool isControlledByPlayer = false;
        public GameObject bulletPrefab;
        public Transform muzzlePosition;
        public float fireRate = 4f;
        public float maxDistance = 3000f;
        public float bulletSpeed = 1000f;
    }

    public class GunBaker : Baker<GunAuthoring>
    {
        public override void Bake(GunAuthoring authoring)
        {
            var gunEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(gunEntity, new Gun
            {
                BulletPrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
                MuzzlePosition = authoring.muzzlePosition.localPosition,
                ShootDelay = 1f / authoring.fireRate,
                MaxDistance = authoring.maxDistance,
                BulletSpeed = authoring.bulletSpeed,
            });
            if (authoring.isControlledByPlayer)
            {
                AddComponent<GunControlledByPlayerTag>(gunEntity);
            }
            AddComponent<FireInput>(gunEntity);
        }
    }
}