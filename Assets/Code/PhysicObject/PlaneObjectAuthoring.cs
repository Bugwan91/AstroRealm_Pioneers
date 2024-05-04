using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.PhysicObject
{
    public class PlaneObjectAuthoring : MonoBehaviour
    {
        public bool noExtraRotation = false;
        private class PlaneObjectBaker : Baker<PlaneObjectAuthoring>
        {
            public override void Bake(PlaneObjectAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<ResetPosition>(entity);
                AddComponent<ResetLinearVelocity>(entity);
                if (authoring.noExtraRotation)
                {
                    AddComponent<ResetRotation>(entity);
                    AddComponent<ResetAngularVelocity>(entity);
                }
            }
        }
    }
}