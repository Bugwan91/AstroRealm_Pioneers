using Unity.Entities;
using UnityEngine;

namespace Code.Gun
{
    public class BulletAuthoring: MonoBehaviour {}

    public class BulletBaker : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            var bulletEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Bullet>(bulletEntity);
        }
    }
}