using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Code.FloatingOrigin
{
    [UpdateInGroup(typeof(FloatingOriginSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(UpdateFloatingOriginSystem))]
    public partial struct UpdateFloatingOriginBodySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FloatingOriginBase>();
            state.RequireForUpdate<FloatingOriginBody>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var fo = SystemAPI.GetSingleton<FloatingOriginBase>();
            if (fo.Enabled && (fo.UpdatePosition || fo.UpdateVelocity))
            {
                new UpdateFloatingOriginBodyJob()
                {
                    DeltaPosition = fo.DeltaPosition,
                    DeltaVelocity = fo.DeltaVelocity
                }.ScheduleParallel();
            }
        }
        
        [BurstCompile]
        [WithAll(typeof(FloatingOriginBody))]
        public partial struct UpdateFloatingOriginBodyJob : IJobEntity
        {

            public float3 DeltaPosition;
            public float3 DeltaVelocity;
            [BurstCompile]
            private void Execute( ref LocalTransform transform,  ref PhysicsVelocity velocity)
            {
                transform.Position += DeltaPosition;
                velocity.Linear += DeltaVelocity;
            }
        }
    }
}