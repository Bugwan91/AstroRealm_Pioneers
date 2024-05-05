using Code.FloatingOrigin;
using Code.Player;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

namespace Code.Camera
{
    [BurstCompile]
    [UpdateInGroup(typeof(CameraSystemGroup))]
    public partial struct UpdateVelocityPointer : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FloatingOriginBase>();
            state.RequireForUpdate<VelocityPointer>();
            state.RequireForUpdate<ShipControlledByPlayerTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var player = SystemAPI.GetSingletonEntity<ShipControlledByPlayerTag>();
            var fo = SystemAPI.GetAspect<FloatingOriginAspect>(SystemAPI.GetSingletonEntity<FloatingOriginBase>());
            var position = SystemAPI.GetComponent<LocalTransform>(player).Position;
            var velocity = SystemAPI.GetComponent<PhysicsVelocity>(player).Linear;
            var v = velocity - fo.Velocity;
            SystemAPI.SetComponent(
                SystemAPI.GetSingletonEntity<VelocityPointer>(),
                new VelocityPointer
                {
                    ForwardPosition = position + v,
                    BackwardPosition = position - v
                });
        }
    }
}