using Unity.Entities;
using UnityEngine;

namespace Code.Flight
{
    public class AccelerationAuthoring : MonoBehaviour
    {
        private class AccelerationAuthoringBaker : Baker<AccelerationAuthoring>
        {
            public override void Bake(AccelerationAuthoring authoring)
            {
                AddComponent<Acceleration>(GetEntity(TransformUsageFlags.Dynamic));
            }
        }
    }
}