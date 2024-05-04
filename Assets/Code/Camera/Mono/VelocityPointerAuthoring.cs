using Unity.Entities;
using UnityEngine;

namespace Code.Camera
{
    public class VelocityPointerAuthoring : MonoBehaviour
    {
        private class VelocityPointerAuthoringBaker : Baker<VelocityPointerAuthoring>
        {
            public override void Bake(VelocityPointerAuthoring authoring)
            {
                AddComponent<VelocityPointer>(GetEntity(TransformUsageFlags.None));
            }
        }
    }
}