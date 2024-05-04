using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Code.Asteroids
{
    
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [BurstCompile]
    public partial struct AsteroidsSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<AsteroidFieldSettings>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var asteroidField = SystemAPI.GetSingletonEntity<AsteroidFieldSettings>();
            var settings = SystemAPI.GetComponent<AsteroidFieldSettings>(asteroidField);
            var prefabs = SystemAPI.GetBuffer<Spawnable>(asteroidField);
            
            for (var i = 0; i < settings.MaxCount; i++)
            {
                var asteroid = state.EntityManager.Instantiate(prefabs[UnityEngine.Random.Range(0, 2)].Prefab);
                var transform = LocalTransform.Identity;
                transform.Position.x = UnityEngine.Random.Range(-settings.Bounds.x, settings.Bounds.x);
                transform.Position.y = math.pow(UnityEngine.Random.Range(-1f, 1f), 7f) * settings.Bounds.y;
                transform.Position.z = UnityEngine.Random.Range(-settings.Bounds.z, settings.Bounds.z);
                transform.RotateX(UnityEngine.Random.Range(-3, 3));
                transform.RotateY(UnityEngine.Random.Range(-3, 3));
                transform.RotateZ(UnityEngine.Random.Range(-3, 3));
                var scale = UnityEngine.Random.Range(0.5f, 8f) + 64f * math.pow(UnityEngine.Random.Range(0f, 1f), 64f);
                transform.Scale *= scale;
                state.EntityManager.SetComponentData(asteroid, transform);
                var phys = SystemAPI.GetComponent<PhysicsMass>(asteroid);
                phys.InverseMass = 2.0f / math.pow(scale, 2f);
                state.EntityManager.SetComponentData(asteroid, phys);
                var velocity = SystemAPI.GetComponent<PhysicsVelocity>(asteroid);
                velocity.Angular = new float3(
                    UnityEngine.Random.Range(-1f, 1f) / math.sqrt(scale),
                    UnityEngine.Random.Range(-1f, 1f) / math.sqrt(scale),
                    UnityEngine.Random.Range(-1f, 1f)) / math.sqrt(scale);
                state.EntityManager.SetComponentData(asteroid, velocity);
            }

            state.Enabled = false;
        }
    }
}