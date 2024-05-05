using Unity.Entities;
using UnityEditor.Search;
using UnityEngine;

namespace Code.Camera
{
    public class CameraTargetAuthoring : MonoBehaviour
    {
        private class CameraTargetAuthoringBaker : Baker<CameraTargetAuthoring>
        {
            public override void Bake(CameraTargetAuthoring authoring)
            {
                AddComponent<CameraTargetTag>(GetEntity(TransformUsageFlags.Dynamic));
            }
        }
    }
}