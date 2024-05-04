using Unity.Entities;
using Unity.Mathematics;

namespace Code.Gun
{
    public struct Gun: IComponentData
    {
        public Entity BulletPrefab;
        public float3 MuzzlePosition; // TODO: set Y to 0 for keeping projectiles in main plane
        public float MaxDistance;
        public float BulletSpeed;
        public float ShootDelay;
        
        public double LastShootTime;
        public float3 Velocity;
    }

    public struct GunIsFiring : IComponentData {}

    public struct GunSlot : IComponentData
    {
        public Entity Gun;
    }
}