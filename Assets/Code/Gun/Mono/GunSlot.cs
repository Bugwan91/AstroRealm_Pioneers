using Unity.Entities;
using UnityEngine;

namespace Code.Gun
{
    public class GunSlotAuthoring : MonoBehaviour
    {
        public GameObject gun;
    }

    public class GunSlotBaker : Baker<GunSlotAuthoring>
    {
        public override void Bake(GunSlotAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<GunSlot>(entity);
        }
    }
}