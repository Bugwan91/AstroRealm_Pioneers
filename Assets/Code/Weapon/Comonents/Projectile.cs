using Unity.Entities;

namespace Code.Weapon
{
    public struct Projectile : IComponentData
    {
        public Entity HitEffectPrefab;
    }
}