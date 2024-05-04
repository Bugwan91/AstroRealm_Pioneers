using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Code.Asteroids
{
    public class AsteroidsFieldAuthoring : MonoBehaviour
    {
        public float3 bounds;
        public float maxCount = 100f;
        public GameObject[] asteroidPrefabs;
        private class AsteroidsFieldBaker : Baker<AsteroidsFieldAuthoring>
        {
            public override void Bake(AsteroidsFieldAuthoring authoring)
            {
                if (authoring.asteroidPrefabs == null || authoring.asteroidPrefabs.Length == 0) return;
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var prefabsBuffer = AddBuffer<Spawnable>(entity);
                foreach (var asteroid in authoring.asteroidPrefabs)
                {
                    prefabsBuffer.Add(new Spawnable { Prefab = GetEntity(asteroid, TransformUsageFlags.Dynamic) });
                }
                AddComponent(entity, new AsteroidFieldSettings
                {
                    Bounds = authoring.bounds,
                    MaxCount = authoring.maxCount,
                });
            }
        }
        
    }
}