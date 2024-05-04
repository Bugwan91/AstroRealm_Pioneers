using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Code.FarWorld;
using UnityEngine.Serialization;

namespace Code.Camera
{
    public class CameraControllerMono : MonoBehaviour
    {
        [FormerlySerializedAs("farWorldController")] public FarWorldControllerMono farWorldControllerMono;
        private Entity _cameraEntity;
        private World _world;
        private Transform _transform;

        private void OnEnable()
        {
            _world = World.DefaultGameObjectInjectionWorld;
            _transform = transform;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
        
        private void LateUpdate()
        {
            var cameraEntity = GetCameraEntity();
            if (cameraEntity == Entity.Null) return;
            
            var targetTransform = _world.EntityManager.GetComponentData<LocalToWorld>(cameraEntity);
            _transform.position = targetTransform.Position;
            _transform.rotation = targetTransform.Rotation;
            farWorldControllerMono.UpdateCameraPosition(_transform);
        }

        private Entity GetCameraEntity()
        {
            if (_cameraEntity == Entity.Null)
            {
                _cameraEntity = _world.
                    EntityManager.
                    CreateEntityQuery(typeof(Camera))
                    .GetSingletonEntity();
            }
            return _cameraEntity;
        }

        private void OnDisable()
        {
            _cameraEntity = Entity.Null;
        }
    }
}