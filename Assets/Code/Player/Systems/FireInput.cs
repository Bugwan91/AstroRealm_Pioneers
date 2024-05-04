using Code.Gun;
using Unity.Burst;
using Unity.Entities;

namespace Code.Player
{
    [BurstCompile]
    [UpdateInGroup(typeof(PlayerInputSystemGroup))]
    public partial struct FireInputSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GunControlledByPlayer>();
            state.RequireForUpdate<PlayerInputData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var playerInput = SystemAPI.GetSingleton<PlayerInputData>();
            var gunEntity = SystemAPI.GetSingletonEntity<GunControlledByPlayer>();
            var fireInput = SystemAPI.GetAspect<FireInputAspect>(gunEntity);
            fireInput.Fire = playerInput.Fire;
        }
    }
}