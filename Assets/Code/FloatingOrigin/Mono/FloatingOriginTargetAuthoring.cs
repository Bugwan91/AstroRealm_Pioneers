using Unity.Entities;
using UnityEngine;

namespace Code.FloatingOrigin
{
    public class FloatingOriginTargetAuthoring : MonoBehaviour
    {
        private class FloatingOriginTargetAuthoringBaker : Baker<FloatingOriginTargetAuthoring>
        {
            public override void Bake(FloatingOriginTargetAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<FloatingOriginTarget>(entity);
            }
        }
    }
}