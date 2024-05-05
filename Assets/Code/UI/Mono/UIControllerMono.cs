using Code.FloatingOrigin;
using Code.Player;
using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace Code.UI
{
    public class UIControllerMono : MonoBehaviour
    {
        public TMP_Text speedLabel;
        
        private EntityManager _entityManager;
        private Entity _shipEntity;
        private Entity _foEntity;
        
        private void OnEnable()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }
        
        private void LateUpdate()
        {
            speedLabel.text = GetShipSpeed().ToString("F0") + " m/s";
        }
        
        private float GetShipSpeed()
        {
            return math.length(_entityManager.GetComponentData<PhysicsVelocity>(GetShipEntity()).Linear -
                               (float3)_entityManager.GetComponentData<PhysicsVelocity>(GetFOEntity()).Linear);
        }
        private Entity GetShipEntity()
        {
            if (_shipEntity == Entity.Null) {
                _shipEntity = _entityManager.CreateEntityQuery(typeof(ShipControlledByPlayerTag)).GetSingletonEntity();
            }
            return _shipEntity;
        }

        private Entity GetFOEntity()
        {
            if (_foEntity == Entity.Null)
            {
                _foEntity = _entityManager.CreateEntityQuery(typeof(FloatingOriginBase)).GetSingletonEntity();
            }
            return _foEntity;
        }

        private void OnDisable()
        {
            _shipEntity = Entity.Null;
            _foEntity = Entity.Null;
        }
    }
}