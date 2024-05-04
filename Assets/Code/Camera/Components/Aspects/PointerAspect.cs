using Code.Utils;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Code.Camera
{
    public readonly partial struct PointerAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<LocalTransform> _transform;

        public float3 Position
        {
            get => _transform.ValueRO.Position;
            set => _transform.ValueRW.Position = value;
        }
    }
}