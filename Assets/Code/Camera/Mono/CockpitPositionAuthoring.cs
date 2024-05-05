using Unity.Entities;
using UnityEngine;

namespace Code.Camera
{
    public class CockpitPositionAuthoring : MonoBehaviour
    {
        private class CockpitPositionAuthoringBaker : Baker<CockpitPositionAuthoring>
        {
            public override void Bake(CockpitPositionAuthoring authoring)
            {
                AddComponent<CockpitPositionTag>(GetEntity(TransformUsageFlags.Dynamic));
            }
        }
    }
}