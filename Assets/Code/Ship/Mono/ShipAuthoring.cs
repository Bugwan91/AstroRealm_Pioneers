using Code.Camera;
using Code.Flight;
using Unity.Entities;
using UnityEngine;
using Code.Player;
using Code.Gun;
using Unity.Transforms;

namespace Code.Ship
{
    public class ShipAuthoring : MonoBehaviour
    {
        public float maxSpeed = 2000;
        public float strafe = 50;
        public float inertialDamper = 200;
        public float maxYaw = 45f;
        public GameObject gun;
    }

    public class ShipBaker : Baker<ShipAuthoring>
    {
        public override void Bake(ShipAuthoring authoring)
        {
            var shipEntity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(shipEntity, new Ship{
                Gun = GetEntity(authoring.gun, TransformUsageFlags.Dynamic) // TODO: Update to Prefab and attach weapons in runtime
            });
            
            AddComponent(shipEntity, new Flight.Flight
            {
                MaxSpeed = authoring.maxSpeed,
                StrafePower = authoring.strafe,
                MaxYaw = Mathf.Deg2Rad * authoring.maxYaw,
                InertialDamperPower = authoring.inertialDamper
            });
            AddComponent<StrafeInput>(shipEntity);
            AddComponent<DirectionInput>(shipEntity);
            AddComponent<InertialDamperInput>(shipEntity);
        }
    }
}