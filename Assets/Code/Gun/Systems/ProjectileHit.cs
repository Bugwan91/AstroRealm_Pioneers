using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

namespace Code.Gun
{
    public partial struct ProjectileHitSystem : ISystem
    {
        ComponentLookup<LocalTransform> _positionLookup;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _positionLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _positionLookup.Update(ref state);
            var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
            return;
            new ProjectileHitJob
            {

            }.Schedule(simulation, state.Dependency);
        }
    }
    
    [BurstCompile]
    public struct ProjectileHitJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<LocalTransform> Positions;
        public EntityCommandBuffer.ParallelWriter ECB;
            
        public void Execute(TriggerEvent triggerEvent)
        {
                
        }
    }
}