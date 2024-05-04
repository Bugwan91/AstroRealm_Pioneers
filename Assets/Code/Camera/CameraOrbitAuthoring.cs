using Unity.Entities;
using UnityEngine;

namespace Code.Camera
{
    public class CameraOrbitAuthoring : MonoBehaviour
    {
        [Range(0.01f, 0.5f)] public float rotationSpeed = 0.1f;
    }

    public class CameraOrbitBaker : Baker<CameraOrbitAuthoring>
    {
        public override void Bake(CameraOrbitAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CameraOrbit
            {
                RotationAngle = 0f
            });
            AddComponent(entity, new CameraOrbitSettings
            {
                RotationSpeed = authoring.rotationSpeed
            });
        }
    }
}