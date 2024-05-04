using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using TMPro;
using Code.Camera;
using Code.Player;
using Code.FloatingOrigin;

namespace Code.UI
{
    public class UIPointerControllerMono : MonoBehaviour
    {
        public TMP_Text distanceLabel;
        public TMP_Text coordinatesZLabel;
        public TMP_Text coordinatesXLabel;
        
        private EntityManager _entityManager;
        private Entity _pointerEntity;
        private Entity _shipEntity;
        private Entity _floatingOrigin;
        
        private Vector3 _pointerPosition;
        private Vector3 _shipPosition;
        private Vector3 _foPosition;

        public UnityEngine.Camera camera;
        
        private void OnEnable()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
        }
        
        private void LateUpdate()
        {
           UpdatePointerPosition();
           UpdateShipPosition();
           UpdateFOPosition();
           var distance = (_pointerPosition - _shipPosition).magnitude;
           var pos = camera.WorldToScreenPoint(_pointerPosition);
           pos.z = 0f;
           transform.position = pos;
           distanceLabel.text = distance.ToString("F0");
           coordinatesXLabel.text = (_pointerPosition.x - _foPosition.x).ToString("F0");
           coordinatesZLabel.text = (_pointerPosition.z - _foPosition.z).ToString("F0");
        }

        private void UpdatePointerPosition()
        {
            var pointer = GetPointerEntity();
            if (pointer == Entity.Null) return;
            _pointerPosition = _entityManager.GetComponentData<LocalTransform>(pointer).Position;
        }
        
        private void UpdateShipPosition()
        {
            var ship = GetShipEntity();
            if (ship == Entity.Null) return;
            _shipPosition = _entityManager.GetComponentData<LocalTransform>(ship).Position;
        }

        private void UpdateFOPosition()
        {
            var foEntity = GetFOEntity();
            if (foEntity == Entity.Null) return;
            _foPosition = _entityManager.GetComponentData<LocalTransform>(foEntity).Position;
        }

        private Entity GetPointerEntity()
        {
            if (_pointerEntity == Entity.Null)
            {
                _pointerEntity = _entityManager
                    .CreateEntityQuery(typeof(Pointer))
                    .GetSingletonEntity();
            }
            return _pointerEntity;
        }

        private Entity GetShipEntity()
        {
            if (_shipEntity == Entity.Null)
            {
                _shipEntity = _entityManager
                    .CreateEntityQuery(typeof(ShipControlledByPlayer))
                    .GetSingletonEntity();
            }
            return _shipEntity;
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
            _pointerEntity = Entity.Null;
            _shipEntity = Entity.Null;
            _floatingOrigin = Entity.Null;
        }
    }
}