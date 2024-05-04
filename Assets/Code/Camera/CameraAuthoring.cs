using Unity.Entities;
using UnityEngine;

namespace Code.Camera
{
    public class CameraAuthoring : MonoBehaviour
    {
        [Range(0f, 10f)] public float minHeight = 5f;
        [Range(0f, 100f)] public float minDistance = 15f;
        [Range(1f, 10000f)] public float maxHeight = 100f;
        [Range(1f, 10000f)] public float maxDistance = 200f;
        [Range(0.01f, 5.0f)] public float scrollSpeed = 0.1f;
        [Range(0.001f, 100.0f)] public float scrollSmooth = 10.0f;
        public bool invertScroll = true;
        
        [Range(0.01f, 0.5f)] public float tiltSpeed = 0.05f;
        [Range(0f, 30f)] public float tiltUpLimit = 15f;
        [Range(0.01f, 0.99f)] public float toCockpitHeightThreshold = 0.1f;

        public float accelerationMultiplier = 1f;
        public float accelerationMultiplierCockpit = 1f;
        public float accelerationSmooth = 0.1f;
    }

    public class CameraBaker : Baker<CameraAuthoring>
    {
        public override void Bake(CameraAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Camera
            {
                Height = 0f,
                TiltAngle = 0.01f
            });
            AddComponent(entity, new CameraSettings
            {
                MinHeight = authoring.minHeight,
                MinDistance = authoring.minDistance,
                MaxHeight = authoring.maxHeight,
                MaxDistance = authoring.maxDistance,
                ScrollSpeed = authoring.scrollSpeed,
                ScrollSmooth = authoring.scrollSmooth,
                InvertScroll = authoring.invertScroll,
                TiltSpeed = authoring.tiltSpeed,
                TiltUpLimit = authoring.tiltUpLimit,
                ToCockpitHeightThreshold = authoring.toCockpitHeightThreshold,
                // AccelerationMultiplier = authoring.accelerationMultiplier,
                // AccelerationMultiplierCockpit =
                AccelerationSmooth = authoring.accelerationSmooth,
                // AccelerationLimit =
                // AccelerationLimitCockpit = 
                
            });
        }
    }
}