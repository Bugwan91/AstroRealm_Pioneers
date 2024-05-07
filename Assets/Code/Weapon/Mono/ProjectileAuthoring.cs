using Unity.Entities;
using UnityEngine;

namespace Code.Weapon
{
    public class ProjectileAuthoring: MonoBehaviour {}

    public class ProjectileBaker : Baker<ProjectileAuthoring>
    {
        public override void Bake(ProjectileAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Projectile>(entity);
        }
    }
}