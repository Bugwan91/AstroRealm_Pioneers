using Unity.Entities;
using Unity.Mathematics;

namespace Code.Weapon
{
    public struct Projectile : IComponentData
    {
        public Entity HitEffectPrefab;
        public float3 CameraRelativeVelocity;
        public float RicochetImpulse;
    }
}