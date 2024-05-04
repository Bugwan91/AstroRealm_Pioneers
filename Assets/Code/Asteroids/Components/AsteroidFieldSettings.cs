using Unity.Entities;
using Unity.Mathematics;

namespace Code.Asteroids
{
    public struct AsteroidFieldSettings : IComponentData
    {
        public float3 Bounds;
        public float MaxCount;
    }
}