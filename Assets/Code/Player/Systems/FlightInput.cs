using Code.Camera;
using Code.Flight;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Code.Player
{
    [BurstCompile]
    [UpdateInGroup(typeof(PlayerInputSystemGroup))]
    public partial struct FlightInputSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerInputData>();
            state.RequireForUpdate<ShipControlledByPlayerTag>();
            state.RequireForUpdate<PointerTag>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var playerInput = SystemAPI.GetSingleton<PlayerInputData>();
            var flightEntity = SystemAPI.GetSingletonEntity<ShipControlledByPlayerTag>();
            var flightInput = SystemAPI.GetAspect<FlightInputAspect>(flightEntity);
            flightInput.Strafe = new float3(playerInput.Strafe.x, 0f, playerInput.Strafe.y);
            flightInput.InertialDumper = playerInput.InertialDamper;

            var flightPosition = SystemAPI.GetComponent<LocalTransform>(flightEntity).Position;
            var pointerPosition = SystemAPI.GetComponent<LocalTransform>(SystemAPI.GetSingletonEntity<PointerTag>()).Position;

            if (playerInput.FreeLook == 0f) {
                var direction = math.normalize(pointerPosition - flightPosition);
                var isNan = math.isnan(direction);
                if (!(isNan.x || isNan.y || isNan.z)) flightInput.Direction = direction;
            } else {
                flightInput.Direction = float3.zero;
            }
            
        }
    }
}