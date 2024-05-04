using Unity.Entities;
using UnityEngine;
using Code.FloatingOrigin;
using Unity.Transforms;

namespace Code.FarWorld
{
    public class FarWorldControllerMono : MonoBehaviour
    {
        [SerializeField] private float rate = 0.001f;
        
        [SerializeField] private Transform farCamera;

        [SerializeField] private Transform sun; // TODO: should be procedurally set to closest star
        [SerializeField] private Light mainLight;

        // TODO: should be procedurally set to closest planet
        // Should be possible to have several secondary lights
        [SerializeField] private Transform planet;
        [SerializeField] private Light planetLight;

        private EntityManager _entityManager;
        private Entity _floatingOrigin;
        
        private Vector3 _foPosition;
        
        private void OnEnable()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }
        
        private void LateUpdate()
        {
            UpdateLight();
        }

        public void UpdateCameraPosition(Transform mainCamera)
        {
            UpdateFOPosition();
            farCamera.localPosition = mainCamera.position * rate - _foPosition;
            farCamera.rotation = mainCamera.rotation;
        }

        private void UpdateLight()
        {
            var observerPosition = farCamera.position;
            // TODO: update light shape and intense
            // TODO: add albedo to planet
            // TODO: implement logic for multiple objects
            mainLight.transform.rotation = Quaternion.LookRotation(observerPosition - sun.position);
            planetLight.transform.rotation = Quaternion.LookRotation(observerPosition - planet.position);
        }
        
        private void UpdateFOPosition()
        {
            var foEntity = GetFOEntity();
            if (foEntity == Entity.Null) return;
            _foPosition = _entityManager.GetAspect<FloatingOriginAspect>(foEntity).Position * rate;
        }
        
        private Entity GetFOEntity()
        {
            if (_floatingOrigin == Entity.Null)
            {
                _floatingOrigin = _entityManager
                    .CreateEntityQuery(typeof(FloatingOriginBase))
                    .GetSingletonEntity();
            }
            return _floatingOrigin;
        }
        
        private void OnDisable()
        {
            _floatingOrigin = Entity.Null;
        }
    }
}