using Unity.Entities;
using UnityEngine;

namespace Code.FloatingOrigin
{
    public class FloatingOriginBodyAuthoring : MonoBehaviour
    {
        private class FloatingOriginBodyBaker : Baker<FloatingOriginBodyAuthoring>
        {
            public override void Bake(FloatingOriginBodyAuthoring authoring)
            {
                AddComponent<FloatingOriginBody>(GetEntity(TransformUsageFlags.Dynamic));
            }
        }
    }
}