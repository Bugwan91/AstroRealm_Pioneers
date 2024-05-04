using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

namespace Code.FloatingOrigin
{
    [UpdateInGroup(typeof(FloatingOriginSystemGroup), OrderFirst = true)]
    public partial struct UpdateFloatingOriginSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FloatingOriginTarget>();
            state.RequireForUpdate<FloatingOriginBase>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var floatingOrigin = SystemAPI.GetAspect<FloatingOriginAspect>(SystemAPI.GetSingletonEntity<FloatingOriginBase>());
            if (!floatingOrigin.Enabled) return;
            var floatingTarget = SystemAPI.GetSingletonEntity<FloatingOriginTarget>();
            var position = SystemAPI.GetComponent<LocalTransform>(floatingTarget).Position;
            var velocity = SystemAPI.GetComponent<PhysicsVelocity>(floatingTarget).Linear;
            floatingOrigin.Update(position, velocity);
        }
    }
}