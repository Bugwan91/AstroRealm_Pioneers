using Unity.Entities;
using Unity.Physics;
using UnityEngine;

namespace Code.FloatingOrigin
{
    public class FloatingOriginBaseAuthoring : MonoBehaviour
    {
        public bool Enabled = true;
        public float positionLimit = 10000f;
        public float velocityLimit = 1000f;
        
        private class FloatingOriginAuthoringBaker : Baker<FloatingOriginBaseAuthoring>
        {
            public override void Bake(FloatingOriginBaseAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new FloatingOriginBase()
                {
                    Enabled = authoring.Enabled,
                    PositionLimit = authoring.positionLimit,
                    VelocityLimit = authoring.velocityLimit
                });
            }
        }
    }
}