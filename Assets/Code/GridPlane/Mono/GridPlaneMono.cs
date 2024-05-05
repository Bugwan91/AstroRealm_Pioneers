using Code.Camera;
using Code.FloatingOrigin;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Code.MainController
{
    public class GridPlane : MonoBehaviour
    {
        
        private Material _material;
        private static readonly int Offset = Shader.PropertyToID("_Offset");
        
        private EntityManager _entityManager;
        private Entity _floatingOrigin;
        
        private void OnEnable()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
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