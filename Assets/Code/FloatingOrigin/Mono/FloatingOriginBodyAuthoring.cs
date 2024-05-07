using Unity.Entities;
using UnityEngine;

namespace Code.FloatingOrigin
{
    [RequireComponent(typeof(Rigidbody))]
    public class FloatingOriginBodyAuthoring : MonoBehaviour
    {
        private class FloatingOriginBodyBaker : Baker<FloatingOriginBodyAuthoring>
        {
            public override void Bake(FloatingOriginBodyAuthoring authoring)
            {
                AddComponent<FloatingOriginBodyTag>(GetEntity(TransformUsageFlags.Dynamic));
            }
        }
    }
}