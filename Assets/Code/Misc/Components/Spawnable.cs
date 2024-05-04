using Unity.Entities;

namespace Code
{
    [InternalBufferCapacity(3)]
    public struct Spawnable : IBufferElementData
    {
        public Entity Prefab;
    }
}