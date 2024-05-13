using Unity.Entities;
using UnityEngine;

namespace Code.Misc.Mono
{
    public struct VFXTag : IComponentData {}
    
    public class VFXTagAuthoring : MonoBehaviour
    {
        private class VFXTagAuthoringBaker : Baker<VFXTagAuthoring>
        {
            public override void Bake(VFXTagAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), default(VFXTag));
            }
        }
    }
}