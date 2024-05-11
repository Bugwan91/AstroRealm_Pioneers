using Unity.Entities;
using Unity.Mathematics;

namespace Code.Weapon
{
    public struct Weapon: IComponentData
    {
        public Entity ProjectilePrefab;
        public float ProjectileSize;
        public float3 MuzzlePosition; // TODO: set Y to 0 for keeping projectiles in main plane
        public float MaxDistance;
        public float BulletSpeed;
        public float ShootDelay;
        public float Damage;
        // public VisualEffect MuzzleFlash;
        
        public double LastShootTime;
        public float3 Velocity;
    }

    public struct GunIsFiring : IComponentData {}

    public struct WeaponSlot : IComponentData
    {
        public Entity Gun;
    }
}