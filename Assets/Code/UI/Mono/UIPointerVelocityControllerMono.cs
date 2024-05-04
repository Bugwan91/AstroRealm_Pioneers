using UnityEngine;
using Unity.Entities;
using Code.Camera;
using UnityEngine.UI;

namespace Code.UI
{
    public class UIPointerVelocityControllerMono : MonoBehaviour
    {
        private EntityManager _entityManager;
        private Entity _vEntity;
        private Vector3 _vF;
        private Vector3 _vB;

        public new UnityEngine.Camera camera;
        public Image forward;
        public Image backward;
        
        private void OnEnable()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }
        
        private void LateUpdate()
        {
           UpdatePointerPosition();
           var f = camera.WorldToScreenPoint(_vF);
           forward.enabled = f.z > 0f;
           f.z = 1f;
           forward.rectTransform.position = f;

           var b = camera.WorldToScreenPoint(_vB);
           backward.enabled = b.z > 0f;
           b.z = 1f;
           backward.rectTransform.position = b;
        }

        private void UpdatePointerPosition()
        {
            var vPointer = GetPointerEntity();
            if (vPointer == Entity.Null) return;
            var component = _entityManager.GetComponentData<VelocityPointer>(vPointer); 
            _vF = component.ForwardPosition;
            _vB = component.BackwardPosition;
        }

        private Entity GetPointerEntity()
        {
            if (_vEntity == Entity.Null)
            {
                _vEntity = _entityManager
                    .CreateEntityQuery(typeof(VelocityPointer))
                    .GetSingletonEntity();
            }
            return _vEntity;
        }
        
        private void OnDisable()
        {
            _vEntity = Entity.Null;
        }
    }
}