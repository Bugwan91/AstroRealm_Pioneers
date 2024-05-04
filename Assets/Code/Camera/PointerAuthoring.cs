using Code.Camera;
using Unity.Entities;
using UnityEngine;

namespace Code.Player
{
    public class PointerAuthoring : MonoBehaviour {}

    public class PointerBaker : Baker<PointerAuthoring>
    {
        public override void Bake(PointerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Pointer>(entity);
        }
    }
}