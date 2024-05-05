using Unity.Entities;
using UnityEngine;

namespace Code
{
    public class LifeTimeAuthoring : MonoBehaviour
    {
        private class LifeTimeAuthoringBaker : Baker<LifeTimeAuthoring>
        {
            public override void Bake(LifeTimeAuthoring authoring)
            {
                AddComponent<LifeTime>(GetEntity(TransformUsageFlags.Dynamic));
            }
        }
    }
}