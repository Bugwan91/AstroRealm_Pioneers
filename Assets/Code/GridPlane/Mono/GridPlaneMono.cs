using Code.Camera;
using Code.FloatingOrigin;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Code.MainController
{
    public class GridPlane : MonoBehaviour
    {
        public Transform cameraPosition;
        public float minRadius = 500f;
        public float maxRadius = 5000f;
        public float minIntensity = 0f;
        public float maxIntensity = 0.1f;
        public float heightOfMaxIntensity = 5000f;
        
        private Material _material;
        private static readonly int Offset = Shader.PropertyToID("_Offset");
        private static readonly int Intensity = Shader.PropertyToID("_Alpha");
        
        private EntityManager _entityManager;
        private Entity _floatingOrigin;

        private float _minScale;
        private float _maxScale;
        private float _scaleDelta;
        private float _intensityDelta;
        
        private void OnEnable()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _minScale = minRadius * 0.1f;
            _maxScale = maxRadius * 0.1f;
            _scaleDelta = _maxScale - _minScale;
            _intensityDelta = maxIntensity - minIntensity;
        }
        
        private void Start() {
            _material = Instantiate(GetComponent<Renderer>().sharedMaterial);
            GetComponent<Renderer>().material = _material;
        }
        
        private void LateUpdate()
        {
            // TODO Optimize
            var position = _entityManager.GetComponentData<LocalTransform>(_entityManager.CreateEntityQuery(typeof(CameraTargetTag)).GetSingletonEntity()).Position;
            position.y = -1f;
            transform.position = position;
            _material.SetVector(Offset, -(Vector3)_entityManager.GetAspect<FloatingOriginAspect>(GetFOEntity()).TotalPosition);

            var h = Mathf.Clamp(cameraPosition.position.y / heightOfMaxIntensity, 0f, 1f);
            var scale = h * _scaleDelta + _minScale;
            transform.localScale = new Vector3(scale, 1f, scale);
            
            _material.SetFloat(Intensity, h * _intensityDelta + minIntensity);
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