using Code.Player;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Code.Weapon
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct UpdateProjectileRelativeVelocitySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ShipControlledByPlayerTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var playerVelocity =
                SystemAPI.GetComponent<PhysicsVelocity>(SystemAPI.GetSingletonEntity<ShipControlledByPlayerTag>()).Linear;
            new UpdateProjectileRelativeVelocityJob
            {
                PlayerVelocity = playerVelocity
            }.ScheduleParallel();
        }
    }
    
    [BurstCompile]
    public partial struct UpdateProjectileRelativeVelocityJob : IJobEntity
    {
        public float3 PlayerVelocity;
        
        [BurstCompile]
        private void Execute(in PhysicsVelocity velocity, ref Projectile projectile)
        {
            projectile.CameraRelativeVelocity = PlayerVelocity - velocity.Linear;
        }
    }
}